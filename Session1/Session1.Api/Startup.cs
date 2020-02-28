using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Session1.Api.Dal;
using Session1.Api.Services;

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
            services.AddOptions();
            services.AddControllers();

            services.AddAutoMapper(typeof(Startup));
            services.AddTransient<IAnimalService, AnimalService>();
            services.AddTransient<IAnimalNamingService, AnimalNamingService>();

            var appSettings = config.GetSection("ApplicationSettings");
            services.Configure<AppSettings>(appSettings);
            services.AddDbContext<AnimalsDbContext>(opt => opt.UseInMemoryDatabase(databaseName: "InMemoryDb"), ServiceLifetime.Singleton, ServiceLifetime.Singleton);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
