using Fiscalo.Sdk.Models;
using Fiscalo.Sdk.Support;

namespace Fiscalo.Sdk.Resources;

public sealed class CustomersResource
{
    private readonly FiscaloHttpClient httpClient;

    public CustomersResource(FiscaloHttpClient httpClient) => this.httpClient = httpClient;

    public Task<FiscaloListResponse<Customer>> ListAsync(CancellationToken cancellationToken = default) =>
        httpClient.SendCollectionAsync<Customer>(HttpMethod.Get, "customers", cancellationToken: cancellationToken);

    public Task<FiscaloResponse<Customer>> CreateAsync(CreateCustomerRequest payload, CancellationToken cancellationToken = default) =>
        httpClient.SendEnvelopeAsync<Customer>(HttpMethod.Post, "customers", payload, cancellationToken: cancellationToken);

    public Task<FiscaloResponse<Customer>> GetAsync(string id, CancellationToken cancellationToken = default) =>
        httpClient.SendEnvelopeAsync<Customer>(HttpMethod.Get, $"customers/{id}", cancellationToken: cancellationToken);

    public Task<FiscaloResponse<Customer>> UpdateAsync(string id, UpdateCustomerRequest payload, CancellationToken cancellationToken = default) =>
        httpClient.SendEnvelopeAsync<Customer>(HttpMethod.Patch, $"customers/{id}", payload, cancellationToken: cancellationToken);
}
