using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HTTPMediaPlayerCore.Models;
using Microsoft.AspNetCore.Http;
using HTTPMediaPlayerCore.Services;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using RobokassaLibCore;
using Newtonsoft.Json;
using System.Threading.Tasks.Dataflow;

namespace HTTPMediaPlayerCore.Controllers
{
  public class AccountController : Controller
  {

    //private readonly DuwaysContext _db;
    private readonly ICourseService _courseService;

    private readonly IWebHostEnvironment _hostingEnvironment;

    private readonly IConfiguration _configuration;

    public AccountController(IWebHostEnvironment hostingEnvironment, ICourseService courseService, IConfiguration configuration)
    {

      _configuration = configuration;
      _courseService = courseService;
      _hostingEnvironment = hostingEnvironment;

    }
    public IActionResult Index()
    {
      return View();
    }

    public async Task<ActionResult> Register(LoginModel model)
    {

      Course cr = await _courseService.GetCourseByURL(model.course);

      if (cr == null) return RedirectToAction("Index", "Home");
      User us = null;

      switch (model.needsdelivery)
      {
        case "option1":
          model.courseprice = cr.Price + 200;
          break;
        case "option2":
          model.courseprice = cr.Price;
          break;
        case "option3":
          model.courseprice = cr.PriceDownloadItem;
          model.isdigitaldownload = "1";
          break;
        default:
          model.courseprice = cr.Price;
          break;
      }
      if (us != null)
      {
        model.id = us.Id;
        model.address = us.Address;
        model.city = us.City;
        model.countryId = (int)us.CountryId;
        model.email = us.Email;
        model.phone = us.Phone;
        model.name = us.Name;
        model.surname = us.Surname;
      }
      return View(model);

    }

    public ActionResult CaptchaImage(string prefix, bool noisy = true)
    {
      var rand = new Random((int)DateTime.Now.Ticks);
      //generate new question 
      int a = rand.Next(10, 99);
      int b = rand.Next(1, 10);
      var captcha = string.Format("{0} + {1} = ?", a, b);

      //store answer 
      HttpContext.Session.SetString("Captcha", (a + b).ToString());

      //image stream 
      FileContentResult img = null;

      using (var mem = new MemoryStream())
      using (var bmp = new Bitmap(130, 30))
      using (var gfx = Graphics.FromImage((Image)bmp))
      {
        gfx.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
        gfx.SmoothingMode = SmoothingMode.AntiAlias;
        gfx.FillRectangle(Brushes.White, new Rectangle(0, 0, bmp.Width, bmp.Height));

        //add noise 
        if (noisy)
        {
          int i, r, x, y;
          var pen = new Pen(Color.Yellow);
          for (i = 1; i < 15; i++)
          {
            pen.Color = Color.FromArgb(
            (rand.Next(0, 255)),
            (rand.Next(0, 255)),
            (rand.Next(0, 255)));

            r = rand.Next(0, (130 / 3));
            x = rand.Next(0, 130);
            y = rand.Next(0, 30);

            gfx.DrawEllipse(pen, x - r, y - r, r, r);
          }
        }

        //add question 
        gfx.DrawString(captcha, new Font("Tahoma", 15), Brushes.Gray, 2, 3);

        //render as Jpeg 
        bmp.Save(mem, System.Drawing.Imaging.ImageFormat.Jpeg);
        img = this.File(mem.GetBuffer(), "image/Jpeg");
      }

      return img;
    }

