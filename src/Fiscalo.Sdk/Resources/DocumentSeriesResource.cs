using Fiscalo.Sdk.Models;
using Fiscalo.Sdk.Support;

namespace Fiscalo.Sdk.Resources;

public sealed class DocumentSeriesResource
{
    private readonly FiscaloHttpClient httpClient;

    public DocumentSeriesResource(FiscaloHttpClient httpClient) => this.httpClient = httpClient;

    public Task<FiscaloListResponse<DocumentSeries>> ListAsync(CancellationToken cancellationToken = default) =>
        httpClient.SendCollectionAsync<DocumentSeries>(HttpMethod.Get, "document-series", cancellationToken: cancellationToken);

    public Task<FiscaloResponse<DocumentSeries>> CreateAsync(CreateDocumentSeriesRequest payload, CancellationToken cancellationToken = default) =>
        httpClient.SendEnvelopeAsync<DocumentSeries>(HttpMethod.Post, "document-series", payload, cancellationToken: cancellationToken);
}
