using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HTTPMediaPlayer.Models
{
  public class ChangePasswordModel
  {
    public int userid { get; set; }
    public string password { get; set; }
    public string confirmation { get; set; }
  }
}