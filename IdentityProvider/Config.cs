using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityModel;

namespace IdentityProvider
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
               new IdentityResource[]
               {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource("roles", "Your roles(s)", new []{ "role"})
               };

        public static IEnumerable<ApiResource> ApiResources => new ApiResource[]
        {
        new("projectelevatorapi", "Project - Elevator API", new []{"role"})
        {
            Scopes =
            {
                "projectelevatorapi.fullaccess",
                "projectelevatorapi.write",
                "projectelevatorapi.read"
            },
            ApiSecrets = {new Secret("2438a004-d396-4e1b-8ff4-30397a02e51d".ToSha256()) }
        }
        };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
            new ApiScope("projectelevatorapi.fullaccess"),
            new ApiScope("projectelevatorapi.write"),
            new ApiScope("projectelevatorapi.read")
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
            new Client
            {
                ClientName = "AdminWebApp Client",
                ClientId = "localadminwebappclient",
                AllowedGrantTypes = GrantTypes.Code,
                AllowOfflineAccess = true,
                UpdateAccessTokenClaimsOnRefresh = true,
                RedirectUris =
                {
                    "https://localhost:7196/signin-oidc"
                },
                PostLogoutRedirectUris =
                {
                    "https://localhost:7196/signout-callback-oidc"
                },

                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "roles",
                    "projectelevatorapi.read",
                    "projectelevatorapi.write",
                },
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },
                RequireConsent = false
            },
            new Client
            {

                ClientName = "AdminWebApp Client",
                ClientId = "adminwebappclient",
                AllowedGrantTypes = GrantTypes.Code,
                AllowOfflineAccess = true,
                UpdateAccessTokenClaimsOnRefresh = true,
                RedirectUris =
                {
                    "https://project-elevator.azurewebsites.net/signin-oidc",
                    "https://localhost:7196/signin-oidc"

                },
                PostLogoutRedirectUris =
                {
                    "https://project-elevator.azurewebsites.net/signout-callback-oidc",
                    "https://localhost:7196/signout-callback-oidc"
                },

                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "roles",
                    "projectelevatorapi.read",
                    "projectelevatorapi.write",
                },
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },
                RequireConsent = false
            },
            new Client
            {
                ClientName = "MobileApp Client",
                ClientId = "mobileappclient",
                AllowedGrantTypes = GrantTypes.Code,
                AllowOfflineAccess = true,
                UpdateAccessTokenClaimsOnRefresh = true,
                RedirectUris =
                {
                    "myapp://",
                    "myapp://signin-oidc"
                },
                PostLogoutRedirectUris =
                {
                    "myapp://",
                    "myapp://signout-callback-oidc"
                },
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "roles"
                },
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                }
            }

            };
    }
}