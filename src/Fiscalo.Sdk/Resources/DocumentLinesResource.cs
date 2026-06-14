using Fiscalo.Sdk.Models;
using Fiscalo.Sdk.Support;

namespace Fiscalo.Sdk.Resources;

public sealed class DocumentLinesResource
{
    private readonly FiscaloHttpClient httpClient;

    public DocumentLinesResource(FiscaloHttpClient httpClient) => this.httpClient = httpClient;

    public Task<FiscaloResponse<Document>> CreateAsync(
        string documentId,
        CreateDocumentLineRequest payload,
        string? idempotencyKey = null,
        CancellationToken cancellationToken = default) =>
        httpClient.SendEnvelopeAsync<Document>(
            HttpMethod.Post,
            $"documents/{documentId}/lines",
            payload,
            idempotencyKey,
            cancellationToken: cancellationToken);
}
