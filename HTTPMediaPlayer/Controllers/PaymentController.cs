using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using RobokassaLib;
using System.Web.Mvc;
using HTTPMediaPlayer.Models;
using HTTPMediaPlayer.Models.DB;

namespace HTTPMediaPlayer.Controllers
{
    public class PaymentController : Controller
    {


    //public ActionResult Index()
    //{
    //  int priceRub = 1000;
    //  int orderId = 1;

    //  // note: use GetRedirectUrl overloading to specify customer email

    //  string redirectUrl = Robokassa.GetRedirectUrl(priceRub, orderId);

    //  return Redirect(redirectUrl);
    //}


    // So called "Result Url" in terms of Robokassa documentation.
    // This url is called by Robokassa robot.
    public ActionResult Result(RobokassaConfirmationRequest confirmationRequest)
    {
      try
      {
        if (confirmationRequest.IsQueryValid(RobokassaQueryType.ResultURL, confirmationRequest.IsTest))
        {
          UserCourse uc = new UserCourse();
          uc.courseid = confirmationRequest.Shp_courseid;
          uc.orderid = confirmationRequest.InvId;
          uc.userid = confirmationRequest.Shp_userid;
          uc.isbook = confirmationRequest.Shp_isbook;
          uc.isdigital = confirmationRequest.Shp_isdigital == "1" ? true : false;
          processOrder(confirmationRequest, Request.Url.PathAndQuery);
          vd_Users user = GlobalModel.GetUserById(confirmationRequest.Shp_userid);
          using (videodbEntities db = new videodbEntities())
          {
            vd_UserLessons ladambook = db.vd_UserLessons.FirstOrDefault(ul => ul.UserId == uc.userid && ul.CourseId == uc.courseid);
            if (confirmationRequest.Shp_isbook == 1 && ladambook != null && ladambook.IsLinkSent != true && uc.isdigital == true)
            {
              ladambook.IsLinkSent = true;
              db.SaveChanges();
              Guid token = ladambook.UniqueId;
              string url = Url.Action("Download", "ru", new { token = token.ToString() }, Request.Url.Scheme);
              string body = System.IO.File.ReadAllText(Server.MapPath("~/EmailTemplates/Ladamtam.html")).Replace("{newuser}", user.Name).Replace("{booklink}", url);
              Mailer.SendMessage(confirmationRequest.EMail, "Ladamtam book", body, true);
              //return RedirectToAction("Activation", "ru");//Json(new { Success = true, Result = "Activation", userid = user.Id }, JsonRequestBehavior.AllowGet);
            }
          }

          
          if (user != null && !user.IsActivated && uc.isbook == 0)
          {
            string url = Url.Action("SetPassword", "Dashboard", new { token = user.Token, id = user.Id }, this.Request.Url.Scheme);
            string body = System.IO.File.ReadAllText(Server.MapPath("~/EmailTemplates/Welcome.html")).Replace("{newuser}", user.Name).Replace("{dashboardlink}", url);
            Mailer.SendMessage(user.Email, "DuWays account access confirmation", body, true);
            //Mailer.SendMessage(user.Email, "Account access confirmation", url);
            //return RedirectToAction("Activation", "ru");//Json(new { Success = true, Result = "Activation", userid = user.Id }, JsonRequestBehavior.AllowGet);
          }


          return Content("OK"); // content for robot
        }
      }
      catch (Exception ex)
      {
        Mailer.SendMessage("pavelkolom@gmail.com", "Failed transaction confirmation", Request.Url.PathAndQuery + " " + ex.Message + " " + ex.InnerException != null ? ex.InnerException.Message : "", false);
        return Content("ERR");
      }

      Mailer.SendMessage("pavelkolom@gmail.com", "Failed transaction confirmation", Request.Url.PathAndQuery, false);
      return Content("ERR");
    }

    // So called "Success Url" in terms of Robokassa documentation.
    // Customer is redirected to this url after successful payment. 
    //http://duways.com/payment/success?inv_id=1&InvId=1&out_summ=10000.00&OutSum=10000.00&crc=2e045768330de6f7e4f4e30cbb451db9&SignatureValue=2e045768330de6f7e4f4e30cbb451db9&Culture=ru&IsTest=1
    public ActionResult Success(RobokassaConfirmationRequest confirmationRequest)
    {
      try
      {

        if (confirmationRequest.IsQueryValid(RobokassaQueryType.SuccessURL, confirmationRequest.IsTest))
        {
          UserCourse uc = new UserCourse();
          uc.courseid = confirmationRequest.Shp_courseid;
          uc.orderid = confirmationRequest.InvId;
          uc.userid = confirmationRequest.Shp_userid;
          uc.isbook = confirmationRequest.Shp_isbook;


          vd_Users user = GlobalModel.GetUserById(confirmationRequest.Shp_userid);
          if (user != null && !user.IsActivated && confirmationRequest.Shp_isbook == 0)
          {
            //string url = Url.Action("SetPassword", "Dashboard", new { token = user.Token, id = user.Id }, this.Request.Url.Scheme);
            //string body = System.IO.File.ReadAllText(Server.MapPath("~/EmailTemplates/Welcome.html")).Replace("{newuser}", user.Name).Replace("{dashboardlink}", url);
            //Mailer.SendMessage(user.Email, "DuWays account access confirmation", body, true);
            //Mailer.SendMessage(user.Email, "Account access confirmation", url);
            return RedirectToAction("Activation", "ru");//Json(new { Success = true, Result = "Activation", userid = user.Id }, JsonRequestBehavior.AllowGet);
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

    private void processOrder(RobokassaConfirmationRequest confirmationRequest, string url)
    {
      try
      {
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
        using (videodbEntities db = new videodbEntities())
        { 
          vd_Orders order = db.vd_Orders.FirstOrDefault(o => o.Id == confirmationRequest.InvId);
          if (order.IsPaid == false &&
            order.UserId == confirmationRequest.Shp_userid &&
            order.Sum == sum &&
            order.CourseId == confirmationRequest.Shp_courseid)
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
            db.SaveChanges();
            bool result = GlobalModel.ActivateUserCourse(db, order.CourseId, order.UserId);
            Mailer.SendMessage("pavelkolom@gmail.com", "Successful transaction confirmation", Request.Url.PathAndQuery, false);
          }
        }
      }
      catch(Exception ex)
      {
        Mailer.SendMessage("pavelkolom@gmail.com", "Failed transaction confirmation", Request.Url.PathAndQuery + " " + ex.Message + " " + ex.InnerException != null ? ex.InnerException.Message : "", false);
      }
    }
  }
}