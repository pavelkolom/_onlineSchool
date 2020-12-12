using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using RobokassaLibCore;
using HTTPMediaPlayerCore.Models;
using HTTPMediaPlayerCore.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace HTTPMediaPlayer.Controllers
{
  public class PaymentController : Controller
  {


    private readonly ICourseService _courseService;

    private readonly IWebHostEnvironment _hostingEnvironment;

    private readonly IConfiguration _configuration;

    Robokassa rk;

    public PaymentController(IWebHostEnvironment hostingEnvironment, ICourseService courseService, IConfiguration configuration)
    {

      _configuration = configuration;
      _courseService = courseService;
      _hostingEnvironment = hostingEnvironment;
      //rk = new Robokassa(_configuration);
      //_db = db;
    }


    // So called "Result Url" in terms of Robokassa documentation.
    // This url is called by Robokassa robot.
    public async Task<ActionResult> Result(RobokassaConfirmationRequest confirmationRequest)
    {
      try
      {
        Author courseauthor = await _courseService.GetAuthor(confirmationRequest.Shp_courseid);
        if(courseauthor.HasOwnRobokassa == true)
          rk = new Robokassa(_configuration, courseauthor.RobokassaShopId, courseauthor.RobokassaPassword1, courseauthor.RobokassaPassword2, courseauthor.RobokassaTestPassword1, courseauthor.RobokassaTestPassword2);
        else
          rk = new Robokassa(_configuration);

        if (rk.RoboConfig.IsQueryValid(confirmationRequest, RobokassaQueryType.ResultURL, confirmationRequest.IsTest))
        {
          UserCourseItem uc = new UserCourseItem();
          uc.courseid = confirmationRequest.Shp_courseid;
          uc.orderid = confirmationRequest.InvId;
          uc.userid = confirmationRequest.Shp_userid;
          uc.isbook = confirmationRequest.Shp_isbook;
          uc.isdigital = confirmationRequest.Shp_isdigital == "1" ? true : false;
          processOrder(confirmationRequest, GetPathAndQuery());
          User user = null;
          if (confirmationRequest.Shp_userid != 0)
            user = await _courseService.GetUserById(confirmationRequest.Shp_userid);

          if (uc.isbook == 1)
          {
            using (DuwaysContext db = new DuwaysContext(new DbContextOptions<DuwaysContext>() { }))
            {
              //User usr = db.User.FirstOrDefault(c => c.Id == uc.userid);
              Course crs = db.Course.FirstOrDefault(c => c.Id == uc.courseid);
              UserLesson ladambook = db.UserLesson.FirstOrDefault(ul => ul.UserId == uc.userid && ul.CourseId == uc.courseid);
              if (confirmationRequest.Shp_isbook == 1 && ladambook != null /*&& ladambook.IsLinkSent != true*/ && uc.isdigital == true)
              {
                //ladambook.IsLinkSent = true;
                //db.SaveChanges();
                Guid token = ladambook.UniqueId;
                string scheme = HttpContext.Request.Scheme;
                //string url = Url.Action("Download", "Sample", new { token = token.ToString() }, scheme);
                string url = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host.Host + (HttpContext.Request.Host.Port != null ? ":" + HttpContext.Request.Host.Port : "") + "/api/sample/"+ crs.AuthorId + "/" + token.ToString();

                string webRootPath = _hostingEnvironment.WebRootPath;
                string path = webRootPath + "\\EmailTemplates\\Book.html";

                string imageurl = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host.Host + (HttpContext.Request.Host.Port != null ? ":" + HttpContext.Request.Host.Port : "") + "/Authors/" + crs.AuthorId + "/" + crs.Id + ".jpg";

                string body = System.IO.File.ReadAllText(path).Replace("{newuser}", user.Name).Replace("{booklink}", url).Replace("{bookdescription}", crs.PageText).Replace("{bookimage}", imageurl);
                await new Mailer().SendMessageAsync(user.Email, crs.Name, body, true);
                //return RedirectToAction("Activation", "ru");//Json(new { Success = true, Result = "Activation", userid = user.Id }, JsonRequestBehavior.AllowGet);
              }
            }
          }


          if (user != null && !user.IsActivated && uc.isbook == 0)
          {
            string scheme = HttpContext.Request.Scheme;
            string url = Url.Action("SetPassword", "Dashboard", new { token = user.Token, id = user.Id }, scheme);


            string webRootPath = _hostingEnvironment.WebRootPath;
            string path = webRootPath + "\\EmailTemplates\\Welcome.html";

            string body = System.IO.File.ReadAllText(path).Replace("{newuser}", user.Name).Replace("{dashboardlink}", url);
            await new Mailer().SendMessageAsync(user.Email, "DuWays account access confirmation", body, true);
          }


          return Content("OK"); // content for robot
        }
      }
      catch (Exception ex)
      {
        await new Mailer().SendMessageAsync("pavelkolom@gmail.com", "Failed transaction confirmation", GetPathAndQuery() + " " + ex.Message + " " + ex.InnerException != null ? ex.InnerException.Message : "", false);
        return Content("ERR");
      }

      await new Mailer().SendMessageAsync("pavelkolom@gmail.com", "Failed transaction confirmation", GetPathAndQuery(), false);
      return Content("ERR");
    }

    // So called "Success Url" in terms of Robokassa documentation.
    // Customer is redirected to this url after successful payment. 
    //http://duways.com/payment/success?inv_id=1&InvId=1&out_summ=10000.00&OutSum=10000.00&crc=2e045768330de6f7e4f4e30cbb451db9&SignatureValue=2e045768330de6f7e4f4e30cbb451db9&Culture=ru&IsTest=1
    [HttpGet]
    public async Task<ActionResult> Success([FromQuery] RobokassaConfirmationRequest confirmationRequest)
    {
      try
      {
        AuthorCourse courseauthor = await _courseService.GetAuthorByCourseId(confirmationRequest.Shp_courseid);
        if (courseauthor.Author.HasOwnRobokassa == true)
          rk = new Robokassa(_configuration, courseauthor.Author.RobokassaShopId, courseauthor.Author.RobokassaPassword1, courseauthor.Author.RobokassaPassword2, courseauthor.Author.RobokassaTestPassword1, courseauthor.Author.RobokassaTestPassword2);
        else
          rk = new Robokassa(_configuration);

        if (rk.RoboConfig.IsQueryValid(confirmationRequest, RobokassaQueryType.SuccessURL, confirmationRequest.IsTest))
        //if (confirmationRequest.IsQueryValid(RobokassaQueryType.SuccessURL, confirmationRequest.IsTest))
        {
          UserCourseItem uc = new UserCourseItem();
          uc.courseid = confirmationRequest.Shp_courseid;
          uc.orderid = confirmationRequest.InvId;
          uc.userid = confirmationRequest.Shp_userid;
          uc.isbook = confirmationRequest.Shp_isbook;

          using (DuwaysContext db = new DuwaysContext(new DbContextOptions<DuwaysContext>() { }))
          {
            Order order = await db.Order.FirstOrDefaultAsync(o => o.Id == confirmationRequest.InvId);
            if(order != null)
            {
              bool? istest = null;
              if (confirmationRequest.IsTest == "1") istest = true;
              decimal incsum = -1;
              decimal.TryParse(confirmationRequest.IncSum, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out incsum);
              decimal fee = -1;
              decimal.TryParse(confirmationRequest.Fee, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out fee);

              order.SignatureValue = confirmationRequest.SignatureValue;
              order.IsTest = istest;
              order.EMail = confirmationRequest.EMail;
              order.PaymentMethod = confirmationRequest.PaymentMethod;
              order.IncSum = incsum;
              order.Fee = fee;
              order.PaymentDateTime = DateTime.Now;
              order.IsPaid = true;
              //order.ResultResponse = url; 
              order.PaymentDateTime = DateTime.Now;
              await db.SaveChangesAsync();

              if (order.UserId != null && order.CourseId != null)
              {
                bool result = await _courseService.ActivateUserCourse((int)order.CourseId, (int)order.UserId);
              }
            }
          }


          User user = await _courseService.GetUserById(confirmationRequest.Shp_userid); // GlobalModel.GetUserById(confirmationRequest.Shp_userid);
          if (user != null && !user.IsActivated && confirmationRequest.Shp_isbook == 0)
          {
            return RedirectToAction("Activation", "Account");
          }
          return View(uc); // content for user
        }
      }
      catch (Exception) { return View("Fail"); }

      return View("Fail");
    }

    //http://duways.com/payment/fail?inv_id=1&InvId=1&out_summ=10000.00&OutSum=10000.00&Culture=ru&IsTest=1
    public ActionResult Fail()
    {
      return View();
    }

    private async void processOrder(RobokassaConfirmationRequest confirmationRequest, string url)
    {
      try
      {
        string pathAndQuery = GetPathAndQuery();
        //await new Mailer().SendMessageAsync("pavelkolom@gmail.com", "Result request arrived", GetPathAndQuery(), false);

        decimal sum = -1;
        decimal.TryParse(confirmationRequest.OutSum, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out sum);
        decimal incsum = -1;
        decimal.TryParse(confirmationRequest.IncSum, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out incsum);
        decimal fee = -1;
        decimal.TryParse(confirmationRequest.Fee, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out fee);
        bool? istest = null;
        if (confirmationRequest.IsTest == "1") istest = true;
        if (confirmationRequest.IsTest == null || confirmationRequest.IsTest == "0") istest = false;
        // TODO:
        // 1. verify your order Id and price here
        // 2. mark your order as paid

        using (DuwaysContext db = new DuwaysContext(new DbContextOptions<DuwaysContext>() { }))
        {
          Order order = await db.Order.FirstOrDefaultAsync(o => o.Id == confirmationRequest.InvId);
          if (order != null && (order.IsPaid == false || order.ResultResponse == null) &&
            (order.UserId == confirmationRequest.Shp_userid || (order.UserId == null && confirmationRequest.Shp_userid == 0)) &&
            order.Sum == sum &&
            (order.CourseId == confirmationRequest.Shp_courseid || (order.CourseId == null && confirmationRequest.Shp_courseid == 0)))
          {
            order.SignatureValue = confirmationRequest.SignatureValue;
            order.IsTest = istest;
            order.EMail = confirmationRequest.EMail;
            order.PaymentMethod = confirmationRequest.PaymentMethod;
            order.IncSum = incsum;
            order.Fee = fee;
            order.PaymentDateTime = DateTime.Now;
            order.IsPaid = true;
            order.ResultResponse = url;
            await db.SaveChangesAsync();

            if (order.UserId != null && order.CourseId != null)
            {
              bool result = await _courseService.ActivateUserCourse((int)order.CourseId, (int)order.UserId);
            }

            await new Mailer().SendMessageAsync("pavelkolom@gmail.com", "Successful transaction confirmation", pathAndQuery, false);
          }
          else
          {
            await new Mailer().SendMessageAsync("pavelkolom@gmail.com", "Order is not updated: ", order == null ? "order = null" : (order.IsPaid.ToString() + " " +
              confirmationRequest.Shp_userid.ToString() + "|" +
              order.Sum.ToString() + "|" + sum + "|" +
              confirmationRequest.Shp_courseid.ToString() + "|" +
              order.Id.ToString() + "|" + confirmationRequest.InvId.ToString()) + ", pathandquery: " + pathAndQuery, false);
          }
        }
      }
      catch (Exception ex)
      {
        await new Mailer().SendMessageAsync("pavelkolom@gmail.com", "Failed transaction confirmation", GetPathAndQuery() + " " + ex.Message + " " + ex.InnerException != null ? ex.InnerException.Message : "", false);
      }
    }

    public string GetPathAndQuery()
    {
      try
      {
        var path = HttpContext.Request.Path;
        var query = HttpContext.Request.QueryString;
        var pathAndQuery = path + query;
        return pathAndQuery;
      }
      catch
      {
        return "httpcontext is disposed";
      }
    }
  }
}