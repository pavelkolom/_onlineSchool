using System;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web;
using HTTPMediaPlayer.Models.DB;

namespace HTTPMediaPlayer.Models
{

  public class VideoStream
  {
    FileStream video;
    public FileInfo FileInfo { get; set; }

    public long Start { get; set; }

    public long End { get; set; }

    private string _filename { get; set; }

    private string URL { get; set; }

    public vd_UserLessons UserLesson { get; set; }
    public int UserId { get; set; }

    public VideoStream(string filename, string ext)
    {
      try
      {
        //vd_UserLessons UserLesson;// = //GlobalModel.GetUserLessonByUniqueId(filename);
        using (videodbEntities db = new videodbEntities())
        {
          if (filename == "teste123")
          {
            UserLesson = new vd_UserLessons();
            UserLesson.vd_Courses = db.vd_Courses.FirstOrDefault();
            UserLesson.vd_Users = db.vd_Users.FirstOrDefault();
            UserLesson.UniqueId = Guid.NewGuid();
            UserLesson.vd_Lessons = db.vd_Lessons.FirstOrDefault();
          }
          else
          {
            UserLesson = db.vd_UserLessons.FirstOrDefault(f => f.UniqueId == new Guid(filename));
          }
          UserId = UserLesson.UserId;
          URL = UserLesson.vd_Lessons.vd_Files.Url;
          _filename = @"C:\Video\Authors\" + UserLesson.vd_Lessons.vd_Users.UrlName + "\\" + UserLesson.vd_Lessons.vd_Files.FileName + "." + ext;
        }
          FileInfo = new FileInfo(_filename);
      }
      catch { return; }
    }

    public async void WriteToStream(Stream outputStream, HttpContent content, TransportContext context)
    {
      try
      {
        var buffer = new byte[65536];

        if (video == null) video = FileInfo.OpenRead();
        using (video = FileInfo.OpenRead())
        {
          if (End == -1)
          {
            End = video.Length;
          }

          var position = Start;
          var bytesLeft = End - Start + 1;
          video.Seek(Start, SeekOrigin.Begin);
          while (position <= End)
          {
            var bytesRead = video.Read(buffer, 0, (int)Math.Min(bytesLeft, buffer.Length));
            await outputStream.WriteAsync(buffer, 0, bytesRead);
            position += bytesRead;
            bytesLeft = End - position + 1;
          }
          //var length = (int)video.Length;
          //var bytesRead = 1;

          //while (length > 0 && bytesRead > 0)
          //{
          //  bytesRead = video.Read(buffer, 0, Math.Min(length, buffer.Length));
          //  await outputStream.WriteAsync(buffer, 0, bytesRead);
          //  length -= bytesRead;
          //}
        }
        //const int chunkSize = 65536;
        //const int BUFFER_SIZE = 20 * 1024;
        //byte[] buffer = new byte[BUFFER_SIZE];


        //using (Stream video = FileInfo.OpenRead())
        //{
        //  End = video.Length;

        //  int index = 0;
        //  while (video.Position < video.Length)
        //  {
        //    //using (Stream output = File.Create(path + "\\" + index))
        //    //{
        //      int remaining = chunkSize, bytesRead;
        //      while (remaining > 0 && (bytesRead = video.Read(buffer, 0,
        //              Math.Min(remaining, BUFFER_SIZE))) > 0)
        //      {
        //        await outputStream.WriteAsync(buffer, 0, bytesRead);
        //        remaining -= bytesRead;
        //      }
        //    //
        //    index++;
        //    //.Sleep(500); // experimental; perhaps try it
        //  }
        //}

      }
      catch (HttpException ex)
      {
        ErrorWriter.WriteError(ex);
        return;
      }
      catch (InvalidOperationException ex)
      {
        ErrorWriter.WriteError(ex);
        return;
      }
      catch (Exception ex)
      {
        ErrorWriter.WriteError(ex);
        return;
      }
      finally
      {
        outputStream.Close();
        GC.Collect();
      }
    }

    static async Task HttpGetForLargeFileInRightWay()
    {
      using (HttpClient client = new HttpClient())
      {
        const string url = "https://github.com/tugberkugurlu/ASPNETWebAPISamples/archive/master.zip";
        using (HttpResponseMessage response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
        using (Stream streamToReadFrom = await response.Content.ReadAsStreamAsync())
        {
          string fileToWriteTo = Path.GetTempFileName();
          using (Stream streamToWriteTo = File.Open(fileToWriteTo, FileMode.Create))
          {
            await streamToReadFrom.CopyToAsync(streamToWriteTo);
          }
        }
      }
    }
  }
}