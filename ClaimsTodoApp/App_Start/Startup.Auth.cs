using System;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.WsFederation;
using Owin;

namespace ClaimsTodoApp
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseExternalSignInCookie(WsFederationAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = WsFederationAuthenticationDefaults.AuthenticationType
            });

            app.UseWsFederationAuthentication(new WsFederationAuthenticationOptions
            {
                MetadataAddress = "https://adfs.tdc.hovland.name/federationmetadata/2007-06/federationmetadata.xml",
                Wtrealm = "https://tdc-webs.tdc.hovland.name/ClaimsTodoApp/",
            });
        }
    }
}
