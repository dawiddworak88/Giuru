using System.Threading.Tasks;

namespace Analytics.Api.Shared.Repositories.AccessToken
{
    public interface IAccessTokenRepository
    {
        Task<string> GetAccessTokenAsync();
    }
}
