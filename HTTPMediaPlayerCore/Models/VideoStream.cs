using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace HTTPMediaPlayerCore.Models
{

  public class VideoStream
  {
    public string Filename { get; set; }

    public VideoStream(string filename, string token, int authorid, int userid)
    {
      try
      {
        using (DuwaysContext db = new DuwaysContext(new DbContextOptions<DuwaysContext>()))
        {

          UserLesson UserLesson = db.UserLesson.FirstOrDefault(f => f.UniqueId == new Guid(token));
          if (UserLesson != null)
          {
            Filename = @"C:\Duways\Videos\" + authorid + "\\" + filename + ".mp4";
          }
        }
      }
      catch { return; }
    }
  }
}