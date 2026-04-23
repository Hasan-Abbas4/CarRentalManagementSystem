using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApplication2.Controllers
{
    public class BaseController : Controller
    {

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Force cache to expire and not be stored
            filterContext.HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            filterContext.HttpContext.Response.Cache.SetValidUntilExpires(false);
            filterContext.HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            filterContext.HttpContext.Response.Cache.SetNoStore();

            // Redirect if session is missing (except for Login/Register actions)
            var userId = filterContext.HttpContext.Session["UserId"];
            var controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            var action = filterContext.ActionDescriptor.ActionName;

            bool isPublic = controller.Equals("User", StringComparison.OrdinalIgnoreCase) &&
                           (action.Equals("Login", StringComparison.OrdinalIgnoreCase) ||
                            action.Equals("Register", StringComparison.OrdinalIgnoreCase));

            if (userId == null && !isPublic)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary {
                { "controller", "User" },
                { "action", "Login" }
                    });
            }

            base.OnActionExecuting(filterContext);
        }
    }
}