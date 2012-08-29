using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Globalization;

using Neoplus.Framework.Common;
using Neoplus.Framework.DataAccess;

namespace MVC4Base.Dac
{
    /// <summary> 
    /// ■ 사용자정보 ■ 
    /// - 작  성  자 : 네오플러스 오진욱<para></para> 
    /// - 최초작성일 : 2011.10.05<para></para>  
    /// - 최초작업내용 : 최종작업<para></para> 
    /// - 최종수정자 : <para></para> 
    /// - 최종수정일 : 년 월 일<para></para> 
    /// - 주요변경로그<para></para> 
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// ■ 로그인시 사용자 정보 가져오기 ■ 
        /// - 작  성  자 : 오진욱(jwoh@neoplus.co.kr) 네오플러스<para></para> 
        /// - 최초작성일 : 2010.06.14<para></para> 
        /// - 최초수정자 : <para></para> 
        /// - 최초수정일 : <para></para> 
        /// - 주요변경로그 <para></para> 
        /// </summary>
        /// <param name="objectID">회원ID</param>
        /// <param name="userPassword">사용자암호</param>
        /// <param name="userIP">사용자IP</param>
        /// <param name="browserUserAgent">브라우져UA</param>
        /// <param name="browserID">브라우져ID</param>
        /// <param name="browserVer">브라우져버전</param>
        /// <param name="platform">플랫폼</param>
        /// <param name="cookiesSupport">쿠키지원</param>
        /// <param name="activeXSupport">엑티브엑스지원</param>
        /// <param name="processCode">프로세스코드</param>
        /// <returns>결과셋</returns>
        public DataSet GetUserInfo(string objectID, string userPassword, string userIP, string browserUserAgent, string browserID, string browserVer, string platform, string cookiesSupport, string activeXSupport, out string processCode)
        {
            SqlParameter[] sqlParm =  { 
                                          new SqlParameter("@ProcessCode",      SqlDbType.VarChar, 20 ) //10 리턴값 
                                        , new SqlParameter("@ObjectID",         objectID)            // 사용자ID 
                                        , new SqlParameter("@UserPassword",     userPassword)      // 사용자비밀번호
                                        , new SqlParameter("@UserIP",           userIP)            // 사용자IP
                                        , new SqlParameter("@BrowserUserAgent", browserUserAgent)  //4 로그인시 사용한 브라우져 문자열 SqlDbType.VarChar, 1000  
                                        , new SqlParameter("@BrowserID",        browserID)         //5 브라우져 종류 SqlDbType.VarChar, 20    
                                        , new SqlParameter("@BrowserVer",       browserVer)        //6 브라우져버전 SqlDbType.VarChar, 10    
                                        , new SqlParameter("@Platform",         platform)          //7 사용자 시스템 SqlDbType.VarChar, 20    
                                        , new SqlParameter("@CookiesSupport",   cookiesSupport)    //8 쿠키허용여부 SqlDbType.VarChar, 5     
                                        , new SqlParameter("@ActiveXSupport",   activeXSupport)    //9 ActiveX허용여부 SqlDbType.VarChar, 5 
                                        
                                      };


            sqlParm[0].Direction = ParameterDirection.Output;

            DataSet ds = SQLDataHelper.GetDataSet("usp_SYSUser_GetInfo", sqlParm, CommandType.StoredProcedure);

            processCode = sqlParm[0].Value.ToString();

            return ds;
        }

        /// <summary>
        /// ■ 로그인후 세션이 사라졌을때 다시 정보를 바인딩. ■ 
        /// - 작  성  자 : 오진욱(jwoh@neoplus.co.kr) 네오플러스<para></para> 
        /// - 최초작성일 : 2010.06.14<para></para> 
        /// - 최초수정자 : <para></para> 
        /// - 최초수정일 : <para></para> 
        /// - 주요변경로그 <para></para> 
        /// </summary>
        /// <param name="objectID">사용자ID</param>
        /// <param name="loginTime">로그인시간</param>
        /// <param name="userIP">로그인IP</param>
        /// <param name="processCode">프로세스코드</param>
        /// <returns>결과셋</returns>
        public DataSet ReGetUserInfo(string objectID, string loginTime, string userIP, out string processCode)
        {
            SqlParameter[] sqlParm =  { 
                                          new SqlParameter("@ProcessCode",      SqlDbType.VarChar, 20 ) //10 리턴값
                                        , new SqlParameter("@ObjectID",     objectID)     // 사용자ID 
                                        , new SqlParameter("@LoginTime",  loginTime)  // 사용자비밀번호
                                        , new SqlParameter("@UserIP",     userIP)     // 사용자IP
                                      };


            sqlParm[0].Direction = ParameterDirection.Output;

            DataSet ds = SQLDataHelper.GetDataSet("usp_SYSUser_ReGetInfo", sqlParm, CommandType.StoredProcedure);

            processCode = sqlParm[0].Value.ToString();

            return ds;

        }


