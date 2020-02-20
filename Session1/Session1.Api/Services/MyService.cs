using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Session1.Api.Services
{
    public class MyService : IMyService
    {
        private AppSettings appSettings;
        private ILogger<MyService> logger;


        public MyService(IOptions<AppSettings> appSettings, ILogger<MyService> logger)
        {
            this.appSettings = appSettings.Value;
            this.logger = logger;
        }

        public string GetMyKey()
        {
            string result = $"{this.GetHashCode()}:{appSettings.SomeKey}";
            logger.LogInformation(result);
            return result;
        }
    }
}
