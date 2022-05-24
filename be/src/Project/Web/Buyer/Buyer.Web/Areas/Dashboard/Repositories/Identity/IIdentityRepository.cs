using Buyer.Web.Areas.Dashboard.DomainModels;
using System;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Dashboard.Repositories.Identity
{
    public interface IIdentityRepository
    {
        Task<Guid> CreateAppSecretAsync(string token, string language);
        Task<Guid> GetSecretAsync(string token, string language);
    }
}
