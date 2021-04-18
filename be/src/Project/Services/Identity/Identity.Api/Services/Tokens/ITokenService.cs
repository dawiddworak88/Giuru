using System;
using System.Threading.Tasks;

namespace Identity.Api.Services.Tokens
{
    public interface ITokenService
    {
        Task<string> GetTokenAsync(string email, Guid organisationId, string appSecret);
    }
}
