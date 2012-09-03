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
            //필터 등록하기
            filters.Add(ContextRegistry.GetContext().GetObject("InitUserInfoAttribute", typeof(System.Web.Mvc.ActionFilterAttribute))); //사용자 권한 필터
        }
    }
}