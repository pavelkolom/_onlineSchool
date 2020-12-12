using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTTPMediaPlayerCore.Models
{
  public class PasswordRecoveryModel
  {
    public PasswordRecoveryModel(bool success, string result, string emailval)
    {
      Success = success;
      Result = result;
      EmailVal = emailval;
    }
    public bool Success { get; set; }
    public string Result { get; set; }
    public string EmailVal { get; set; }
  }
}
