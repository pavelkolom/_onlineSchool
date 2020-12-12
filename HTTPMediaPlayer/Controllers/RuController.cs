using System;
using System.Web;
using System.Linq;
using System.Web.Mvc;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using HTTPMediaPlayer.Models;
using HTTPMediaPlayer.Models.DB;

namespace HTTPMediaPlayer.Controllers
{
  public class RuController : Controller
  {
    public ActionResult Index()
    {
      ViewBag.Title = "Home Page";
      return View();
    }

    public ActionResult Policy()
    {
      ViewBag.Title = "Home Page";
      return View();
    }

    public ActionResult DoneFor21()
    {
      ViewBag.Title = "Done For 21";
      return View();
    }

    public ActionResult EmailTemplate()
    {
      ViewBag.Title = "Home Page";
      return View();
    }
    public ActionResult EmailTemplate1()
    {
      ViewBag.Title = "Home Page";
      return View();
    }
    public ActionResult EmailTemplate2()
    {
      ViewBag.Title = "Home Page";
      return View();
    }
    public ActionResult EmailTemplate3()
    {
      ViewBag.Title = "Home Page";
      return View();
    }
    public ActionResult Signup()
    {
      ViewBag.Title = "Sign Up";
      return View();
    }
    public ActionResult Videos()
    {
      return View();
    }
    public ActionResult Users()
    {
      return View();
    }
    public ActionResult Ladamtam()
    {
      LoginModel model = new LoginModel();
      model.course = "ladambook";
      model.istrial = 0;
      return View(model);
    }

    public ActionResult Course(string name)
    {
      vd_Courses course = GlobalModel.GetCourseByURL(name);
      if (course != null)
        return View(course);
      else
        return View("Index");
    }

    public ActionResult MaterialKit()
    {
      ViewBag.Title = "MaterialKit";
      return View();
    }

