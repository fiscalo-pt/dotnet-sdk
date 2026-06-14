# Fiscalo .NET SDK

Official .NET SDK for the Fiscalo API.

`Fiscalo.Sdk` helps teams integrate Fiscalo faster in ASP.NET Core, console apps, Windows services and other .NET workloads, with first-class support for customers, items, document series, documents, idempotency and PDF retrieval.

## Installation

Install from NuGet:

```bash
dotnet add package Fiscalo.Sdk
```

For local development inside the SDK repository:

```xml
<ProjectReference Include="..\\..\\src\\Fiscalo.Sdk\\Fiscalo.Sdk.csproj" />
```

## Requirements

- .NET 8 or later
- A Fiscalo API key
- Sandbox base URL for development: `https://api.fiscalo.test/api/v1`
- Production base URL: `https://api.fiscalo.pt/api/v1`

## Quick Start

```csharp
using Fiscalo.Sdk;

var fiscalo = new FiscaloClient(
    apiKey: Environment.GetEnvironmentVariable("FISCALO_API_KEY")!,
    baseUrl: Environment.GetEnvironmentVariable("FISCALO_API_BASE_URL")
        ?? "https://api.fiscalo.test/api/v1",
    timeout: TimeSpan.FromSeconds(30));
```

## ASP.NET Core

```csharp
using Fiscalo.Sdk.Abstractions;
using Fiscalo.Sdk.Extensions;

builder.Services.AddFiscalo(options =>
{
    options.ApiKey = builder.Configuration["Fiscalo:ApiKey"]!;
    options.BaseUrl = builder.Configuration["Fiscalo:BaseUrl"]
        ?? "https://api.fiscalo.test/api/v1";
    options.Timeout = TimeSpan.FromSeconds(30);
});

app.MapGet("/customers", async (IFiscaloClient fiscalo) =>
{
    var response = await fiscalo.Customers.ListAsync();

    return Results.Ok(response.Items);
});
```

## Example Document Flow

```csharp
using Fiscalo.Sdk;
using Fiscalo.Sdk.Models;

var fiscalo = new FiscaloClient(
    apiKey: "fisc_test_xxx",
    baseUrl: "https://api.fiscalo.test/api/v1");

var customer = await fiscalo.Customers.CreateAsync(new CreateCustomerRequest
{
    Name = "Cliente .NET",
    CustomerType = "company",
    Country = "PT",
    Email = "cliente@example.pt",
});

var item = await fiscalo.Items.CreateAsync(new CreateItemRequest
{
    Name = "Serviço de implementação",
    Type = "service",
    Unit = "un",
    UnitPrice = 100m,
});

var series = await fiscalo.DocumentSeries.CreateAsync(new CreateDocumentSeriesRequest
{
    DocumentType = "invoice",
    Code = "A",
});

var document = await fiscalo.Documents.CreateAsync(new CreateDocumentRequest
{
    CustomerId = customer.Data?.Id,
    SeriesId = series.Data?.Id,
    DocumentType = "invoice",
}, idempotencyKey: Guid.NewGuid().ToString("N"));

await fiscalo.Documents.AddLineAsync(document.Data?.Id!, new CreateDocumentLineRequest
{
    ItemId = item.Data?.Id,
    Description = "Serviço de implementação",
    Quantity = 1,
    Unit = "un",
    UnitPrice = 100m,
});

await fiscalo.Documents.IssueAsync(document.Data?.Id!);
await fiscalo.Documents.GeneratePdfAsync(document.Data?.Id!);
var pdf = await fiscalo.Documents.GetPdfAsync(document.Data?.Id!);
```

## Error Handling

The SDK raises typed exceptions so integrations can react to API failures in a predictable way:

- `FiscaloAuthenticationException`
- `FiscaloValidationException`
- `FiscaloNotFoundException`
- `FiscaloRateLimitException`
- `FiscaloIdempotencyException`
- `FiscaloApiException`

```csharp
using Fiscalo.Sdk.Exceptions;

try
{
    await fiscalo.Customers.CreateAsync(new CreateCustomerRequest
    {
        Name = "Cliente Demo",
        CustomerType = "company",
    });
}
catch (FiscaloValidationException exception)
{
    Console.WriteLine(exception.RequestId);
    Console.WriteLine(exception.Message);
}
```

## Integration Notes

- The SDK sends `Authorization: Bearer` automatically.
- Use `Idempotency-Key` for create, line and issue operations.
- Response envelopes expose `RequestId` for observability and support.
- JSON payloads are serialized in `snake_case`.

## Official Links

- Documentation: [docs.fiscalo.pt/integrations/dotnet](https://docs.fiscalo.pt/integrations/dotnet)
- API reference: [docs.fiscalo.pt](https://docs.fiscalo.pt)
- GitHub: [github.com/fiscalo-pt/dotnet-sdk](https://github.com/fiscalo-pt/dotnet-sdk)
- NuGet: [nuget.org/packages/Fiscalo.Sdk](https://www.nuget.org/packages/Fiscalo.Sdk)
- Downloads: [downloads.fiscalo.pt/sdk/dotnet/latest](https://downloads.fiscalo.pt/sdk/dotnet/latest)
- Fiscalo platform: [fiscalo.pt](https://fiscalo.pt)

## Repository Layout

- `src/Fiscalo.Sdk`
- `tests/Fiscalo.Sdk.Tests`
- `examples/ConsoleInvoiceFlow`
- `examples/AspNetCoreMinimalApi`
- `examples/WindowsService`
