using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC4Base.Filters;
using MVC4Base.Services;
using MVC4Base.Models;

namespace MVC4Base.Controllers
{
    public class AdminController : Controller
    {
        private IAuthService authService = null;

        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult Member()
        {
            //목록보기 권한이 있을때
            if (authService.UserInfomation.HasAuthority(UserAuthority.LIST))
            {
                ViewBag.Message = "목록보기 권한이 있어요.";
            }
            else
            {
                ViewBag.Message = "목록보기 권한이 없어요.";
            }

            return View();
        }

        public ActionResult MemberAction()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }
    }
}
