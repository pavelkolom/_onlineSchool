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


    [HttpPost]
    public async Task<ActionResult> SendMessage(string name, string email, string message, string authoremail)
    {
      try
      {
        await new Mailer().SendMessageAsync("pavelkolom@gmail.com;" + authoremail, "Сообщение от DuWays", "от " + name + ", email: " + email + " сообщение: " + message, false);
        string serialized = JsonConvert.SerializeObject(new PasswordRecoveryModel(true, "Сообщение успешно отправлено", null));
        return Content(serialized, "application/json");
      }
      catch (Exception ex)
      {
        string serialized = JsonConvert.SerializeObject(new PasswordRecoveryModel(false, ex.Message, null));
        return Content(serialized, "application/json");
      }
    }



  }
}
