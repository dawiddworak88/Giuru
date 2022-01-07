using Foundation.GenericRepository.Extensions;
using Identity.Api.Infrastructure.Organisations.Definitions;
using Identity.Api.Infrastructure.Organisations.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Web;

namespace Identity.Api.Infrastructure.Organisations.Seeds
{
    public static class OrganisationsSeed
    {
        public static void SeedOrganisations(IdentityContext context, IConfiguration configuration)
        {
            var organisations = configuration["Organisations"]?.Split(";");

            if (organisations != null)
            {
                foreach (var organisation in organisations)
                {
                    var organisationConfiguration = organisation.Split("&");

                    if (!context.Organisations.Any(x => x.Name == organisationConfiguration[OrganisationsSeedConstants.NameIndex]))
                    {
                        var organisationEntity = new Organisation
                        {
                            Id = Guid.Parse(organisationConfiguration[OrganisationsSeedConstants.IdIndex]),
                            Name = organisationConfiguration[OrganisationsSeedConstants.NameIndex],
                            Key = organisationConfiguration[OrganisationsSeedConstants.KeyIndex],
                            Domain = organisationConfiguration[OrganisationsSeedConstants.DomainIndex],
                            IsSeller = bool.Parse(organisationConfiguration[OrganisationsSeedConstants.IsSellerIndex]),
                            Language = organisationConfiguration[OrganisationsSeedConstants.LanguageIndex],
                            ContactEmail = organisationConfiguration[OrganisationsSeedConstants.ContactEmailIndex]
                        };

                        context.Organisations.Add(organisationEntity.FillCommonProperties());

                        var plOrganisationTranslationEntity = new OrganisationTranslation
                        {
                            Language = "pl",
                            OrganisationId = Guid.Parse(organisationConfiguration[OrganisationsSeedConstants.IdIndex]),
                            Description = HttpUtility.UrlDecode(organisationConfiguration[OrganisationsSeedConstants.PlDescriptionIndex])
                        };

                        var enOrganisationTranslationEntity = new OrganisationTranslation
                        {
                            Language = "en",
                            OrganisationId = Guid.Parse(organisationConfiguration[OrganisationsSeedConstants.IdIndex]),
                            Description = HttpUtility.UrlDecode(organisationConfiguration[OrganisationsSeedConstants.EnDescriptionIndex])
                        };

                        var deOrganisationTranslationEntity = new OrganisationTranslation
                        {
                            Language = "de",
                            OrganisationId = Guid.Parse(organisationConfiguration[OrganisationsSeedConstants.IdIndex]),
                            Description = HttpUtility.UrlDecode(organisationConfiguration[OrganisationsSeedConstants.DeDescriptionIndex])
                        };

                        context.OrganisationTranslations.Add(plOrganisationTranslationEntity.FillCommonProperties());
                        context.OrganisationTranslations.Add(enOrganisationTranslationEntity.FillCommonProperties());
                        context.OrganisationTranslations.Add(deOrganisationTranslationEntity.FillCommonProperties());

                        var appSecretOrganisation = new OrganisationAppSecret
                        { 
                            AppSecret = organisationConfiguration[OrganisationsSeedConstants.AppSecretIndex],
                            OrganisationId = organisationEntity.Id,
                            CreatedBy = "admin"
                        };

                        context.OrganisationAppSecrets.Add(appSecretOrganisation.FillCommonProperties());
                    }

                    context.SaveChanges();
                }
            }
        }
    }
}
