using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Session1.Api.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Session1.Api.Services
{
    public class AnimalService : IAnimalService
    {
        private readonly AppSettings appSettings;
        private readonly ILogger<AnimalService> logger;
        private readonly AnimalsDbContext context;
        private readonly IMyDependantService myDependantService;
        private readonly IMapper mapper;


        public AnimalService(IOptions<AppSettings> appSettings, ILogger<AnimalService> logger, AnimalsDbContext context, IMyDependantService myDependantService, IMapper mapper)
        {
            this.appSettings = appSettings.Value;
            this.logger = logger;
            this.context = context;
            this.myDependantService = myDependantService;
            this.mapper = mapper;
        }

        public async Task<List<Model.Animal>> GetAll()
        {
            try
            {
                await myDependantService.SomeMethod();
                var dtos = context.Animal.Where(a => !appSettings.OnlyShowRealAnimals || a.IsReal).ToList();
                var model = mapper.Map<List<Model.Animal>>(dtos);

                return model;

            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Error getting all animals");
                throw;
            }
        }

        public async Task<Model.Animal> Get(int id)
        {
            try
            {
                await myDependantService.SomeMethod();
                var dto = context.Animal.FirstOrDefault(a => a.Id == id && ( !appSettings.OnlyShowRealAnimals || a.IsReal));
                var model = mapper.Map<Model.Animal>(dto);

                return model;

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error getting all animals");
                throw;
            }
        }

        public async Task<List<Model.Animal>> Search(string name)
        {
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            try
            {
                await myDependantService.SomeMethod();
                var dtos = context.Animal.Where(a => a.Name.Contains(name) && (!appSettings.OnlyShowRealAnimals || a.IsReal)).ToList();
                var model = mapper.Map<List<Model.Animal>>(dtos);

                return model;

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error searching animals");
                throw;
            }
        }

        public async Task<Model.Animal> Add(Model.Animal animal)
        {
            if (animal is null)
            {
                throw new ArgumentNullException(nameof(animal));
            }
            try
            {
                var dto = mapper.Map<Dal.Dto.Animal>(animal);

                context.Animal.Add(dto);

                await context.SaveChangesAsync();

                var model = mapper.Map<Model.Animal>(dto);

                return model;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error inserting animals");
                throw;
            }

        }


    }
}
