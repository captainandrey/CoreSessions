using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Session1.Api.Model;
using Session1.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Session1.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private AppSettings appSettings;

        private readonly IMyService serivce;
        private readonly IMyService serivce2;

        public TestController(IOptions<AppSettings> config, IMyService serivce, IMyService serivce2)
        {
            this.appSettings = config.Value;
            this.serivce = serivce;
            this.serivce2 = serivce2;
        }

        [HttpGet]
        public ActionResult Get()
        {
            return Ok($"{serivce.GetMyKey()}\n{serivce2.GetMyKey()}");
        }

        //2.3 Lets add a post action so we can have something more interesting to work with.
        [HttpPost]
        public async Task<ActionResult<TestResponse>> Post(TestRequest requestModel)
        {
            var result = new TestResponse() { SomeValue = await serivce.GetMyKey() };

            return Ok(result);
        }
    }
}
