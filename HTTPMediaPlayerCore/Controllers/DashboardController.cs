using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using HTTPMediaPlayerCore.Models;
using Microsoft.EntityFrameworkCore;
using HTTPMediaPlayerCore.Services;


namespace HTTPMediaPlayerCore.Controllers
{
  public class DashboardController : Controller
  {
    private readonly ICourseService _courseService;

    public DashboardController(ICourseService courceService)
    {
      _courseService = courceService;
    }

    // GET: Dashboard
    public ActionResult Index()
    {
      string userID = HttpContext.Session.GetString("UserID");
      if(!string.IsNullOrEmpty(userID))
        return View(Convert.ToInt32(userID));
      else
        return RedirectToAction("Index", "Home");
    }

    // GET: Dashboard
  
    public ActionResult MyCourse(int? id, int? userId, int? courseId)
    {
      if (id == null || userId == null || courseId == null)
        return RedirectToAction("Index");

      string usrId = HttpContext.Session.GetString("UserID");
      if (usrId != userId.ToString())
        return RedirectToAction("Index");
      //var course = GlobalModel.GetUserCourseById(id, (int)Session["UserID"]);
      //if(course != null)
      return View("MyCourse", new UserCourseItem { userid = (int)userId, courseid = (int)courseId });
      //return RedirectToAction("Index");
    }

    public ActionResult GetLessonDescription(int lessonid, int userid)
    {
      string userID = HttpContext.Session.GetString("UserID");

      if (string.IsNullOrEmpty(userID))
      {
        return PartialView("_redirectToLogin");
      }

      using (DuwaysContext db = new DuwaysContext(new DbContextOptions<DuwaysContext>() { }))
      {
        var lesson = db.UserLesson.FirstOrDefault(f => f.LessonId == lessonid && f.UserId == userid);
        var less = db.Lesson.FirstOrDefault(f => f.Id == lesson.LessonId);
        var file = db.File.FirstOrDefault(f => f.Id == less.FileId);

        ActionLog log = new ActionLog();
        log.UserId = userid;
        log.StartTime = DateTime.Now;
        log.Remarks = file.FileName;
        log.ActionTypeId = 3;
        db.ActionLog.Add(log);
        db.SaveChanges();
      }

      //return PartialView("_lessonDescription", new UserCourseItem { lessonid = lessonid });

      return ViewComponent("LessonDescription", new UserCourseItem { lessonid = lessonid });
    }

    public async Task<ActionResult> GetLessonContents(int lessonid, int courseid, int authorid, int userid)
    {
      string userID = HttpContext.Session.GetString("UserID");

      if (string.IsNullOrEmpty(userID))
      {
        return PartialView("_redirectToLogin");
      }
      string readText = "We are working on it now.";
      if (lessonid != 0)
      {
        string path = @"c:\Duways\Pages\" + authorid + "\\" + courseid + "\\" + lessonid + ".html";
        // Open the file to read from.
        readText = await System.IO.File.ReadAllTextAsync(path);
      }

      return ViewComponent("LessonContents", new UserCourseItem {html = readText.Replace("\r\n", "") });
    }

    public ActionResult GetUserCourse(int userId, int courseId)
    {

      string userID = HttpContext.Session.GetString("UserID");
      string SessionID = HttpContext.Session.GetString("SessionID");
      if (string.IsNullOrEmpty(userID))
      {
        int? usrId = Convert.ToInt32(userID);
        int? sessionId = Convert.ToInt32(SessionID);
        GlobalStats.OnSessionEnd(usrId, sessionId);
        return PartialView("_redirectToLogin");
      }
      return ViewComponent("LessonList", new UserCourseItem { userid = userId, courseid = courseId });
    }

    // GET: Dashboard
    public async Task<ActionResult> SetPassword(string token, int id)
    {
      if (token == null) return RedirectToAction("Index", "Home");
      User user = await _courseService.GetUserByToken(token);
      if (user != null && user.Token.ToString() == token)
      {
        using (DuwaysContext db = new DuwaysContext(new DbContextOptions<DuwaysContext>() { }))
        {
          Log sessionLog = new Log();
          sessionLog.LoginDT = DateTime.Now;
          sessionLog.UserId = user.Id;
          db.Log.Add(sessionLog);
          db.SaveChanges();
          ActionLog lg = new ActionLog();
          lg.UserId = user.Id;
          lg.StartTime = DateTime.Now;
          lg.SessionId = sessionLog.Id;
          lg.Remarks = "Login";
          lg.ActionTypeId = 1;
          db.ActionLog.Add(lg);
          db.SaveChanges();

          //Session["SessionID"] = sessionLog.Id;
          HttpContext.Session.SetString("SessionID", sessionLog.Id.ToString());

          //Session["UserID"] = user.Id;
          HttpContext.Session.SetString("UserID", user.Id.ToString());

          //Session["IsAuthor"] = user.IsAuthor;
          HttpContext.Session.SetString("IsAuthor", user.IsAuthor.ToString());

          //Session["IsAdmin"] = user.IsAdmin;
          HttpContext.Session.SetString("IsAdmin", user.IsAdmin.ToString());

          //Session["Email"] = user.Email;
          HttpContext.Session.SetString("Email", user.Email);

          //Session["LoginName"] = user.LoginName;
          HttpContext.Session.SetString("LoginName", user.LoginName);

          //Session["IsActivated"] = false;
          HttpContext.Session.SetString("IsActivated", false.ToString());

          //Session["UserUrl"] = user.UrlName;
          HttpContext.Session.SetString("UserUrl", user.UrlName);
        }
        return View(user);
      }
      else return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult ChangePassword(ChangePasswordModel model)
    {
      if (ModelState.IsValid)
      {
        //videodbEntities db = //GlobalModel.GetDB((int)Session["UserID"]);
        using (DuwaysContext db = new DuwaysContext(new DbContextOptions<DuwaysContext>() { }))
        {
          User user = db.User.FirstOrDefault(u => u.Id == model.userid);

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
            HttpContext.Session.SetString("IsActivated", true.ToString());
            //Session["IsActivated"] = true;
          }
          catch (Exception)
          {
            return RedirectToAction("SignUp", new { res = "failed" });
          }

          string userId = HttpContext.Session.GetString("UserID");
          GlobalStats.LoggedInUsers.Add(userId, user.Email);
          return RedirectToAction("Index");
        }
      }
      return RedirectToAction("Login", new { res = "regfailed" });
    }
  }
}
