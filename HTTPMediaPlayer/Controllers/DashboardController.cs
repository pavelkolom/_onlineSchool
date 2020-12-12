using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HTTPMediaPlayer.Models;
using HTTPMediaPlayer.Models.DB;

namespace HTTPMediaPlayer.Controllers
{
  public class DashboardController : Controller
  {
    // GET: Dashboard
    [SessionCheck]
    public ActionResult Index(string token)
    {
      // vd_Users user = GlobalModel.GetUserById((int)Session["UserID"]);
      //if (user != null)
        return View((int)Session["UserID"]);
      //else return RedirectToAction("Index", "ru");
    }

    // GET: Dashboard
    [SessionCheck]
    public ActionResult MyCourse(int? id, int? userId, int? courseId)
    {
      if(id == null || userId == null || courseId== null)
        return RedirectToAction("Index");
      if ((int)Session["UserID"] != userId)
        return RedirectToAction("Index");
      //var course = GlobalModel.GetUserCourseById(id, (int)Session["UserID"]);
      //if(course != null)
      return View("MyCourse", new UserCourse { userid = (int)userId, courseid = (int)courseId });
      //return RedirectToAction("Index");
    }

    // GET: Dashboard
    public ActionResult SetPassword(string token, int id)
    {
      if(token == null) return RedirectToAction("Index", "ru");
      var user = GlobalModel.GetUserByToken(token);
      if (user != null && user.Token.ToString() == token)
      {
        using (videodbEntities db = new videodbEntities())
        {
          vd_Log sessionLog = new vd_Log();
          sessionLog.LoginDT = DateTime.Now;
          sessionLog.UserId = user.Id;
          db.vd_Log.Add(sessionLog);
          db.SaveChanges();
          vd_ActionLog lg = new vd_ActionLog();
          lg.UserId = user.Id;
          lg.StartTime = DateTime.Now;
          lg.SessionId = sessionLog.Id;
          lg.Remarks = "Login";
          lg.ActionType = 1;
          db.vd_ActionLog.Add(lg);
          db.SaveChanges();

          Session["SessionID"] = sessionLog.Id;
          Session["UserID"] = user.Id;
          Session["IsAuthor"] = user.IsAuthor;
          Session["IsAdmin"] = user.IsAdmin;
          Session["Email"] = user.Email;
          Session["LoginName"] = user.LoginName;
          Session["IsActivated"] = false;
          Session["UserUrl"] = user.UrlName;
        }
        return View(user);
      }
      else return RedirectToAction("Index", "ru");
    }

    //[SessionCheck]
    public ActionResult GetUserCourse(int userId, int courseId)
    {
      if (Session["UserID"] == null)
      {
        int? usrId = Convert.ToInt32(Session["UserID"]);
        int? sessionId = Convert.ToInt32(Session["SessionID"]);
        GlobalModel.OnSessionEnd(usrId, sessionId);
        return PartialView("_redirectToLogin");
        //return new HttpUnauthorizedResult();
      }

      return PartialView("_lessonsList", new UserCourse { userid = userId , courseid = courseId });
    }

    //[SessionCheck]
    public ActionResult GetLessonDescription(int lessonid, int userid)
    {
      if (Session["UserID"] == null)
      {
        return PartialView("_redirectToLogin");
      }

      //var lesson = GlobalModel.GetUserLessonById(lessonId);
      //if (course == null)
      //{
      //  return new HttpUnauthorizedResult();
      //}
      //vd_UserLessons ul = GlobalModel.GetUserLessonsByUserIAndLesson(lessonid, userid);
      //  Guid? newId = GlobalModel.ChangeUserLessonGuidByUserLesson(ul);

      //using (videodbEntities db = new videodbEntities())
      //{
      //  var lesson = db.vd_UserLessons.FirstOrDefault(f => f.LessonId == lessonid && f.UserId == userid);
      //  lesson.UniqueId = Guid.NewGuid();
      //  db.SaveChanges();

      //  var x = GlobalModel.UsersVideos.Where(d => d.Value.UserLesson.Id == lesson.Id).ToList();

      //  for (int i = 0; i < x.Count(); i++)
      //    GlobalModel.UsersVideos.Remove(x[i].Key);

      //}

      using (videodbEntities db = new videodbEntities())
      {
        var lesson = db.vd_UserLessons.FirstOrDefault(f => f.LessonId == lessonid && f.UserId == userid);
        vd_ActionLog log = new vd_ActionLog();
        log.UserId = userid;
        log.StartTime = DateTime.Now;
        log.Remarks = lesson.vd_Lessons.vd_Files.FileName;
        log.ActionType = 3;
        db.vd_ActionLog.Add(log);
        db.SaveChanges();
      }

      return PartialView("_lessonDescription", new UserCourse { lessonid = lessonid });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult ChangePassword(ChangePasswordModel model)
    {
      if (ModelState.IsValid)
      {
        //videodbEntities db = //GlobalModel.GetDB((int)Session["UserID"]);
        using (videodbEntities db = new videodbEntities())
        {
          vd_Users user = db.vd_Users.FirstOrDefault(u => u.Id == model.userid);

          if (model.password != model.confirmation)
          {
            RedirectToAction("SetPassword", new { res = "notmatch", token = user.Token });
          }

          user.Password = CryptoModel.GetMD5HashString(model.password);
          user.Token = Guid.NewGuid();
          user.IsActivated = true;

          try
          {
            db.SaveChanges();
            Session["IsActivated"] = true;
          }
          catch (Exception )
          {
            return RedirectToAction("SignUp", new { res = "failed" });
          }
          GlobalModel.LoggedInUsers.Add(Session["UserID"], user);
          return RedirectToAction("Index");
        }
      }
      return RedirectToAction("Login", new { res = "regfailed" });
    }
  }
}