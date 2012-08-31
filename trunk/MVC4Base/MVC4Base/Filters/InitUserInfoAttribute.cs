using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using MVC4Base.Models;
using System.Web.Routing;
using System.Web.Mvc;
using System.Text;

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

            //04. DebugWindow 정보
            if (HttpContext.Current.Request.IsLocal && !string.IsNullOrEmpty(AuthManager.UserInfomation.UserID))
            {
                StringBuilder debug = new StringBuilder();
                debug.AppendLine("<b>[로그인 정보]</b><br />");
                debug.AppendFormat(" 1) 사용자ID : {0}<br />", AuthManager.UserInfomation.UserID);
                debug.AppendFormat(" 2) 사용자이름 : {0}<br />", AuthManager.UserInfomation.UserName);
                debug.AppendFormat(" 3) 로그인한 시간 : {0}<br />", AuthManager.UserInfomation.LoginTime);
                debug.AppendFormat(" 4) 현재시간 : {0}<br />", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                debug.AppendFormat(" 5) 쿠키 타임아웃 예정시간 : {0}<br />", AuthManager.UserInfomation.CookieTimeout == "0001-01-01 오전 12:00:00" ? "만료됨" : AuthManager.UserInfomation.CookieTimeout);
                debug.AppendFormat(" 6) 세션 타임아웃 예정시간 : {0}<br />", DateTime.Now.AddMinutes(HttpContext.Current.Session.Timeout).ToString("yyyy-MM-dd HH:mm:ss"));
                
                DebugWindowManager.Write(debug);
            }

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