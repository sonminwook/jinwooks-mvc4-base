﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC4Base.Models;
using MVC4Base.Filters;

namespace MVC4Base.Controllers
{
    /// <summary>
    /// 로그인 관련 컨트롤러
    /// </summary>
    [InitUserInfo]
    public class MemberController : Controller
    {
        //
        // GET: /Member/Login

        /// <summary>
        /// 로그인 페이지 이동시
        /// </summary>
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
        public ActionResult LogOff(string returnUrl)
        {
            AuthManager.Logout();

            return RedirectToLocal(returnUrl);
        }


        /// <summary>
        /// 페이지 이동 함수(로그인 페이지와 도메인이 같아야 이동)
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl) && returnUrl.Contains(Request.Url.Authority))
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
