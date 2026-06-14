using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Fiscalo.Sdk.Exceptions;
using Fiscalo.Sdk.Models;
using Fiscalo.Sdk.Support;

namespace Fiscalo.Sdk.Tests;

public sealed class FiscaloHttpClientTests
{
    [Fact]
    public async Task AddsAuthorizationAndIdempotencyHeaders()
    {
        var handler = new StubHttpMessageHandler((request, _) =>
        {
            Assert.Equal("Bearer", request.Headers.Authorization?.Scheme);
            Assert.Equal("fisc_test_dotnet", request.Headers.Authorization?.Parameter);
            Assert.True(request.Headers.TryGetValues("Idempotency-Key", out var values));
            Assert.Contains("doc-001", values!);

            return Task.FromResult(Json(HttpStatusCode.OK, """{"data":{"id":"doc_1"}}""", "req_dotnet"));
        });

        var httpClient = new HttpClient(handler);
        var client = new FiscaloHttpClient("fisc_test_dotnet", "https://api.fiscalo.pt/api/v1", TimeSpan.FromSeconds(30), httpClient);

        var response = await client.SendEnvelopeAsync<Document>(HttpMethod.Post, "documents", new { }, "doc-001");

        Assert.Equal("req_dotnet", response.RequestId);
    }

    [Fact]
    public Task UsesNormalizedBaseUrlOnHttpClient()
    {
        var httpClient = new HttpClient(new StubHttpMessageHandler((_, _) =>
            Task.FromResult(Json(HttpStatusCode.OK, """{"data":{"id":"doc_1"}}"""))));

        var client = new FiscaloHttpClient("fisc_test_dotnet", "https://api.fiscalo.pt/api/v1/", TimeSpan.FromSeconds(30), httpClient);

        Assert.Equal("https://api.fiscalo.pt/api/v1", client.BaseUrl);

        return Task.CompletedTask;
    }

    [Fact]
    public async Task ThrowsValidationExceptionFromJsonError()
    {
        var handler = new StubHttpMessageHandler((_, _) =>
            Task.FromResult(Json(HttpStatusCode.UnprocessableEntity, """{"error":{"code":"validation_error","message":"Os dados enviados são inválidos.","details":{"customer_id":["Obrigatório."]},"fields":{"customer_id":[{"code":"required","message":"Obrigatório.","suggestion":"Preencha `customer_id`."}]}},"meta":{"request_id":"req_validation"}}""")));

        var httpClient = new HttpClient(handler);
        var client = new FiscaloHttpClient("fisc_test_dotnet", "https://api.fiscalo.pt/api/v1", TimeSpan.FromSeconds(30), httpClient);

        var exception = await Assert.ThrowsAsync<FiscaloValidationException>(() =>
            client.SendEnvelopeAsync<Document>(HttpMethod.Post, "documents", new { }));

        Assert.Equal("validation_error", exception.Code);
        Assert.Equal("req_validation", exception.RequestId);
        Assert.NotNull(exception.FieldErrors);
        Assert.Equal("required", exception.FieldErrors?["customer_id"][0].Code);
    }

    [Fact]
    public async Task ThrowsAuthenticationExceptionFromJsonError()
    {
        var handler = new StubHttpMessageHandler((_, _) =>
            Task.FromResult(Json(HttpStatusCode.Unauthorized, """{"error":{"code":"invalid_api_key","message":"API key inválida.","suggestion":"Revise a API key utilizada."},"meta":{"request_id":"req_auth"}}""")));

        var client = new FiscaloHttpClient("fisc_test_dotnet", "https://api.fiscalo.pt/api/v1", TimeSpan.FromSeconds(30), new HttpClient(handler));

        var exception = await Assert.ThrowsAsync<FiscaloAuthenticationException>(() =>
            client.SendEnvelopeAsync<Document>(HttpMethod.Get, "documents/doc_1"));

        Assert.Equal("invalid_api_key", exception.Code);
        Assert.Equal("req_auth", exception.RequestId);
    }

    private static HttpResponseMessage Json(HttpStatusCode statusCode, string json, string? requestId = null)
    {
        var response = new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json"),
        };

        if (!string.IsNullOrWhiteSpace(requestId))
        {
            response.Headers.Add("x-request-id", requestId);
        }

        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        return response;
    }
}

internal sealed class StubHttpMessageHandler : HttpMessageHandler
{
    private readonly Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> callback;

    public StubHttpMessageHandler(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> callback)
    {
        this.callback = callback;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        => callback(request, cancellationToken);
}
