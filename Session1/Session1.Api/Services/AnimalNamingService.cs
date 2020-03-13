using Session1.Api.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Session1.Api.Services
{
    public class AnimalNamingService : IAnimalNamingService
    {
        private readonly List<string> names;

        public AnimalNamingService()
        {
            names = new List<string> { "Bob", "Geff", "Sam", "Rodrigo"};
        }

        public Task<Animal> GenerateName(Animal animal)
        {
            var random = new Random();
            var index = random.Next(0, names.Count);
            animal.GivenName =$"{names[index]} the {animal.Name}";

            return Task.FromResult(animal);
        }
        
    }
}