        /// <summary>
        /// ■ 사용자 정보 가져오기 ■ 
        /// - 작  성  자 : 오진욱(jwoh@neoplus.co.kr) 네오플러스<para></para> 
        /// - 최초작성일 : 2010.11.09<para></para> 
        /// - 최초수정자 : <para></para> 
        /// - 최초수정일 : <para></para> 
        /// - 주요변경로그 <para></para> 
        /// </summary>
        /// <param name="objectID">사용자ID</param>
        /// <returns>결과셋</returns>
        public DataSet GetUserInfoAll(string objectID)
        {
            SqlParameter[] sqlParm =  { 
                                          new SqlParameter("@ObjectID",     objectID)     // 사용자ID 
                                      };

            return SQLDataHelper.GetDataSet("usp_SYSUser_GetInfoAll", sqlParm, CommandType.StoredProcedure);

        }

        /// <summary>
        /// ■ 아이디 찾기 ■ 
        /// - 작  성  자 : 오진욱(jwoh@neoplus.co.kr) 네오플러스<para></para> 
        /// - 최초작성일 : 2010.11.09<para></para> 
        /// - 최초수정자 : <para></para> 
        /// - 최초수정일 : <para></para> 
        /// - 주요변경로그 <para></para> 
        /// </summary>
        /// <param name="objectID">사용자ID</param>
        /// <returns>결과셋</returns>
        public DataSet FindID(string name, string email)
        {
            SqlParameter[] sqlParm =  { 
                                          new SqlParameter("@UserName",     name)      // 사용자이름 
                                        , new SqlParameter("@Email",        email)     // 이메일 
                                      };

            return SQLDataHelper.GetDataSet("usp_SYSUser_FindID", sqlParm, CommandType.StoredProcedure);
        }


        /// <summary>
        /// ■ 비밀번호 찾기 ■ 
        /// - 작  성  자 : 오진욱(jwoh@neoplus.co.kr) 네오플러스<para></para> 
        /// - 최초작성일 : 2010.11.09<para></para> 
        /// - 최초수정자 : <para></para> 
        /// - 최초수정일 : <para></para> 
        /// - 주요변경로그 <para></para> 
        /// </summary>
        /// <param name="objectID">사용자ID</param>
        /// <returns>결과셋</returns>
        public DataSet FindPassword(string objectID, string name, string email, string tempPassword)
        {
            SqlParameter[] sqlParm =  { 
                                          new SqlParameter("@ObjectID",     objectID)                           // 사용자ID
                                        , new SqlParameter("@UserName",     name)                               // 사용자이름 
                                        , new SqlParameter("@Email",        email)                              // 이메일 
                                        , new SqlParameter("@TempPassword", tempPassword)   // 랜덤숫자
                                      };

            return SQLDataHelper.GetDataSet("usp_SYSUser_FindPassword", sqlParm, CommandType.StoredProcedure);
        }

