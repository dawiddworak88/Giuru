using Identity.Api.Infrastructure.Organisations.Entities;
using System;
using System.Threading.Tasks;

namespace Identity.Api.Repositories.AppSecrets
{
    public interface IAppSecretRepository
    {
        Task<OrganisationAppSecret> GetOrganisationAppSecretAsync(Guid organisationId, string appSecret);
    }
}
