using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using HTTPMediaPlayerCore.Models;
using Microsoft.EntityFrameworkCore;

namespace HTTPMediaPlayerCore.Services
{
  public interface ICourseService
  {
    public Task<List<Course>> GetLanguageCourses();

    public Task<List<Course>> GetWritingCourses();

    public Task<List<Course>> GetFreeCourses();

    public Task<CourceCategoryContainer> GetCourceCategoryContainer(int authorId, bool incluemainpageAuthors);

    public Task<CourceCategoryContainer> GetCourceCategoryContainer(string authorUrl, bool incluemainpageAuthors);

    public Task<Course> GetCourseByURL(string url_name);

    public Task<AuthorCourse> GetAuthorCourseByURL(string url_name);

    public Task<User> GetUserByURL(string url_name);

    public Task<UserCourse> SubscribeUserToCourse(LoginModel objUser);

    public Task<UserCourse> GetUserCourse(int courseid, int userid);

    public Task<Course> GetCourse(int courseid);

    public Task<UserLesson> GetUserLesson(int courseid, int userid, int lessonid);

    public Task<Lesson> GetLesson(int lessonid);

    public Task<File> GetFile(int fileid);

    public Task<File> GetFile(string token);

    public Task<List<CourseLesson>> GetCourseLessons(int courseid);

    public Task<User> GetUserByEmail(string email);

    public Task<User> GetUserById(int id); 
    
    public Task<User> GetUserByToken(string token);

    public Task<bool> ActivateUserCourse(int courseid, int userid);

    public Task<Guid?> ChangeUserLessonGuidByToken(Guid token);

    public Task<Guid?> ChangeUserLessonGuidByToken(string token);

    public Task<Author> GetAuthor(int authorId);
    public Task<AuthorCourse> GetAuthorByCourseId(int courseId);

    public Task<Author> GetAuthor(string authorUrl);

    //public Task<string> GetCoursePageHtml(int authorId, int courseId);

  }

  public class CourseService : ICourseService
  {
    private DuwaysContext dbcontext = new DuwaysContext(new DbContextOptions<DuwaysContext>() { });

    public async Task<bool> ActivateUserCourse(int courseid, int userid)
    {
      UserCourse course = await GetUserCourse(courseid, userid);
      if (course != null)
      {
        course.IsActivated = true;
        course.PaymentDate = DateTime.Now;
        course.IsPaid = true;

        try
        {
          await dbcontext.SaveChangesAsync();
        }
        catch { return false; }
        return true;
      }
      else
        return false;
    }


    public async Task<List<Course>> GetLanguageCourses()
    {
      List<Course> courses = null;
      courses = await dbcontext.Course.Where(uc => uc.IsLanguageCourse == true).OrderBy(d => d.Price).ToListAsync();
      return courses;
    }

    public async Task<List<Course>> GetWritingCourses()
    {
      List<Course> courses = null;
      courses = await dbcontext.Course.Where(uc => uc.IsWritingCourse == true).OrderBy(d => d.Price).ToListAsync();
      return courses;
    }

    public async Task<Author> GetAuthor(int authorId)
    {
      Author author = await dbcontext.Author.SingleOrDefaultAsync(uc => uc.Id == authorId);
      return author;
    }

    public async Task<AuthorCourse> GetAuthorByCourseId(int courseId)
    {
      AuthorCourse author = await dbcontext.AuthorCourse.SingleOrDefaultAsync(uc => uc.CourseId == courseId);
      return author;
    }


    public async Task<Author> GetAuthor(string authorUrl)
    {
      Author author = await dbcontext.Author.SingleOrDefaultAsync(uc => uc.AuthorUrl == authorUrl);
      return author;
    }

    public async Task<List<Course>> GetFreeCourses()
    {
      List<Course> courses = null;
      courses = await dbcontext.Course.Where(uc => uc.HasTrial == true).OrderBy(d => d.Price).ToListAsync();
      return courses;
    }

    public async Task<Course> GetCourseByURL(string url_name)
    {
      Course course = await dbcontext.Course.Where(uc => uc.UrlName == url_name).FirstOrDefaultAsync();
      return course;
    }
    public async Task<AuthorCourse> GetAuthorCourseByURL(string url_name)
    {
      AuthorCourse course = await dbcontext.AuthorCourse.Where(uc => uc.CourseName == url_name).FirstOrDefaultAsync();
      return course;
    }

