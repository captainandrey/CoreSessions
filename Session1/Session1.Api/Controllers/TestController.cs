﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
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
        //we configured injection of 
        public TestController(IOptions<AppSettings> config)
        {
            this.appSettings = config.Value;

        }

        //7. our first action
        [HttpGet]
        public ActionResult Get()
        {
            return Ok(appSettings.SomeKey);
        }
    }
}