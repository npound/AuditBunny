using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace LoanLogics
{


    public class Startup
    {

    private static string ConnectionString;

    public Startup(IHostingEnvironment env)
        {
      // Set up configuration sources.
      var builder = new ConfigurationBuilder()
          .SetBasePath(env.ContentRootPath)
          .AddJsonFile("appsettings.json")
          .AddEnvironmentVariables();
      Configuration = builder.Build();

      Startup.ConnectionString = Configuration.GetConnectionString("DefaultConnection");
    }

        public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
        {

      services.AddDbContext<AdventureWorks2014Context>(options =>
               options.UseSqlServer(ConnectionString));

      services.AddMvc();

      services.Configure<SpaSettings>(Configuration.GetSection("SpaSettings"));
      services.AddMvcCore()
         .AddJsonFormatters();



    }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IOptions<SpaSettings> spaSettings)
        {


      app.UseDefaultFiles();
      app.UseStaticFiles();
  
      ConfigureRoutes(app, spaSettings.Value);
    }

    private void ConfigureRoutes(IApplicationBuilder app, SpaSettings spaSettings)
    {



      // If the route contains '.' then assume a file to be served
      // and try to serve using StaticFiles
      // if the route is spa route then let it fall through to the
      // spa index file and have it resolved by the spa application
      app.MapWhen(context => {
        var path = context.Request.Path.Value;
        return !path.Contains("api");
      },
      spa => {
        spa.Use((context, next) =>
        {
          context.Request.Path = new PathString("/" + spaSettings.DefaultPage);
          return next();
        });

        spa.UseStaticFiles();
      });

      app.UseMvc();

    }
  }

  public class SpaSettings
  {
    public string DefaultPage { get; set; }
    public string ApplicationName { get; set; }
  }
}
