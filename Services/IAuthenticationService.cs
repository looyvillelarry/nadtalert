using System.Threading.Tasks;

namespace NADT.Services
{
    public interface IAuthenticationService
    {
        Task<string> FetchTokenAsync();
    }
}