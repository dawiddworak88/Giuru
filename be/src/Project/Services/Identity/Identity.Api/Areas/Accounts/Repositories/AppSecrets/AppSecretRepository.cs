using Identity.Api.Infrastructure;
using Identity.Api.Infrastructure.Organisations.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Identity.Api.Areas.Accounts.Repositories.AppSecrets
{
    public class AppSecretRepository : IAppSecretRepository
    {
        private readonly IdentityContext context;

        public AppSecretRepository(IdentityContext context)
        {
            this.context = context;
        }

        public async Task<AppSecretOrganisation> GetOrganisationAppSecretAsync(Guid organisationId, string appSecret)
        {
            return await this.context.AppSecretsOrganisations.FirstOrDefaultAsync(x => x.OrganisationId == organisationId && x.AppSecret == appSecret && x.IsActive);
        }
    }
}
