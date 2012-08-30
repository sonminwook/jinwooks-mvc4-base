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
            UserID = string.Empty;
            UserName = string.Empty;
            Nickname = string.Empty;
            LoginTime = string.Empty;
            LoginIP = string.Empty;
            IsLoginUser = false;

            CurrentMenuID = string.Empty;
            MenuAuthList = new List<string>();
            RoleList = new Dictionary<string, string>();
        }


        #region == 맴버변수 로그인 사용자 정보 ==

        // 사용자 기본 정보

        /// <summary>
        ///  사용자ID 
        /// </summary>
        public string UserID { get; set; } 
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

        //사용자 권한 정보

        /// <summary>
        /// 현재 메뉴ID
        /// </summary>
        public string CurrentMenuID { get; set; }
        /// <summary>
        /// 메뉴 사용 권한
        /// </summary>
        public List<string> MenuAuthList { get; set; }
        /// <summary>
        /// 권한 그룹 정보(권한그룹ID, 권한그룹명)
        /// </summary>
        public Dictionary<string, string> RoleList { get; set; }
        /// <summary>
        /// 권한이 있는지 여부
        /// </summary>
        public bool HasAuthority(string AuthID)
        {
            return MenuAuthList.Contains(AuthID);
        }

        #endregion == 맴버변수 로그인 사용자 정보 ==

    }
}
