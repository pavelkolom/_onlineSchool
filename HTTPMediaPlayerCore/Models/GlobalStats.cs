using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HTTPMediaPlayerCore.Models
{
  public static class GlobalStats
  {
    public static Dictionary<object, string> LoggedInUsers { get; } = new Dictionary<object, string>();

    public static void OnSessionEnd(int? userId, int? sessionId)
    {
      if (userId != null && userId != 0)
      {

        //int? sessionId = Convert.ToInt32(Session["SessionID"]);
        if (sessionId != null && sessionId != 0)
        {
          if (LoggedInUsers.ContainsKey((int)userId))
          {
            LoggedInUsers.Remove(userId);
          }

          //var x = UsersVideos.Where(d => d.Value.UserLesson.UserId == userId).ToList();
          //for (int i = 0; i < x.Count(); i++)
          //  UsersVideos.Remove(x[i].Key);

          using (DuwaysContext db = new DuwaysContext(new DbContextOptions<DuwaysContext>() { }))
          {
            Log sessionLog = db.Log.FirstOrDefault(log => log.Id == sessionId);
            if (sessionLog != null) sessionLog.LogoutDT = DateTime.Now;
            ActionLog lg = new ActionLog();
            lg.UserId = (int)userId;
            lg.StartTime = DateTime.Now;
            if (sessionLog != null) lg.SessionId = sessionLog.Id;
            lg.Remarks = "Logout";
            lg.ActionTypeId = 2;
            db.ActionLog.Add(lg);
            db.SaveChanges();
          }
          //var routeData = new RouteData();
          //routeData.Values["controller"] = "Ru";
          //routeData.Values["action"] = "Index";
          ////Response.StatusCode = 500;
          //IController controller = new RuController();
          //var rc = new RequestContext(new HttpContextWrapper(Context), routeData);
          //controller.Execute(rc);
        }
      }
    }

    private static List<Category> _categories = new List<Category>();

    public static List<Category> Categories
    {
      get
      {
        if (_categories.Count == 0)
        {
          using (DuwaysContext dbcontext = new DuwaysContext(new DbContextOptions<DuwaysContext>() { }))
          {
            _categories = dbcontext.Category.OrderBy(l => l.Id).ToList();
          }
        }
        return _categories;
      }
    }

  }
}
