using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using SkymeyLib.Models.Users.Login;
using SkymeyLib.Enums.Users;
using SkymeyLib.Enums;
using Microsoft.AspNetCore.Http;

namespace SkymeyLib.Middleware
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var account = (LoginModel?)context.HttpContext.Items["User"];
            if (account == null)
            {
                // not logged in
                context.Result = new JsonResult(new { message = UserLogin.Unauthorized.StringValue() }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}
