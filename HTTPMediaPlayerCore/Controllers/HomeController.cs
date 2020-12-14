using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HTTPMediaPlayerCore.Models;
using Microsoft.AspNetCore.Http;
using HTTPMediaPlayerCore.Services;
using Newtonsoft.Json;


namespace HTTPMediaPlayerCore.Controllers
{
  public class HomeController : Controller
  {
    private readonly ICourseService _courseService;

    public HomeController(ICourseService courceService)
    {
      _courseService = courceService;
    }

    public async Task<IActionResult> Index()
    {
      return View(await _courseService.GetCourceCategoryContainer(3, true));
    }

    public IActionResult Privacy()
    {
      return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


    [HttpPost]
    public async Task<ActionResult> SendMessage(string name, string email, string message)
    {
      try
      {
        await new Mailer().SendMessageAsync("pavelkolom@gmail.com;anchetakolomeitceva@gmail.com", "Сообщение от DuWays", "от " + name + ", email: " + email + "сообщение: " + message, false);
        string serialized = JsonConvert.SerializeObject(new PasswordRecoveryModel(true, "Сообщение успешно отправлено", null));
        return Content(serialized, "application/json");
      }
      catch(Exception ex)
      {
        string serialized = JsonConvert.SerializeObject(new PasswordRecoveryModel(false, ex.Message, null));
        return Content(serialized, "application/json");
      }
    }

        [HttpPost]
        public async Task<IActionResult> SendMessageToAuthor(string name, string email, string message, string authoremail)
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
