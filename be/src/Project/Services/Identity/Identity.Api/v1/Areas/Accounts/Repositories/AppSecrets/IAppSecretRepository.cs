using Identity.Api.Infrastructure.Organisations.Entities;
using System;
using System.Threading.Tasks;

namespace Identity.Api.v1.Areas.Accounts.Repositories.AppSecrets
{
    public interface IAppSecretRepository
    {
        Task<OrganisationAppSecret> GetOrganisationAppSecretAsync(Guid organisationId, string appSecret);
    }
}
