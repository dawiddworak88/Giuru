using Foundation.Account.ServicesModels.Tokens;
using System.Threading;
using System.Threading.Tasks;

namespace Foundation.Account.Services.Tokens
{
    public interface IAccessTokenRefreshService
    {
        Task<RefreshedTokensServiceModel> RefreshAsync(string refreshToken, CancellationToken cancellationToken);
        Task RevokeAsync(string refreshToken, CancellationToken cancellationToken);
    }
}
