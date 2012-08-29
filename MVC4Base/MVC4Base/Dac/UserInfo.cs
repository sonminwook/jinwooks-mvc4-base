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
    /// - 최초작성일 : 2012.08.29<para></para>  
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
        /// - 최초작성일 : 2012.08.29<para></para> 
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
                                        , new SqlParameter("@UserID",         objectID)            // 사용자ID 
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
                                        , new SqlParameter("@UserID",     objectID)     // 사용자ID 
                                        , new SqlParameter("@LoginTime",  loginTime)  // 사용자비밀번호
                                        , new SqlParameter("@UserIP",     userIP)     // 사용자IP
                                      };


            sqlParm[0].Direction = ParameterDirection.Output;

            DataSet ds = SQLDataHelper.GetDataSet("usp_SYSUser_ReGetInfo", sqlParm, CommandType.StoredProcedure);

            processCode = sqlParm[0].Value.ToString();

            return ds;

        }
    }
}
