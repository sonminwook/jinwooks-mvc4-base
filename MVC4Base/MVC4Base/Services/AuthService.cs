using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections;
using System.Configuration;
using System.Web.Security;
using System.Web.UI;
using MVC4Base.Models;
using System.Data.Objects;

namespace MVC4Base.Services
{
    public class AuthService : MVC4Base.Services.IAuthService
    {
        private MVC4BaseEntities db = null;

        /// <summary>
        /// 세션에 있는 로그인 정보를 반환한다.(변경불가!!)
        /// </summary>
        public UserInfo UserInfomation 
        { 
            get 
            {
                if(HttpContext.Current.Session["AuthServiceLoginInfo"] != null){
                    return (UserInfo)System.Web.HttpContext.Current.Session["AuthServiceLoginInfo"];
                 }else{
                    return new UserInfo();
                 }
            }
        }

        /// <summary>
        /// 로그인 정보를 변경합니다.
        /// </summary>
        /// <param name="userInfo"></param>
        private void ChangeUserInfomation(UserInfo userInfo)
        {
            /// 로그인 정보를 변경하려면 UserInfo 전체를 다시 할당해야합니다.
            HttpContext.Current.Session["AuthServiceLoginInfo"] = userInfo;
        }

        #region 로그인 관련 부분

        /// <summary>
        /// 사용자 플랫폼 가져오기
        /// </summary>
        /// <returns></returns>
        public string GetUserPlatform()
        {
            string strPlatform = string.Empty;

            if (HttpContext.Current.Request.UserAgent.ToString().IndexOf("Android") > 0)
            {
                strPlatform = "Android";
            }
            else if (HttpContext.Current.Request.UserAgent.ToString().IndexOf("iPhone") > 0)
            {
                strPlatform = "iPhone";
            }
            else if (HttpContext.Current.Request.UserAgent.ToString().IndexOf("iPad") > 0)
            {
                strPlatform = "iPad";
            }
            else
            {
                strPlatform = HttpContext.Current.Request.Browser.Platform.ToString();
            }

            return strPlatform;
        }

        /// <summary>
        /// 사용자 로그인
        /// </summary>
        /// <param name="page">페이지객체</param>
        /// <param name="userID">사용자ID</param>
        /// <param name="password">비밀번호</param>
        /// <param name="isSaveID">아이디 저장여부</param>
        /// <param name="processCode">결과 메시지</param>
        /// <returns></returns>
        public bool Login(string userID, string password, bool isSaveID, out string processCode)
        {
            processCode = string.Empty;
            HttpRequest request = System.Web.HttpContext.Current.Request;
            HttpRequest response = System.Web.HttpContext.Current.Request;

            List<fnSYSUserGetInfo_Result> data = null;
            try
            {
                ObjectParameter oProcessCode = new ObjectParameter("ProcessCode", typeof(String));
                data = db.fnSYSUserGetInfo(userID, password,
                                             request.UserHostAddress,
                                             request.UserAgent.ToString(),
                                             request.Browser.Id.ToString(),
                                             request.Browser.Version.ToString(),
                                             GetUserPlatform(),
                                             request.Browser.Cookies.ToString(),
                                             request.Browser.ActiveXControls.ToString(),
                                             oProcessCode).ToList<fnSYSUserGetInfo_Result>();

                if (string.IsNullOrEmpty(oProcessCode.Value.ToString()))
                {
                    MakeLoginInfo(data); // 사용자 정보 생성 (쿠키&세션)

                    //아이디 기억여부
                    if (isSaveID)
                    {
                        if (!response.Cookies.AllKeys.Contains("EpSaveLoginID"))
                        {
                            response.Cookies.Add(new HttpCookie("EpSaveLoginID"));
                        }
                        response.Cookies["EpSaveLoginID"].Value = userID;
                        response.Cookies["EpSaveLoginID"].Expires = System.DateTime.Now.AddMonths(3); //3개월간 저장
                    }
                    else
                    {
                        if (!response.Cookies.AllKeys.Contains("EpSaveLoginID"))
                        {
                            response.Cookies.Add(new HttpCookie("EpSaveLoginID"));
                        }
                        response.Cookies["EpSaveLoginID"].Value = null;
                        response.Cookies["EpSaveLoginID"].Expires = System.DateTime.Now.AddDays(-1);
                    }

                    // 로그인 성공
                    return true;
                }
                else
                {   // 로그인 실패일 경우
                    processCode = oProcessCode.Value.ToString();
                    return false;
                }
            }
            finally
            {
            }

        }

