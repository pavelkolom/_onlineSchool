using System;
using System.Web;
using System.Net.Mail;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;

namespace HTTPMediaPlayer.Models
{
  public static class Mailer
  {
    public static async Task<bool> SendMessageAsync(string Mail, string Subject, string Body)
    {
      List<string> addresses = Mail.Split(new char[] { ',', ';' }).ToList();
      foreach (string address in addresses)
        try
        {
        MailMessage ms = PrepareMessage(address, Subject, Body, true);
        SmtpClient client = new SmtpClient();
        //client.Timeout = 90;
        await client.SendMailAsync(ms);
      }
      catch
      {
        return await Task.FromResult(false);
      }
      return await Task.FromResult(true);
    }

    public static string SendMessage(string Mail, string Subject, string Body, bool isHTML)
    {
      List<string> addresses = Mail.Split(new char[] { ',', ';' }).ToList();
      foreach (string address in addresses)
        try
        {
          MailMessage ms = PrepareMessage(address, Subject, Body, isHTML);
          SmtpClient client = new SmtpClient();
          
          //client.EnableSsl = true;
          //client.Host = "smtp.gmail.com";
          //client.Credentials = new System.Net.NetworkCredential("anna@duways.com", "UfkFPvF3Wo;%Pzhqu34;.kLYT(HP7$!=");
          client.Send(ms);
        }
        catch (Exception ex)
        {
          return ex.Message;
        }
      return "Success";
    }

    private static MailMessage PrepareMessage(string Mail, string Subject, string Body, bool isHTML)
    {
        try
        {
          if (!isHTML)
          {
            MailMessage ms = new MailMessage();
            ms.To.Add(new MailAddress(Mail));
            ms.Subject = Subject;
            ms.Body = Body;
            ms.IsBodyHtml = true;
            return ms;
          }
          else
          {
            MailMessage mailMessage = new MailMessage();
            mailMessage.IsBodyHtml = true;
            mailMessage.To.Add(Mail);
            mailMessage.Subject = Subject;
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(Body);
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//img/@src");

            foreach (HtmlNode node in nodes)
              foreach (HtmlAttribute attr in node.Attributes)
                if (attr.Name == "src")
                  if (!attr.Value.Contains("http"))
                    attr.Value = "http://" + HttpContext.Current.Request.Url.Authority + attr.Value;

            string newBody = doc.DocumentNode.InnerHtml;
            AlternateView htmlView =
                AlternateView.CreateAlternateViewFromString(newBody
                , null, MediaTypeNames.Text.Html);

            AlternateView plainTextView = AlternateView.CreateAlternateViewFromString(Body, null, "text/plain");
            mailMessage.AlternateViews.Add(plainTextView);
            mailMessage.AlternateViews.Add(htmlView);
            //SmtpClient smtpClient = new SmtpClient();
            //smtpClient.Send(mailMessage);
            return mailMessage;
          }
        }
        catch (Exception)
        {
          return null;
        }
    }

    public static string SendMessageWithAttacment(string Mail, string Subject, string Body, string PathToAttach = "")
    {
      List<string> addresses = Mail.Split(new char[] { ',', ';' }).ToList();
      foreach (string address in addresses)
      {
        try
        {
          SmtpClient client = new SmtpClient();
          MailMessage ms = new MailMessage();
          ms.To.Add(new MailAddress(address));
          ms.Subject = Subject;
          ms.Body = Body;
          ms.IsBodyHtml = true;
          //client.EnableSsl = true;
          //client.Timeout = 90;

          if (!string.IsNullOrEmpty(PathToAttach))
          {
            Attachment data = new Attachment(
                                     PathToAttach,
                                     MediaTypeNames.Application.Octet);
            // your path may look like Server.MapPath("~/file.ABC")
            ms.Attachments.Add(data);
          }

          client.Send(ms);

        }
        catch (Exception ex)
        {
          return ex.Message;
        }
      }
      return "Success";
    }
  }
}