using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;
using Bijector.Infrastructure.Discovery;
using Microsoft.Extensions.Configuration;

namespace Bijector.Accounts
{
    public static class Configs
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),                
                new IdentityResources.Profile()
            };
        }


        public static IEnumerable<ApiResource> GetApiResources() => new List<ApiResource>
        {
            new ApiResource("api.v1", "bijector-api"),
            new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
        };

        public static IEnumerable<Client> GetClients(IConfiguration configuration) => new List<Client>
        {                                
            new Client
            {
                ClientId = "bijector.ng-front",
                AllowedGrantTypes = GrantTypes.Implicit,
                AllowAccessTokensViaBrowser = true,
                RequireConsent = false,

                AlwaysIncludeUserClaimsInIdToken = true,
                AlwaysSendClientClaims = true,
 
                RedirectUris = configuration.GetSection("RedirectUris").Get<string[]>(),
                //PostLogoutRedirectUris = new string[]{"https://localhost:"},
                //AllowedCorsOrigins = new string[]{},

                AllowedScopes = {
                    IdentityServerConstants.LocalApi.ScopeName,
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,                    
                    "api.v1"
                }
            }
        };
    }
}