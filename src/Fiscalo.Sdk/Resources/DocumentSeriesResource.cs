using Fiscalo.Sdk.Models;
using Fiscalo.Sdk.Support;

namespace Fiscalo.Sdk.Resources;

/// <summary>
/// Provides access to Fiscalo document series endpoints.
/// </summary>
public sealed class DocumentSeriesResource
{
    private readonly FiscaloHttpClient httpClient;

    /// <summary>
    /// Creates a document series resource wrapper over the shared Fiscalo transport.
    /// </summary>
    public DocumentSeriesResource(FiscaloHttpClient httpClient) => this.httpClient = httpClient;

    /// <summary>
    /// Lists document series available in the current Fiscalo environment.
    /// </summary>
    public Task<FiscaloListResponse<DocumentSeries>> ListAsync(CancellationToken cancellationToken = default) =>
        httpClient.SendCollectionAsync<DocumentSeries>(HttpMethod.Get, "document-series", cancellationToken: cancellationToken);

    /// <summary>
    /// Creates a new document series in Fiscalo.
    /// </summary>
    public Task<FiscaloResponse<DocumentSeries>> CreateAsync(CreateDocumentSeriesRequest payload, CancellationToken cancellationToken = default) =>
        httpClient.SendEnvelopeAsync<DocumentSeries>(HttpMethod.Post, "document-series", payload, cancellationToken: cancellationToken);
}
