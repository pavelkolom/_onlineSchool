using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HTTPMediaPlayerCore.Services;
using HTTPMediaPlayerCore.Models;

namespace HTTPMediaPlayerCore.Controllers
{
  public class CourseController : Controller
  {
    //public IActionResult Index()
    //{
    //  return View();
    //}

    private readonly ICourseService _courseService;
    public CourseController(ICourseService courseService)
    {
      _courseService = courseService;
    }

    public async Task<IActionResult> Index(string name)
    {
      AuthorCourse course = await _courseService.GetAuthorCourseByURL(name);
      if (course != null)
        return View(course);
      else
        return View("Index");
    }
  }
}
