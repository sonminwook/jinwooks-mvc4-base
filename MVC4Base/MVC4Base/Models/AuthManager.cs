using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections;
using System.Configuration;
using System.Web.Security;
using System.Web.UI;

namespace MVC4Base.Models
{
    public class AuthManager
    {
        protected UserInfo userInfo = new UserInfo();

        #region 로그인 관련 부분

        /// <summary>
        /// 로그인이 안된 사람은 로그인 페이지로 이동
        /// </summary>
        public void CheckAndRedirectToLoginPage()
        {
            if (!userInfo.IsLoginUser)
            {
                FormsAuthentication.SignOut();
                FormsAuthentication.RedirectToLoginPage();
            }
        }

        /// <summary>
        /// 로그인이 안된 사람은 로그인 페이지로 이동
        /// </summary>
        /// <param name="extraQueryString">넘겨줄 파라미터</param>
        public void CheckAndRedirectToLoginPage(string extraQueryString)
        {
            if (!userInfo.IsLoginUser)
            {
                FormsAuthentication.SignOut();
                FormsAuthentication.RedirectToLoginPage(extraQueryString);
            }
        }

        /// <summary>
        /// 로그인이 안된 사람은 로그인 팝업 띄우기
        /// </summary>
        public void CheckAndLoginPopup(Page page)
        {
            if (!userInfo.IsLoginUser)
            {
                ScriptManager.RegisterClientScriptBlock(page, this.GetType(), "login", "Common.LoginPopup();", true);
            }
        }


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
        /// 사용자 로그인 후 페이지 이동
        /// </summary>
        /// <param name="page">페이지객체</param>
        /// <param name="userID">사용자ID</param>
        /// <param name="password">비밀번호</param>
        /// <param name="isSaveID">아이디 저장여부</param>
        /// <param name="message">결과 메시지</param>
        /// <returns></returns>
        public bool LoginAndRedirectFromLoginPage(string userID, string password, bool isSaveID, out string message)
        {
            bool result = Login(userID, password, isSaveID, out message);

            if (result)
            {
                FormsAuthentication.SignOut();

                string prevUrl = FormsAuthentication.GetRedirectUrl(HttpContext.Current.User.Identity.Name, false);

                if (prevUrl.Contains("Login.aspx"))
                {
                    //로그인 성공후 페이지로 이동한다.
                    FormsAuthentication.SetAuthCookie(userID, false);
                    HttpContext.Current.Response.RedirectPermanent(FormsAuthentication.DefaultUrl);
                }
                else
                {
                    //로그인 성공후 페이지로 이동한다.
                    FormsAuthentication.RedirectFromLoginPage(userID, false);
                }

            }


            return result;
        }

        /// <summary>
        /// 로그아웃 후 메인페이지로 이동
        /// </summary>
        public void LogoutAndRedirectToLoginPage()
        {
            Logout();
            // 쿠키와 세션을 삭제한후 메인 페이지로 이동한다.
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
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
            string strProcessCode = string.Empty;
            processCode = string.Empty;
            HttpRequest request = System.Web.HttpContext.Current.Request;
            HttpRequest response = System.Web.HttpContext.Current.Request;

            DataSet ds = null;
            try
            {
                ds = new DataSet();

                Dac.UserInfo oClass = new Dac.UserInfo();

                ds = oClass.GetUserInfo(userID, password,
                                             request.UserHostAddress,
                                             request.UserAgent.ToString(),
                                             request.Browser.Id.ToString(),
                                             request.Browser.Version.ToString(),
                                             GetUserPlatform(),
                                             request.Browser.Cookies.ToString(),
                                             request.Browser.ActiveXControls.ToString(),
                                             out strProcessCode);

                if (string.IsNullOrEmpty(strProcessCode))
                {
                    MakeLoginInfo(ds); // 사용자 정보 생성 (쿠키&세션)

                    if (isSaveID)
                    {
                        response.Cookies["EpSaveLoginID"].Value = userID;
                        response.Cookies["EpSaveLoginID"].Expires = System.DateTime.Now.AddMonths(3); //3개월간 저장
                    }
                    else
                    {
                        response.Cookies["EpSaveLoginID"].Value = null;
                        response.Cookies["EpSaveLoginID"].Expires = System.DateTime.Now.AddDays(-1);
                    }

                    // 로그인 성공
                    return true;
                }
                else
                {   // 로그인 실패일 경우
                    processCode = strProcessCode;
                    return false;
                }
            }
            finally
            {
                if (ds != null) ds.Dispose();
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
                System.Web.HttpContext.Current.Response.Cookies["AuthManagerLoginInfo"].Domain = ConfigurationManager.AppSettings["CookieDomain"];  // Web.Config에 있는 도메인
            }

            System.Web.HttpContext.Current.Response.Cookies["AuthManagerLoginInfo"].Value = null;
            System.Web.HttpContext.Current.Response.Cookies["AuthManagerLoginInfo"].Values.Clear();
            System.Web.HttpContext.Current.Response.Cookies["AuthManagerLoginInfo"].Expires = System.DateTime.Now.AddDays(-1);


            // 세션삭제
            System.Web.HttpContext.Current.Session["AuthManagerLoginInfo"] = null; // 사용자정보 세션삭제
            System.Web.HttpContext.Current.Session.RemoveAll(); // 세션 클리어
        }

