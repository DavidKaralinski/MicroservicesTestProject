using Duende.IdentityServer.Models;

namespace IdentityService;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("auctionService", "Auction service full access"),
        };

    public static IEnumerable<Client> Clients(IConfiguration configuration) =>
        new Client[]
        {
            new() 
            {
                ClientId = "postman",
                ClientName = "postman",
                ClientSecrets = new [] { new Secret("Secret".Sha256()) },
                AllowedScopes = new [] { "openid", "profile", "auctionService" },
                AllowedGrantTypes = new [] { GrantType.ResourceOwnerPassword },
                RedirectUris = new [] { "https://example.com" }
            },
            new()
            {
                ClientId = "nextApp",
                ClientName = "nextApp",
                ClientSecrets = new [] { new Secret("Secret".Sha256()) },
                AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                RequirePkce = false,
                RedirectUris = new [] { $"{configuration.GetValue<string>("ClientAppUrl")}/api/auth/callback/id-server" },
                AllowOfflineAccess = true,
                AllowedScopes = new [] { "openid", "profile", "auctionService" },
                AccessTokenLifetime = 3600*24,
                AlwaysIncludeUserClaimsInIdToken = true
            }
        };
}
