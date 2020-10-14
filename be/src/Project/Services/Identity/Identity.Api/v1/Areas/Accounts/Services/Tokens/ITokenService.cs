using System;
using System.Threading.Tasks;

namespace Identity.Api.v1.Areas.Accounts.Services.Tokens
{
    public interface ITokenService
    {
        Task<string> GetTokenAsync(string email, Guid organisationId, string appSecret);
    }
}
