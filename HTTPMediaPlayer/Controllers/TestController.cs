using System;
using System.Linq;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections;
using System.Collections.Generic;
using System.Web.Http;
using HTTPMediaPlayer.Models;
using HTTPMediaPlayer.Models.DB;

namespace HTTPMediaPlayer.Controllers
{
  public class TestController : ApiController
  {
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
      }
      else
      {
        video = new VideoStream(filename, ext);
        //add action log
        using (videodbEntities db = new videodbEntities())
        {
          vd_ActionLog log = new vd_ActionLog();
          log.UserId = video.UserLesson.UserId;
          log.StartTime = DateTime.Now;
          log.Remarks = video.FileInfo.Name;
          log.ActionType = 3;
          db.vd_ActionLog.Add(log);
          db.SaveChanges();
        }
        //
        vd_UserLessons ulsn = video.UserLesson;//GlobalModel.GetUserLessonByUniqueId(filename);

        Guid? newId = GlobalModel.ChangeUserLessonGuidById(ulsn.Id);
        video.UserLesson.UniqueId = (Guid)newId;
        var x = GlobalModel.UsersVideos.Where(d => d.Value.UserLesson.Id == ulsn.Id).ToList();
        for (int i = 0; i < x.Count(); i++)
          GlobalModel.UsersVideos.Remove(x[i].Key);
        GlobalModel.UsersVideos.Add(filename, video);
      }


      try
      {
        response.Content = new PushStreamContent((Action<Stream, HttpContent, TransportContext>)video.WriteToStream, new MediaTypeHeaderValue("video/" + ext));//"application/octet-stream");//

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