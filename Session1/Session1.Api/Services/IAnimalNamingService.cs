using Session1.Api.Model;
using System.Threading.Tasks;

namespace Session1.Api.Services
{
    public interface IAnimalNamingService
    {
        Task<Animal> GenerateName(Animal animal);
    }
}
