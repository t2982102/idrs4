using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using IdentityServer4.AccessTokenValidation;
[assembly: OwinStartup(typeof(api.Startup))]

namespace api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {

                Authority = "http://localhost:5000",
                //ClientId = "Api1",
                RequiredScopes = new[] { "api1" }
            });
            // 有关如何配置应用程序的详细信息，请访问 https://go.microsoft.com/fwlink/?LinkID=316888
        }
    }
}
