using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HTTPMediaPlayerCore.Services;
using Microsoft.AspNetCore.Mvc;
using HTTPMediaPlayerCore.Models;

namespace HTTPMediaPlayerCore.ViewComponents
{
  public class LessonListViewComponent : ViewComponent
  {
    ICourseService _service;
    public LessonListViewComponent(ICourseService service)
    {
      _service = service;
    }

    public async Task<IViewComponentResult> InvokeAsync(UserCourseItem userCourseItem)
    {
      UserCourse myUserCourse = await _service.GetUserCourse(userCourseItem.courseid, userCourseItem.userid);
      return View(myUserCourse);
    }
  }
}
