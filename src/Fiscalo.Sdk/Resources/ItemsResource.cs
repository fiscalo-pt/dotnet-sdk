using Fiscalo.Sdk.Models;
using Fiscalo.Sdk.Support;

namespace Fiscalo.Sdk.Resources;

/// <summary>
/// Provides access to Fiscalo item endpoints.
/// </summary>
public sealed class ItemsResource
{
    private readonly FiscaloHttpClient httpClient;

    /// <summary>
    /// Creates an item resource wrapper over the shared Fiscalo transport.
    /// </summary>
    public ItemsResource(FiscaloHttpClient httpClient) => this.httpClient = httpClient;

    /// <summary>
    /// Lists items available in the current Fiscalo environment.
    /// </summary>
    public Task<FiscaloListResponse<Item>> ListAsync(CancellationToken cancellationToken = default) =>
        httpClient.SendCollectionAsync<Item>(HttpMethod.Get, "items", cancellationToken: cancellationToken);

    /// <summary>
    /// Creates a new item in Fiscalo.
    /// </summary>
    public Task<FiscaloResponse<Item>> CreateAsync(CreateItemRequest payload, CancellationToken cancellationToken = default) =>
        httpClient.SendEnvelopeAsync<Item>(HttpMethod.Post, "items", payload, cancellationToken: cancellationToken);

    /// <summary>
    /// Retrieves a single item by identifier.
    /// </summary>
    public Task<FiscaloResponse<Item>> GetAsync(string id, CancellationToken cancellationToken = default) =>
        httpClient.SendEnvelopeAsync<Item>(HttpMethod.Get, $"items/{id}", cancellationToken: cancellationToken);

    /// <summary>
    /// Applies a partial update to an existing item.
    /// </summary>
    public Task<FiscaloResponse<Item>> UpdateAsync(string id, UpdateItemRequest payload, CancellationToken cancellationToken = default) =>
        httpClient.SendEnvelopeAsync<Item>(HttpMethod.Patch, $"items/{id}", payload, cancellationToken: cancellationToken);
}
