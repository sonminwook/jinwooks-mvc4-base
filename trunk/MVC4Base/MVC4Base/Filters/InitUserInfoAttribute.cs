using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using MVC4Base.Models;
using System.Web.Routing;
using System.Web.Mvc;

namespace MVC4Base.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class InitUserInfoAttribute : System.Web.Mvc.ActionFilterAttribute
    {
        public string AuthorizeYN { get; set; }

        public override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
        {
            AuthManager.CheckLoginUser();

            int count = filterContext.ActionDescriptor.GetCustomAttributes(true)
                .Where(n => n.GetType().ToString() == "System.Web.Mvc.AllowAnonymousAttribute").Count();
            
            if (count == 0 && AuthorizeYN == "Y" && !AuthManager.UserInfomation.IsLoginUser)
            {
                HttpContext.Current.Response.RedirectToRoute(
                    new { controller = "Member", action = "Login", returnUrl= filterContext.HttpContext.Request.Url.ToString() });
            }
        }
    }
}