

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Eval360.Security;


[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class CustomAuthorization : Attribute, IAuthorizationFilter
{
    private string[] allowedRoles;
    public string? Roles { get; set; }

    public CustomAuthorization()
    {
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {

        var currentUser = context.HttpContext.User; 

        bool roleFlag = false;


        allowedRoles = Roles != null ? Roles.Split(",") : null;

        //check if the user have the role to access the 
        foreach (var _role in allowedRoles)
        {
            if (currentUser.IsInRole(_role))
            {
                roleFlag = true;
            }
        }
        //if he does have a role then give him access
        if (roleFlag)
        {
            return;
        }

        //if he is not allowed to access this page he will be redirected based in his role
        if (currentUser.IsInRole("Admin"))
        {
            context.Result = new RedirectToRouteResult(new
                RouteValueDictionary(new { controller = "Home", action = "index" }));
        }else if (currentUser.IsInRole("Employee"))
        {
            context.Result = new RedirectToActionResult("index", "Employee",null);
        }
        else if (currentUser.IsInRole("Gestionnaire"))
        {
            context.Result = new RedirectToRouteResult(new
                RouteValueDictionary(new { controller = "Gestionnaire", action = "index" }));
        }
        else
        {
            context.Result = new RedirectToRouteResult(new
                RouteValueDictionary(new { controller = "Account", action = "Login" }));
        }


    }
}