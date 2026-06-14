using Fiscalo.Sdk.Models;
using Fiscalo.Sdk.Support;

namespace Fiscalo.Sdk.Resources;

public sealed class ItemsResource
{
    private readonly FiscaloHttpClient httpClient;

    public ItemsResource(FiscaloHttpClient httpClient) => this.httpClient = httpClient;

    public Task<FiscaloListResponse<Item>> ListAsync(CancellationToken cancellationToken = default) =>
        httpClient.SendCollectionAsync<Item>(HttpMethod.Get, "items", cancellationToken: cancellationToken);

    public Task<FiscaloResponse<Item>> CreateAsync(CreateItemRequest payload, CancellationToken cancellationToken = default) =>
        httpClient.SendEnvelopeAsync<Item>(HttpMethod.Post, "items", payload, cancellationToken: cancellationToken);

    public Task<FiscaloResponse<Item>> GetAsync(string id, CancellationToken cancellationToken = default) =>
        httpClient.SendEnvelopeAsync<Item>(HttpMethod.Get, $"items/{id}", cancellationToken: cancellationToken);

    public Task<FiscaloResponse<Item>> UpdateAsync(string id, UpdateItemRequest payload, CancellationToken cancellationToken = default) =>
        httpClient.SendEnvelopeAsync<Item>(HttpMethod.Patch, $"items/{id}", payload, cancellationToken: cancellationToken);
}
