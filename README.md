# Fiscalo .NET SDK

SDK oficial da Fiscalo para .NET, C# e ecossistema Microsoft.

O pacote `Fiscalo.Sdk` foi preparado para publicação no GitHub e no NuGet com foco em integrações reais para:

- ASP.NET Core
- Blazor Server / Web Apps
- Windows Services
- Console Apps
- ERPs e software houses

## Instalação

Produção / NuGet:

```bash
dotnet add package Fiscalo.Sdk
```

Desenvolvimento local dentro do monorepo:

```xml
<ProjectReference Include="..\..\src\Fiscalo.Sdk\Fiscalo.Sdk.csproj" />
```

## Compatibilidade

- .NET 8+
- ASP.NET Core
- C# com nullable reference types
- `HttpClient` injetável

## Estrutura

- `src/Fiscalo.Sdk`
- `tests/Fiscalo.Sdk.Tests`
- `examples/ConsoleInvoiceFlow`
- `examples/AspNetCoreMinimalApi`
- `examples/WindowsService`

## Configuração básica

```csharp
using Fiscalo.Sdk;

var fiscalo = new FiscaloClient(
    apiKey: Environment.GetEnvironmentVariable("FISCALO_API_KEY")!,
    baseUrl: Environment.GetEnvironmentVariable("FISCALO_API_BASE_URL") ?? "https://api.fiscalo.pt/api/v1",
    timeout: TimeSpan.FromSeconds(30));
```

## ASP.NET Core DI

```csharp
using Fiscalo.Sdk.Abstractions;
using Fiscalo.Sdk.Extensions;

builder.Services.AddFiscalo(options =>
{
    options.ApiKey = builder.Configuration["Fiscalo:ApiKey"]!;
    options.BaseUrl = builder.Configuration["Fiscalo:BaseUrl"] ?? "https://api.fiscalo.pt/api/v1";
    options.Timeout = TimeSpan.FromSeconds(30);
});

app.MapGet("/customers", async (IFiscaloClient fiscalo) =>
{
    var response = await fiscalo.Customers.ListAsync();

    return Results.Ok(response.Items);
});
```

## Fluxo básico

```csharp
using Fiscalo.Sdk;
using Fiscalo.Sdk.Models;

var fiscalo = new FiscaloClient(
    apiKey: "fisc_test_xxx",
    baseUrl: "https://api.fiscalo.pt/api/v1"
);

var customer = await fiscalo.Customers.CreateAsync(new CreateCustomerRequest
{
    Name = "Cliente .NET",
    CustomerType = "company",
    Country = "PT",
    Email = "cliente@example.pt"
});

var item = await fiscalo.Items.CreateAsync(new CreateItemRequest
{
    Name = "Serviço de implementação",
    Type = "service",
    Unit = "un",
    UnitPrice = 100m
});

var series = await fiscalo.DocumentSeries.CreateAsync(new CreateDocumentSeriesRequest
{
    DocumentType = "invoice",
    Code = "A"
});

var document = await fiscalo.Documents.CreateAsync(new CreateDocumentRequest
{
    CustomerId = customer.Data?.Id,
    SeriesId = series.Data?.Id,
    DocumentType = "invoice"
}, idempotencyKey: Guid.NewGuid().ToString("N"));

await fiscalo.Documents.AddLineAsync(document.Data?.Id!, new CreateDocumentLineRequest
{
    ItemId = item.Data?.Id,
    Description = "Serviço de implementação",
    Quantity = 1,
    Unit = "un",
    UnitPrice = 100m
});

await fiscalo.Documents.IssueAsync(document.Data?.Id!);
await fiscalo.Documents.GeneratePdfAsync(document.Data?.Id!);
var pdf = await fiscalo.Documents.GetPdfAsync(document.Data?.Id!);
```

## Idempotency e request tracing

O SDK trata automaticamente:

- `Authorization: Bearer`
- `Idempotency-Key`
- leitura de `x-request-id`
- serialização JSON em `snake_case`
- exceções tipadas por status HTTP

## Tratamento de erros

Exceções disponíveis:

- `FiscaloApiException`
- `FiscaloAuthenticationException`
- `FiscaloValidationException`
- `FiscaloNotFoundException`
- `FiscaloRateLimitException`
- `FiscaloIdempotencyException`

## Sandbox

Para integração inicial:

- use `fisc_test_*`
- base URL `https://api.fiscalo.test/api/v1`
- valide `customers`, `items`, `document-series`, `documents` e `pdf`

## Links oficiais

- Docs: `https://docs.fiscalo.pt/integrations/dotnet`
- GitHub: `https://github.com/fiscalo-pt/dotnet-sdk`
- NuGet: `https://www.nuget.org/packages/Fiscalo.Sdk`
- Downloads: `https://downloads.fiscalo.pt/sdk/dotnet/latest`

## Exemplos incluídos

- `examples/ConsoleInvoiceFlow`
- `examples/AspNetCoreMinimalApi`
- `examples/WindowsService`

## Estado desta fase

- pacote preparado para GitHub + NuGet
- DI ASP.NET Core suportada
- examples completos para fluxo documental
- testes unitários criados para auth, idempotência, request ID, erros e PDF
- validação `dotnet` pendente nesta máquina porque o runtime não está instalado