        /// <summary>
        /// 로그아웃
        /// </summary>
        public void Logout()
        {
            // 쿠키 삭제
            if (!System.Web.HttpContext.Current.Request.IsLocal)
            {
                System.Web.HttpContext.Current.Response.Cookies["AuthServiceLoginInfo"].Domain = ConfigurationManager.AppSettings["CookieDomain"];  // Web.Config에 있는 도메인
            }

            System.Web.HttpContext.Current.Response.Cookies["AuthServiceLoginInfo"].Value = null;
            System.Web.HttpContext.Current.Response.Cookies["AuthServiceLoginInfo"].Values.Clear();
            System.Web.HttpContext.Current.Response.Cookies["AuthServiceLoginInfo"].Expires = System.DateTime.Now.AddDays(-1);


            // 세션삭제
            System.Web.HttpContext.Current.Session["AuthServiceLoginInfo"] = null; // 사용자정보 세션삭제
            System.Web.HttpContext.Current.Session.RemoveAll(); // 세션 클리어
        }

        /// <summary>
        /// 로그인 체크
        /// </summary>
        /// <param name="pageRole"></param>
        public void CheckLoginUser()
        {
            // 1.1 쿠키가 있는지 체크한다.
            if (System.Web.HttpContext.Current.Request.Cookies["AuthServiceLoginInfo"] != null)
            {
                // 쿠키의 내용이 없는 경우 로그아웃
                if (string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Cookies["AuthServiceLoginInfo"].Value))
                {
                    Logout();
                    return;
                }

                // 1.2 쿠키값을 읽어 들인다.
                string strCookieValue = System.Web.HttpContext.Current.Request.Cookies["AuthServiceLoginInfo"].Value.Replace("*", "+");

                // 1.3 암호화 Class객체 생성 
                Neoplus.Framework.Common.CryptoManager oCryptoManager = new Neoplus.Framework.Common.CryptoManager();

                // 1.4 암호화 키를 현재의 IP로 지정
                oCryptoManager.Password = System.Web.HttpContext.Current.Request.UserHostAddress;

                // 1.5 쿠키값을 풀이한다.
                string[] strArrCookieValues = Neoplus.Framework.Common.StringManager.Split(oCryptoManager.Decrypt(strCookieValue).ToString(), "|");

                // 1.6 파싱한 IP와 동일한 IP인지 체크한다.
                if (strArrCookieValues[2].Equals(System.Web.HttpContext.Current.Request.UserHostAddress))
                {
                    // 세션에 들어 있는 Hash테이블을 파싱하여 사용자 정보를 프로퍼티에 바인딩한다.
                    if (System.Web.HttpContext.Current.Session["AuthServiceLoginInfo"] != null &&
                        !string.IsNullOrEmpty(UserInfomation.UserName))
                    {
                        if (!HttpContext.Current.User.Identity.IsAuthenticated)
                        {
                            FormsAuthentication.SetAuthCookie(UserInfomation.UserID, false);
                        }
                    }
                    else
                    {
                        string strProcessCode = string.Empty;
                        // 세션이 없으면 DB에서 다시 사용자 정보를 가져온다.
                        // 단, 로그인 정보 테이블에 있는 내용과 일치할 경우만 사용자 정보를 반환하고 아니면 아웃 시킨다.
                        ReSetUserInfo(strArrCookieValues[0], strArrCookieValues[1], strArrCookieValues[2], out  strProcessCode);
                        if (string.IsNullOrEmpty(strProcessCode))
                        {
                            if (!HttpContext.Current.User.Identity.IsAuthenticated)
                            {
                                FormsAuthentication.SetAuthCookie(UserInfomation.UserID, false);
                            }
                        }
                    }
                }
            }
            else if (UserInfomation.UserID == string.Empty)
            {
                Logout();
                return;
            }
        }

