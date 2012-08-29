using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MVC4Base
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            /* ASP.NET MVC 응용 프로그램에서 하나 이상의 영역을 등록 하는 방법을 제공 합니다. */
            AreaRegistration.RegisterAllAreas();

            /* 들어오는 HttpRequestMessage를 발송하고, 그 결과로 HttpResponseMessage를 만드는 HttpMessageHandler 구현을 정의합니다. */
            WebApiConfig.Register(GlobalConfiguration.Configuration);

            /* 전역 필터 컬렉션을 나타냅니다. */
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            /* 응용 프로그램의 URL 경로를 저장합니다. */
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            /*
             System.Web.Optimization 네임 스페이스에는 자바 스크립트 최적화 및 계단식 스타일 시트 (CSS) 파일 크기를 줄이고 
             * 파일 페이지 성능을 향상 웹사이트에 프로세스를 지 원하는 클래스가 포함 되어 있습니다. 
             * 이 네임 스페이스의 클래스 사용 개발자가 그들의 자바 스크립트와 CSS 파일을 최적화 하기 위해 
             * 번들 및 축소 작업을 수행할 수 있습니다.
             */
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            /* OAuthWebSecurity 클래스 사용 : 구글과 야후와 같은 페이 스 북, 트위터, 링크 드 인, 윈도우 라이브와 오픈 Id 인증 공급자 같은 OAuth 인증 공급자를 
             * 사용하여 보안을 관리 합니다. */
            AuthConfig.RegisterAuth();
        }
    }
}