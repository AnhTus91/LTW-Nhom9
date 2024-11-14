using System;
using System.Web;
using System.Web.Mvc;

namespace DAWeb.Filters
{
    public class RoleAuthorizeAttribute : AuthorizeAttribute
    {
        public string AllowedRole { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var userRole = httpContext.Session["userRole"]?.ToString();
            return !string.IsNullOrEmpty(userRole) && userRole == AllowedRole;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("/User/Login");
        }
    }
}
