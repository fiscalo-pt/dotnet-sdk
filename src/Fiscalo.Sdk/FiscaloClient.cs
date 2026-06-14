using System.Text.Json;
using Fiscalo.Sdk.Abstractions;
using Fiscalo.Sdk.Configuration;
using Fiscalo.Sdk.Resources;
using Fiscalo.Sdk.Support;

namespace Fiscalo.Sdk;

/// <summary>
/// Primary entry point for interacting with the Fiscalo API from .NET applications.
/// </summary>
public class FiscaloClient : IFiscaloClient
{
    private readonly FiscaloHttpClient httpClient;
    private CustomersResource? customers;
    private ItemsResource? items;
    private DocumentSeriesResource? documentSeries;
    private DocumentsResource? documents;
    private DocumentLinesResource? documentLines;
    private PdfResource? pdf;

    /// <summary>
    /// Default production base URL for the Fiscalo API.
    /// </summary>
    public const string DefaultBaseUrl = "https://api.fiscalo.pt/api/v1";

    /// <summary>
    /// Creates a new Fiscalo client using the provided API key and optional transport settings.
    /// </summary>
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

    /// <summary>
    /// Creates a new Fiscalo client from a structured options object.
    /// </summary>
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

    /// <summary>
    /// Creates a new Fiscalo client using an externally managed <see cref="HttpClient" />.
    /// </summary>
    public FiscaloClient(
        HttpClient httpClient,
        FiscaloOptions options,
        JsonSerializerOptions? serializerOptions = null)
        : this(options, httpClient, serializerOptions)
    {
    }

    /// <summary>
    /// API key used for authenticated requests.
    /// </summary>
    public string ApiKey { get; }

    /// <summary>
    /// Normalized API base URL used by the SDK.
    /// </summary>
    public string BaseUrl { get; }

    /// <summary>
    /// Effective request timeout for outbound API calls.
    /// </summary>
    public TimeSpan Timeout { get; }

    /// <summary>
    /// Low-level HTTP transport used by the SDK.
    /// </summary>
    public FiscaloHttpClient HttpClient => httpClient;

    /// <summary>
    /// Customer endpoints for create, read, list and update operations.
    /// </summary>
    public CustomersResource Customers => customers ??= new CustomersResource(httpClient);

    /// <summary>
    /// Item endpoints for catalog and pricing operations.
    /// </summary>
    public ItemsResource Items => items ??= new ItemsResource(httpClient);

    /// <summary>
    /// Document series endpoints for issuing configuration.
    /// </summary>
    public DocumentSeriesResource DocumentSeries => documentSeries ??= new DocumentSeriesResource(httpClient);

    /// <summary>
    /// Document endpoints for creation, issuing and PDF generation workflows.
    /// </summary>
    public DocumentsResource Documents => documents ??= new DocumentsResource(httpClient);

    public DocumentLinesResource DocumentLines => documentLines ??= new DocumentLinesResource(httpClient);

    public PdfResource Pdf => pdf ??= new PdfResource(httpClient);
}
