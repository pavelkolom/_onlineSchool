using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HTTPMediaPlayerCore.Services;
using HTTPMediaPlayerCore.Models;

namespace HTTPMediaPlayerCore.Controllers
{
  public class AuthorController : Controller
  {

    private readonly ICourseService _courseService;
    public AuthorController(ICourseService courseService)
    {
      _courseService = courseService;
    }

    public IActionResult Index()
    {
      return View();
    }


    public IActionResult Page()
    {
      return View();
    }

    public async Task<IActionResult> Course(string name)
    {
      
      Course course = await _courseService.GetCourseByURL(name);
      if (course != null)
        return View(course);
      else
        return View("Index");
    }



    public async Task<IActionResult> Profile(string name)
    {

        return View(await _courseService.GetCourceCategoryContainer(name, false));
    }



  }
}
