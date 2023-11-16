using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using BooyahNetAPI.Models;
using BooyahNetAPI.Models;

namespace BooyahNetAPI.Helpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = (User)context.HttpContext.Items["Customer"];
            if (user == null)
            {
                // not logged in
                context.Result = new JsonResult(new { message = "Membutuhkan Izin" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}