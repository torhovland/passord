using System.Security.Claims;
using Microsoft.Owin;
using Owin;
using System.IdentityModel.Tokens;
using Thinktecture.IdentityModel;
using Thinktecture.IdentityModel.Tokens;
using Thinktecture.Samples.Security;

[assembly: OwinStartup(typeof(Thinktecture.Samples.Startup))]

namespace Thinktecture.Samples
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // authorization manager
            ClaimsAuthorization.CustomAuthorizationManager = new AuthorizationManager();

            // no mapping of incoming claims to Microsoft types
            JwtSecurityTokenHandler.InboundClaimTypeMap = ClaimMappings.None;

            // validate JWT tokens from AuthorizationServer
            app.UseJsonWebToken(
                issuer: "AS",
                audience: "todoApi",
                signingKey: "1fTiS2clmPTUlNcpwYzd5i4AEFJ2DEsd8TcUsllmaKQ=");

            app.UseResourceAuthorization(new TodoAuthorization());

            app.UseWebApi(WebApiConfig.Configure());
        }
    }
}