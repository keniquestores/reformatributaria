namespace ReformaTributaria.Domain.Common
{
    public sealed class AppSettings
    {
        public SecurityOptions Security { get; init; } = new SecurityOptions();
        public OAuthConfigOptions OAuthConfig { get; init; } = new OAuthConfigOptions();
        public AppConfigOptions AppConfig { get; init; } = new AppConfigOptions();

    }
    public sealed class SecurityOptions
    {
        public FileIntegrityCryptoOptions FileIntegrityCrypto { get; init; } = new FileIntegrityCryptoOptions();
    }

    public sealed class FileIntegrityCryptoOptions
    {
        public string Key { get; init; } = string.Empty;
    }

    public sealed class OAuthConfigOptions { 
    
        public string Uri { get; init; } = string.Empty;
        public string PathAuthorizeProduct { get; init; } = string.Empty;
    }

    public sealed class AppConfigOptions
    {
        public string PrivateKey { get; init; } = string.Empty;
        public string ApiKey { get; init; } = string.Empty;
    }
}
