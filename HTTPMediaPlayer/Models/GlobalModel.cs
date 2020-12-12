using System;
using System.Web.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using HTTPMediaPlayer.Models.DB;
using HTTPMediaPlayer.Models;

namespace HTTPMediaPlayer.Models
{
  public static class GlobalModel
  {
    public static int MainPageVideoId = 1;
    public static string WebSiteName = WebConfigurationManager.AppSettings["websitename"];
    public static videodbEntities MainDB = new videodbEntities();

    private static InMemoryCache cacheProvider;

    private static List<vd_Courses> freeCourses;
    private static List<vd_Courses> adultsCourses;
    private static List<vd_Courses> kidsCourses;
    private static List<vd_Courses> notFreeCourses;

    public static Dictionary<object, vd_Users> LoggedInUsers { get; } = new Dictionary<object, vd_Users>();

    public static Dictionary<object, VideoStream> UsersVideos { get; } = new Dictionary<object, VideoStream>();


    public static List<vd_Courses> FreeCourses
    {
      get
      {
        //if (freeCourses == null)
        //{
          freeCourses = GetFreeCourses();
        //}
        return freeCourses;
      }
    }


    public static List<vd_Courses> WritingCourses
    {
      get
      {
        //if (freeCourses == null)
        //{
        adultsCourses = GetWritingCourses();
        //}
        return adultsCourses;
      }
    }
    public static List<vd_Courses> LanguageCourses
    {
      get
      {
        //if (freeCourses == null)
        //{
        adultsCourses = GetLanguageCourses();
        //}
        return adultsCourses;
      }
    }
    public static List<vd_Courses> KidsCourses
    {
      get
      {
        //if (freeCourses == null)
        //{
        kidsCourses = GetCoursesForKids();
        //}
        return kidsCourses;
      }
    }

    public static List<vd_Courses> NotFreeCourses
    {
      get
      {
        //if (notFreeCourses == null)
        //{
          notFreeCourses = GetNotFreeCourses();
        //}
        return notFreeCourses;
      }
    }

    public static vd_Courses Ladamtam
    {
      get
      {
        using (videodbEntities db = new videodbEntities())
        {
          vd_Courses book = db.vd_Courses.FirstOrDefault(c => c.UrlName == "ladambook");
          return book;
        }
      }
    }

    private static CountryDirectory countryDirectory;
    public static CountryDirectory CountryDirectory
    {
      get
      {
        if (countryDirectory == null)
        {
          countryDirectory = new CountryDirectory();
          return countryDirectory;
        }
        else
          return countryDirectory;
      }
    }





    static GlobalModel()
    {
      cacheProvider =
        new InMemoryCache(Convert.ToInt32(WebConfigurationManager.AppSettings["CacheTimeout"]));
    }


    public static vd_UserCourses GetUserCourse(int userId, int courseId)
    {
      using (videodbEntities db = new videodbEntities())
      {
        vd_UserCourses myUserCourse = db.vd_UserCourses.FirstOrDefault(ul => ul.vd_Users.Id == userId && ul.CourseId == courseId);
        return myUserCourse;
      }
    }

    public static List<vd_UserLessons> GetUserLessons(int userId, int courseId)
    {
      using (videodbEntities db = new videodbEntities())
      {
        List<vd_UserLessons> myUserCourse = db.vd_UserLessons.Where(ul => ul.vd_Users.Id == userId && ul.CourseId == courseId).ToList();
        return myUserCourse;
      }
    }

    public static vd_Files GetFile(string token)
    {
      using (videodbEntities db = new videodbEntities())
      {
        vd_UserLessons myUserCourse = db.vd_UserLessons.FirstOrDefault(ul => ul.UniqueId.ToString() == token);
        return myUserCourse.vd_Lessons.vd_Files;
      }
    }

    public static vd_Users GetUserById(int id)
    {
      vd_Users user;
      videodbEntities db = new videodbEntities(); 
      using (db)
      {
        user = db.vd_Users.FirstOrDefault(c => c.Id == id);
      }
        if (user != null)
          return user;
        else return null;
    }

    public static vd_Users GetUserByEmail(string email)
    {
      vd_Users user;
      videodbEntities db = new videodbEntities();
      using (db)
      {
        user = db.vd_Users.FirstOrDefault(c => c.Email == email);
      }
      if (user != null)
        return user;
      else return null;
    }


    public static Guid? ChangeUserLessonGuidById(int Id)
    {
      videodbEntities db = new videodbEntities();//GetDB(userId);
      using (db)
      {
        Guid newGuid = Guid.NewGuid();
        vd_UserLessons lesson = db.vd_UserLessons.FirstOrDefault(f => f.Id == Id);
        if (lesson != null)
        {
          lesson.UniqueId = newGuid;
          lesson.IsReadByUser = true;
          db.SaveChanges();
          return newGuid;
        }
        return null;
      }
    }

