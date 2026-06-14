using Fiscalo.Sdk.Abstractions;
using Fiscalo.Sdk;
using Fiscalo.Sdk.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fiscalo.Sdk.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFiscalo(
        this IServiceCollection services,
        Action<FiscaloOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configure);

        var options = new FiscaloOptions();
        configure(options);
        options.Validate();

        services.AddSingleton(options);
        services.AddHttpClient<IFiscaloClient, FiscaloClient>((serviceProvider, httpClient) =>
        {
            var currentOptions = serviceProvider.GetRequiredService<FiscaloOptions>();

            httpClient.BaseAddress = new Uri(currentOptions.BaseUrl.TrimEnd('/') + "/");
            httpClient.Timeout = currentOptions.Timeout;
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Fiscalo.Sdk/0.1.0");
        });

        return services;
    }
}
