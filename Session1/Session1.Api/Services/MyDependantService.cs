using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Session1.Api.Services
{
    //2.1 simple service
    public class MyDependantService : IMyDependantService
    {
        //we have logger and IOptions injection configured. Logger was in ConfigureWebHostDefaults
        public MyDependantService()
        {
        }

        public Task<string> SomeMethod()
        {
            return Task.FromResult( "Some Value");
        }
    }
}
