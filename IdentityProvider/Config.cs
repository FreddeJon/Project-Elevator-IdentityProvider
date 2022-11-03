using Duende.IdentityServer;
using Duende.IdentityServer.Models;

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
            }
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
                ClientId = "adminwebappclient",
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
                    "https://project-elevator.azurewebsites.net/signin-oidc"
                },
                PostLogoutRedirectUris =
                {
                    "https://project-elevator.azurewebsites.net/signout-callback-oidc"
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
            }
            };
    }
}