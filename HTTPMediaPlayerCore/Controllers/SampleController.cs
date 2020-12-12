using System;
using System.Linq;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Http.Headers;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using HTTPMediaPlayerCore.Models;
using HTTPMediaPlayerCore.Services;
using Microsoft.EntityFrameworkCore;


namespace HTTPMediaPlayerCore.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class SampleController : ControllerBase
  {
    private readonly ICourseService _courseService;

    public SampleController(ICourseService courseService)
    {
      _courseService = courseService;
    }

    [HttpGet("{authorid}/{token}")]
    public async Task<IActionResult> Download(string token, string authorid)
    {
      try
      {
        var path = "";
        var partpath = HttpContext.Session.GetString(token);
        var fn = HttpContext.Session.GetString(token + "_fn");
        if (string.IsNullOrEmpty(partpath))
        {
          Models.File file = await _courseService.GetFile(token);
          HttpContext.Session.SetString(token, authorid + "\\" + file.FileName);
          fn = file.FileName;
          HttpContext.Session.SetString(token + "_fn", fn);
          path = "C:\\Duways\\Authors\\" + authorid + "\\" + file.FileName + ".pdf";
        }
        else
        {
          path = "C:\\Duways\\Authors\\" + partpath + ".pdf";
        }
        
        
        byte[] fileBytes = System.IO.File.ReadAllBytes(path);
        return File(fileBytes, "application/force-download", fn + ".pdf");
      }
      catch
      {
        return RedirectToAction("Index", "Dashboard");
      }
    }

    [HttpGet("{filename}/{token}/{authorid}/{fileid}/{userid}")]
    public IActionResult GetVideoContent(string filename, string token, int authorid, int userid)
    {
      string fname = "";
      try
      {
        string referer = Request.Headers["Referer"].ToString();
        var response = new ContentResult();

        try
        {
          if (string.IsNullOrEmpty(referer))
          {
            response.StatusCode = (int?)HttpStatusCode.Forbidden;
            return response;
          }
          else
          {
            if (referer.Contains(token))
            {
              response.StatusCode = (int?)HttpStatusCode.Forbidden;
              return response;
            }
          }
        }
        catch (Exception ex)
        {
          response.Content = ex.Message;
          response.StatusCode = (int?)HttpStatusCode.ExpectationFailed;
        }

        if (string.IsNullOrEmpty(HttpContext.Session.GetString(token)))
        {
          using (DuwaysContext db = new DuwaysContext(new DbContextOptions<DuwaysContext>()))
          {

            UserLesson UserLesson = db.UserLesson.FirstOrDefault(f => f.UniqueId == new Guid(token));
            if (UserLesson != null)
            {
              fname = @"C:\Duways\Videos\" + authorid + "\\" + filename + ".mp4";
              HttpContext.Session.SetString(token, authorid + "\\" + filename);
            }

          }
        }
        else
        {
          fname = @"C:\Duways\Videos\" + HttpContext.Session.GetString(token) + ".mp4";
        }

        FileStream fs = System.IO.File.Open(fname, FileMode.Open);
        FileStreamResult result = File(
            fileStream: fs,
            contentType: new MediaTypeHeaderValue("video/mp4").MediaType,
            enableRangeProcessing: true //<-- enable range requests processing
        );
        return result;
      }
      catch(Exception ex)
      {
        StringBuilder sb = new StringBuilder();
       
        sb.Append(ex.Message + " " + (ex.InnerException != null ? ex.InnerException.Message : " no inner"));
        // flush every 20 seconds as you do it
        System.IO.File.AppendAllText("C:\\Duways\\" + "errorlog.txt", Environment.NewLine);
        System.IO.File.AppendAllText("C:\\Duways\\" + "errorlog.txt", DateTime.Now.ToString() + ": file=" + fname + ", error=" + sb.ToString());
        sb.Clear();
        
        return BadRequest(ex.Message);
      }
    }
  }
}