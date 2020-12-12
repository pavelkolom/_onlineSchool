using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace HTTPMediaPlayerCore.Models
{
  public class SessionCheck : IActionFilter
  {
    public void OnActionExecuting(ActionExecutingContext filterContext)
    {
      string sessionUserID = filterContext.HttpContext.Session.GetString("UserID");
      string sessionAuthorId = filterContext.HttpContext.Session.GetString("AuthorId");
      string sessionIsActivated = filterContext.HttpContext.Session.GetString("IsActivated");

      if(string.IsNullOrEmpty(sessionUserID))

      //if (session != null && session["UserID"] == null && session["AuthorId"] == null || session["IsActivated"] == null)
      {
        filterContext.Result = new RedirectToRouteResult(
            new RouteValueDictionary {
                                { "Controller", "Account" },
                                { "Action", "Login" }
                        });
      }
    }

    public void OnActionExecuted(ActionExecutedContext filterContext)
    {

    }
  }



}