        /// <summary>
        /// ■ 접속로그 남기기 ■ 
        /// - 작  성  자 : 오진욱(jwoh@neoplus.co.kr) 네오플러스<para></para> 
        /// - 최초작성일 : 2011.09.27<para></para> 
        /// - 최초수정자 : <para></para> 
        /// - 최초수정일 : <para></para> 
        /// - 주요변경로그 <para></para> 
        /// </summary>
        /// <param name="menuID">메뉴ID</param>
        /// <param name="ip">IP</param>
        /// <param name="objectID">회원ID</param>
        /// <param name="visitType">방문타입</param>
        /// <param name="copNo">동호회번호</param>
        /// <param name="browserUserAgent">브라우져UA</param>
        /// <param name="browserID">브라우져ID</param>
        /// <param name="browserVer">브라우져버전</param>
        /// <param name="platform">플랫폼</param>
        /// <param name="cookiesSupport">쿠키지원</param>
        /// <param name="activeXSupport">엑티브엑스지원</param>
        /// <returns>결과셋</returns>
        public DataSet InsertVisitLog(
            string menuID,
            string ip,
            string objectID,
            string visitType,
            string copNo,
            string browserUserAgent,
            string browserID,
            string browserVer,
            string platform,
            string cookiesSupport,
            string activeXSupport)
        {
            string strQuery = "usp_VisitLog_Insert";

            SqlParameter[] paramArray = { new SqlParameter("@MenuID",           menuID)            // 메뉴ID SqlDbType.VarChar, 20    
                                        , new SqlParameter("@IP",               ip)                // IP SqlDbType.VarChar, 15    
                                        , new SqlParameter("@ObjectID",         objectID)          // 회원ID SqlDbType.VarChar, 50    
                                        , new SqlParameter("@VisitType",        visitType)         // 방문타입 SqlDbType.VarChar, 20    
                                        , new SqlParameter("@CopNo",            copNo)             // 동호회번호 SqlDbType.Int            
                                        , new SqlParameter("@BrowserUserAgent", browserUserAgent)  // 브라우져UA SqlDbType.NVarChar, 1000 
                                        , new SqlParameter("@BrowserID",        browserID)         // 브라우져ID SqlDbType.VarChar, 20    
                                        , new SqlParameter("@BrowserVer",       browserVer)        // 브라우져버전 SqlDbType.VarChar, 10    
                                        , new SqlParameter("@Platform",         platform)          // 플랫폼 SqlDbType.VarChar, 20    
                                        , new SqlParameter("@CookiesSupport",   cookiesSupport)    // 쿠키지원 SqlDbType.Char, 1        
                                        , new SqlParameter("@ActiveXSupport",   activeXSupport)    // 엑티브엑스지원 SqlDbType.Char, 1    
                                        };

            return SQLDataHelper.GetDataSet(strQuery, paramArray, CommandType.StoredProcedure);
        }

        /// <summary> 
        /// ■ Role 구하기 ■ 
        /// - 작  성  자 : 오진욱(jwoh@neoplus.co.kr) 네오플러스<para></para> 
        /// - 최초작성일 : 2011.09.27<para></para> 
        /// - 최초수정자 : <para></para> 
        /// - 최초수정일 : <para></para> 
        /// - 주요변경로그 <para></para> 
        /// </summary> 
        /// <param name="objectID">사용자ID</param>
        /// <returns>결과셋</returns>
        public DataSet GetRoles(string objectID)
        {
            SqlParameter[] sqlParm =  { 
                                        new SqlParameter("@ObjectID",     objectID)     // 사용자ID  
                                      };

            DataSet ds = SQLDataHelper.GetDataSet("usp_SYSRoleUser_Select", sqlParm, CommandType.StoredProcedure);

            return ds;

        }

        /// <summary> 
        /// ■ Role에 속해 있는지 구하기 ■ 
        /// - 작  성  자 : 오진욱(jwoh@neoplus.co.kr) 네오플러스<para></para> 
        /// - 최초작성일 : 2011.09.27<para></para> 
        /// - 최초수정자 : <para></para> 
        /// - 최초수정일 : <para></para> 
        /// - 주요변경로그 <para></para> 
        /// </summary> 
        /// <param name="objectID">사용자ID</param>
        /// <param name="roleID">롤ID</param>
        /// <returns>조회결과</returns>
        public bool IsUserInRole(string objectID, string roleID)
        {
            DataSet ds = null;
            bool iResult = false;

            SqlParameter[] sqlParm =  { 
                                        new SqlParameter("@ObjectID",     objectID)     // 사용자ID  
                                      , new SqlParameter("@RoleID",       roleID)       // RoleID  
                                      };
            try
            {
                ds = SQLDataHelper.GetDataSet("usp_SYSRoleUser_IsUserInRole", sqlParm, CommandType.StoredProcedure);
                iResult = ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0;
            }
            finally
            {

                if (ds != null)
                {
                    ds.Dispose();
                }
            }
            return iResult;
        }


        /// <summary> 
        /// ■ 이메일 중복 확인 ■ 
        /// - 작  성  자 : 오진욱(jwoh@neoplus.co.kr) 네오플러스<para></para> 
        /// - 최초작성일 : 2011.09.27<para></para> 
        /// - 최초수정자 : <para></para> 
        /// - 최초수정일 : <para></para> 
        /// - 주요변경로그 <para></para> 
        /// </summary> 
        /// <param name="email">사용자 이메일</param>
        /// <returns>중복여부</returns>
        public bool IsEmailExist(string email, string objectID = "")
        {
            DataSet ds = null;
            bool iResult = false;

            SqlParameter[] sqlParm =  { 
                                          new SqlParameter("@ObjectID",       objectID)     // 사용자ID  
                                        , new SqlParameter("@Email",          email)     // 메일  
                                      };
            try
            {
                ds = SQLDataHelper.GetDataSet("usp_SYSUser_IsEmailExist", sqlParm, CommandType.StoredProcedure);
                iResult = ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0;
            }
            finally
            {

                if (ds != null)
                {
                    ds.Dispose();
                }
            }
            return iResult;

        }


