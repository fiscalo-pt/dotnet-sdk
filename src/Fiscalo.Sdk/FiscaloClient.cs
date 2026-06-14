using System.Text.Json;
using Fiscalo.Sdk.Abstractions;
using Fiscalo.Sdk.Configuration;
using Fiscalo.Sdk.Resources;
using Fiscalo.Sdk.Support;

namespace Fiscalo.Sdk;

public class FiscaloClient : IFiscaloClient
{
    private readonly FiscaloHttpClient httpClient;
    private CustomersResource? customers;
    private ItemsResource? items;
    private DocumentSeriesResource? documentSeries;
    private DocumentsResource? documents;
    private DocumentLinesResource? documentLines;
    private PdfResource? pdf;

    public const string DefaultBaseUrl = "https://api.fiscalo.pt/api/v1";

    public FiscaloClient(
        string apiKey,
        string baseUrl = DefaultBaseUrl,
        TimeSpan? timeout = null,
        HttpClient? httpClient = null,
        JsonSerializerOptions? serializerOptions = null)
        : this(
            new FiscaloOptions
            {
                ApiKey = apiKey,
                BaseUrl = baseUrl,
                Timeout = timeout ?? TimeSpan.FromSeconds(30),
            },
            httpClient,
            serializerOptions)
    {
    }

    public FiscaloClient(
        FiscaloOptions options,
        HttpClient? httpClient = null,
        JsonSerializerOptions? serializerOptions = null)
    {
        ArgumentNullException.ThrowIfNull(options);
        options.Validate();

        ApiKey = options.ApiKey;
        BaseUrl = options.BaseUrl.TrimEnd('/');
        Timeout = options.Timeout;
        this.httpClient = new FiscaloHttpClient(ApiKey, BaseUrl, Timeout, httpClient, serializerOptions);
    }

    public FiscaloClient(
        HttpClient httpClient,
        FiscaloOptions options,
        JsonSerializerOptions? serializerOptions = null)
        : this(options, httpClient, serializerOptions)
    {
    }

    public string ApiKey { get; }

    public string BaseUrl { get; }

    public TimeSpan Timeout { get; }

    public FiscaloHttpClient HttpClient => httpClient;

    public CustomersResource Customers => customers ??= new CustomersResource(httpClient);

    public ItemsResource Items => items ??= new ItemsResource(httpClient);

    public DocumentSeriesResource DocumentSeries => documentSeries ??= new DocumentSeriesResource(httpClient);

    public DocumentsResource Documents => documents ??= new DocumentsResource(httpClient);

    public DocumentLinesResource DocumentLines => documentLines ??= new DocumentLinesResource(httpClient);

    public PdfResource Pdf => pdf ??= new PdfResource(httpClient);
}
