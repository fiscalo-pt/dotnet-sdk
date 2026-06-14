using Fiscalo.Sdk.Models;
using Fiscalo.Sdk.Support;

namespace Fiscalo.Sdk.Resources;

/// <summary>
/// Provides access to Fiscalo document creation, issuing and PDF endpoints.
/// </summary>
public sealed class DocumentsResource
{
    private readonly FiscaloHttpClient httpClient;

    /// <summary>
    /// Creates a document resource wrapper over the shared Fiscalo transport.
    /// </summary>
    public DocumentsResource(FiscaloHttpClient httpClient) => this.httpClient = httpClient;

    /// <summary>
    /// Lists documents available in the current Fiscalo environment.
    /// </summary>
    public Task<FiscaloListResponse<Document>> ListAsync(CancellationToken cancellationToken = default) =>
        httpClient.SendCollectionAsync<Document>(HttpMethod.Get, "documents", cancellationToken: cancellationToken);

    /// <summary>
    /// Creates a new draft document.
    /// </summary>
    public Task<FiscaloResponse<Document>> CreateAsync(CreateDocumentRequest payload, string? idempotencyKey = null, CancellationToken cancellationToken = default) =>
        httpClient.SendEnvelopeAsync<Document>(HttpMethod.Post, "documents", payload, idempotencyKey, cancellationToken: cancellationToken);

    /// <summary>
    /// Retrieves a single document by identifier.
    /// </summary>
    public Task<FiscaloResponse<Document>> GetAsync(string id, CancellationToken cancellationToken = default) =>
        httpClient.SendEnvelopeAsync<Document>(HttpMethod.Get, $"documents/{id}", cancellationToken: cancellationToken);

    /// <summary>
    /// Adds a new line to an existing draft document.
    /// </summary>
    public Task<FiscaloResponse<Document>> AddLineAsync(string id, CreateDocumentLineRequest payload, string? idempotencyKey = null, CancellationToken cancellationToken = default) =>
        httpClient.SendEnvelopeAsync<Document>(HttpMethod.Post, $"documents/{id}/lines", payload, idempotencyKey, cancellationToken: cancellationToken);

    /// <summary>
    /// Issues a draft document and locks its fiscal data.
    /// </summary>
    public Task<FiscaloResponse<Document>> IssueAsync(string id, IssueDocumentRequest? payload = null, string? idempotencyKey = null, CancellationToken cancellationToken = default) =>
        httpClient.SendEnvelopeAsync<Document>(HttpMethod.Post, $"documents/{id}/issue", payload ?? new IssueDocumentRequest(), idempotencyKey, cancellationToken: cancellationToken);

    /// <summary>
    /// Requests Fiscalo to generate the PDF representation for a document.
    /// </summary>
    public Task<FiscaloResponse<DocumentPdf>> GeneratePdfAsync(string id, CancellationToken cancellationToken = default) =>
        httpClient.SendEnvelopeAsync<DocumentPdf>(HttpMethod.Post, $"documents/{id}/pdf", cancellationToken: cancellationToken);

    /// <summary>
    /// Downloads the generated PDF bytes for a document.
    /// </summary>
    public Task<FiscaloBinaryResponse> GetPdfAsync(string id, CancellationToken cancellationToken = default) =>
        httpClient.GetBinaryAsync($"documents/{id}/pdf", cancellationToken);
}
