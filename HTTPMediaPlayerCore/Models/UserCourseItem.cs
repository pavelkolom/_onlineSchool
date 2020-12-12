using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTTPMediaPlayerCore.Models
{
  public class UserCourseItem
  {
    public int userid { get; set; }
    public int courseid { get; set; }
    public int orderid { get; set; }
    public int isbook { get; set; }
    public bool isdigital { get; set; }
    public int lessonid { get; set; }
    public string html { get; set; }
  }
}
