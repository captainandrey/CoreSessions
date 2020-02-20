using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Session1.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Session1.Api.Controllers
{
    [ApiController]
    [Route("[controller]")] //6. routing attribute
    public class TestController : ControllerBase //inherit base controller
    {
        private AppSettings appSettings;

        private IMyService serivce;
        private IMyService serivce2;

        //we configured injection of IOptions
        public TestController(IOptions<AppSettings> config, IMyService serivce, IMyService serivce2)
        {
            this.appSettings = config.Value;
            this.serivce = serivce;
            this.serivce2 = serivce2;
        }

        //7. our first action
        [HttpGet]
        public ActionResult Get()
        {
            return Ok($"{serivce.GetMyKey()}\n{serivce2.GetMyKey()}");
        }
    }
}
