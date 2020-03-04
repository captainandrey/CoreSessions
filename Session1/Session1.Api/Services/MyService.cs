using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Session1.Api.Services
{
    //2.1 simple service
    public class MyService : IMyService
    {
        private AppSettings appSettings;
        private ILogger<MyService> logger;

        //we have logger and IOptions injection configured. Logger was in ConfigureWebHostDefaults
        public MyService(IOptions<AppSettings> appSettings, ILogger<MyService> logger)
        {
            this.appSettings = appSettings.Value;
            this.logger = logger;
        }

        public Task<string> GetMyKey()
        {
            string result = $"{this.GetHashCode()}:{appSettings.SomeKey}";
            logger.LogInformation(result);
            return Task.FromResult(result);
        }
    }
}
