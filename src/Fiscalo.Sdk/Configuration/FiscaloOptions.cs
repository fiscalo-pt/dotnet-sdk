namespace Fiscalo.Sdk.Configuration;

public sealed class FiscaloOptions
{
    public string ApiKey { get; set; } = string.Empty;

    public string BaseUrl { get; set; } = FiscaloClient.DefaultBaseUrl;

    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(30);

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(ApiKey))
        {
            throw new InvalidOperationException("Fiscalo ApiKey must be configured.");
        }

        if (string.IsNullOrWhiteSpace(BaseUrl))
        {
            throw new InvalidOperationException("Fiscalo BaseUrl must be configured.");
        }
    }
}
