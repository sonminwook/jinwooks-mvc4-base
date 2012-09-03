using System;
namespace MVC4Base.Services
{
    public interface IAuthService
    {
        void CheckAuthority(string controllerName, string actionName);
        void CheckLoginUser();
        string GetUserPlatform();
        void InsertVisitLog(string controllerName, string actionName);
        bool Login(string userID, string password, bool isSaveID, out string processCode);
        void Logout();
        MVC4Base.Models.UserInfo UserInfomation { get; }
    }
}
