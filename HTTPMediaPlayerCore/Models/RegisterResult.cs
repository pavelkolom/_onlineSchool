using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTTPMediaPlayerCore.Models
{
  public class RegisterResult
  {
    public RegisterResult(bool success, string result, string redirect, int? userId, int? courseId, int? coursePrice)
    {
      Success = success;
      Result = result;
      Redirect = redirect;
      UserId = userId;
      CourseId = courseId;
      CoursePrice = coursePrice;
    }

    public bool Success { get; set; }
    public string Result { get; set; }
    public string Redirect { get; set; }
    public int? UserId { get; set; }
    public int? CourseId { get; set; }
    public int? CoursePrice { get; set; }


  }
}

