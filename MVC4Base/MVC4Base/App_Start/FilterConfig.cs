using System.Web;
using System.Web.Mvc;
using MVC4Base.Filters;
using Spring.Context.Support;

namespace MVC4Base
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //사용자 인증 필터 등록하기(모든 컨트롤러에서 실행)
            filters.Add(ContextRegistry.GetContext().GetObject("InitUserInfoAttribute", typeof(MVC4Base.Filters.InitUserInfoAttribute))); //사용자 권한 필터
        }
    }
}