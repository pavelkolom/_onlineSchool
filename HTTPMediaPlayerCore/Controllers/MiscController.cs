using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HTTPMediaPlayerCore.Controllers
{
  public class MiscController : Controller
  {
    public IActionResult Index()
    {
      return View();
    }

    public IActionResult FAQ()
    {
      return View();
    }
    public IActionResult Pricelist()
    {
      return View();
    }
  }
}