        /// <summary> 
        /// ■ 사용자정보 조회 ■ 
        /// - 작  성  자 : 오진욱(jwoh@neoplus.co.kr) 네오플러스<para></para> 
        /// - 최초작성일 : 2011.09.27<para></para> 
        /// - 최초수정자 : <para></para> 
        /// - 최초수정일 : <para></para> 
        /// - 주요변경로그 <para></para> 
        /// </summary> 
        /// <param name="objectID">사용자ID</param> 
        /// <returns>결과셋</returns> 
        public DataSet GetSYSUser(string objectID)
        {
            string strQuery = "usp_SYSUser_Select";

            SqlParameter[] paramArray = { 
                                            new SqlParameter("@ObjectID",           objectID)                   //사용자ID
                                               
                                        };

            return SQLDataHelper.GetDataSet(strQuery, paramArray, CommandType.StoredProcedure);
        }

        /// <summary> 
        /// ■ 아이디 중복 검사 ■ 
        /// - 작  성  자 : 오진욱(jwoh@neoplus.co.kr) 네오플러스<para></para> 
        /// - 최초작성일 : 2011.09.27<para></para> 
        /// - 최초수정자 : <para></para> 
        /// - 최초수정일 : <para></para> 
        /// - 주요변경로그 <para></para> 
        /// </summary> 
        /// <returns>중복 여부 : true - 중복됨</returns> 
        public bool IDCheck(string objectID)
        {
            DataSet ds = null;
            bool iResult = false;

            SqlParameter[] paramArray = { 
                                            new SqlParameter("@ObjectID",           objectID)                   //사용자ID
                                               
                                        };
            try
            {
                ds = SQLDataHelper.GetDataSet("usp_SYSUser_IsObjectIDExist", paramArray, CommandType.StoredProcedure);
                iResult = ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0;

            }
            finally
            {

                if (ds != null)
                {
                    ds.Dispose();
                }
            }
            return iResult;
        }


        /// <summary> 
        /// ■ 회원가입, 수정 ■ 
        /// - 작  성  자 : 오진욱(jwoh@neoplus.co.kr) 네오플러스<para></para> 
        /// - 최초작성일 : 2011.10.05<para></para> 
        /// - 최초수정자 : <para></para> 
        /// - 최초수정일 : <para></para> 
        /// - 주요변경로그 <para></para> 
        /// </summary> 
        /// <param name="param">회원정보</param> 
        /// <returns>성공여부</returns> 
        public bool ProcessSYSUser(Hashtable param)
        {
            SqlParameter[] paramArray = { 
                                           new SqlParameter("@Arg",              param["Arg"])
                                         , new SqlParameter("@ObjectID",         param["ObjectID"])
                                         , new SqlParameter("@Name",             param["Name"])
                                         , new SqlParameter("@Nickname",         param["Nickname"])
                                         , new SqlParameter("@PrevPassword",     param.ContainsKey("PrevPassword") ? param["PrevPassword"] : null)
                                         , new SqlParameter("@Password",         param["Password"])
                                         , new SqlParameter("@PhoneNum",         param["PhoneNum"])
                                         , new SqlParameter("@CellNum",          param["CellNum"])
                                         , new SqlParameter("@PostNum",          param["PostNum"])
                                         , new SqlParameter("@Address",          param["Address"])
                                         , new SqlParameter("@JobGroup",         param["JobGroup"])
                                         , new SqlParameter("@MailAddress",      param["MailAddress"])
                                         , new SqlParameter("@MailChoice",       param["MailChoice"])
                                         , new SqlParameter("@WebzineChoice",    param["WebzineChoice"])
                                         , new SqlParameter("@SMSChoice",        param["SMSChoice"])
                                         , new SqlParameter("@Fax",              param["Fax"])
                                         , new SqlParameter("@OfficeName",       param["OfficeName"])
                                         , new SqlParameter("@Charge",           param["Charge"])
                                         , new SqlParameter("@NotificationYN",   param["NotificationYN"])
                                         , new SqlParameter("@PublicYN",         param["PublicYN"])
                                         , new SqlParameter("@NationKeys",       param["NationKeys"])
                                         , new SqlParameter("@ItemKeys",         param["ItemKeys"])
                                         , new SqlParameter("@RegID",            param["RegID"])
                                         , new SqlParameter("@RegIP",            param["RegIP"]) 
                                        };

            return SQLDataHelper.Execute("usp_SYSUser_Process", paramArray, CommandType.StoredProcedure) > 0;
        }

    }
}
