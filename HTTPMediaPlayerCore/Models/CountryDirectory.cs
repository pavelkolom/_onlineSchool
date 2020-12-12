using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace HTTPMediaPlayerCore.Models
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
          using (DuwaysContext db = new DuwaysContext(new Microsoft.EntityFrameworkCore.DbContextOptions<DuwaysContext>()))
          {
            foreach (var item in db.Country)
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