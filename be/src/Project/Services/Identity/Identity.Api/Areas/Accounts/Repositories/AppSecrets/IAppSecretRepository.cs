using Identity.Api.Infrastructure.Organisations.Entities;
using System;
using System.Threading.Tasks;

namespace Identity.Api.Areas.Accounts.Repositories.AppSecrets
{
    public interface IAppSecretRepository
    {
        Task<AppSecretOrganisation> GetOrganisationAppSecretAsync(Guid organisationId, string appSecret);
    }
}
