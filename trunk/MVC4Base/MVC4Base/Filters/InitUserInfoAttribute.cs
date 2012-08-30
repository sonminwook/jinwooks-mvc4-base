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
        /// <summary>
        /// 로그인 확인후 로그인 페이지로 이동여부
        /// </summary>
        public string AuthorizeYN { get; set; }

        public override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
        {
            //01. 로그인정보를 확인하여 세션에 담는다.
            AuthManager.CheckLoginUser();

            //02. 로그를 남긴다.
            AuthManager.InsertVisitLog(
                filterContext.ActionDescriptor.ControllerDescriptor.ControllerName
              , filterContext.ActionDescriptor.ActionName);

            //03. 권한을 수집한다.
            AuthManager.CheckAuthority(filterContext.ActionDescriptor.ControllerDescriptor.ControllerName
              , filterContext.ActionDescriptor.ActionName);

            //익명로그인 속성이 없으면 로그인 체크후 로그인 페이지로 이동한다.
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