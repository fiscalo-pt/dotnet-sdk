using Fiscalo.Sdk.Abstractions;
using Fiscalo.Sdk.Extensions;
using Fiscalo.Sdk.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFiscalo(options =>
{
    options.ApiKey = builder.Configuration["Fiscalo:ApiKey"] ?? "fisc_test_xxx";
    options.BaseUrl = builder.Configuration["Fiscalo:BaseUrl"] ?? "https://api.fiscalo.pt/api/v1";
    options.Timeout = TimeSpan.FromSeconds(30);
});

var app = builder.Build();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.MapGet("/bootstrap", async (IFiscaloClient fiscalo) =>
{
    var customer = await fiscalo.Customers.CreateAsync(new CreateCustomerRequest
    {
        Name = "Cliente ASP.NET",
        CustomerType = "company",
        Country = "PT",
        Email = "aspnet@example.pt",
    });

    var item = await fiscalo.Items.CreateAsync(new CreateItemRequest
    {
        Name = "Serviço ASP.NET",
        Type = "service",
        Unit = "un",
        UnitPrice = 180m,
    });

    var series = await fiscalo.DocumentSeries.CreateAsync(new CreateDocumentSeriesRequest
    {
        DocumentType = "invoice",
        Code = "WEB",
    });

    var document = await fiscalo.Documents.CreateAsync(new CreateDocumentRequest
    {
        CustomerId = customer.Data?.Id,
        SeriesId = series.Data?.Id,
        DocumentType = "invoice",
    }, idempotencyKey: Guid.NewGuid().ToString("N"));

    await fiscalo.Documents.AddLineAsync(document.Data?.Id ?? string.Empty, new CreateDocumentLineRequest
    {
        ItemId = item.Data?.Id,
        Description = "Fluxo ASP.NET",
        Quantity = 1,
        Unit = "un",
        UnitPrice = 180m,
    });

    await fiscalo.Documents.IssueAsync(document.Data?.Id ?? string.Empty);
    var pdf = await fiscalo.Documents.GeneratePdfAsync(document.Data?.Id ?? string.Empty);

    return Results.Ok(new
    {
        customer_id = customer.Data?.Id,
        item_id = item.Data?.Id,
        series_id = series.Data?.Id,
        document_id = document.Data?.Id,
        pdf_url = pdf.Data?.DownloadUrl,
    });
});

app.MapPost("/customers", async (IFiscaloClient fiscalo, CreateCustomerRequest request) =>
{
    var response = await fiscalo.Customers.CreateAsync(request);

    return Results.Json(response.Data);
});

app.Run();
