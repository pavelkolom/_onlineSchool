using System;
using System.IO;
using System.Linq;
using System.Web;

namespace HTTPMediaPlayer.Models
{
  public static class ErrorWriter
  {
    public static void WriteError(Exception ex)
    {
      //string filePath = @"C:\Error.txt";

      //using (StreamWriter writer = new StreamWriter(filePath, true))
      //{
      //  writer.WriteLine("-----------------------------------------------------------------------------");
      //  writer.WriteLine("Date : " + DateTime.Now.ToString());
      //  writer.WriteLine();

      //  while (ex != null)
      //  {
      //    writer.WriteLine(ex.GetType().FullName);
      //    writer.WriteLine("Message : " + ex.Message);
      //    writer.WriteLine("StackTrace : " + ex.StackTrace);

      //    ex = ex.InnerException;
      //  }
      //}
    }
  }
}