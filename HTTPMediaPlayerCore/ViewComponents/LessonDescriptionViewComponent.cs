using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HTTPMediaPlayerCore.Services;
using Microsoft.AspNetCore.Mvc;
using HTTPMediaPlayerCore.Models;

namespace HTTPMediaPlayerCore.ViewComponents
{
  public class LessonDescriptionViewComponent : ViewComponent
  {

    ICourseService _service;
    public LessonDescriptionViewComponent(ICourseService service)
    {
      _service = service;
    }



    public async Task<IViewComponentResult> InvokeAsync(UserCourseItem userCourseItem)
    {
      Lesson lesson = await _service.GetLesson(userCourseItem.lessonid);
      return View(lesson);
    }

  }
}