        /// <summary>
        /// 로그남기기
        /// </summary>
        public void InsertVisitLog(string controllerName, string actionName)
        {
            HttpRequest request = System.Web.HttpContext.Current.Request;
            HttpRequest response = System.Web.HttpContext.Current.Request;

            db.fnVisitLogInsert(string.Format("{0}_{1}",controllerName, actionName), 
                            request.UserHostAddress,
                            UserInfomation.UserID,
                            request.UserAgent.ToString(),
                            request.Browser.Id.ToString(),
                            request.Browser.Version.ToString(),
                            GetUserPlatform(),
                            request.Browser.Cookies.ToString(),
                            request.Browser.ActiveXControls.ToString());
        }

        /// <summary>
        /// 권한 가져오기
        /// </summary>
        public void CheckAuthority(string controllerName, string actionName)
        {
            var UserInfoTemp = UserInfomation;
            UserInfoTemp.CurrentMenuID = string.Format("{0}_{1}", controllerName, actionName);

            //로그인 한 사람만 권한 수집
            if (UserInfomation.IsLoginUser)
            {
                List<fnSYSUserGetAuthorityGroup_Result> oAuthorityGroup = null;
                List<fnSYSUserGetAuthority_Result> oAuthority = null;
                try
                {

                    oAuthorityGroup = db.fnSYSUserGetAuthorityGroup(UserInfomation.UserID, UserInfomation.CurrentMenuID).ToList<fnSYSUserGetAuthorityGroup_Result>();

                    if (oAuthorityGroup.Count() > 1)
                    {
                        
                        //권한그룹정보 수집
                        foreach (fnSYSUserGetAuthorityGroup_Result row in oAuthorityGroup)
                        {
                            UserInfoTemp.RoleList.Add(row.RoleID, row.RoleName);
                        }
                    }

                    oAuthority = db.fnSYSUserGetAuthority(UserInfomation.UserID, UserInfomation.CurrentMenuID).ToList<fnSYSUserGetAuthority_Result>();
                    {
                        //권한 수집
                        foreach (fnSYSUserGetAuthority_Result row in oAuthority)
                        {
                            UserInfoTemp.RoleList.Add(row.AuthorityID, row.AuthorityName);
                        }
                    }
                }
                finally
                {
                }
            }

            //사용자 변경된 내용 저장
            ChangeUserInfomation(UserInfoTemp);
        }


        #endregion 로그인 관련 부분

        #region == 로그인한 사용자의 정보 생성 인증로그인 쿠키 & 사용자정보 세션 ==

        /// <summary>
        /// 사용자 정보를 DB에서 읽어와서 다시 세션에 담는다.
        /// </summary>
        private void ReSetUserInfo(string userID, string loginTime, string userIP, out string processCode)
        {
            List<fnSYSUserGetInfo_Result> data = null;
            ObjectParameter oProcessCode = new ObjectParameter("ProcessCode", typeof(String));
            try
            {
                data = db.fnSYSUserReGetInfo(userID, loginTime, userIP, oProcessCode).ToList<fnSYSUserGetInfo_Result>();

                if (string.IsNullOrEmpty(oProcessCode.Value.ToString()))
                {
                    // DataSet으로 사용자 정보가 담긴 세션을 만든다.
                    MakeSession(data);
                }

                processCode = oProcessCode.Value.ToString();
            }
            finally
            {
            }

        }     /// <summary>

