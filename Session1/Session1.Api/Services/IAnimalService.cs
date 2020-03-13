using System.Collections.Generic;
using System.Threading.Tasks;

namespace Session1.Api.Services
{
    public interface IAnimalService
    {
        Task<Model.Animal> Get(int id);
        Task<List<Model.Animal>> GetAll();
        Task<List<Model.Animal>> Search(string name);
        Task<Model.Animal> Add(Model.Animal animal);
    }
}
