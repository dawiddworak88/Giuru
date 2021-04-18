using Identity.Api.Infrastructure;
using Identity.Api.Infrastructure.Organisations.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Identity.Api.Repositories.AppSecrets
{
    public class AppSecretRepository : IAppSecretRepository
    {
        private readonly IdentityContext context;

        public AppSecretRepository(IdentityContext context)
        {
            this.context = context;
        }

        public async Task<OrganisationAppSecret> GetOrganisationAppSecretAsync(Guid organisationId, string appSecret)
        {
            return await this.context.OrganisationAppSecrets.FirstOrDefaultAsync(x => x.OrganisationId == organisationId && x.AppSecret == appSecret && x.IsActive);
        }
    }
}
