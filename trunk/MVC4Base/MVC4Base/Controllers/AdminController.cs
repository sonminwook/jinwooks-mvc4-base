using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC4Base.Filters;
using MVC4Base.Services;
using MVC4Base.Models;
using System.Data.Objects;
using Spring.Transaction.Interceptor;

namespace MVC4Base.Controllers
{
    public class AdminController : Controller
    {
        private IAuthService authService = null;
        private MVC4BaseEntities db = null;

        /// <summary>
        /// 관리자 메인페이지 /Admin/Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        #region 메인코드 관리

        /// <summary>
        /// 메인코드 관리 목록 /Admin/MainCodeList
        /// </summary>
        /// <returns></returns>
        [Transaction]
        public ActionResult MainCodeList(PagingModel pagingModel, 
            string titleYN ,
            string mainCode ,
            tbSYSCode sysCode)
        {
            ViewData.Add("pagingModel", pagingModel);
            ViewData.Add("titleYN", titleYN);
            ViewData.Add("mainCode", mainCode);
            ViewData.Add("sysCode", sysCode);

            if (ModelState.IsValid)
            {
                ObjectParameter oTotalCount = new ObjectParameter("TotalCount", typeof(Decimal));
                ViewData["mainCodeModel"] = db.fnSYSCodeList(
                    mainCode,
                    string.Empty, 
                    string.Empty, 
                    titleYN, 
                    pagingModel.PageIndex, 
                    pagingModel.PageSize, 
                    pagingModel.Order, 
                    oTotalCount).ToList<fnSYSCodeList_Result>();
                pagingModel.TotalCount = (Decimal)oTotalCount.Value;
            }
            return View();
        }

        /// <summary>
        /// 메인코드 관리 목록 /Admin/MainCode
        /// </summary>
        /// <returns></returns>
        public ActionResult MainCodeView()
        {
            return View();
        }

        /// <summary>
        /// 메인코드 관리 목록 /Admin/MainCode
        /// </summary>
        /// <returns></returns>
        public ActionResult MainCodeAction()
        {
            return View();
        }

        #endregion 메인코드 관리

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
                //회원 목록 세팅
            }
            else
            {
                ModelState.AddModelError("", "목록보기 권한이 없습니다.");
            }

            return View();
        }

        /// <summary>
        /// 회원 등록/수정 페이지 /Admin/MemberEdit
        /// </summary>
        /// <returns></returns>
        public ActionResult MemberEdit(MemeberModel model)
        {
            if (authService.UserInfomation.HasAuthority(UserAuthority.CREATE)
                || authService.UserInfomation.HasAuthority(UserAuthority.UPDATE))
            {
                // 회원 정보 세팅
            }
            else
            {
                ModelState.AddModelError("", "권한이 없습니다.");
            }

            return View();
        }


        /// <summary>
        /// 회원 Action 등록, 수정, 삭제 /Admin/MemberAction
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MemberAction(MemeberModel model)
        {
            if (ModelState.IsValid)
            {
                if (authService.UserInfomation.HasAuthority(UserAuthority.CREATE)
                || authService.UserInfomation.HasAuthority(UserAuthority.UPDATE))
                {
                    //처리에 성공했을 경우
                    if (true)
                    {
                        return RedirectToActionPermanent("Member");
                    }
                    //실패하였을 경우
                    ModelState.AddModelError("", "실패하였습니다.");
                }
                else
                {
                    ModelState.AddModelError("", "권한이 없습니다.");
                }
            }

            //목록으로 돌아가기
            return MemberEdit(model);
        }

        #endregion 회원 관리

    }
}
