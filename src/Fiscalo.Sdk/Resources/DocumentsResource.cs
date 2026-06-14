using Fiscalo.Sdk.Models;
using Fiscalo.Sdk.Support;

namespace Fiscalo.Sdk.Resources;

public sealed class DocumentsResource
{
    private readonly FiscaloHttpClient httpClient;

    public DocumentsResource(FiscaloHttpClient httpClient) => this.httpClient = httpClient;

    public Task<FiscaloListResponse<Document>> ListAsync(CancellationToken cancellationToken = default) =>
        httpClient.SendCollectionAsync<Document>(HttpMethod.Get, "documents", cancellationToken: cancellationToken);

    public Task<FiscaloResponse<Document>> CreateAsync(CreateDocumentRequest payload, string? idempotencyKey = null, CancellationToken cancellationToken = default) =>
        httpClient.SendEnvelopeAsync<Document>(HttpMethod.Post, "documents", payload, idempotencyKey, cancellationToken: cancellationToken);

    public Task<FiscaloResponse<Document>> GetAsync(string id, CancellationToken cancellationToken = default) =>
        httpClient.SendEnvelopeAsync<Document>(HttpMethod.Get, $"documents/{id}", cancellationToken: cancellationToken);

    public Task<FiscaloResponse<Document>> AddLineAsync(string id, CreateDocumentLineRequest payload, string? idempotencyKey = null, CancellationToken cancellationToken = default) =>
        httpClient.SendEnvelopeAsync<Document>(HttpMethod.Post, $"documents/{id}/lines", payload, idempotencyKey, cancellationToken: cancellationToken);

    public Task<FiscaloResponse<Document>> IssueAsync(string id, IssueDocumentRequest? payload = null, string? idempotencyKey = null, CancellationToken cancellationToken = default) =>
        httpClient.SendEnvelopeAsync<Document>(HttpMethod.Post, $"documents/{id}/issue", payload ?? new IssueDocumentRequest(), idempotencyKey, cancellationToken: cancellationToken);

    public Task<FiscaloResponse<DocumentPdf>> GeneratePdfAsync(string id, CancellationToken cancellationToken = default) =>
        httpClient.SendEnvelopeAsync<DocumentPdf>(HttpMethod.Post, $"documents/{id}/pdf", cancellationToken: cancellationToken);

    public Task<FiscaloBinaryResponse> GetPdfAsync(string id, CancellationToken cancellationToken = default) =>
        httpClient.GetBinaryAsync($"documents/{id}/pdf", cancellationToken);
}