    public static Guid? ChangeUserLessonGuidByUserLesson(vd_UserLessons lesson)
    {
      videodbEntities db = new videodbEntities();//GetDB(userId);
      using (db)
      {
        Guid newGuid = Guid.NewGuid();
        //vd_UserLessons lesson = db.vd_UserLessons.FirstOrDefault(f => f.Id == Id);
        if (lesson != null)
        {
          lesson.UniqueId = newGuid;
          lesson.IsReadByUser = true;
          db.SaveChanges();
          return newGuid;
        }
        return null;
      }
    }

    public static Guid? ChangeUserLessonGuidByGuid(string Id)
    {
      videodbEntities db = new videodbEntities();//GetDB(userId);
      using (db)
      {
        Guid newGuid = Guid.NewGuid();
        vd_UserLessons lesson = db.vd_UserLessons.FirstOrDefault(f => f.UniqueId.ToString() == Id);
        if (lesson != null)
        {
          lesson.UniqueId = newGuid;
          lesson.IsReadByUser = true;
          db.SaveChanges();
          return newGuid;
        }
        return null;
      }
    }
    public static VideoStream GetVideoStream(object key)
    {
      if (UsersVideos.ContainsKey(key))
      {
        try
        {
          Guid? newId = ChangeUserLessonGuidById(UsersVideos[key].UserLesson.Id);
          UsersVideos[key].UserLesson.UniqueId = (Guid)newId;
        }
        catch { }
        return UsersVideos[key];
      }
      else
        return null;
    }

    /// <summary>
    /// GetUserLessonByUniqueId from Videomodel and API controller
    /// </summary>
    /// <param name="uniqueId"></param>
    /// <returns></returns>
    public static vd_UserLessons GetUserLessonByUniqueId(string uniqueId)
    {
      //videodbEntities db = GetDB(userId);
      vd_UserLessons file;
      using (videodbEntities db = new videodbEntities())
      {
        file = db.vd_UserLessons.FirstOrDefault(f => f.UniqueId == new Guid(uniqueId));
      }
      return file;
    }
    public static vd_Lessons GetUserLessonById(int Id)
    {
      //videodbEntities db = GetDB(userId);
      vd_Lessons file;
      using (videodbEntities db = new videodbEntities())
      {
        file = db.vd_Lessons.FirstOrDefault(f => f.Id == Id);
      }
      return file;
    }

    public static List<vd_UserLessons> GetUserLessonsByUserId(int userId)
    {
      using (videodbEntities db = new videodbEntities())
      {
        return db.vd_UserLessons.Where(f => f.UserId == userId).ToList();
      }

    }
    public static vd_UserLessons GetUserLessonsByUserIAndLesson(int lessonId, int userId)
    {
      using (videodbEntities db = new videodbEntities())
      {
        return db.vd_UserLessons.FirstOrDefault(f => f.UserId == userId && f.LessonId == lessonId);
      }

    }

    public static vd_Courses GetCourseByURL(string url_name)
    {
      using (videodbEntities db = new videodbEntities())
      {
        vd_Courses course = db.vd_Courses.Where(uc => uc.UrlName == url_name).FirstOrDefault();
        return course;
      }
    }

    private static List<vd_Courses> GetFreeCourses()
    {
      using (videodbEntities db = new videodbEntities())
      {
        List<vd_Courses> courses = db.vd_Courses.Where(uc => uc.HasTrial == true).ToList();
        return courses;
      }
    }
    private static List<vd_Courses> GetWritingCourses()
    {
      using (videodbEntities db = new videodbEntities())
      {
        List<vd_Courses> courses = db.vd_Courses.Where(uc => uc.IsWritingCourse == true).OrderBy(d => d.Price).ToList();
        return courses;
      }
    }
    private static List<vd_Courses> GetLanguageCourses()
    {
      using (videodbEntities db = new videodbEntities())
      {
        List<vd_Courses> courses = db.vd_Courses.Where(uc => uc.IsLanguageCourse == true).OrderBy(d => d.Price).ToList();
        return courses;
      }
    }
    private static List<vd_Courses> GetCoursesForKids()
    {
      using (videodbEntities db = new videodbEntities())
      {
        List<vd_Courses> courses = db.vd_Courses.Where(uc => uc.IsForKids == true).OrderBy(d => d.Price).ToList();
        return courses;
      }
    }


    private static List<vd_Courses> GetNotFreeCourses()
    {
      using (videodbEntities db = new videodbEntities())
      {
        List<vd_Courses> courses = db.vd_Courses.Where(uc => uc.Price != 0).ToList();
        return courses;
      }
    }

    public static List<vd_UserCourses> GetUserCourses(videodbEntities db, int courseid, int userid)
    {
      List<vd_UserCourses> courses = db.vd_UserCourses.Where(uc => uc.CourseId == courseid && uc.UserId == userid).ToList();
      if (courses.Count > 0)
        return courses;
      else
        return null;
    }

    public static bool ActivateUserCourse(videodbEntities db, int courseid, int userid)
    {
      List<vd_UserCourses> courses = GetUserCourses(db, courseid, userid);
      if (courses != null)
      {
        foreach (var course in courses)
        {
          course.IsActivated = true;
          course.PaymentDate = DateTime.Now;
          course.IsPaid = true;
        }
        try
        {
          db.SaveChanges();
        }
        catch { return false; }
        return true;
      }
      else
        return false;
    }

