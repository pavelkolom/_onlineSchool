using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HTTPMediaPlayerCore.Models;
using HTTPMediaPlayerCore.Extensons;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.FileProviders;

namespace HTTPMediaPlayerCore
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddSingleton<SubdomainRouteTransformer>();

      services.AddDbContext<DuwaysContext>();
      services.AddScoped<SessionCheck>(); 
      services.AddScoped<AdminCheck>();

      services.AddDistributedMemoryCache();
      services.AddDuwaysMvc();

      services.AddSession(options => {
        options.IdleTimeout = TimeSpan.FromMinutes(60);//You can set Time   
      });

      // If using IIS:
      services.Configure<IISServerOptions>(options =>
      {
        options.AllowSynchronousIO = true;
      });

      services.AddControllersWithViews();
      services.AddDbContext<DuwaysContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString("coursedb")));

      services.AddRazorPages();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }
      app.UseHttpsRedirection();
      app.UseStaticFiles();
      app.UseSession();
      app.UseRouting();

      app.UseAuthorization();

      app.UseFileServer(new FileServerOptions
      {
        FileProvider = new PhysicalFileProvider(@"C:\\Duways\Authors"),
        RequestPath = new PathString("/Authors"),
        EnableDirectoryBrowsing = false
      });

      app.UseFileServer(new FileServerOptions
      {
        FileProvider = new PhysicalFileProvider(@"C:\\Duways\Pages"),
        RequestPath = new PathString("/Pages"),
        EnableDirectoryBrowsing = false
      });


      app.UseEndpoints(endpoints =>
      {
        endpoints.MapDynamicControllerRoute<SubdomainRouteTransformer>(
          pattern: "/");

        endpoints.MapControllerRoute(
            name: "author",
            pattern: "author/{*name}",
            defaults: new { controller = "Author", action = "Profile" }
            );

        endpoints.MapControllerRoute(
            name: "course",
            pattern: "course/{*name}",
            defaults: new { controller = "Course", action = "Index" });

        endpoints.MapControllerRoute(name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

      });


    }
  }

  public class SubdomainRouteTransformer: DynamicRouteValueTransformer
  {
    public override ValueTask<RouteValueDictionary> TransformAsync(HttpContext httpContext, RouteValueDictionary values)
    {
      string[] items = httpContext.Request.Host.Host.Split(".");
      IPAddress address;
      bool isIP = IPAddress.TryParse(httpContext.Request.Host.Host, out address);

      if (items[0] != "duways" && items[0] != "localhost" && !isIP)
      {
        var subDomain = items[0];

        if (!string.IsNullOrEmpty(subDomain))
        {

          RouteValueDictionary val = new RouteValueDictionary();
          val.Add("controller", "Author");
          val.Add("action", "Profile");
          val.Add("name", subDomain);
          return new ValueTask<RouteValueDictionary>(val);
          //values["controller"] = "Author";
          //values["action"] = "Profile";
          //values["name"] = subDomain;
        }

        //return new ValueTask<RouteValueDictionary>(values);
        
      }
      else
      {
        RouteValueDictionary val = new RouteValueDictionary();
        val.Add("controller", "Home");
        val.Add("action", "Index");
        return new ValueTask<RouteValueDictionary>(val);
      }


      return new ValueTask<RouteValueDictionary>(values);
    }

  }
}
