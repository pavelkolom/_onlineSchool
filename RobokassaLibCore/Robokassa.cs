using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace RobokassaLibCore
{
  public class Robokassa
  {
    private readonly IConfiguration _configuration;
    private readonly RobokassaConfig _roboconfig;
    
    public Robokassa(IConfiguration configuration)
    {
      _configuration = configuration;
      _roboconfig = new RobokassaConfig(_configuration);
    }

    public Robokassa(IConfiguration configuration, string login, string pass1, string pass2, string testpass1, string testpass2)
    {
      _configuration = configuration;
      _roboconfig = new RobokassaConfig(configuration,
              login,
              pass1,
              pass2,
              testpass1,
              testpass2);
      
    }

    public RobokassaConfig RoboConfig { get { return _roboconfig; } }


    public string GetRedirectUrl(int priceRub, int orderId, int userId, string coursename, int courseId, int isBook, string isDigital, string email = "")
    {
      // ugly code, legacy from Robokassa website

      // your registration data
      string sMrchLogin = _roboconfig.Login;
      string sMrchPass1 = _roboconfig.Mode == RobokassaMode.Production ? _roboconfig.Pass1 : _roboconfig.TestPass1;
      // order properties
      decimal nOutSum = priceRub;
      int nInvId = orderId;
      string sDesc = coursename;
      string isDigitalDownload = isDigital == "1" ? "1" : "0";
      string sOutSum = nOutSum.ToString("0.00", CultureInfo.InvariantCulture);
      string sCrcBase = string.Format("{0}:{1}:{2}:{3}:Shp_courseid={4}:Shp_isbook={5}:Shp_isdigital={6}:Shp_userid={7}",
                                       sMrchLogin, sOutSum, nInvId, sMrchPass1, courseId.ToString(), isBook.ToString(), isDigitalDownload, userId.ToString());

      // build CRC value
      MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
      byte[] bSignature = md5.ComputeHash(Encoding.ASCII.GetBytes(sCrcBase));

      StringBuilder sbSignature = new StringBuilder();
      foreach (byte b in bSignature)
        sbSignature.AppendFormat("{0:x2}", b);

      string sCrc = sbSignature.ToString();
      string returnurl = 
       getBaseUrl() +
                "&MerchantLogin=" + sMrchLogin +
                "&OutSum=" + sOutSum +
                "&IncCurrLabel=" + "QCardR" +
                "&InvId=" + nInvId +
                "&InvoceID=" + nInvId +
                "&InvDesc=" +  (sDesc != null ? sDesc.Replace(" ", "-") : "INV-" + nInvId.ToString()) +
                "&Description=" + (sDesc != null ? sDesc : "INV-" + nInvId.ToString()) +
                "&SignatureValue=" + sCrc +
                (
                //string.IsNullOrEmpty(email) ? "" : "&Email=" + email +
                "&Shp_courseid=" + courseId.ToString() +
                "&Shp_isbook=" + isBook.ToString() +
                "&Shp_isdigital=" + isDigitalDownload +
                "&Shp_userid=" + userId.ToString()
                );
      return returnurl;
    }

    private string getBaseUrl()
    {
      switch (_roboconfig.Mode)
      {
        case RobokassaMode.Test:
          return "https://auth.robokassa.ru/Merchant/Index.aspx?IsTest=1";
        case RobokassaMode.Production:
          return "https://auth.robokassa.ru/Merchant/Index.aspx?IsTest=0";
        default:
          throw new NotSupportedException();
      }
    }
  }
}
