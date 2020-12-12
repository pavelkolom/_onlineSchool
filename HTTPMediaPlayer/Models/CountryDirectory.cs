using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HTTPMediaPlayer.Models.DB;

namespace HTTPMediaPlayer.Models
{
  public class CountryDirectory
  {
    private Dictionary<int, string> countryOptions;
    // All of your current viewmodel fields here
    public string SelectedCountry { get; set; }

    public Dictionary<int, string> CountryOptions
    {
      get
      {
        if (countryOptions == null)
        {
          countryOptions = new Dictionary<int, string>();
          using (videodbEntities db = new videodbEntities())
          {
            foreach (var item in db.vd_Countries)
            {
              countryOptions.Add(item.Id, item.RusName);
            }
            return countryOptions;
          }
        }
        else
          return countryOptions;
      }
    }
  }
}