using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace WebApplication1
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        //protected void Application_EndRequest()
        //{
        //    var context = new HttpContextWrapper(Context);
        //    // If we're an ajax request, and doing a 302, then we actually need to do a 401
        //    if (Context.Response.StatusCode == 302 && context.Request.IsAjaxRequest())
        //    {
        //        Context.Response.Clear();
        //        Context.Response.StatusCode = 401;
        //    }
        //}
    }
}
