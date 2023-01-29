using System.Threading.Tasks;

namespace Analytics.Api.Repositories.AccessToken
{
    public interface IAccessTokenRepository
    {
        Task<string> GetAccessTokenAsync();
    }
}
