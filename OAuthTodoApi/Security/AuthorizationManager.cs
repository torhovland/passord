using System.Diagnostics;
using System.Security.Claims;
using System.Linq;

namespace Thinktecture.Samples.Security
{
    public class AuthorizationManager : ClaimsAuthorizationManager
    {
        public override bool CheckAccess(AuthorizationContext context)
        {
            // inspect sub, action, resource
            if (context == null) return false;
            if (context.Principal == null) return false;
            if (context.Principal.FindFirst("sub") == null) return true;

            Debug.WriteLine(context.Principal.FindFirst("sub").Value);
            Debug.WriteLine(context.Action.First().Value);
            Debug.WriteLine(context.Resource.First().Value);

            return true;
        }
    }
}