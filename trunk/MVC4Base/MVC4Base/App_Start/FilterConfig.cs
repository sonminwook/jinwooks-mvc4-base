using System.Web;
using System.Web.Mvc;
using MVC4Base.Filters;

namespace MVC4Base
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new InitUserInfoAttribute()); //사용자 권한 필터
        }
    }
}