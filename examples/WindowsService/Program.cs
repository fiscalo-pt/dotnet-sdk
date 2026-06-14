using Fiscalo.Sdk.Abstractions;
using Fiscalo.Sdk.Extensions;
using Fiscalo.Sdk.Models;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddWindowsService(options =>
{
    options.ServiceName = "Fiscalo Windows Service Example";
});

builder.Services.AddFiscalo(options =>
{
    options.ApiKey = builder.Configuration["Fiscalo:ApiKey"] ?? "fisc_test_xxx";
    options.BaseUrl = builder.Configuration["Fiscalo:BaseUrl"] ?? "https://api.fiscalo.pt/api/v1";
    options.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddHostedService<InvoiceWorker>();

var host = builder.Build();
await host.RunAsync();

internal sealed class InvoiceWorker : BackgroundService
{
    private readonly IFiscaloClient fiscalo;
    private readonly ILogger<InvoiceWorker> logger;

    public InvoiceWorker(IFiscaloClient fiscalo, ILogger<InvoiceWorker> logger)
    {
        this.fiscalo = fiscalo;
        this.logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var customer = await fiscalo.Customers.CreateAsync(new CreateCustomerRequest
        {
            Name = "Cliente Windows Service",
            CustomerType = "company",
            Country = "PT",
            Email = "windows-service@example.pt",
        }, stoppingToken);

        var item = await fiscalo.Items.CreateAsync(new CreateItemRequest
        {
            Name = "Serviço recorrente",
            Type = "service",
            Unit = "un",
            UnitPrice = 250m,
        }, stoppingToken);

        var series = await fiscalo.DocumentSeries.CreateAsync(new CreateDocumentSeriesRequest
        {
            DocumentType = "invoice",
            Code = "WIN",
        }, stoppingToken);

        var document = await fiscalo.Documents.CreateAsync(new CreateDocumentRequest
        {
            CustomerId = customer.Data?.Id,
            SeriesId = series.Data?.Id,
            DocumentType = "invoice",
        }, idempotencyKey: Guid.NewGuid().ToString("N"), cancellationToken: stoppingToken);

        await fiscalo.DocumentLines.CreateAsync(document.Data?.Id ?? string.Empty, new CreateDocumentLineRequest
        {
            ItemId = item.Data?.Id,
            Description = "Execução automática via Windows Service",
            Quantity = 1,
            Unit = "un",
            UnitPrice = 250m,
        }, cancellationToken: stoppingToken);

        await fiscalo.Documents.IssueAsync(document.Data?.Id ?? string.Empty, cancellationToken: stoppingToken);
        await fiscalo.Documents.GeneratePdfAsync(document.Data?.Id ?? string.Empty, stoppingToken);
        var pdf = await fiscalo.Pdf.GetAsync(document.Data?.Id ?? string.Empty, stoppingToken);

        logger.LogInformation(
            "Fluxo Fiscalo concluído. Customer {CustomerId}, Document {DocumentId}, PdfBytes {PdfBytes}",
            customer.Data?.Id,
            document.Data?.Id,
            pdf.Buffer.Length);
    }
}
