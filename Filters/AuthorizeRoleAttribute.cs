using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication2.Filters
{
    public class AuthorizeRoleAttribute : ActionFilterAttribute
    {
        private readonly string[] allowedRoles;

        public AuthorizeRoleAttribute(params string[] roles)
        {
            this.allowedRoles = roles;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var role = HttpContext.Current.Session["UserRole"] as string;
            if (string.IsNullOrEmpty(role) || !allowedRoles.Contains(role))
            {
                filterContext.Result = new RedirectResult("~/User/Login");
            }
        }
    }
}