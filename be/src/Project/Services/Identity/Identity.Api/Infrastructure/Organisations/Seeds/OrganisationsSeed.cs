using Foundation.GenericRepository.Helpers;
using Identity.Api.Infrastructure.Organisations.Definitions;
using Identity.Api.Infrastructure.Organisations.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

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
                        };

                        context.Organisations.Add(EntitySeedHelper.SeedEntity(organisationEntity));

                        var appSecretOrganisation = new AppSecretOrganisation
                        { 
                            AppSecret = organisationConfiguration[OrganisationsSeedConstants.AppSecretIndex],
                            OrganisationId = organisationEntity.Id,
                            CreatedBy = "admin"
                        };

                        context.AppSecretsOrganisations.Add(EntitySeedHelper.SeedEntity(appSecretOrganisation));
                    }

                    context.SaveChanges();
                }
            }
        }
    }
}
