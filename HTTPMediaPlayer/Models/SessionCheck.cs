using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Http.Controllers;
using System.Net;
using System.Net.Http;

namespace HTTPMediaPlayer.Models
{
  public class SessionCheck : ActionFilterAttribute
  {
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
       HttpSessionStateBase session = filterContext.HttpContext.Session;
      if (session != null && session["UserID"] == null && session["AuthorId"] == null || session["IsActivated"] == null)
      {
        filterContext.Result = new RedirectToRouteResult(
            new RouteValueDictionary {
                                { "Controller", "ru" },
                                { "Action", "Login" }
                        });
      }
    }
  }

}