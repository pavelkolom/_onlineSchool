using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace RobokassaLibCore
{
  public class RobokassaConfirmationRequest
  {
   
    public RobokassaConfirmationRequest()
    {
    }

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


  }
}