    [HttpPost]
    public async Task<ActionResult> Subscribe(LoginModel objUser)
    {
      string serialized;
      if (ModelState.IsValid)
      {
        string captcha = HttpContext.Session.GetString("Captcha");

        //validate captcha 
        if (captcha == null || captcha != objUser.captcha)
        {
          serialized = JsonConvert.SerializeObject(new RegisterResult(false, "Сумма рассчитана неверно, попробуйте еще раз", null, null, null, null));
          return Content(serialized, "application/json");
        }

        var course = await _courseService.GetCourseByURL(objUser.course);

        if (course == null)
        {
          serialized = JsonConvert.SerializeObject(new RegisterResult(false, "Курс не найден", null, null, null, null));
          return Content(serialized, "application/json");
        }

        UserCourse userCource = await _courseService.SubscribeUserToCourse(objUser);
        User user = await _courseService.GetUserByEmail(objUser.email);
        Author courseauthor = await _courseService.GetAuthor(course.AuthorId);

        if (objUser.istrial == 1)
        {
          if (user.IsActivated)
          {
            serialized = JsonConvert.SerializeObject(new RegisterResult(true, "Success", null, user.Id, course.Id, objUser.courseprice));
            return Content(serialized, "application/json");
          }

          if (!user.IsActivated && course.IsBook == false)
          {
            string scheme = HttpContext.Request.Scheme;

            string url = Url.Action("SetPassword", "Dashboard", new { token = user.Token, id = user.Id }, scheme);
            string webRootPath = _hostingEnvironment.WebRootPath;
            string path = webRootPath + "\\EmailTemplates\\Welcome.html";

            string body = System.IO.File.ReadAllText(path).Replace("{newuser}", objUser.name).Replace("{dashboardlink}", url);
            Mailer m = new Mailer(this.Request.Host.Host);

            await m.SendMessageAsync(user.Email, "DuWays подтверждение email", body, true);


            serialized = JsonConvert.SerializeObject(new RegisterResult(true, "Activation", url, user.Id, objUser.id, objUser.courseprice));
            return Content(serialized, "application/json");

            //return Json(new { Success = true, Result = "Activation", userid = user.Id, courseid = objUser.course, courseprice = objUser.courseprice });
          }
          else
          {
            serialized = JsonConvert.SerializeObject(new RegisterResult(true, "Success", null, null, null, null));
            return Content(serialized, "application/json");

            //return Json(new { Success = true, Result = "Success", userid = user.Id, courseid = course.Id, courseprice = objUser.courseprice });
          }
        }
        else
        {
          Order order = new Order();
          order.IsPaid = false;
          order.UserCourseId = userCource.Id;
          order.CourseId = course.Id;
          order.AuthorId = course.AuthorId;
          order.CreationDateTime = DateTime.Now;
          order.UserId = user.Id;
          order.EMail = user.Email;
          order.IsPaid = false;
          order.Sum = (int)objUser.courseprice;
          using (DuwaysContext db = new DuwaysContext(new DbContextOptions<DuwaysContext>() { }))
          {
            db.Order.Add(order);
            db.SaveChanges();
          }

          Robokassa rk;
          if (courseauthor.HasOwnRobokassa == true)
          {
            rk = new Robokassa(_configuration,
              courseauthor.RobokassaShopId,
              courseauthor.RobokassaPassword1,
              courseauthor.RobokassaPassword2,
              courseauthor.RobokassaTestPassword1,
              courseauthor.RobokassaTestPassword2);
          }
          else
           rk = new Robokassa(_configuration);

          string url = rk.GetRedirectUrl((int)order.Sum, order.Id, user.Id, course.Name, course.Id, course.IsBook == true ? 1 : 0, course.IsBook == true ? "1" : "0");
          serialized = JsonConvert.SerializeObject(new RegisterResult(true, "Purchase", url, null, null, null));
          return Content(serialized, "application/json");
        }
      }

      serialized = JsonConvert.SerializeObject(
      new RegisterResult(false, "В процессе регистрации возникла ошибка", null, null, null, null));
      return Content(serialized, "application/json");
    }

