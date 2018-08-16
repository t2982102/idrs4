using IdentityModel;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace idrs4.Configuration
{
    public class Resources
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new[]
            {
                // some standard scopes from the OIDC spec
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                // custom identity resource with some consolidated claims
                new IdentityResource("custom.profile", new[] { JwtClaimTypes.Name, JwtClaimTypes.Email,JwtClaimTypes.Role, "ClientId","permission" }),
               
            };
        }

        //public static IEnumerable<IdentityResource> GetIdentityResources()
        //{
        //    return new[]
        //    {
        //        new IdentityResources.OpenId(),
        //        new IdentityResources.Profile(),
        //        new IdentityResource("role","identitfyRole",new []{JwtClaimTypes.Role}),
        //        new IdentityResource("custom.profile", new[] { JwtClaimTypes.Name, JwtClaimTypes.Email,JwtClaimTypes.Role, "location" })
        //    };
        //}


        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new[]
            {
                new ApiResource
                {
                    Name="api1",
                    DisplayName="My Api1",

                    Scopes ={
                        new Scope
                        {
                            Name = "api1",
                            DisplayName = "API1 access",
                            Description = "My API",
                        }
                    } 

                },
                // simple version with ctor
                
                // expanded version if more control is needed
                new ApiResource
                {
                    Name = "api2",

                    ApiSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    UserClaims =
                    {
                        JwtClaimTypes.Name,
                        JwtClaimTypes.Email
                    },

                    Scopes =
                    {
                        new Scope
                        {
                            Name = "api2.full_access",
                            DisplayName = "Full access to API 2"
                        },
                        new Scope
                        {
                            Name = "api2.read_only",
                            DisplayName = "Read only access to API 2"
                        }
                    }
                }
            };
        }
    }
}