        /// <summary>
        /// 로그인 체크
        /// </summary>
        /// <param name="pageRole"></param>
        public void CheckLoginUser()
        {
            userInfo.LoginIP = HttpContext.Current.Request.UserHostAddress;
            // 1.1 쿠키가 있는지 체크한다.
            if (System.Web.HttpContext.Current.Request.Cookies["AuthManagerLoginInfo"] != null)
            {
                // 1.2 쿠키값을 읽어 들인다.
                string strCookieValue = System.Web.HttpContext.Current.Request.Cookies["AuthManagerLoginInfo"].Value.Replace("*", "+");

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
                    if (System.Web.HttpContext.Current.Session["AuthManagerLoginInfo"] != null)
                    {
                        // 세션에서 사용자 정보를 바인딩한다.
                        SetUserInfo();
                        userInfo.IsLoginUser = true;
                        if (!HttpContext.Current.User.Identity.IsAuthenticated)
                        {
                            FormsAuthentication.SetAuthCookie(userInfo.UserID, false);
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
                            // 세션에 있는 사용자 정보를 바인딩한다.
                            SetUserInfo();
                            userInfo.IsLoginUser = true;
                            if (!HttpContext.Current.User.Identity.IsAuthenticated)
                            {
                                FormsAuthentication.SetAuthCookie(userInfo.UserID, false);
                            }
                        }
                    }
                }
            }
        }



        #endregion 로그인 관련 부분

        #region == 로그인한 사용자의 정보 생성 인증로그인 쿠키 & 사용자정보 세션 ==


        /// <summary>
        /// 세션에서 사용자 정보를 바인딩한다.
        /// </summary>
        private void SetUserInfo()
        {

            Hashtable hstParam = (Hashtable)System.Web.HttpContext.Current.Session["AuthManagerLoginInfo"];

            userInfo.UserID = hstParam["User_UserID"].ToString();		    // 사용자ID 
            userInfo.UserName = hstParam["User_UserName"].ToString();	        // 사용자명  
            userInfo.Nickname = hstParam["User_Nickname"].ToString();           // 별명
            userInfo.LoginTime = hstParam["User_LoginTime"].ToString();	        // 로그인 시간 
            userInfo.LoginIP = hstParam["User_LoginIP"].ToString();	            // 로그인 IP 
        }

        /// <summary>
        /// 사용자 정보를 DB에서 읽어와서 다시 세션에 담는다.
        /// </summary>
        private void ReSetUserInfo(string userID, string loginTime, string userIP, out string processCode)
        {
            DataSet ds = null;
            Dac.UserInfo oUserInfo = null;

            try
            {
                ds = new DataSet();
                oUserInfo = new Dac.UserInfo();

                ds = oUserInfo.ReGetUserInfo(userID, loginTime, userIP, out processCode);

                if (string.IsNullOrEmpty(processCode))
                {
                    // DataSet으로 사용자 정보가 담긴 세션을 만든다.
                    MakeSession(ds);

                    // 사용자 정보를 다시 바인딩 한다.
                    SetUserInfo();
                }
            }
            finally
            {
                if (ds != null) ds.Dispose();
            }

        }     /// <summary>

