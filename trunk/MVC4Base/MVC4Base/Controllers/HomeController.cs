using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC4Base.Filters;

namespace MVC4Base.Controllers
{
    //로그인 정보담기, AuthorizeYN="Y"이고 로그인이 안되어 있으면 로그인페이지로 이동
    //단 AllowAnonymous 속성을 지정하면 로그인 페이지로 이동하지 않는다.
    [InitUserInfo(AuthorizeYN = "Y")]
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
