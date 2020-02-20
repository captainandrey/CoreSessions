using Session1.Api.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Session1.Api.Services
{
    public class MyServiceWithHttpClient : IMyServiceWithHttpClient
    {
        public async Task<string> CallSomeApi1()
        {
            using(HttpClient client = new HttpClient())
            {
                var request = new HttpRequestMessage(new HttpMethod("POST"), "https://localhost:5001/test");

                var requestModel = new TestRequest() { SomeProperty = "Foo" };
                var requestString = JsonSerializer.Serialize(requestModel);

                request.Content = new  StringContent(requestString, Encoding.UTF8, "application/json");

                var response = await client.SendAsync(request);
                var contentStream = await response.Content.ReadAsStreamAsync();

                var model = await JsonSerializer.DeserializeAsync<TestResponse>(contentStream, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }); 

                return model.SomeValue;
            }

        }
    }
}
