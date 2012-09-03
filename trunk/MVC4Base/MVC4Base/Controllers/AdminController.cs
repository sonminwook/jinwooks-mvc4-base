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

        /// <summary>
        /// 관리자 메인페이지 /Admin/Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        #region 회원 관리

        /// <summary>
        /// 회원 목록 페이지 /Admin/MemeberList
        /// </summary>
        /// <returns></returns>
        public ActionResult MemberList()
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

        /// <summary>
        /// 회원 등록 페이지 /Admin/MemberCreate
        /// </summary>
        /// <returns></returns>
        public ActionResult MemberCreate()
        {
            //목록보기 권한이 있을때
            if (authService.UserInfomation.HasAuthority(UserAuthority.CREATE))
            {
                ViewBag.Message = "목록보기 권한이 있어요.";
            }
            else
            {
                ViewBag.Message = "목록보기 권한이 없어요.";
            }

            return View();
        }


        /// <summary>
        /// 회원 Action 등록, 수정, 삭제 /Admin/MemberAction
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MemberAction()
        {
            if (ModelState.IsValid)
            {
                //처리에 성공했을 경우
                if (true)
                {
                    return RedirectToActionPermanent("Member");
                }
                //실패하였을 경우
                ModelState.AddModelError("", "실패하였습니다.");
            }

            //목록으로 돌아가기
            return MemberAction();
        }

        #endregion 회원 관리

    }
}