    public async Task<User> GetUserByURL(string url_name)
    {
      User user = await dbcontext.User.Where(uc => uc.UrlName == url_name).FirstOrDefaultAsync();
      return user;
    }

    public async Task<User> GetUserByEmail(string email)
    {
      User user = null;
      try
      {
        user = await dbcontext.User.Where(u => u.Email == email).FirstOrDefaultAsync();
        return user;
      }
      catch (Exception ex)
      {
        return null;
      }
    }

    public async Task<User> GetUserById(int id)
    {
      User user = null;
      try
      {
        user = await dbcontext.User.Where(u => u.Id == id).FirstOrDefaultAsync();
        return user;
      }
      catch (Exception ex)
      {
        return null;
      }
    }

    public async Task<User> GetUserByToken(string token)
    {
      User user = null;
      try
      {
        Guid gtoken = new Guid(token);
        user = await dbcontext.User.Where(u => u.Token == gtoken).FirstOrDefaultAsync();
        return user;
      }
      catch (Exception ex)
      {
        return null;
      }
    }

    public async Task<UserCourse> SubscribeUserToCourse(LoginModel objUser)
    {
      using (DuwaysContext db = new DuwaysContext(new DbContextOptions<DuwaysContext>() { }))
      {
        User user = await GetUserByEmail(objUser.email);

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

          var userurl = objUser.name.Replace(" ", "").ToLower();
          if (objUser.surname != null) userurl = userurl + "-" + objUser.surname.Replace(" ", "").ToLower();
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

        Course cs = await db.Course.Where(uc => uc.UrlName == objUser.course).FirstOrDefaultAsync();//GlobalModel.GetCourseByURL(objUser.course);
        UserCourse ucs = await GetUserCourse(cs.Id, user.Id);

        if (ucs == null)
        {
          UserCourse uc = new UserCourse();
          uc.CourseId = cs.Id;
          uc.UserId = user.Id;
          uc.IsPaid = cs.Price == 0 ? true : false;
          uc.IsActivated = true;
          uc.SubscriptionDate = DateTime.Now;
          db.UserCourse.Add(uc);


          var lessons = await GetCourseLessons(cs.Id);
          int i = 0;
          DateTime activationDateFirst = DateTime.Now > cs.ActivationDate ? DateTime.Now.Date : cs.ActivationDate.Value.Date;
          foreach (var item in lessons)
          {
            UserLesson ulsn = new UserLesson();
            ulsn.UserId = user.Id;
            ulsn.CourseId = cs.Id;
            ulsn.LessonId = item.LessonId;
            if (item.IsForTrial == true) 
              ulsn.ActivationDate = activationDateFirst;
            else
              ulsn.ActivationDate = activationDateFirst + new TimeSpan(i, 0, 0, 0);
            ulsn.UniqueId = Guid.NewGuid();
            db.UserLesson.Add(ulsn);
            if (item.IsForTrial == false) i = i + (int)cs.ActivationFreq;
          }

          try
          {
            await db.SaveChangesAsync();
            return uc;
          }
          catch (Exception)
          {
            return null;
          }
        }
        else
          return ucs;
      }
    }

    public async Task<Guid?> ChangeUserLessonGuidByToken(Guid token)
    {
      Guid newGuid = Guid.NewGuid();
      UserLesson lesson = await dbcontext.UserLesson.FirstOrDefaultAsync(f => f.UniqueId == token);
      if (lesson != null)
      {
        lesson.UniqueId = newGuid;
        lesson.IsReadByUser = true;
        dbcontext.SaveChanges();
        return newGuid;
      }
      return null;
    }

    public async Task<Guid?> ChangeUserLessonGuidByToken(string token)
    {
      Guid guidToken = new Guid(token);
      return await ChangeUserLessonGuidByToken(guidToken);
    }

    public async Task<UserCourse> GetUserCourse(int courseid, int userid)
    {
      try
      {
        UserCourse course = await dbcontext.UserCourse.FirstOrDefaultAsync(uc => uc.CourseId == courseid && uc.UserId == userid);
        return course;
      }
      catch (Exception ex)
      {
        return null;
      }
    }

