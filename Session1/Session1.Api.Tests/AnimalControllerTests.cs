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

namespace Session1.Api.Tests
{
    [TestClass]
    [TestCategory("Integration")]
    public class AnimalControllerTests
    {
        private static IHost host;
        [ClassInitialize]
        public static async Task TestFixtureSetup(TestContext context)
        {
            var hostBuilder = new HostBuilder()
                .ConfigureWebHost(webHost =>
                {
                    webHost.UseTestServer();
                    webHost.UseStartup<Startup>()
                    .ConfigureTestServices(services =>
                    {
                        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AnimalsDbContext>));

                        if (descriptor != null)
                        {
                            services.Remove(descriptor);
                        }

                        services.AddDbContext<AnimalsDbContext>(opt => opt.UseInMemoryDatabase(databaseName: "InMemoryDb"), ServiceLifetime.Scoped, ServiceLifetime.Scoped);
                        
                    });
                });
            hostBuilder.ConfigureAppConfiguration((context, conf) =>
            {
                conf.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            });

            host = await hostBuilder.StartAsync();
        }
        [TestMethod]
        public async Task TestGet()
        {
            var client = host.GetTestClient();
            var response = await client.GetAsync("animal");
            var stringResponse = await response.Content.ReadAsStringAsync();
            Assert.IsTrue(response.IsSuccessStatusCode);

        }

        [TestMethod]
        [DataRow(100)]
        [DataRow(0)]
        [DataRow(-100)]
        public async Task TestGetOne(int id)
        {
            var client = host.GetTestClient();
            var response = await client.GetAsync($"animal/{id}");
            var stringResponse = await response.Content.ReadAsStringAsync();
            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.NotFound);
        }

        [TestMethod]
        public async Task TestPost()
        {
            Animal animal = new Animal() { Id = 1, Name = "Aadvark" };

            var requestString = JsonSerializer.Serialize(animal);

            var stringContent = new StringContent(requestString, System.Text.Encoding.UTF8, "application/json");

            var client = host.GetTestClient();
            var response = await client.PostAsync("animal", stringContent);

            Assert.IsTrue(response.IsSuccessStatusCode);
        }


    }
}
