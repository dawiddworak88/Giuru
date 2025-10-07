using Foundation.Extensions.Exceptions;
using Foundation.GenericRepository.Extensions;
using Foundation.Localization;
using Identity.Api.Infrastructure;
using Identity.Api.Infrastructure.Organisations.Entities;
using Identity.Api.ServicesModels.Organisations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Identity.Api.Services.Organisations
{
    public class OrganisationService : IOrganisationService
    {
        private readonly IdentityContext identityContext;
        private readonly IStringLocalizer globalLocalizer;

        public OrganisationService(
            IdentityContext identityContext,
            IStringLocalizer<GlobalResources> globalLocalizer)
        {
            this.identityContext = identityContext;
            this.globalLocalizer = globalLocalizer;
        }

        public async Task<OrganisationServiceModel> CreateAsync(CreateOrganisationServiceModel serviceModel)
        {
            var existingOrganisation = await this.identityContext.Organisations.FirstOrDefaultAsync(x => x.ContactEmail == serviceModel.Email && x.IsActive);

            if (existingOrganisation != null)
            {
                throw new ConflictException(this.globalLocalizer.GetString("OrganisationExistsAlready"));
            }

            var organisation = new Organisation
            {
                Name = serviceModel.Name,
                ContactEmail = serviceModel.Email,
                IsSeller = false,
                Domain = new MailAddress(serviceModel.Email).Host,
                Key = new MailAddress(serviceModel.Email).Host.Split('.')[0],
                Language = serviceModel.CommunicationsLanguage
            };

            this.identityContext.Organisations.Add(organisation.FillCommonProperties());

            await this.identityContext.SaveChangesAsync();

            return await this.GetAsync(new GetOrganisationModel { Email = serviceModel.Email, Language = serviceModel.Language, OrganisationId = serviceModel.OrganisationId, Username = serviceModel.Username });
        }

        public async Task<bool> IsSellerAsync(Guid id)
        {
            var organisation = await this.identityContext.Organisations.FirstOrDefaultAsync(x => x.Id == id && x.IsActive);

            return organisation?.IsSeller is true;
        }

        public async Task<OrganisationServiceModel> GetAsync(GetSellerModel serviceModel)
        {
            var organisation = await this.identityContext.Organisations.FirstOrDefaultAsync(x => x.Id == serviceModel.Id && x.IsSeller && x.IsActive);

            return await this.GetOrganisationAsync(organisation, serviceModel.Language);
        }

        public async Task<OrganisationServiceModel> GetAsync(GetOrganisationModel serviceModel)
        {
            var organisation = await this.identityContext.Organisations.FirstOrDefaultAsync(x => x.ContactEmail == serviceModel.Email && x.IsActive);

            return await this.GetOrganisationAsync(organisation, serviceModel.Language);
        }

        private async Task<OrganisationServiceModel> GetOrganisationAsync(Organisation organisation, string language)
        {
            if (organisation != null)
            {
                var result = new OrganisationServiceModel
                {
                    Id = organisation.Id,
                    Name = organisation.Name,
                    Images = this.identityContext.OrganisationImages.Where(x => x.OrganisationId == organisation.Id && x.IsActive).Select(x => x.MediaId).ToList(),
                    Videos = this.identityContext.OrganisationVideos.Where(x => x.OrganisationId == organisation.Id && x.IsActive).Select(x => x.MediaId).ToList(),
                    Files = this.identityContext.OrganisationFiles.Where(x => x.OrganisationId == organisation.Id && x.IsActive).Select(x => x.MediaId).ToList()
                };

                var organisationTranslation = await this.identityContext.OrganisationTranslations.FirstOrDefaultAsync(x => x.OrganisationId == organisation.Id && x.Language == language && x.IsActive);

                if (organisationTranslation == null)
                {
                    organisationTranslation = await this.identityContext.OrganisationTranslations.FirstOrDefaultAsync(x => x.OrganisationId == organisation.Id && x.IsActive);
                }

                if (organisationTranslation != null)
                {
                    result.Description = organisationTranslation.Description;
                }

                return result;
            }

            return default;
        }
    }
}
