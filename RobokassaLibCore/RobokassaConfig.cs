using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;

namespace RobokassaLibCore
{
  public class RobokassaConfig
  {

    private IConfiguration _configuration;

    public RobokassaConfig(IConfiguration configuration)
    {
      _configuration = configuration;
      Login = _configuration["RobokassaLogin"];
      Pass1 = _configuration["RobokassaPass1"];
      Pass2 = _configuration["RobokassaPass2"];
      TestPass1 = _configuration["RobokassaTestPass1"];
      TestPass2 = _configuration["RobokassaTestPass2"];
    }

    public RobokassaConfig(IConfiguration configuration, string login, string pass1, string pass2, string testpass1, string testpass2)
    {
      _configuration = configuration;
      Login = login;
      Pass1 = pass1;
      Pass2 = pass2;
      TestPass1 = testpass1;
      TestPass2 = testpass2;
    }

    public string Login { get; set; }

    public string Pass1 { get; set; }

    public string Pass2 { get; set; }
    public string TestPass1 { get; set; }

    public string TestPass2 { get; set; }

    public RobokassaMode Mode
    {
      get
      {
        string mode = _configuration["RobokassaMode"];
        switch (mode.ToLower())
        {
          case "test":
            return RobokassaMode.Test;
          case "production":
            return RobokassaMode.Production;
          default:
            throw new NotSupportedException("Mode is not supported, available modes: test or production");
        }
      }
    }

    public void AssertConfigurationIsValid()
    {
      if (String.IsNullOrWhiteSpace(Login))
        throw new Exception("Robokassa configuration: login is required");

      if (String.IsNullOrWhiteSpace(Pass1))
        throw new Exception("Robokassa configuration: first password is required");

      if (String.IsNullOrWhiteSpace(Pass2))
        throw new Exception("Robokassa configuration: second password is required");

      var mode = Mode;
    }

    public bool IsQueryValid(RobokassaConfirmationRequest req,RobokassaQueryType queryType, string isTest)
    {
      string currentPassword = "";
      if (isTest == null || isTest == "0")
        currentPassword = (queryType == RobokassaQueryType.ResultURL) ?
          Pass2 :
          Pass1;
      if (isTest == "1")
        currentPassword = (queryType == RobokassaQueryType.ResultURL) ?
          TestPass2 :
          TestPass1;


      string str = string.Format("{0}:{1}:{2}:Shp_courseid={3}:Shp_isbook={4}:Shp_isdigital={5}:Shp_userid={6}",
                              req.OutSum, req.InvId, currentPassword, req.Shp_courseid, req.Shp_isbook, req.Shp_isdigital, req.Shp_userid);

      MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
      byte[] calculatedSignature = md5.ComputeHash(Encoding.ASCII.GetBytes(str));

      StringBuilder sbSignature = new StringBuilder();

      foreach (byte b in calculatedSignature)
        sbSignature.AppendFormat("{0:x2}", b);

      return string.Equals(
          sbSignature.ToString().ToLower(),
          req.SignatureValue.ToLower(),
          StringComparison.InvariantCultureIgnoreCase);
    }



  }
}
