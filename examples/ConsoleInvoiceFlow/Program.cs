using Fiscalo.Sdk;
using Fiscalo.Sdk.Models;

var client = new FiscaloClient(
    apiKey: Environment.GetEnvironmentVariable("FISCALO_API_KEY") ?? "fisc_test_xxx",
    baseUrl: Environment.GetEnvironmentVariable("FISCALO_API_BASE_URL") ?? "https://api.fiscalo.pt/api/v1");

var customer = await client.Customers.CreateAsync(new CreateCustomerRequest
{
    Name = "Cliente Console .NET",
    CustomerType = "company",
    Country = "PT",
    Email = "cliente@example.pt",
});

var item = await client.Items.CreateAsync(new CreateItemRequest
{
    Name = "Serviço console .NET",
    Type = "service",
    Unit = "un",
    UnitPrice = 120m,
});

var series = await client.DocumentSeries.CreateAsync(new CreateDocumentSeriesRequest
{
    DocumentType = "invoice",
    Code = "A",
});

var document = await client.Documents.CreateAsync(new CreateDocumentRequest
{
    CustomerId = customer.Data?.Id,
    SeriesId = series.Data?.Id,
    DocumentType = "invoice",
    Source = "api",
}, idempotencyKey: Guid.NewGuid().ToString("N"));

await client.Documents.AddLineAsync(document.Data?.Id ?? string.Empty, new CreateDocumentLineRequest
{
    ItemId = item.Data?.Id,
    Description = "Linha exemplo .NET",
    Quantity = 1,
    Unit = "un",
    UnitPrice = 120m,
});

await client.Documents.IssueAsync(document.Data?.Id ?? string.Empty);
await client.Documents.GeneratePdfAsync(document.Data?.Id ?? string.Empty);
var pdf = await client.Documents.GetPdfAsync(document.Data?.Id ?? string.Empty);

Console.WriteLine($"Customer: {customer.Data?.Id}");
Console.WriteLine($"Item: {item.Data?.Id}");
Console.WriteLine($"Series: {series.Data?.Id}");
Console.WriteLine($"Documento criado: {document.Data?.Id}");
Console.WriteLine($"PDF bytes: {pdf.Buffer.Length}");
