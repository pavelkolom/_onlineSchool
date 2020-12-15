using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HTTPMediaPlayerCore.Services;
using HTTPMediaPlayerCore.Models;
using Newtonsoft.Json;

namespace HTTPMediaPlayerCore.Controllers
{
  public class AuthorController : Controller
  {
    private readonly ICourseService _courseService;
    public AuthorController(ICourseService courseService)
    {
      _courseService = courseService;
    }

    public async Task<IActionResult> Profile(string name)
    {
        return View(await _courseService.GetAuthorPageContainer(name));
    }
  }
}
