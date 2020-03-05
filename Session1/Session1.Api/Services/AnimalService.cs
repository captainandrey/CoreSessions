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
        private readonly IAnimalNamingService animalNamingService;
        private readonly IMapper mapper;


        public AnimalService(IOptions<AppSettings> appSettings, ILogger<AnimalService> logger, AnimalsDbContext context, IAnimalNamingService animalNamingService, IMapper mapper)
        {
            this.appSettings = appSettings.Value;
            this.logger = logger;
            this.context = context;
            this.animalNamingService = animalNamingService;
            this.mapper = mapper;
        }

        public async Task<List<Model.Animal>> GetAll()
        {
            try
            {

                var dtos = context.Animal.Where(a => !appSettings.OnlyShowRealAnimals || a.IsReal).ToList();
                var model = mapper.Map<List<Model.Animal>>(dtos);

                var tasks = new List<Task<Model.Animal>>();
                foreach(var animal in model)
                {
                    tasks.Add(animalNamingService.GenerateName(animal));
                }

                var result = await Task.WhenAll(tasks);

                return result.ToList();

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
                var dto = context.Animal.FirstOrDefault(a => a.Id == id && ( !appSettings.OnlyShowRealAnimals || a.IsReal));
                if(dto == null)
                {
                    return null;
                }
                var model = mapper.Map<Model.Animal>(dto);

                var result = await animalNamingService.GenerateName(model);

                return result;

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
                var dtos = context.Animal.Where(a => a.Name.Contains(name, StringComparison.InvariantCultureIgnoreCase) && (!appSettings.OnlyShowRealAnimals || a.IsReal)).ToList();
                var model = mapper.Map<List<Model.Animal>>(dtos);

                var tasks = new List<Task<Model.Animal>>();
                foreach (var animal in model)
                {
                    tasks.Add(animalNamingService.GenerateName(animal));
                }

                var result = await Task.WhenAll(tasks);

                return result.ToList();

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error searching animals");
                throw;
            }
        }

        public async Task<Model.Animal> Add(Model.Animal animal)
        {
            try
            {
                if (animal is null)
                {
                    throw new ArgumentNullException(nameof(animal));
                }
            
                var existing = context.Animal.FirstOrDefault(a => a.Id == animal.Id);

                if(existing != null)
                {
                    throw new ArgumentOutOfRangeException(nameof(animal), "Value already exists in database");
                }

                var dto = mapper.Map<Dal.Dto.Animal>(animal);

                context.Animal.Add(dto);
                
                await context.SaveChangesAsync();

                var model = mapper.Map<Model.Animal>(dto);

                var result = await animalNamingService.GenerateName(model);

                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error inserting animals");
                throw;
            }

        }


    }
}
