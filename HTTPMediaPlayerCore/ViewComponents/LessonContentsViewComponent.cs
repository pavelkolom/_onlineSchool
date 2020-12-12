using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HTTPMediaPlayerCore.Services;
using Microsoft.AspNetCore.Mvc;
using HTTPMediaPlayerCore.Models;

namespace HTTPMediaPlayerCore.ViewComponents
{
  public class LessonContentsViewComponent : ViewComponent
  {

    ICourseService _service;
    public LessonContentsViewComponent(ICourseService service)
    {
      _service = service;
    }



    public async Task<IViewComponentResult> InvokeAsync(UserCourseItem userCourseItem)
    {
      //Lesson lesson = await _service.GetLesson(userCourseItem.lessonid);
      return View(userCourseItem);
      //return View();
    }

  }
}
