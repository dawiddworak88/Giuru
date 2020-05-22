using System.Threading.Tasks;

namespace Feature.Account.Services.TokenServices
{
    public interface ITokenService
    {
        Task<string> GetTokenAsync(string email, string password);
    }
}
