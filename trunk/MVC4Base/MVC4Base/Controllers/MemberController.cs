using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC4Base.Models;

namespace MVC4Base.Controllers
{
    [Authorize]
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
            AuthManager manager = new AuthManager();
            string processCode = string.Empty;
            if (manager.Login(model.UserID, model.Password, model.RememberMe, out processCode))
            {
                return RedirectToLocal(returnUrl);
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "아이디 또는 비밀번호가 일치하지 않습니다.");
            return View(model);
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
