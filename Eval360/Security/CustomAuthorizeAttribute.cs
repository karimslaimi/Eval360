

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Eval360.Security;
public class CustomAuthorizeAttribute : ActionFilterAttribute
{
    public string Roles { get; set; }

    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        //if user isn't logged in.
        if (filterContext.HttpContext.User.Identity == null || !filterContext.HttpContext.User.Identity.IsAuthenticated)
        {
            filterContext.Result = new RedirectResult("/Unauthorized/");
        }

        var user = filterContext.HttpContext.User;
        //Check user rights here
        if (!user.IsInRole(Roles))
        {
            filterContext.HttpContext.Response.StatusCode = 403;
            filterContext.Result = new RedirectResult("/Unauthorized/");
        }
    }
}

