using System;
using System.Web;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using HTTPMediaPlayer.Models;
using HTTPMediaPlayer.Models.DB;
using HTTPMediaPlayer.Controllers;

namespace HTTPMediaPlayer
{
  public class WebApiApplication : System.Web.HttpApplication
  {
    protected void Application_Start()
    {
      AreaRegistration.RegisterAllAreas();
      GlobalConfiguration.Configure(WebApiConfig.Register);
      FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
      RouteConfig.RegisterRoutes(RouteTable.Routes);
      BundleConfig.RegisterBundles(BundleTable.Bundles);

    }

    protected void Application_Error()
    {
      var exception = Server.GetLastError();
      // TODO: Log the exception or something
      /*Response.Clear();
      Server.ClearError();

      var routeData = new RouteData();
      routeData.Values["controller"] = "Ru";
      routeData.Values["action"] = "Index";
      //Response.StatusCode = 500;
      IController controller = new RuController();
      var rc = new RequestContext(new HttpContextWrapper(Context), routeData);
      controller.Execute(rc);*/
    }

    internal protected void Application_BeginRequest(object sender, EventArgs e)
    {
      //GlobalConfiguration.Configuration.Services.Replace(typeof(IHostBufferPolicySelector), new NoBufferPolicySelector());
    }

    protected void Session_End(object sender, EventArgs e)
    {
      if (Session["UserID"] != null)
      {
        int? userId = Convert.ToInt32(Session["UserID"]);
        int? sessionId = Convert.ToInt32(Session["SessionID"]);
        GlobalModel.OnSessionEnd(userId, sessionId);
      }
    }
  }
}
