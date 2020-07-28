using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //BuildWebHost(args).Run();
            CreateHostBuilder(args).Build().Run();

        }

        //public static IWebHost BuildWebHost(string[] args) =>
        //    WebHost.CreateDefaultBuilder(args)
        //        .UseStartup<Startup>()
        //        .ConfigureLogging(log =>
        //        {
        //            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
        //                                == Microsoft.Extensions.Hosting.EnvironmentName.Development)
        //            {
        //                log.ClearProviders();
        //                log.AddDebug();
        //                log.AddConsole();
        //            }
        //        })
        //        .Build();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
               .ConfigureWebHostDefaults(webBuilder =>
               {
                   webBuilder.UseStartup<Startup>();
               });
    }
}

