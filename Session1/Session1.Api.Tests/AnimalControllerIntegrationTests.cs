using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Session1.Api.Model;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Session1.Api.Dal;
using System;
using Session1.Api.Services;

namespace Session1.Api.Tests
{
    [TestClass]
    [TestCategory("Integration")]
    public class AnimalControllerIntegrationTests
    {
        private static IHost host;
        private static HttpClient client;

        [ClassInitialize]
        public static async Task TestFixtureSetup(TestContext context)
        {
            var hostBuilder = new HostBuilder()
                .ConfigureWebHost(webHost =>
                {
                    webHost.UseTestServer();
                    webHost.UseStartup<Startup>()
                    //although its an integration test, we dont always want to use a real db, we can replace injected context with our own, in memory one.
                    .ConfigureTestServices(services =>
                    {
                        var descriptors = services.Where(d => d.ServiceType == typeof(DbContextOptions<AnimalsDbContext>) || d.ServiceType == typeof(AnimalsDbContext)).ToList();

                        foreach(var descriptor in descriptors)
                        {
                            services.Remove(descriptor);
                        }

                        services.AddDbContext<AnimalsDbContext>(opt => opt.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()), ServiceLifetime.Transient, ServiceLifetime.Transient);

                    })
                    ;
                });
            hostBuilder.ConfigureAppConfiguration((context, conf) =>
            {
                conf.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            });

            host = await hostBuilder.StartAsync();
            client = host.GetTestClient();
        }
        [TestMethod]
        public async Task TestGet()
        {
            var response = await client.GetAsync("animal");
            var stringResponse = await response.Content.ReadAsStringAsync();
            Assert.IsTrue(response.IsSuccessStatusCode);

        }

        [TestMethod]
        [DataRow(100)]
        [DataRow(0)]
        [DataRow(-100)]
        public async Task TestGetOneNotFound(int id)
        {
            var response = await client.GetAsync($"animal/{id}");
            var stringResponse = await response.Content.ReadAsStringAsync();
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.NotFound);
        }

        [TestMethod]
        public async Task TestPost()
        {
            Animal animal = new Animal() { Id = 1, Name = "Aadvark", IsReal = true };

            var requestString = JsonSerializer.Serialize(animal);

            var stringContent = new StringContent(requestString, System.Text.Encoding.UTF8, "application/json");

            var response = await client.PostAsync("animal", stringContent);

            Assert.IsTrue(response.IsSuccessStatusCode);
        }


    }
}
