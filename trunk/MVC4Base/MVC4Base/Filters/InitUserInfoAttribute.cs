using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using MVC4Base.Models;
using System.Web.Routing;
using System.Web.Mvc;
using System.Text;
using MVC4Base.Services;

namespace MVC4Base.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class InitUserInfoAttribute : System.Web.Mvc.ActionFilterAttribute
    {
        private IAuthService authService = null;
        private IDebugWindowService debugWindowService = null;

        public override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
        {
            //01. 로그인정보를 확인하여 세션에 담는다.
            authService.CheckLoginUser();

            //02. 로그를 남긴다.
            authService.InsertVisitLog(
                filterContext.ActionDescriptor.ControllerDescriptor.ControllerName
              , filterContext.ActionDescriptor.ActionName);

            //03. 권한을 수집한다.
            authService.CheckAuthority(filterContext.ActionDescriptor.ControllerDescriptor.ControllerName
              , filterContext.ActionDescriptor.ActionName);

            //04. DebugWindow 정보
            if (HttpContext.Current.Request.IsLocal && !string.IsNullOrEmpty(authService.UserInfomation.UserID))
            {
                StringBuilder debug = new StringBuilder();
                debug.AppendLine("<b>[로그인 정보]</b><br />");
                debug.AppendFormat(" 1) 사용자ID : {0}<br />", authService.UserInfomation.UserID);
                debug.AppendFormat(" 2) 사용자이름 : {0}<br />", authService.UserInfomation.UserName);
                debug.AppendFormat(" 3) 로그인한 시간 : {0}<br />", authService.UserInfomation.LoginTime);
                debug.AppendFormat(" 4) 현재시간 : {0}<br />", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                debug.AppendFormat(" 5) 쿠키 타임아웃 예정시간 : {0}<br />", authService.UserInfomation.CookieTimeout == "0001-01-01 오전 12:00:00" ? "만료됨" : authService.UserInfomation.CookieTimeout);
                debug.AppendFormat(" 6) 세션 타임아웃 예정시간 : {0}<br />", DateTime.Now.AddMinutes(HttpContext.Current.Session.Timeout).ToString("yyyy-MM-dd HH:mm:ss"));

                debug.AppendLine("<br /><b>[페이지 정보]</b><br />");
                debug.AppendFormat(" 1) 페이지ID : {0}_{1}<br />"
                    , filterContext.ActionDescriptor.ControllerDescriptor.ControllerName
                    , filterContext.ActionDescriptor.ActionName);
                debug.AppendFormat(" 2) 페이지 권한 : {0}<br />", authService.UserInfomation.MenuAuthList.Count == 0 ? "권한 없음" : string.Join(",", authService.UserInfomation.MenuAuthList));
                debugWindowService.Write(debug);
            }

            //익명로그인 속성이 없으면 로그인 체크후 로그인 페이지로 이동한다.
            int count = filterContext.ActionDescriptor.GetCustomAttributes(true)
                .Where(n => n.GetType().ToString() == "System.Web.Mvc.AllowAnonymousAttribute").Count();

            if (count == 0 && !authService.UserInfomation.IsLoginUser)
            {
                HttpContext.Current.Response.RedirectToRoute(
                    new { controller = "Member", action = "Login", returnUrl= filterContext.HttpContext.Request.Url.ToString() });
            }
        }
    }
}