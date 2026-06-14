using System.Net;

namespace Fiscalo.Sdk.Tests;

public sealed class PdfResourceTests
{
    [Fact]
    public async Task GetPdfReturnsBinaryPayload()
    {
        var handler = new StubHttpMessageHandler((request, _) =>
        {
            Assert.Equal(HttpMethod.Get, request.Method);
            Assert.Equal("https://api.fiscalo.pt/api/v1/documents/doc_123/pdf", request.RequestUri?.ToString());

            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent("%PDF-sandbox"u8.ToArray()),
            });
        });

        var fiscalo = new Fiscalo.Sdk.FiscaloClient(
            apiKey: "fisc_test_dotnet",
            baseUrl: "https://api.fiscalo.pt/api/v1",
            httpClient: new HttpClient(handler));

        var response = await fiscalo.Pdf.GetAsync("doc_123");

        Assert.NotEmpty(response.Buffer);
    }
}
