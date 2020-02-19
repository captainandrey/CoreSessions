using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Session1.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //this adds a lot of default config eg adding appsettings.json, appsettings.{Environment}.json as config files, console logging 
                    webBuilder.UseStartup<Startup>()
                    
                    //but we can make changes
                    .ConfigureAppConfiguration((hostingContext, config) =>
                    {
                        config.AddJsonFile("myownjsonfile.json", optional: true, reloadOnChange: true);
                    });
                });
    }
}