    public ActionResult Login(string res)
    {
      string userId = HttpContext.Session.GetString("UserID");
      string isActivated = HttpContext.Session.GetString("IsActivated");


      if (!string.IsNullOrEmpty(userId) && isActivated == true.ToString())
        return RedirectToAction("Index", "Dashboard");
      
      return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task< ActionResult> LoginUser(LoginModel objUser)
    {
      if (ModelState.IsValid)
      {
        using (DuwaysContext db = new DuwaysContext(new DbContextOptions<DuwaysContext>() { }))
        {
          User userobj = await db.User.FirstOrDefaultAsync(u => u.Email == objUser.email);

          if (userobj != null && userobj.IsActivated == true && 
            userobj.Password == CryptoModel.GetMD5HashString(objUser.password))
          {
            Log sessionLog = new Log();
            sessionLog.LoginDT = DateTime.Now;
            sessionLog.UserId = userobj.Id;
            db.Log.Add(sessionLog);
            db.SaveChanges();
            
            ActionLog lg = new ActionLog();
            lg.UserId = userobj.Id;
            lg.StartTime = DateTime.Now;
            lg.SessionId = sessionLog.Id;
            lg.Remarks = "Login";
            lg.ActionTypeId = 1;
            db.ActionLog.Add(lg);
            await db.SaveChangesAsync();


            //Session["SessionID"] = sessionLog.Id;
            HttpContext.Session.SetString("SessionID", sessionLog.Id.ToString());

            //Session["UserID"] = objUser.id = userobj.Id;
            HttpContext.Session.SetString("UserID", userobj.Id.ToString());
            
            HttpContext.Session.SetString("AuthorID", userobj.AuthorID.ToString());

            //Session["IsAuthor"] = objUser.isAuthor = userobj.IsAuthor;
            HttpContext.Session.SetString("IsAuthor", userobj.IsAuthor.ToString());

            //Session["Email"] = userobj.Email.ToString();
            HttpContext.Session.SetString("Email", userobj.Email.ToString());


            //Session["LoginName"] = userobj.Name.ToString();
            HttpContext.Session.SetString("LoginName", userobj.Name.ToString());


            //Session["UserUrl"] = userobj.UrlName;
            HttpContext.Session.SetString("UserUrl", userobj.UrlName.ToString());


            //Session["IsActivated"] = true;
            HttpContext.Session.SetString("IsActivated", true.ToString());


            //Session["IsAdmin"] = objUser.isAdmin = userobj.IsAdmin;
            HttpContext.Session.SetString("IsAdmin", userobj.IsAdmin.ToString());

            if (!GlobalStats.LoggedInUsers.ContainsKey(userobj.Id.ToString()))
              GlobalStats.LoggedInUsers.Add(userobj.Id.ToString(), userobj.Email);


            return RedirectToAction("Index", "Dashboard");
          }
        }
      }
      return RedirectToAction("Login", new { res = "loginfailed" });
    }

    [HttpPost]
    public async Task<ActionResult> PasswordRecovery(string email, string captcha)
    {
      string serialized;
      if (ModelState.IsValid)
      {

        string captchaSession = HttpContext.Session.GetString("Captcha");
        //validate captcha 
        if (string.IsNullOrEmpty(captchaSession) || captchaSession != captcha)
        {
          //dispay error and generate a new captcha 
          serialized = JsonConvert.SerializeObject(new PasswordRecoveryModel(false, "Сумма рассчитана неверно, попробуйте еще раз", null));
          return Content(serialized, "application/json");
        }

        User user = await _courseService.GetUserByEmail(email);

        if (user != null)
        {
          string scheme = HttpContext.Request.Scheme;
          string url = Url.Action("SetPassword", "Dashboard", new { token = user.Token, id = user.Id }, scheme);

          string webRootPath = _hostingEnvironment.WebRootPath;
          string path = webRootPath + "\\EmailTemplates\\PasswordRecovery.html";


          string body = System.IO.File.ReadAllText(path).Replace("{recoverylink}", url);
          await new Mailer("duways.com").SendMessageAsync(user.Email, "DuWays восстановление пароля", body, true);

          serialized = JsonConvert.SerializeObject(new PasswordRecoveryModel(true, "Success", email));
          return Content(serialized, "application/json");

          //return Json(new { Success = true, Result = "Success", emailval = email });
        }
        else
        {
          serialized = JsonConvert.SerializeObject(new PasswordRecoveryModel(false, "Email введен неверно", null));
          return Content(serialized, "application/json");
          //return Json(new { Success = false, Result = "Email введен неверно" });
        }

      }
      serialized = JsonConvert.SerializeObject(new PasswordRecoveryModel(false, "Ошибка. Попробуйте еще раз", null));
      return Content(serialized, "application/json");
      //return Json(new { Success = false, Result = "Ошибка. Попробуйте еще раз" });
    }

    public ActionResult Logout()
    {
      string SessionID = HttpContext.Session.GetString("SessionID");
      string UserID = HttpContext.Session.GetString("UserID");
      if (SessionID != null)
      {
        int sessionId = Convert.ToInt32(SessionID);
        using (DuwaysContext db = new DuwaysContext(new DbContextOptions<DuwaysContext>() { }))
        {
          Log sessionLog = db.Log.FirstOrDefault(log => log.Id == sessionId);
          sessionLog.LogoutDT = DateTime.Now;


          ActionLog lg = new ActionLog();
          lg.UserId = Convert.ToInt32(UserID);
          lg.StartTime = DateTime.Now;
          lg.SessionId = sessionLog.Id;
          lg.Remarks = "Logout";
          lg.ActionTypeId = 2;
          db.ActionLog.Add(lg);

          db.SaveChanges();
        }
      }
      //add action log
      //using (videodbEntities db = new videodbEntities())
      //{
      //  db.SaveChanges();
      //}
      //


      if (UserID != null && GlobalStats.LoggedInUsers.ContainsKey(UserID))
      {
        GlobalStats.LoggedInUsers.Remove(UserID);
      }
      /*if (UserID != null)
      {
        var x = GlobalStats.UsersVideos.Where(d => d.Value.UserId == Convert.ToInt32(UserID)).ToList();
        for (int i = 0; i < x.Count(); i++)
          GlobalStats.UsersVideos.Remove(x[i].Key);
      }*/

      HttpContext.Session.SetString("UserID", "");

      HttpContext.Session.SetString("AdminID", "");

      HttpContext.Session.SetString("LoginName", "");

      HttpContext.Session.SetString("Email", "");

      return RedirectToAction("Index", "Home");
    }

  }
}