    public ActionResult Pay()
    {
      ViewBag.Title = "MaterialKit";
      return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult LoginUser(LoginModel objUser)
    {
      if (ModelState.IsValid)
      {
        using (videodbEntities db = new videodbEntities())
        {
          vd_Users userobj = null;
          foreach (var user in db.vd_Users)
          {
            if (user.Email == objUser.email && user.Password ==
            CryptoModel.GetMD5HashString(objUser.password))
            {
              userobj = user;
              break;
            }
          }

          if (userobj != null && userobj.IsActivated == true)
          {
            vd_Log sessionLog = new vd_Log();
            sessionLog.LoginDT = DateTime.Now;
            sessionLog.UserId = userobj.Id;
            db.vd_Log.Add(sessionLog);
            db.SaveChanges();
            vd_ActionLog lg = new vd_ActionLog();
            lg.UserId = userobj.Id;
            lg.StartTime = DateTime.Now;
            lg.SessionId = sessionLog.Id;
            lg.Remarks = "Login";
            lg.ActionType = 1;
            db.vd_ActionLog.Add(lg);
            db.SaveChanges();

            Session["SessionID"] = sessionLog.Id;
            Session["UserID"] = objUser.id = userobj.Id;
            Session["IsAuthor"] = objUser.isAuthor = userobj.IsAuthor;
            Session["Email"] = userobj.Email.ToString();
            Session["LoginName"] = userobj.Name.ToString();
            Session["UserUrl"] = userobj.UrlName;
            Session["IsActivated"] = true;
            Session["IsAdmin"] = objUser.isAdmin = userobj.IsAdmin;
            if (!GlobalModel.LoggedInUsers.ContainsKey(Session["UserID"]))
              GlobalModel.LoggedInUsers.Add(Session["UserID"], userobj);
            return RedirectToAction("Index", "Dashboard");
          }
        }
      }
      return RedirectToAction("Login", new { res = "loginfailed" });
    }

    [HttpPost]
    public ActionResult PasswordRecovery(string email, string captcha)
    {
      if (ModelState.IsValid)
      {
        //validate captcha 
        if (Session["Captcha"] == null || Session["Captcha"].ToString() != captcha)
        {
          //dispay error and generate a new captcha 
          return Json(new { Success = false, Result = "Сумма рассчитана неверно, попробуйте еще раз" }, JsonRequestBehavior.AllowGet);
        }
        vd_Users user = GlobalModel.GetUserByEmail(email);
        if (user != null)
        {
          string url = Url.Action("SetPassword", "Dashboard", new { token = user.Token, id = user.Id }, this.Request.Url.Scheme);

          string body = System.IO.File.ReadAllText(Server.MapPath("~/EmailTemplates/PasswordRecovery.html")).Replace("{recoverylink}", url);
          Mailer.SendMessage(user.Email, "DuWays восстановление пароля", body, true);

          return Json(new { Success = true, Result = "Success", emailval = email }, JsonRequestBehavior.AllowGet);
        }
        else
          return Json(new { Success = false, Result = "Email введен неверно" }, JsonRequestBehavior.AllowGet);

      }
      return Json(new { Success = false, Result = "Ошибка. Попробуйте еще раз" }, JsonRequestBehavior.AllowGet);
    }

    [HttpPost]
    public ActionResult Subscribe(LoginModel objUser)
    {
      if (ModelState.IsValid)
      {
        //validate captcha 
        if (Session["Captcha"] == null || Session["Captcha"].ToString() != objUser.captcha)
        {
          //dispay error and generate a new captcha 
          return Json(new { Success = false, Result = "Сумма рассчитана неверно, попробуйте еще раз" }, JsonRequestBehavior.AllowGet);
        }

        vd_Courses course = GlobalModel.GetCourseByURL(objUser.course);
        if (course == null)
          return Json(new { Success = false, Result = "Курс не найден" }, JsonRequestBehavior.AllowGet);

        using (videodbEntities db = new videodbEntities())
        {
          vd_UserCourses userCource = GlobalModel.SubscribeUserToCourse(db, objUser);
          vd_Users user = userCource.vd_Users;


          if (objUser.istrial == 1)
          {
            if (user.IsActivated /*&& userCource.IsPaid == true*/)
            {
              return Json(new { Success = true, Result = "Success", userid = user.Id, courseid = course.Id, courseprice = objUser.courseprice }, JsonRequestBehavior.AllowGet);
            }

            if (!user.IsActivated && course.IsBook == false)
            {
              string url = Url.Action("SetPassword", "Dashboard", new { token = user.Token, id = user.Id }, this.Request.Url.Scheme);

              string body = System.IO.File.ReadAllText(Server.MapPath("~/EmailTemplates/Welcome.html")).Replace("{newuser}", objUser.name).Replace("{dashboardlink}", url);
              Mailer.SendMessage(user.Email, "DuWays подтверждение email", body, true);
              return Json(new { Success = true, Result = "Activation", userid = user.Id, courseid = objUser.course, courseprice = objUser.courseprice }, JsonRequestBehavior.AllowGet);
            }
            else
              return Json(new { Success = true, Result = "Success", userid = user.Id, courseid = course.Id, courseprice = objUser.courseprice }, JsonRequestBehavior.AllowGet);
          }
          else
          {
            //create order and send payment command
            vd_Orders order = new vd_Orders();
            order.IsPaid = false;
            order.UserCourseId = userCource.Id;
            order.CourseId = course.Id;
            order.CreationDateTime = DateTime.Now;
            order.UserId = user.Id;
            order.IsPaid = false;
            order.Sum = (int)objUser.courseprice;
            db.vd_Orders.Add(order);
            db.SaveChanges();

            string url = RobokassaLib.Robokassa.GetRedirectUrl((int)order.Sum, order.Id, user.Id, course.Name, course.Id, course.IsBook == true ? 1 : 0, objUser.isdigitaldownload, user.Email);
            return Json(new { Success = true, Result = "Purchase", redirect = url }, JsonRequestBehavior.AllowGet);
          }
        }
      }

      return Json(new { Success = false, Result = "В процессе регистрации возникла ошибка" }, JsonRequestBehavior.AllowGet);
    }

    public ActionResult Login(string res)
    {
      if (Session["UserID"] != null && Convert.ToBoolean(Session["IsActivated"]) == true)
        return RedirectToAction("Index", "Dashboard");
      return View();
    }

    public ActionResult Success(int? user, int? course)
    {
      if (user == null || course == null)
        return RedirectToAction("Fail", "payment");
      using (videodbEntities db = new videodbEntities())
      {
        vd_UserCourses uc = GlobalModel.GetUserCourses(db, (int)course, (int)user).FirstOrDefault();
        if (uc == null) return RedirectToAction("Fail", "payment");
        return View(new UserCourse { userid = (int)user, courseid = (int)course, /*orderid = uc.vd_Orders.FirstOrDefault().Id,*/ isbook = uc.vd_Courses.IsBook == true ? 1 : 0 });
      }
    }

    public ActionResult Activation()
    {
      return View();
    }
    public ActionResult Teste123()
    {
      return View();
    }
    public ActionResult Teste234()
    {
      return View();
    }

    public ActionResult Adaptive234()
    {
      return View();
    }


    public ActionResult Register(LoginModel model)
    {

      using (videodbEntities db = new videodbEntities())
      {
        vd_Courses cr = db.vd_Courses.Where(uc => uc.UrlName == model.course).FirstOrDefault();
        if (cr == null) return RedirectToAction("Index", "ru");
        vd_Users us = null;
        if (Session["UserID"] != null)
        {
          int session = (int)Session["UserID"];
          us = db.vd_Users.FirstOrDefault(c => c.Id == session);
        }

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
    }

    public ActionResult Logout()
    {
      if (Session["SessionID"] != null)
      {
        int sessionId = Convert.ToInt32(Session["SessionID"]);
        using (videodbEntities db = new videodbEntities())
        {
          vd_Log sessionLog = db.vd_Log.FirstOrDefault(log => log.Id == sessionId);
          sessionLog.LogoutDT = DateTime.Now;

          vd_ActionLog lg = new vd_ActionLog();
          lg.UserId = Convert.ToInt32(Session["UserID"]);
          lg.StartTime = DateTime.Now;
          lg.SessionId = sessionLog.Id;
          lg.Remarks = "Logout";
          lg.ActionType = 2;
          db.vd_ActionLog.Add(lg);

          db.SaveChanges();
        }
      }
      //add action log
      using (videodbEntities db = new videodbEntities())
      {
        db.SaveChanges();
      }
      //


      if (Session["UserID"] != null && GlobalModel.LoggedInUsers.ContainsKey(Session["UserID"]))
      {
        GlobalModel.LoggedInUsers.Remove(Session["UserID"]);
      }
      if (Session["UserID"] != null)
      {
        var x = GlobalModel.UsersVideos.Where(d => d.Value.UserId == Convert.ToInt32(Session["UserID"])).ToList();
        for (int i = 0; i < x.Count(); i++)
          GlobalModel.UsersVideos.Remove(x[i].Key);
      }

      Session["UserID"] = null;
      Session["AdminID"] = null;
      Session["LoginName"] = null;
      Session["Email"] = null;

      return RedirectToAction("Index", "ru");
    }

    public FileResult Download(string token)
    {
      vd_Files file = GlobalModel.GetFile(token);
      //string path = "C:\\Video\\Authors\\anna-kolomeitceva\\" + file.FileName + ".pdf";
      //var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
      //FileStreamResult result = new FileStreamResult(fileStream, MimeMapping.GetMimeMapping(path))
      //{
      //  FileDownloadName = file.FileName + ".pdf",

      //};

      //return result;

      var path = "C:\\Video\\Authors\\anna-kolomeitceva\\" + file.FileName + ".pdf";
      GlobalModel.ChangeUserLessonGuidByGuid(token);
      return File(path, "application/pdf", "Ladamtam.pdf");



      //string fileName = file.FileName + ".pdf";
      //byte[] fileBytes = System.IO.File.ReadAllBytes(path);
      //return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Pdf, fileName);
    }

    public ActionResult CaptchaImage(string prefix, bool noisy = true)
    {
      var rand = new Random((int)DateTime.Now.Ticks);
      //generate new question 
      int a = rand.Next(10, 99);
      int b = rand.Next(1, 10);
      var captcha = string.Format("{0} + {1} = ?", a, b);

      //store answer 
      Session["Captcha"]/* + prefix]*/ = a + b;

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
  }
}

