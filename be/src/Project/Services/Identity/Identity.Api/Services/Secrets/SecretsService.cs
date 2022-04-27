using Feature.Account;
using Foundation.Extensions.Exceptions;
using Identity.Api.Definitions;
using Identity.Api.Infrastructure;
using Identity.Api.Infrastructure.Organisations.Entities;
using Identity.Api.ServicesModels.Secrets;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Identity.Api.Services.Secrets
{
    public class SecretsService : ISecretsService
    {
        private readonly IdentityContext identityContext;
        private readonly IStringLocalizer<AccountResources> accountLocalizer;

        public SecretsService(
            IdentityContext identityContext,
            IStringLocalizer<AccountResources> accountLocalizer)
        {
            this.identityContext = identityContext;
            this.accountLocalizer = accountLocalizer;
        }

        public async Task<SecretServiceModel> CreateAsync(CreateSecretServiceModel model)
        {
            var organisation = await this.identityContext.Organisations.FirstOrDefaultAsync(x => x.Id == model.OrganisationId);

            if (organisation is null)
            {
                throw new CustomException(this.accountLocalizer.GetString("OrganisationNotFound"), (int)HttpStatusCode.NotFound);
            }

            var secret = await this.identityContext.OrganisationAppSecrets.FirstOrDefaultAsync(x => x.OrganisationId == model.OrganisationId);

            if (secret is not null)
            {
                throw new CustomException(this.accountLocalizer.GetString("AppSecretExist"), (int)HttpStatusCode.NotFound);
            }

            var appSecret = new OrganisationAppSecret
            {
                OrganisationId = organisation.Id,
                AppSecret = Guid.NewGuid().ToString(),
                CreatedBy = IdentityConstants.AppSecretCreatedBy
            };

            await this.identityContext.OrganisationAppSecrets.AddAsync(appSecret);

            return new SecretServiceModel
            {
                AppSecret = appSecret.AppSecret
            };
        }
    }
}