    public async Task<Course> GetCourse(int courseid)
    {
      try
      {
        Course course = await dbcontext.Course.FirstOrDefaultAsync(uc => uc.Id == courseid);
        return course;
      }
      catch (Exception ex)
      {
        return null;
      }
    }

    public async Task<UserLesson> GetUserLesson(int courseid, int userid, int lessonid)
    {
      try
      {
        UserLesson ul = await dbcontext.UserLesson.FirstOrDefaultAsync(
          ul => ul.LessonId == lessonid && ul.UserId == userid && ul.CourseId == courseid);
        return ul;
      }
      catch (Exception ex)
      {
        return null;
      }
    }

    public async Task<Lesson> GetLesson(int lessonid)
    {
      try
      {
        Lesson l = await dbcontext.Lesson.FirstOrDefaultAsync(l => l.Id == lessonid);
        return l;
      }
      catch (Exception ex)
      {
        return null;
      }

    }
    public async Task<File> GetFile(int fileid)
    {
      try
      {
        File l = await dbcontext.File.FirstOrDefaultAsync(l => l.Id == fileid);
        return l;
      }
      catch (Exception ex)
      {
        return null;
      }
    }

    public async Task<File> GetFile(string token)
    {
      UserLesson ul = await dbcontext.UserLesson.FirstOrDefaultAsync(ul => ul.UniqueId.ToString() == token);
      Lesson lesson = await dbcontext.Lesson.FirstOrDefaultAsync(l => l.Id == ul.LessonId);
      File file = await dbcontext.File.FirstOrDefaultAsync(f => f.Id == lesson.FileId);

      return file;
    }

    public async Task<List<CourseLesson>> GetCourseLessons(int courseid)
    {
      try
      {
        List<CourseLesson> lessons = await dbcontext.CourseLesson.Where(uc => uc.CourseId == courseid).OrderBy(l => l.OrderNumber).ToListAsync();
        return lessons;
      }
      catch (Exception ex)
      {
        return null;
      }
    }

    public async Task<List<Category>> GetCategories()
    {
      try
      {
        List<Category> ctg = await dbcontext.Category.OrderBy(l => l.Id).ToListAsync();
        return ctg;
      }
      catch (Exception ex)
      {
        return null;
      }
    }

    public async Task<CourceCategoryContainer> GetCourceCategoryContainer(int authorId, bool incluemainpageAuthors)
    {
      var author = await GetAuthor(authorId);

      string readText = "";
      if (author != null)
      {
        string path = @"c:\Duways\Pages\" + authorId + "\\" + "main.html";
        // Open the file to read from.
        if (System.IO.File.Exists(path))
        {
          readText = await System.IO.File.ReadAllTextAsync(path);
        }
      }
      List<Author> authors = new List<Author>();
      if(incluemainpageAuthors)
      {
        foreach(int id in GlobalStats.Authors)
        {
          Author a = await dbcontext.Author.FirstOrDefaultAsync(au => au.Id == id);
          if (author != null) authors.Add(a);
        }
      }

      var container = new CourceCategoryContainer(author, GlobalStats.Categories, readText, authors);
      return container;
    }





    public async Task<CourceCategoryContainer> GetCourceCategoryContainer(string authorUrl, bool incluemainpageAuthors)
    {
      var author = await GetAuthor(authorUrl);
      string readText = "";
      if (author != null)
      {
        string path = @"c:\Duways\Pages\" + author.Id + "\\" + "main.html";
        // Open the file to read from.
        if (System.IO.File.Exists(path))
        {
          readText = await System.IO.File.ReadAllTextAsync(path);
        }
      }

      List<Author> authors = new List<Author>();
      if (incluemainpageAuthors)
      {
        foreach (int id in GlobalStats.Authors)
        {
          Author a = await dbcontext.Author.FirstOrDefaultAsync(au => au.Id == id);
          if (author != null) authors.Add(a);
        }
      }
      var container = new CourceCategoryContainer(author, GlobalStats.Categories, readText, authors);
      return container;
    }
  }
}
