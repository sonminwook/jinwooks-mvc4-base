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
using MVC4Base.Models;

namespace MVC4Base.Dao
{
    /// <summary>
    /// 코드 관리
    /// </summary>
    public class CodeDao
    {
        /// <summary> 
        /// ■ 메인코드 검색용 ■ 
        /// * 페이징처리를 Query(SP)에서 처리합니다. <para></para> 
        /// - 작  성  자 : 오진욱(jwoh@neoplus.co.kr) 네오플러스<para></para> 
        /// - 최초작성일 : 2010.06.24<para></para> 
        /// - 최초수정자 : <para></para> 
        /// - 최초수정일 : <para></para> 
        /// - 주요변경로그 <para></para> 
        /// </summary> 
        /// <param name="pageIndex">목록의 현재페이지 번호 (**페이지번호는 1페이지 부터 시작합니다.**)</param> 
        /// <param name="pageSize">목록의 한 페이지에 표현되는 글 목록수 </param> 
        /// <param name="order">정렬조건</param> 
        /// <param name="mainCode">메인코드</param> 
        /// <param name="subCode">서브코드</param> 
        /// <returns></returns> 
        public DataSet GetSYSCode(PagingModel model, string titleYN, string mainCode, string subCode, string codeName)
        {
            string strQuery = "usp_SYSCode_List";

            SqlParameter[] paramArray = { new SqlParameter("@PageIndex",        model.PageIndex)                //조회할 페이지 ** 1페이지부터 시작합니다. 
                                        , new SqlParameter("@PageSize",         model.PageSize)                 //페이지당 리스트갯수  
                                        , new SqlParameter("@Order",            model.Order)                    //정렬할 컬럼 
                                        , new SqlParameter("@MainCode",         mainCode)                 //메인코드
                                        , new SqlParameter("@SubCode",          subCode)                  //서브코드
                                        , new SqlParameter("@CodeName",         codeName)                 //코드명   
                                        , new SqlParameter("@TitleYN",          titleYN)                  //제목여부
                                        };



            return SQLDataHelper.GetDataSet(strQuery, paramArray, CommandType.StoredProcedure);
        }
    }
}
