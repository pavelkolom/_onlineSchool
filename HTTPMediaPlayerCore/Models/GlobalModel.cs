using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;



namespace HTTPMediaPlayerCore.Models
{
  public static class GlobalModel
  {
    public static int MainPageVideoId = 1;
    //public static string WebSiteName = WebConfigurationManager.AppSettings["websitename"];
    //public static videodbEntities MainDB = new videodbEntities();

    //private static InMemoryCache cacheProvider;

    private static List<Course> freeCourses;
    private static List<Course> adultsCourses;
    private static List<Course> kidsCourses;
    private static List<Course> notFreeCourses;

    //private static Dictionary<int, videodbEntities> dbDictionary = new Dictionary<int, videodbEntities>();
    private static Dictionary<object, User> loggedInUsers = new Dictionary<object, User>();
    private static Dictionary<object, VideoStream> usersVideos = new Dictionary<object, VideoStream>();


    public static List<Course> FreeCourses
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


    public static List<Course> WritingCourses
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
    public static List<Course> LanguageCourses
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
    public static List<Course> KidsCourses
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

    public static List<Course> NotFreeCourses
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

    public static Course Ladamtam
    {
      get
      {
        using (DuwaysContext db = new DuwaysContext(new DbContextOptions<DuwaysContext>()))
        {
          Course book = db.Course.FirstOrDefault(c => c.UrlName == "ladambook");
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

    public static Dictionary<object, User> LoggedInUsers
    {
      get
      {
        return loggedInUsers;
      }
    }

    public static Dictionary<object, VideoStream> UsersVideos
    {
      get
      {
        return usersVideos;
      }
    }

    static GlobalModel()
    {
      //cacheProvider =
      //  new InMemoryCache(Convert.ToInt32(WebConfigurationManager.AppSettings["CacheTimeout"]));
    }


    public static UserCourse GetUserCourse(int userId, int courseId)
    {
      using (DuwaysContext db = new DuwaysContext(new DbContextOptions<DuwaysContext>()))
      {
        UserCourse myUserCourse = db.UserCourse.FirstOrDefault(ul => ul.User.UserId == userId && ul.CourseId == courseId);
        return myUserCourse;
      }
    }

    public static List<UserLesson> GetUserLessons(int userId, int courseId)
    {
      using (DuwaysContext db = new DuwaysContext(new DbContextOptions<DuwaysContext>()))
      {
        List<UserLesson> myUserCourse = db.UserLesson.Where(ul => ul.User.UserId == userId && ul.CourseId == courseId).ToList();
        return myUserCourse;
      }
    }

    public static File GetFile(string token)
    {
      using (DuwaysContext db = new DuwaysContext(new DbContextOptions<DuwaysContext>()))
      {
        UserLesson myUserCourse = db.UserLesson.FirstOrDefault(ul => ul.UniqueId.ToString() == token);
        return myUserCourse.Lesson.File;
      }
    }

    public static User GetUserById(int id)
    {
      User user;
      DuwaysContext db = new DuwaysContext(new DbContextOptions<DuwaysContext>());
      using (db)
      {
        user = db.User.FirstOrDefault(c => c.UserId == id);
      }
        if (user != null)
          return user;
        else return null;
    }

    public static User GetUserByEmail(string email)
    {
      User user;
      DuwaysContext db = new DuwaysContext(new DbContextOptions<DuwaysContext>());
      using (db)
      {
        user = db.User.FirstOrDefault(c => c.Email == email);
      }
      if (user != null)
        return user;
      else return null;
    }


    public static Guid? ChangeUserLessonGuidById(int Id)
    {
      DuwaysContext db = new DuwaysContext(new DbContextOptions<DuwaysContext>());
      using (db)
      {
        Guid newGuid = Guid.NewGuid();
        UserLesson lesson = db.UserLesson.FirstOrDefault(f => f.UserId == Id);
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

    public static Guid? ChangeUserLessonGuidByUserLesson(UserLesson lesson)
    {
      DuwaysContext db = new DuwaysContext(new DbContextOptions<DuwaysContext>());
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
      DuwaysContext db = new DuwaysContext(new DbContextOptions<DuwaysContext>());
      using (db)
      {
        Guid newGuid = Guid.NewGuid();
        UserLesson lesson = db.UserLesson.FirstOrDefault(f => f.UniqueId.ToString() == Id);
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
      if (usersVideos.ContainsKey(key))
      {
        try
        {
          Guid? newId = ChangeUserLessonGuidById(usersVideos[key].UserLesson.UserLessonId);
          usersVideos[key].UserLesson.UniqueId = (Guid)newId;
        }
        catch { }
        return usersVideos[key];
      }
      else
        return null;
    }

    /// <summary>
    /// GetUserLessonByUniqueId from Videomodel and API controller
    /// </summary>
    /// <param name="uniqueId"></param>
    /// <returns></returns>
    public static UserLesson GetUserLessonByUniqueId(string uniqueId)
    {
      //videodbEntities db = GetDB(userId);
      UserLesson file;
      using (DuwaysContext db = new DuwaysContext(new DbContextOptions<DuwaysContext>()))
      {
        file = db.UserLesson.FirstOrDefault(f => f.UniqueId == new Guid(uniqueId));
      }
      return file;
    }
    public static Lesson GetUserLessonById(int Id)
    {
      //videodbEntities db = GetDB(userId);
      Lesson file;
      using (DuwaysContext db = new DuwaysContext(new DbContextOptions<DuwaysContext>()))
      {
        file = db.Lesson.FirstOrDefault(f => f.LessonId == Id);
      }
      return file;
    }

    public static List<UserLesson> GetUserLessonsByUserId(int userId)
    {
      using (DuwaysContext db = new DuwaysContext(new DbContextOptions<DuwaysContext>()))
      {
        return db.UserLesson.Where(f => f.UserId == userId).ToList();
      }

    }
    public static UserLesson GetUserLessonsByUserIAndLesson(int lessonId, int userId)
    {
      using (DuwaysContext db = new DuwaysContext(new DbContextOptions<DuwaysContext>()))
      {
        return db.UserLesson.FirstOrDefault(f => f.UserId == userId && f.LessonId == lessonId);
      }

    }

    public static Course GetCourseByURL(string url_name)
    {
      using (DuwaysContext db = new DuwaysContext(new DbContextOptions<DuwaysContext>()))
      {
        Course course = db.Course.Where(uc => uc.UrlName == url_name).FirstOrDefault();
        return course;
      }
    }

    private static List<Course> GetFreeCourses()
    {
      using (DuwaysContext db = new DuwaysContext(new DbContextOptions<DuwaysContext>()))
      {
        List<Course> courses = db.Course.Where(uc => uc.HasTrial == true).ToList();
        return courses;
      }
    }
    private static List<Course> GetWritingCourses()
    {
      using (DuwaysContext db = new DuwaysContext(new DbContextOptions<DuwaysContext>()))
      {
        List<Course> courses = db.Course.Where(uc => uc.IsWritingCourse == true).OrderBy(d => d.Price).ToList();
        return courses;
      }
    }
    private static List<Course> GetLanguageCourses()
    {
      using (DuwaysContext db = new DuwaysContext(new DbContextOptions<DuwaysContext>()))
      {
        List<Course> courses = db.Course.Where(uc => uc.IsLanguageCourse == true).OrderBy(d => d.Price).ToList();
        return courses;
      }
    }
    private static List<Course> GetCoursesForKids()
    {
      using (DuwaysContext db = new DuwaysContext(new DbContextOptions<DuwaysContext>()))
      {
        List<Course> courses = db.Course.Where(uc => uc.IsForKids == true).OrderBy(d => d.Price).ToList();
        return courses;
      }
    }


    private static List<Course> GetNotFreeCourses()
    {
      using (DuwaysContext db = new DuwaysContext(new DbContextOptions<DuwaysContext>()))
      {
        List<Course> courses = db.Course.Where(uc => uc.Price != 0).ToList();
        return courses;
      }
    }

    public static List<UserCourse> GetUserCourses(DuwaysContext db, int courseid, int userid)
    {
      List<UserCourse> courses = db.UserCourse.Where(uc => uc.CourseId == courseid && uc.UserId == userid).ToList();
      if (courses.Count > 0)
        return courses;
      else
        return null;
    }

    public static bool ActivateUserCourse(DuwaysContext db, int courseid, int userid)
    {
      List<UserCourse> courses = GetUserCourses(db, courseid, userid);
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

    public static UserCourse SubscribeUserToCourse(DuwaysContext db, LoginModel objUser)
    {
      User user = db.User.FirstOrDefault(u => u.Email == objUser.email);
      if (user == null)
      {
        user = new User();
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

        string userurl = objUser.name.Replace(" ", "").ToLower();
        if(objUser.surname != null) userurl = userurl + "-" + objUser.surname.Replace(" ", "").ToLower();
        user.UrlName = userurl;
        var findurl = db.User.FirstOrDefault(eu => eu.UrlName == userurl);
        int i = 1;
        for (; findurl != null; i++)
        {
          findurl = db.User.FirstOrDefault(eu => eu.UrlName == userurl + i.ToString());
          user.UrlName = userurl + i.ToString();
        }
        user.LoginName = objUser.name.Replace(" ", "");
        if (objUser.surname != null) user.LoginName = user.LoginName + " " + objUser.surname.Replace(" ", "");

        db.User.Add(user);
        try
        {
          db.SaveChanges();
        }
        catch (Exception)
        {
          return null;
        }
      }

      Course cs = db.Course.Where(uc => uc.UrlName == objUser.course).FirstOrDefault();//GlobalModel.GetCourseByURL(objUser.course);
      List<UserCourse> ucs = GetUserCourses(db, cs.CourseId, user.UserId);

      if (ucs == null)
      {
        UserCourse uc = new UserCourse();
        uc.CourseId = cs.CourseId;
        uc.UserId = user.UserId;
        uc.IsPaid = cs.Price == 0 ? true : false;
        uc.IsActivated = true;
        uc.SubscriptionDate = DateTime.Now;
        db.UserCourse.Add(uc);

        int i = 0;
        foreach (var item in cs.CourseLessons)
        {
          UserLesson ulsn = new UserLesson();
          ulsn.UserId = user.UserId;
          ulsn.CourseId = cs.CourseId;
          ulsn.LessonId = item.LessonId;
          ulsn.ActivationDate = DateTime.Now > cs.ActivationDate ? DateTime.Now.Date + new TimeSpan(i, 0, 0, 0) : cs.ActivationDate.Value.Date + new TimeSpan(i, 0, 0, 0);
          ulsn.UniqueId = Guid.NewGuid();
          db.UserLesson.Add(ulsn);
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
    public static User GetUserByToken(string token)
    {
      using (DuwaysContext db = new DuwaysContext(new DbContextOptions<DuwaysContext>()))
      {
        Guid gtoken = new Guid(token);
        User user = db.User.FirstOrDefault(c => c.Token == gtoken);
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
          using (DuwaysContext db = new DuwaysContext(new DbContextOptions<DuwaysContext>()))
          {
            Log sessionLog = db.Log.FirstOrDefault(log => log.LogId == sessionId);
            if (sessionLog != null) sessionLog.LogoutDT = DateTime.Now;
            ActionLog lg = new ActionLog();
            lg.UserId = (int)userId;
            lg.StartTime = DateTime.Now;
            if (sessionLog != null) lg.SessionId = sessionLog.LogId;
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

  }
}