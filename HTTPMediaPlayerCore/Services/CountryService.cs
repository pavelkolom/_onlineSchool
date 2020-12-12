using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HTTPMediaPlayerCore.Models;
using Microsoft.EntityFrameworkCore;

namespace HTTPMediaPlayerCore.Services
{

  public interface ICountryService
  {
    public Task<List<Country>> GetCountries();
    public string SelectedCountry { get; set; }


  }
  public class CountryService: ICountryService
  {

    public async Task<List<Country>> GetCountries()
    {
      List<Country> countries = null;
      using (DuwaysContext db = new DuwaysContext(new DbContextOptions<DuwaysContext>() { }))
      {
        countries = await db.Country.ToListAsync();
      }
      return countries;

    }

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
          using (DuwaysContext db = new DuwaysContext(new DbContextOptions<DuwaysContext>() { }))
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
