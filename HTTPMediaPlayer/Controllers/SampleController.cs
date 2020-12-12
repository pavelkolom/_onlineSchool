using System;
using System.Linq;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Collections.Generic;
using System.Web.Http;
using System.Threading.Tasks;
using HTTPMediaPlayer.Models;
using HTTPMediaPlayer.Models.DB;

namespace HTTPMediaPlayer.Controllers
{
  public class SampleController : ApiController
  {
    [SessionCheck]
    public HttpResponseMessage Get(string filename, string ext)
    {
      var response = Request.CreateResponse();
      try
      {
        if (Request.Headers.Referrer == null)
        {
          response.StatusCode = HttpStatusCode.Forbidden;
          return response;
        }
        else
        {
          string origin = Request.Headers.Referrer.AbsoluteUri;
          if (origin.Contains(filename))
          {
            response.StatusCode = HttpStatusCode.Forbidden;
            return response;
          }
        }
      }
      catch (Exception ex)
      {
        response.ReasonPhrase = ex.Message;
        response.StatusCode = HttpStatusCode.ExpectationFailed;
      }

      VideoStream video;

      if (GlobalModel.UsersVideos.ContainsKey(filename))
      {
        video = GlobalModel.GetVideoStream(filename);
        //if (GlobalModel.LoggedInUsers.ContainsKey(lesson.UserId))
          GlobalModel.UsersVideos.Add(filename, video);
      }
      else
      {
        video = new VideoStream(filename, ext);
        //add action log
        if (video.UserLesson != null && filename != "teste123")
        {
          //using (videodbEntities db = new videodbEntities())
          //{
          //  vd_ActionLog log = new vd_ActionLog();
          //  log.UserId = video.UserLesson.UserId;
          //  log.StartTime = DateTime.Now;
          //  log.Remarks = video.FileInfo.Name;
          //  log.ActionType = 3;
          //  db.vd_ActionLog.Add(log);
          //  db.SaveChanges();
          //}

          //
          vd_UserLessons ulsn = video.UserLesson;
          //if (filename != "teste123")
          //{
          //  Guid? newId = GlobalModel.ChangeUserLessonGuidById(ulsn.Id);
          //  try
          //  {
          //    video.UserLesson.UniqueId = (Guid)newId;
          //  }
          //  catch { }
          //}
          //var x = GlobalModel.UsersVideos.Where(d => d.Value.UserLesson.Id == ulsn.Id).ToList();
          //for (int i = 0; i < x.Count(); i++)
          //  GlobalModel.UsersVideos.Remove(x[i].Key);

          //if(GlobalModel.LoggedInUsers.ContainsKey(ulsn.UserId))
          //  GlobalModel.UsersVideos.Add(filename, video);
        }
      }

      try
      {
        response.Content =  new PushStreamContent((Action<Stream, HttpContent, TransportContext>) video.WriteToStream, new MediaTypeHeaderValue("video/" + ext));//"application/octet-stream");//
        /*var result = new HttpResponseMessage(HttpStatusCode.OK)
        {
          Content = new PushStreamContent(async (outputStream, httpContext, transportContext) =>
          {
          try
          {
            var buffer = new byte[65536];
              using (FileStream vd = video.FileInfo.OpenRead())
              {
                if (video.End == -1)
                {
                  video.End = vd.Length;
                }

                var position = video.Start;
                var bytesLeft = video.End - video.Start + 1;
                vd.Seek(video.Start, SeekOrigin.Begin);
                while (position <= video.End)
                {
                  var bytesRead = vd.Read(buffer, 0, (int)Math.Min(bytesLeft, buffer.Length));
                  await outputStream.WriteAsync(buffer, 0, bytesRead);
                  position += bytesRead;
                  bytesLeft = video.End - position + 1;
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
            }
            catch (HttpException ex)
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
          }),
        };*/



        RangeHeaderValue rangeHeader = Request.Headers.Range;
        if (rangeHeader != null)
        {
          long totalLength = video.FileInfo.Length;
          var range = rangeHeader.Ranges.First();
          video.Start = range.From ?? 0;
          video.End = range.To ?? totalLength - 1;
          response.Content.Headers.ContentLength = video.End - video.Start + 1;
          response.Content.Headers.ContentRange = new ContentRangeHeaderValue(video.Start, video.End,
              totalLength);
          response.StatusCode = HttpStatusCode.PartialContent;
        }
        else
        {
          response.ReasonPhrase = "header is missing";
          response.StatusCode = HttpStatusCode.ServiceUnavailable;
        }
        ///
      }
      catch(Exception ex)
      {
        response.ReasonPhrase = ex.Message;
        response.StatusCode = HttpStatusCode.NotAcceptable;
      }

      return response;
    }

    /*public HttpResponseMessage Get(string filename, string ext)
    {
      if (filename == null)
      {
        return new HttpResponseMessage(HttpStatusCode.BadRequest);
      }

      if (Request.Headers.Range != null)
      {
        try
        {
          HttpResponseMessage partialResponse = Request.CreateResponse(HttpStatusCode.Moved);// HttpStatusCode.PartialContent);
          vd_UserLessons UserLesson = GlobalModel.GetUserLessonByUniqueId(filename);
          string _filename = @"C:\Video\Authors\" + UserLesson.vd_Lessons.vd_Users.UrlName + "\\" + UserLesson.vd_Lessons.vd_Files.FileName + "." + ext;
          FileInfo FileInfo = new FileInfo(_filename);
          //FileStream stream = FileInfo.OpenRead();
          //partialResponse.Content = new ByteRangeStreamContent(stream, Request.Headers.Range, new MediaTypeHeaderValue("application/octet-stream"));
          //const int BufferSize = 1024 * 1024;
          //responseMessage = new HttpResponseMessage();


          using (var wc = new System.Net.WebClient())
          {
            //string accessKey = Cp.Service.Settings.AccessKey;
            //string secretAccessKey = Cp.Service.Settings.SecretAccessKey;
            string url = "https://www.youtube.com/embed/zJm-XPSQewc?list=RDzJm-XPSQewc";

            partialResponse.Content = new StreamContent(wc.OpenRead(url));
          }


          //partialResponse.Content = new StreamContent(stream, BufferSize);

          return partialResponse;

        }
        catch (Exception)
        {
          return new HttpResponseMessage(HttpStatusCode.InternalServerError);
        }
        finally
        {
          GC.Collect();
        }
      }

      return new HttpResponseMessage(HttpStatusCode.RequestedRangeNotSatisfiable);
    }*/
  }
}