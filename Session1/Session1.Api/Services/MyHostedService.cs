using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Session1.Api.Services
{
    public class MyHostedService : BackgroundService
    {
        private IServiceProvider serviceProvider;
        private ILogger<MyHostedService> logger;

        //we wont be able to inject this
        private IMyService service;

        //this wont work because IMyService is scoped
        //public MyHostedService(ILogger<MyHostedService> logger, IMyService service)
        //{
        //    this.logger = logger;
        //    this.service = service;
        //}

        public MyHostedService(IServiceProvider serviceProvider, ILogger<MyHostedService> logger)
        {
            this.logger = logger;
            this.serviceProvider = serviceProvider;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("MyHostedService Service is starting.");
            await base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("MyHostedService Service is stopping.");
            await base.StopAsync(cancellationToken);
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int count = 0;
            while (!stoppingToken.IsCancellationRequested)
            {
                logger.LogInformation($"MyHostedService Service is looping. count:{count}");

                // cant do that because MyService is scoped and BackgroundService is singleton
                // service.GetMyKey();

                //we need to create a scope
                using (var scope = serviceProvider.CreateScope())
                {
                    //lets make 2 just to prove its scoped
                    var scopedService1 = scope.ServiceProvider.GetRequiredService<IMyService>();
                    scopedService1.GetMyKey();
                    var scopedService2 = scope.ServiceProvider.GetRequiredService<IMyService>();
                    scopedService2.GetMyKey();
                }

                count++;
                await Task.Delay(1000);
            }
        }
    }
}
