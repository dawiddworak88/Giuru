using Identity.Api.Infrastructure;
using Identity.Api.v1.Areas.Accounts.Models;
using Identity.Api.v1.Areas.Accounts.ResultModels;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Identity.Api.v1.Areas.Accounts.Services.Organisations
{
    public class SellerService : ISellerService
    {
        private readonly IdentityContext identityContext;

        public SellerService(IdentityContext identityContext)
        {
            this.identityContext = identityContext;
        }

        public async Task<SellerResultModel> GetAsync(GetSellerModel serviceModel)
        {
            var result = new SellerResultModel();

            var organisation = await this.identityContext.Organisations.FirstOrDefaultAsync(x => x.Id == serviceModel.Id && x.IsActive);

            if (organisation != null)
            {
                result.Id = organisation.Id;
                result.Name = organisation.Name;

                var organisationTranslation = await this.identityContext.OrganisationTranslations.FirstOrDefaultAsync(x => x.OrganisationId == organisation.Id && x.Language == serviceModel.Language && x.IsActive);

                if (organisationTranslation == null)
                { 
                    organisationTranslation = await this.identityContext.OrganisationTranslations.FirstOrDefaultAsync(x => x.OrganisationId == organisation.Id && x.IsActive);
                }

                if (organisationTranslation != null)
                {
                    result.Description = organisationTranslation.Description;
                }
            }

            return result;
        }
    }
}
