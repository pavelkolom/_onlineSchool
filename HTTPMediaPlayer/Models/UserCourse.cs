using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HTTPMediaPlayer.Models
{
  public class UserCourse
  {
    public int userid { get; set; }
    public int courseid { get; set; }
    public int orderid { get; set; }
    public int isbook { get; set; }
    public bool isdigital { get; set; }
    public int lessonid { get; set; }
  }
}