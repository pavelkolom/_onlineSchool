using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RobokassaLib
{
  public class RobokassaConfirmationRequest
  {
    public string OutSum { get; set; }
    public int InvId { get; set; }
    public string IncSum { get; set; }
    public string IsTest { get; set; }
    public string EMail { get; set; }
    public string PaymentMethod { get; set; }
    public string Fee { get; set; }
    public string SignatureValue { get; set; }
    public int Shp_courseid { get; set; }
    public int Shp_userid { get; set; }
    public int Shp_isbook { get; set; }
    public string Shp_isdigital { get; set; }


    // in Robokassa we have two types of back-queries:
    //
    // 1. ResultURL query
    //    Robokassa server tries to get this url
    //    Requires Pass2 (!!!)
    //
    // 2. SuccessUrl query
    //    Robokassa redirects user to this url
    //    Requires Pass1 (!!!)

    public bool IsQueryValid(RobokassaQueryType queryType, string isTest)
    {
      string currentPassword = "";
      if(isTest == null || isTest == "0")
        currentPassword = (queryType == RobokassaQueryType.ResultURL) ?
          RobokassaConfig.Pass2 :
          RobokassaConfig.Pass1;
      if (isTest == "1")
        currentPassword = (queryType == RobokassaQueryType.ResultURL) ?
          RobokassaConfig.TestPass2 :
          RobokassaConfig.TestPass1;


      string str = string.Format("{0}:{1}:{2}:Shp_courseid={3}:Shp_isbook={4}:Shp_isdigital={5}:Shp_userid={6}",
                              OutSum, InvId, currentPassword, Shp_courseid, Shp_isbook, Shp_isdigital, Shp_userid);

      MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
      byte[] calculatedSignature = md5.ComputeHash(Encoding.ASCII.GetBytes(str));

      StringBuilder sbSignature = new StringBuilder();

      foreach (byte b in calculatedSignature)
        sbSignature.AppendFormat("{0:x2}", b);

      return string.Equals(
          sbSignature.ToString().ToLower(),
          SignatureValue.ToLower(),
          StringComparison.InvariantCultureIgnoreCase);
    }
  }
}
