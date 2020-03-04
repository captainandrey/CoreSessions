using System.Threading.Tasks;

namespace Session1.Api.Services
{
    public interface IMyServiceWithHttpClient
    {
        Task<string> CallSomeApi1();
        Task<string> CallSomeApi2();
        Task<string> CallSomeApi3();
    }
}