        /// DB에서 로그인정보가 정확하면 사용자 정보로 로그인 쿠키와 세션을 생성해 준다.
        /// </summary>
        /// <param name="ds"></param>
        private void MakeLoginInfo(DataSet ds)
        {
            // 로그인 인증용 쿠키를 생성한다.
            MakeCookie(ds);

            // 로그인 사용자 정보를 담고 있는 세션을 생성한다.
            MakeSession(ds);
        }

        /// <summary>
        /// 로그인 인증용 쿠키를 만든다.
        /// </summary>
        /// <param name="ds"></param>
        private void MakeCookie(DataSet ds)
        {
            DataRow drw = ds.Tables[0].Rows[0];

            #region == 로그인용 쿠키 ==

            // 1. 로그인용 쿠키를 생성한다.
            // 1.1 쿠키객체 생성
            HttpCookie oCookie = new HttpCookie("AuthManagerLoginInfo");

            // 쿠키 도메인 지정
            // 로컬일때는 도메인을 지정하지 않는다. (개발상태라고 본다.)
            if (!System.Web.HttpContext.Current.Request.IsLocal)
            {
                oCookie.Domain = ConfigurationManager.AppSettings["CookieDomain"];  // Web.Config에 있는 도메인
            }

            // 1.2 쿠키 값 입력
            string strCookieValue = drw["UserID"].ToString() + "|" + drw["LoginTime"].ToString() + "|" + drw["LoginIP"].ToString();

            // 1.3 암호화 클래스 생성
            Neoplus.Framework.Common.CryptoManager oCryptoManager = new Neoplus.Framework.Common.CryptoManager();
            // 1.4 암호화 키를 로그인한 IP로 지정
            oCryptoManager.Password = drw["LoginIP"].ToString();
            // 1.5 사용자ID, LoginTime, LoginIP의 문자열을 합쳐서 암호화 문자열 생성
            oCookie.Value = oCryptoManager.Encrypt(strCookieValue).Replace("+", "*");
            // 1.6 쿠키 굽기
            System.Web.HttpContext.Current.Response.Cookies.Add(oCookie);

            #endregion == 로그인용 쿠키 ==
        }

        /// <summary>
        /// 사용자 정보를 담고 있는 세션을 만든다.
        /// </summary>
        /// <param name="ds"></param>
        private void MakeSession(DataSet ds)
        {

            DataRow drw = ds.Tables[0].Rows[0];

            #region == 사용자 정보 세션 생성 ==

            // 세션이 있으면 삭제한다.
            if (System.Web.HttpContext.Current.Session["AuthManagerLoginInfo"] != null)
                System.Web.HttpContext.Current.Session.Remove("AuthManagerLoginInfo");

            // 2. 세션에 담을 정보를 바인딩 한다.
            // 2.1 해쉬테이블 생성
            Hashtable hstParam = new Hashtable();
            // 2.2 해쉬테이블에 사용자 정보 입력
            hstParam.Add("User_UserID", drw["UserID"].ToString());
            hstParam.Add("User_Nickname", drw["Nickname"].ToString());
            hstParam.Add("User_UserName", drw["UserName"].ToString());
            hstParam.Add("User_LoginTime", drw["LoginTime"].ToString());
            hstParam.Add("User_LoginIP", drw["LoginIP"].ToString());

            // 2.3 세션에 사용자 정보 해쉬테이블을 담는다.
            System.Web.HttpContext.Current.Session["AuthManagerLoginInfo"] = hstParam;

            #endregion == 사용자 정보 세션 생성 ==
        }



        #endregion == 로그인한 사용자의 정보 생성 인증로그인 쿠키 & 사용자정보 세션 ==
    }
}