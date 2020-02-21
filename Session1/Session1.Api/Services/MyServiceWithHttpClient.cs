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
        MyTypedClient typedClient;

        IHttpClientFactory clientFactory;
        //2.5 We can inject HttpClientFactory to handle client creation for us
        public MyServiceWithHttpClient(IHttpClientFactory clientFactory,/*2.7 Typed Client injected instead*/ MyTypedClient typedClient)
        {
            this.clientFactory = clientFactory;
            this.typedClient = typedClient;
        }

        //2.4 This works but has problems. Creating and disposing http client every time will lead to socket exhaustion
        public async Task<string> CallSomeApi1()
        {
            using (HttpClient client = new HttpClient())
            {
                var request = new HttpRequestMessage(new HttpMethod("POST"), "https://localhost:5001/test");

                var requestModel = new TestRequest() { SomeProperty = "Foo" };
                var requestString = JsonSerializer.Serialize(requestModel);

                request.Content = new StringContent(requestString, Encoding.UTF8, "application/json");

                var response = await client.SendAsync(request);
                var contentStream = await response.Content.ReadAsStreamAsync();

                var model = await JsonSerializer.DeserializeAsync<TestResponse>(contentStream, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

                return model.SomeValue;
            }

        }

        //2.5  let the client factory manage client creation
        public async Task<string> CallSomeApi2()
        {
            //2.6 get the preconfigured client
            var client = clientFactory.CreateClient("myclient");

            var request = new HttpRequestMessage(new HttpMethod("POST"), "test");

            var requestModel = new TestRequest() { SomeProperty = "Foo" };
            var requestString = JsonSerializer.Serialize(requestModel);

            request.Content = new StringContent(requestString, Encoding.UTF8, "application/json");

            var response = await client.SendAsync(request);
            var contentStream = await response.Content.ReadAsStreamAsync();

            var model = await JsonSerializer.DeserializeAsync<TestResponse>(contentStream, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            return model.SomeValue;


        }

        //2.7 used typed client
        public async Task<string> CallSomeApi3()
        {
            var request = new TestRequest() { SomeProperty = "Foo" };
            var result = await typedClient.GetSomeValue(request);
            return result.SomeValue;
        }
    }

    //2.7 alternatively we can add a typed client and not worry about string names
    public class MyTypedClient
    {
        private HttpClient client;

        public MyTypedClient(HttpClient client)
        {
            this.client = client;
            this.client.BaseAddress = new Uri("https://localhost:5001/");
            this.client.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public async Task<TestResponse> GetSomeValue(TestRequest requestModel)
        {
            var request = new HttpRequestMessage(new HttpMethod("POST"), "test");

            var requestString = JsonSerializer.Serialize(requestModel);

            request.Content = new StringContent(requestString, Encoding.UTF8, "application/json");

            var response = await client.SendAsync(request);
            var contentStream = await response.Content.ReadAsStreamAsync();

            var model = await JsonSerializer.DeserializeAsync<TestResponse>(contentStream, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            return model;
        }
    }
}
