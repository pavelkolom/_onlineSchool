using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HTTPMediaPlayerCore.Models
{
  public class LoginModel
  {
    public int id { get; set; }

    public string name { get; set; }

    public string surname { get; set; }

    public string fathername { get; set; }

    public string urlname { get; set; }

    public string email { get; set; }

    public string phone { get; set; }

    public string password { get; set; }

    public bool? isAuthor { get; set; }

    public bool? isAdmin { get; set; }

    public int? istrial { get; set; }

    public int countryId { get; set; }

    public string city { get; set; }

    public string zip { get; set; }

    public string address { get; set; }

    public string course { get; set; }

    public int? courseprice
    {
      get; set;
    }

    public string needsdelivery { get; set; }
    public string isdigitaldownload { get; set; }

    //model specific fields 
    public string captcha { get; set; }
  }
}