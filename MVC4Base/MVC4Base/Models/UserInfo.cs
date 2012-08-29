using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using System.Configuration;
using System.Web;
using Neoplus.Framework.Common;
using Neoplus.Framework.Web;
using System.Web.UI;
using System.Web.Security;

namespace MVC4Base.Models
{
    public class UserInfo
    {
        public UserInfo()
        {
            ObjectID = string.Empty;
            UserName = string.Empty;
            Nickname = string.Empty;
            LoginTime = string.Empty;
            LoginIP = string.Empty;
            IsLoginUser = false;
        }


        #region == 맴버변수 로그인 사용자 정보 ==

        // 사용자 기본 정보

        /// <summary>
        ///  사용자ID 
        /// </summary>
        public string ObjectID { get; set; } 
        /// <summary>
        /// 사용자명      
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 별명
        /// </summary>
        public string Nickname { get; set; }   
        /// <summary>
        /// 로그인 시간 
        /// </summary>
        public string LoginTime { get; set; }   
        /// <summary>
        /// 로그인 IP 
        /// </summary>
        public string LoginIP { get; set; }              
        /// <summary>
        /// 정상적인 로그인 유저인지 체크 
        /// </summary>
        public bool IsLoginUser { get; set; }

        #endregion == 맴버변수 로그인 사용자 정보 ==

    }
}
