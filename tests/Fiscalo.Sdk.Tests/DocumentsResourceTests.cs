using System.Net;
using System.Text;
using Fiscalo.Sdk.Models;

namespace Fiscalo.Sdk.Tests;

public sealed class DocumentsResourceTests
{
    [Fact]
    public async Task CreateDocumentUsesDocumentsEndpoint()
    {
        var handler = new StubHttpMessageHandler((request, _) =>
        {
            Assert.Equal(HttpMethod.Post, request.Method);
            Assert.Equal("https://api.fiscalo.pt/api/v1/documents", request.RequestUri?.ToString());

            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("""{"data":{"id":"doc_123","status":"draft"}}""", Encoding.UTF8, "application/json"),
            });
        });

        var fiscalo = new Fiscalo.Sdk.FiscaloClient(
            apiKey: "fisc_test_dotnet",
            baseUrl: "https://api.fiscalo.pt/api/v1",
            httpClient: new HttpClient(handler));

        var response = await fiscalo.Documents.CreateAsync(new CreateDocumentRequest
        {
            CustomerId = "cus_123",
            SeriesId = "ser_123",
            DocumentType = "invoice",
        });

        Assert.Equal("doc_123", response.Data?.Id);
    }

    [Fact]
    public async Task GeneratePdfUsesDocumentPdfEndpoint()
    {
        var handler = new StubHttpMessageHandler((request, _) =>
        {
            Assert.Equal(HttpMethod.Post, request.Method);
            Assert.Equal("https://api.fiscalo.pt/api/v1/documents/doc_123/pdf", request.RequestUri?.ToString());

            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("""{"data":{"document_id":"doc_123","download_url":"https://api.fiscalo.pt/api/v1/documents/doc_123/pdf"}}""", Encoding.UTF8, "application/json"),
            });
        });

        var fiscalo = new Fiscalo.Sdk.FiscaloClient(
            apiKey: "fisc_test_dotnet",
            baseUrl: "https://api.fiscalo.pt/api/v1",
            httpClient: new HttpClient(handler));

        var response = await fiscalo.Documents.GeneratePdfAsync("doc_123");

        Assert.Equal("doc_123", response.Data?.DocumentId);
        Assert.Equal("https://api.fiscalo.pt/api/v1/documents/doc_123/pdf", response.Data?.DownloadUrl);
    }
}
