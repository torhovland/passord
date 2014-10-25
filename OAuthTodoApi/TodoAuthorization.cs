using System.Linq;
using System.Threading.Tasks;
using Thinktecture.IdentityModel.Owin.ResourceAuthorization;

namespace Thinktecture.Samples
{
    public class TodoAuthorization : ResourceAuthorizationManager
    {
        public override Task<bool> CheckAccessAsync(ResourceAuthorizationContext context)
        {
            if (!context.Principal.Identity.IsAuthenticated)
                return Nok();

            var resource = context.Resource.First().Value;

            if (resource == "Todo")
                return CheckTodoAccessAsync(context);

            return Nok();
        }

        Task<bool> CheckTodoAccessAsync(ResourceAuthorizationContext context)
        {
            var action = context.Action.First().Value;

            if (action == "Read")
                return CheckTodoReadAccessAsync(context);

            if (action == "Post")
                return CheckTodoPostAccessAsync(context);

            return Nok();
        }

        Task<bool> CheckTodoReadAccessAsync(ResourceAuthorizationContext context)
        {
            if (!context.Principal.Claims.Any(claim => claim.Type == "scope" && claim.Value == "read"))
                return Nok();

            return Ok();
        }

        Task<bool> CheckTodoPostAccessAsync(ResourceAuthorizationContext context)
        {
            if (!context.Principal.Claims.Any(claim => claim.Type == "scope" && claim.Value == "post"))
                return Nok();

            if (!context.Principal.IsInRole("TodoWriter"))
                return Nok();

            return Ok();
        }
    }
}