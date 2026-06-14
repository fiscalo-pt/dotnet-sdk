using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Fiscalo.Sdk.Exceptions;
using Fiscalo.Sdk.Models;

namespace Fiscalo.Sdk.Support;

public sealed class FiscaloHttpClient
{
    private readonly HttpClient httpClient;
    private readonly JsonSerializerOptions serializerOptions;

    public FiscaloHttpClient(
        string apiKey,
        string baseUrl,
        TimeSpan timeout,
        HttpClient? httpClient = null,
        JsonSerializerOptions? serializerOptions = null)
    {
        ApiKey = apiKey;
        BaseUrl = baseUrl.TrimEnd('/');
        Timeout = timeout;
        this.serializerOptions = serializerOptions ?? new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            DictionaryKeyPolicy = JsonNamingPolicy.SnakeCaseLower,
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };

        if (httpClient is null)
        {
            this.httpClient = new HttpClient();
        }
        else
        {
            this.httpClient = httpClient;
        }

        this.httpClient.Timeout = timeout;
        this.httpClient.BaseAddress = new Uri(BaseUrl + "/");
    }

    public string ApiKey { get; }

    public string BaseUrl { get; }

    public TimeSpan Timeout { get; }

    public async Task<FiscaloResponse<TResponse>> SendEnvelopeAsync<TResponse>(
        HttpMethod method,
        string path,
        object? payload = null,
        string? idempotencyKey = null,
        string accept = "application/json",
        CancellationToken cancellationToken = default)
    {
        var transport = await SendTransportAsync(method, path, payload, idempotencyKey, accept, cancellationToken).ConfigureAwait(false);
        var envelope = Deserialize<ApiEnvelope<TResponse>>(transport.RawBody);
        var data = envelope is null ? default : envelope.Data;

        return new FiscaloResponse<TResponse>(
            transport.StatusCode,
            transport.Headers,
            transport.RawBody,
            data,
            envelope?.Meta,
            envelope?.Links);
    }

    public async Task<FiscaloListResponse<TResponse>> SendCollectionAsync<TResponse>(
        HttpMethod method,
        string path,
        object? payload = null,
        string? idempotencyKey = null,
        string accept = "application/json",
        CancellationToken cancellationToken = default)
    {
        var transport = await SendTransportAsync(method, path, payload, idempotencyKey, accept, cancellationToken).ConfigureAwait(false);
        var envelope = Deserialize<ApiCollectionEnvelope<TResponse>>(transport.RawBody);

        return new FiscaloListResponse<TResponse>(
            transport.StatusCode,
            transport.Headers,
            transport.RawBody,
            envelope?.Data ?? Array.Empty<TResponse>(),
            envelope?.Meta,
            envelope?.Links);
    }

    public async Task<FiscaloBinaryResponse> GetBinaryAsync(
        string path,
        CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, path.TrimStart('/'));
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ApiKey);
        request.Headers.Accept.Clear();
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/pdf"));

        using var response = await httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
        var bytes = await response.Content.ReadAsByteArrayAsync(cancellationToken).ConfigureAwait(false);
        var rawBody = Encoding.UTF8.GetString(bytes);
        var headers = ToHeaders(response);

        if (!response.IsSuccessStatusCode)
        {
            ThrowTypedException((int)response.StatusCode, rawBody, headers);
        }

        return new FiscaloBinaryResponse((int)response.StatusCode, headers, bytes);
    }

    private async Task<TransportResponse> SendTransportAsync(
        HttpMethod method,
        string path,
        object? payload,
        string? idempotencyKey,
        string accept,
        CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(method, path.TrimStart('/'));
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ApiKey);
        request.Headers.Accept.Clear();
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(accept));

        if (!string.IsNullOrWhiteSpace(idempotencyKey))
        {
            request.Headers.TryAddWithoutValidation("Idempotency-Key", idempotencyKey);
        }

        if (payload is not null)
        {
            var json = JsonSerializer.Serialize(payload, serializerOptions);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        using var response = await httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
        var rawBody = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        var headers = ToHeaders(response);

        if (!response.IsSuccessStatusCode)
        {
            ThrowTypedException((int) response.StatusCode, rawBody, headers);
        }

        return new TransportResponse((int) response.StatusCode, headers, rawBody);
    }

    private static Dictionary<string, string> ToHeaders(HttpResponseMessage response)
    {
        var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        foreach (var header in response.Headers)
        {
            headers[header.Key.ToLowerInvariant()] = string.Join(",", header.Value);
        }

        foreach (var header in response.Content.Headers)
        {
            headers[header.Key.ToLowerInvariant()] = string.Join(",", header.Value);
        }

        return headers;
    }

    private void ThrowTypedException(int statusCode, string rawBody, IReadOnlyDictionary<string, string> headers)
    {
        var envelope = Deserialize<ApiErrorEnvelope>(rawBody);
        var code = envelope?.Error?.Code ?? "api_error";
        var message = envelope?.Error?.Message ?? $"Fiscalo API request failed with status {statusCode}.";
        var suggestion = envelope?.Error?.Suggestion;
        var details = envelope?.Error?.Details;
        var fieldErrors = envelope?.Error?.Fields;
        var requestId = headers.TryGetValue("x-request-id", out var currentRequestId)
            ? currentRequestId
            : envelope?.Meta?.RequestId;

        throw statusCode switch
        {
            401 or 403 => new FiscaloAuthenticationException(message, code, statusCode, requestId, details, fieldErrors, suggestion),
            404 => new FiscaloNotFoundException(message, code, statusCode, requestId, details, fieldErrors, suggestion),
            409 => new FiscaloIdempotencyException(message, code, statusCode, requestId, details, fieldErrors, suggestion),
            422 => new FiscaloValidationException(message, code, statusCode, requestId, details, fieldErrors, suggestion),
            429 => new FiscaloRateLimitException(message, code, statusCode, requestId, headers.TryGetValue("retry-after", out var retryAfter) ? retryAfter : null, details, fieldErrors, suggestion),
            _ => new FiscaloApiException(message, code, statusCode, requestId, details, fieldErrors, suggestion),
        };
    }

    private TResponse? Deserialize<TResponse>(string rawBody)
    {
        if (string.IsNullOrWhiteSpace(rawBody))
        {
            return default;
        }

        try
        {
            return JsonSerializer.Deserialize<TResponse>(rawBody, serializerOptions);
        }
        catch
        {
            return default;
        }
    }

    private sealed record TransportResponse(
        int StatusCode,
        IReadOnlyDictionary<string, string> Headers,
        string RawBody);
}

public sealed class FiscaloBinaryResponse
{
    public FiscaloBinaryResponse(int statusCode, IReadOnlyDictionary<string, string> headers, byte[] buffer)
    {
        StatusCode = statusCode;
        Headers = headers;
        Buffer = buffer;
        RequestId = headers.TryGetValue("x-request-id", out var requestId) ? requestId : null;
    }

    public int StatusCode { get; }

    public IReadOnlyDictionary<string, string> Headers { get; }

    public byte[] Buffer { get; }

    public string? RequestId { get; }
}
