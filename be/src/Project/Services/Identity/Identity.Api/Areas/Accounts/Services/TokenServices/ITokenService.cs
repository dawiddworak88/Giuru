using System;
using System.Threading.Tasks;

namespace Identity.Api.Areas.Accounts.Services.TokenServices
{
    public interface ITokenService
    {
        Task<string> GetTokenAsync(string email, Guid organisationId, string appSecret);
    }
}
