using System.Threading.Tasks;

namespace Session1.Api.Services
{
    public interface IMyService
    {
        Task<string> GetMyKey();
    }
}
