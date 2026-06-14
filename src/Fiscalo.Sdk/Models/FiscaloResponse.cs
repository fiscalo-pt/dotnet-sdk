namespace Fiscalo.Sdk.Models;

public class FiscaloResponse<T>
{
    public FiscaloResponse(
        int statusCode,
        IReadOnlyDictionary<string, string> headers,
        string rawBody,
        T? data = default,
        ResponseMeta? meta = null,
        ApiLinks? links = null)
    {
        StatusCode = statusCode;
        Headers = headers;
        RawBody = rawBody;
        Data = data;
        Meta = meta;
        Links = links;
        RequestId = headers.TryGetValue("x-request-id", out var requestId) ? requestId : meta?.RequestId;
    }

    public int StatusCode { get; }

    public IReadOnlyDictionary<string, string> Headers { get; }

    public string RawBody { get; }

    public string? RequestId { get; }

    public T? Data { get; }

    public ResponseMeta? Meta { get; }

    public ApiLinks? Links { get; }
}

public sealed class FiscaloListResponse<T> : FiscaloResponse<IReadOnlyList<T>>
{
    public FiscaloListResponse(
        int statusCode,
        IReadOnlyDictionary<string, string> headers,
        string rawBody,
        IReadOnlyList<T>? data = null,
        ResponseMeta? meta = null,
        ApiLinks? links = null)
        : base(statusCode, headers, rawBody, data ?? Array.Empty<T>(), meta, links)
    {
    }

    public IReadOnlyList<T> Items => Data ?? Array.Empty<T>();
}
