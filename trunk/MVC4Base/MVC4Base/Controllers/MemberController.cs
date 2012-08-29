using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC4Base.Models;
using MVC4Base.Filters;

namespace MVC4Base.Controllers
{
    [Authorize]
    [InitUserInfo]
    public class MemberController : Controller
    {
        //
        // GET: /Member/Login

        /// <summary>
        /// 로그인 페이지 이동시
        /// </summary>
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Member/Login
        /// <summary>
        /// 로그인 페이지에서 로그인 클릭시
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            string processCode = string.Empty;
            if (AuthManager.Login(model.UserID, model.Password, model.RememberMe, out processCode))
            {
                return RedirectToLocal(returnUrl);
            }

            ModelState.AddModelError("", "아이디 또는 비밀번호가 일치하지 않습니다.");
            return View(model);
        }

        //
        // POST: /Account/LogOff
        /// <summary>
        /// 로그아웃 클릭시
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthManager.Logout();
            return RedirectToAction("Index", "Home");
        }



        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