        /// DB에서 로그인정보가 정확하면 사용자 정보로 로그인 쿠키와 세션을 생성해 준다.
        /// </summary>
        /// <param name="ds"></param>
        private void MakeLoginInfo(List<fnSYSUserGetInfo_Result> data)
        {
            // 로그인 인증용 쿠키를 생성한다.
            MakeCookie(data);

            // 로그인 사용자 정보를 담고 있는 세션을 생성한다.
            MakeSession(data);
        }

        /// <summary>
        /// 로그인 인증용 쿠키를 만든다.
        /// </summary>
        /// <param name="ds"></param>
        private void MakeCookie(List<fnSYSUserGetInfo_Result> data)
        {
            fnSYSUserGetInfo_Result drw = data[0];

            #region == 로그인용 쿠키 ==

            // 1. 로그인용 쿠키를 생성한다.
            // 1.1 쿠키객체 생성
            HttpCookie oCookie = new HttpCookie("AuthServiceLoginInfo");

            // 쿠키 도메인 지정
            // 로컬일때는 도메인을 지정하지 않는다. (개발상태라고 본다.)
            if (!System.Web.HttpContext.Current.Request.IsLocal)
            {
                oCookie.Domain = ConfigurationManager.AppSettings["CookieDomain"];  // Web.Config에 있는 도메인
            }

            // 1.2 쿠키 값 입력
            string strCookieValue = drw.UserID + "|" + drw.LoginTime + "|" + drw.LoginIP;

            // 1.3 암호화 클래스 생성
            Neoplus.Framework.Common.CryptoManager oCryptoManager = new Neoplus.Framework.Common.CryptoManager();
            // 1.4 암호화 키를 로그인한 IP로 지정
            oCryptoManager.Password = drw.LoginIP;
            // 1.5 사용자ID, LoginTime, LoginIP의 문자열을 합쳐서 암호화 문자열 생성
            oCookie.Value = oCryptoManager.Encrypt(strCookieValue).Replace("+", "*");
            // 1.6 쿠키설정
            System.Web.HttpContext.Current.Response.Cookies.Add(oCookie);
            // 1.7 시간제한
            System.Web.HttpContext.Current.Response.Cookies["AuthServiceLoginInfo"].Expires = DateTime.Now.AddMinutes(Int32.Parse(ConfigurationManager.AppSettings["AuthServiceCookieTimeout"]));

            #endregion == 로그인용 쿠키 ==
        }

        /// <summary>
        /// 사용자 정보를 담고 있는 세션을 만든다.
        /// </summary>
        /// <param name="ds"></param>
        private void MakeSession(List<fnSYSUserGetInfo_Result> data)
        {

            fnSYSUserGetInfo_Result drw = data[0];

            #region == 사용자 정보 세션 생성 ==

            // 세션이 있으면 삭제한다.
            if (System.Web.HttpContext.Current.Session["AuthServiceLoginInfo"] != null)
                System.Web.HttpContext.Current.Session.Remove("AuthServiceLoginInfo");

            // 2. 세션에 담을 정보를 바인딩 한다.
            UserInfo Info = new UserInfo();

            Info.UserID = drw.UserID;
            Info.Nickname = drw.Nickname;
            Info.UserName = drw.UserName;
            Info.LoginTime = drw.LoginTime;
            Info.LoginIP = drw.LoginIP;
            Info.IsLoginUser = true;
            Info.CookieTimeout = System.Web.HttpContext.Current.Response.Cookies["AuthServiceLoginInfo"].Expires.ToString();
            // 2.3 세션에 사용자 정보 해쉬테이블을 담는다.
            System.Web.HttpContext.Current.Session["AuthServiceLoginInfo"] = Info;

            #endregion == 사용자 정보 세션 생성 ==
        }



        #endregion == 로그인한 사용자의 정보 생성 인증로그인 쿠키 & 사용자정보 세션 ==


    }
}