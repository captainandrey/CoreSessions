using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Session1.Api.Dal;
using Session1.Api.Services;
using Session1.Api.Services.Mappers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Session1.Api.Tests
{
    [TestClass]
    [TestCategory("Unit")]
    public class AnimalServiceUnitTests
    {
        IAnimalService service;
        Mock<ILogger<AnimalService>> loggerMock;
        AnimalsDbContext context;
        Mock<IAnimalNamingService> animalNamingServiceMock;

        [TestInitialize]
        public async Task Initialize()
        {
            //these 2 we want to mock out
            loggerMock = new Mock<ILogger<AnimalService>>();
            animalNamingServiceMock = new Mock<IAnimalNamingService>();

            //we want to inject our settings instead of dealing with config files
            var options = Options.Create(new AppSettings { OnlyShowRealAnimals = true });

            //we have two options, we can mock out the context completely or we can have an in memory context
            var contextOptions = new DbContextOptionsBuilder<AnimalsDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .EnableDetailedErrors(true)
                .EnableSensitiveDataLogging(true)
                .Options;

            context = new AnimalsDbContext(contextOptions);

            //lets move this setup step here since its common to all test cases
            var allAnimals = await context.Animal.ToListAsync();
            allAnimals.ForEach(animal => animalNamingServiceMock.Setup(s => s.GenerateName(It.Is<Model.Animal>(a => a.Id == animal.Id))).ReturnsAsync(new Model.Animal
            {
                Id = animal.Id,
                IsReal = animal.IsReal,
                Name = animal.Name,
                GivenName = $"Bob the {animal.Name}"
            }));

            //again, we can mock out the mapper completely, but as an example lets use the real mapper with our mapping profile
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AnimalMapper());
            });
            var mapper = mapperConfig.CreateMapper();

            //populate the db with some test data
            await PopulateTestData();

            //now we can instanciate the service we are testing and inject all the dependencies we have, some are mocks, some are real implementations
            service = new AnimalService(options, loggerMock.Object, context, animalNamingServiceMock.Object, mapper);
        }

        private async Task PopulateTestData()
        {
           
            context.Animal.Add(new Dal.Dto.Animal { Id = 1, IsReal = true, Name = "Aardvark" });
            context.Animal.Add(new Dal.Dto.Animal { Id = 2, IsReal = false, Name = "Unicorn" });
            context.Animal.Add(new Dal.Dto.Animal { Id = 3, IsReal = true, Name = "Narwhal" });
            context.Animal.Add(new Dal.Dto.Animal { Id = 4, IsReal = true, Name = "Kiwi" });
            context.Animal.Add(new Dal.Dto.Animal { Id = 5, IsReal = true, Name = "Manatee" });
            await context.SaveChangesAsync();
        }

        [TestMethod]
        public async Task TestGetAll()
        {
            //we can do better than this
            //animalNamingServiceMock.Setup(s => s.GenerateName(It.IsAny<Model.Animal>())).ReturnsAsync(new Model.Animal { Id = 1, IsReal = false, Name = "TestName", GivenName = "TestGivenName" });

            var animals = await service.GetAll();
            Assert.AreEqual(4, animals.Count);
            animals.ForEach(a => Assert.IsNotNull(a?.GivenName));

            animals.ForEach(a => Console.WriteLine(a.GivenName));
        }

        [TestMethod]
        public async Task TestSearchNull()
        {
            //oops its not that exception
            //await Assert.ThrowsExceptionAsync<NullReferenceException>(() => service.Search(null));
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => service.Search(null));
        }

        [TestMethod]
        //we can have parametrized tests
        [DataRow("ee")]
        [DataRow("aa")]
        public async Task TestSearch(string search)
        {
            var result = await service.Search(search);
            Assert.AreEqual(1, result.Count);
            animalNamingServiceMock.Verify(s => s.GenerateName(It.IsAny<Model.Animal>()), Times.Once);

        }


    }
}
