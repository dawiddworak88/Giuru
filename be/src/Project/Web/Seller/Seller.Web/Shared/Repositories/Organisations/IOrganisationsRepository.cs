using Seller.Web.Shared.DomainModels.Organisations;
using System;
using System.Threading.Tasks;

namespace Seller.Web.Shared.Repositories.Organisations
{
    public interface IOrganisationsRepository
    {
        Task<Organisation> GetAsync(string token, string language, string email);
        Task<Guid> SaveAsync(string token, string language, string name, string email, string communicationLanguage);
    }
}