    public static vd_UserCourses SubscribeUserToCourse(videodbEntities db, LoginModel objUser)
    {
      vd_Users user = db.vd_Users.FirstOrDefault(u => u.Email == objUser.email);
      if (user == null)
      {
        user = new vd_Users();
        user.Email = objUser.email;
        user.Phone = objUser.phone;
        user.Name = objUser.name;
        user.Surname = objUser.surname;
        user.Address = objUser.address + (string.IsNullOrEmpty(objUser.fathername) ? "" : " Отчество: " + objUser.fathername) + (string.IsNullOrEmpty(objUser.zip) ? "" : " Индекс: " + objUser.zip);
        user.City = objUser.city;
        user.CountryId = objUser.countryId;
        user.IsAuthor = false;
        user.IsAdmin = false;
        user.Token = Guid.NewGuid();
        user.IsActivated = false;
        user.RegistrationDate = DateTime.Now;
        objUser.password = CryptoModel.CreatePassword(8);
        user.Password = CryptoModel.GetMD5HashString(objUser.password);

        var userurl = objUser.name.Replace(" ", "").ToLower();
        if(objUser.surname != null) userurl = userurl + "-" + objUser.surname.Replace(" ", "").ToLower();
        user.UrlName = userurl;
        var findurl = db.vd_Users.FirstOrDefault(eu => eu.UrlName == userurl);
        int i = 1;
        for (; findurl != null; i++)
        {
          findurl = db.vd_Users.FirstOrDefault(eu => eu.UrlName == userurl + i.ToString());
          user.UrlName = userurl + i.ToString();
        }
        user.LoginName = objUser.name.Replace(" ", "");
        if (objUser.surname != null) user.LoginName = user.LoginName + " " + objUser.surname.Replace(" ", "");

        db.vd_Users.Add(user);
        try
        {
          db.SaveChanges();
        }
        catch (Exception)
        {
          return null;
        }
      }

      vd_Courses cs = db.vd_Courses.Where(uc => uc.UrlName == objUser.course).FirstOrDefault();//GlobalModel.GetCourseByURL(objUser.course);
      List<vd_UserCourses> ucs = GetUserCourses(db, cs.Id, user.Id);

      if (ucs == null)
      {
        vd_UserCourses uc = new vd_UserCourses();
        uc.CourseId = cs.Id;
        uc.UserId = user.Id;
        uc.IsPaid = cs.Price == 0 ? true : false;
        uc.IsActivated = true;
        uc.SubscriptionDate = DateTime.Now;
        db.vd_UserCourses.Add(uc);

        int i = 0;
        foreach (var item in cs.vd_CourseLessons)
        {
          vd_UserLessons ulsn = new vd_UserLessons();
          ulsn.UserId = user.Id;
          ulsn.CourseId = cs.Id;
          ulsn.LessonId = item.LessonId;
          ulsn.ActivationDate = DateTime.Now > cs.ActivationDate ? DateTime.Now.Date + new TimeSpan(i, 0, 0, 0) : cs.ActivationDate.Value.Date + new TimeSpan(i, 0, 0, 0);
          ulsn.UniqueId = Guid.NewGuid();
          db.vd_UserLessons.Add(ulsn);
          i = i + (int)cs.ActivationFreq;
        }

        try
        {
          db.SaveChanges();
          return uc;
        }
        catch (Exception)
        {
          return null;
        }
      }
      else
        return ucs.FirstOrDefault();
    }

    /// <summary>
    /// GetUserByToken for Set new password
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public static vd_Users GetUserByToken(string token)
    {
      using (videodbEntities db = new videodbEntities())
      {
        Guid gtoken = new Guid(token);
        vd_Users user = db.vd_Users.FirstOrDefault(c => c.Token == gtoken);
        return user;
      }
    }

    public static void OnSessionEnd(int? userId, int? sessionId)
    {
      if (userId != null && userId != 0)
      {

        //int? sessionId = Convert.ToInt32(Session["SessionID"]);
        if (sessionId != null && sessionId != 0)
        {
          if (GlobalModel.LoggedInUsers.ContainsKey((int)userId))
          {
            GlobalModel.LoggedInUsers.Remove(userId);
          }
          var x = GlobalModel.UsersVideos.Where(d => d.Value.UserLesson.UserId == userId).ToList();
          for (int i = 0; i < x.Count(); i++)
            GlobalModel.UsersVideos.Remove(x[i].Key);
          using (videodbEntities db = new videodbEntities())
          {
            vd_Log sessionLog = db.vd_Log.FirstOrDefault(log => log.Id == sessionId);
            if (sessionLog != null) sessionLog.LogoutDT = DateTime.Now;
            vd_ActionLog lg = new vd_ActionLog();
            lg.UserId = (int)userId;
            lg.StartTime = DateTime.Now;
            if (sessionLog != null) lg.SessionId = sessionLog.Id;
            lg.Remarks = "Logout";
            lg.ActionType = 2;
            db.vd_ActionLog.Add(lg);
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

  }
}