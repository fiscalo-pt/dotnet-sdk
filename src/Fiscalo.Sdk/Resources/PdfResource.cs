using Fiscalo.Sdk.Support;

namespace Fiscalo.Sdk.Resources;

public sealed class PdfResource
{
    private readonly FiscaloHttpClient httpClient;

    public PdfResource(FiscaloHttpClient httpClient) => this.httpClient = httpClient;

    public Task<FiscaloBinaryResponse> GetAsync(string documentId, CancellationToken cancellationToken = default) =>
        httpClient.GetBinaryAsync($"documents/{documentId}/pdf", cancellationToken);
}
