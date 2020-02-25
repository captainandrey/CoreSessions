using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Session1.Api
{
    public class Startup
    {
        private IConfiguration config;

        public Startup(IConfiguration config)
        {
            //3. lets use a constructor that will have IConfiguraion injected for us
            this.config = config;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //4. inject config as IOptions<> for additional isolation
            services.AddOptions();

            //5 lets use controllers
            services.AddControllers();

            //4. lets load configuration into a model we can inject into our services/controllers
            var appSettings = config.GetSection("ApplicationSettings");
            services.Configure<AppSettings>(appSettings);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, /*We can also inject logger here, but not in constructor*/ ILogger<Startup> logger)
        {
            //custom middleware example
            // docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-3.1

            app.Use(async (context, next) =>
            {
                // Do work that doesn't write to the Response.
                logger.LogInformation("Our custom middleware is being used!");

                //if we remove this line, we will short circuit the pipeline, no further middleware will be called!
                await next.Invoke();
                // Do logging or other work that doesn't write to the Response.
            });


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //6 lets also have static files, this is added before routing middleware
            //if request is handled by static file middleware, it is short circuited
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
            Path.Combine(Directory.GetCurrentDirectory(), "MyStaticFiles")),
                RequestPath = "/StaticFiles"
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                //5 lets use controllers
                endpoints.MapControllers(); //6. Map attribute-routed API controllers
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
