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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;

namespace MVC4Base.Models
{
    /// <summary>
    /// 로그인 관련 모델
    /// </summary>
    public class LoginModel
    {
        [Required]
        [Display(Name = "아이디")]
        public string UserID { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "비밀번호")]
        public string Password { get; set; }

        [Display(Name = "아이디 저장")]
        public bool RememberMe { get; set; }
    }

    /// <summary>
    /// 회원 정보 수정 관련 모델
    /// </summary>
    public class MemeberModel
    {
        [Required]
        [Display(Name = "아이디")]
        public string UserID { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "현재 비밀번호")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0}는 {2}자리 이상만 가능합니다.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "새 비밀번호")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "새 비밀번호 확인")]
        [Compare("NewPassword", ErrorMessage = "새 비밀번호와 일치하지 않습니다.")]
        public string ConfirmPassword { get; set; }
    }

    /// <summary>
    /// 사용자 권한
    /// </summary>
    public enum UserAuthority
    {
        /// <summary>
        /// 관리자 권한
        /// </summary>
        [Description("ADMIN")]
        ADMIN = 0,
        /// <summary>
        /// 목록 보기 권한
        /// </summary>
        [Description("LIST")]
        LIST = 1,
        /// <summary>
        /// 읽기 권한
        /// </summary>
        [Description("READ")]
        READ = 2,
        /// <summary>
        /// 생성 권한
        /// </summary>
        [Description("CREATE")]
        CREATE = 3,
        /// <summary>
        /// 수정 권한
        /// </summary>
        [Description("UPDATE")]
        UPDATE = 4,
        /// <summary>
        /// 삭제 권한
        /// </summary>
        [Description("DELETE")]
        DELETE = 5,
        /// <summary>
        /// 프린트 권한
        /// </summary>
        [Description("PRINT")]
        PRINT = 6,
        /// <summary>
        /// 업로드 권한
        /// </summary>
	    [Description("UPLOAD")]
        UPLOAD = 7,
        /// <summary>
        /// 다운로드 권환
        /// </summary>
	    [Description("DOWNLOAD")]
        DOWNLOAD = 8,
        /// <summary>
        /// 스크랩 권한
        /// </summary>
	    [Description("SCRAP")]
        SCRAP = 9,
        /// <summary>
        /// 댓글 권한
        /// </summary>
	    [Description("COMMENT")]
        COMMENT = 10,
        /// <summary>
        /// 답글 권한
        /// </summary>
	    [Description("REPLY")]
        REPLY = 11
    }

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

        #endregion == 맴버변수 로그인 사용자 정보 ==


        #region == 맴버변수 로그인 사용자 권한 정보 ==

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
        public bool HasAuthority(UserAuthority AuthID)
        {
            return MenuAuthList.Contains(AuthID.ToString());
        }
        /// <summary>
        /// 쿠키 타입아웃
        /// </summary>
        public string CookieTimeout { get; set; }

        #endregion == 맴버변수 로그인 사용자 권한 정보 ==



    }
}
