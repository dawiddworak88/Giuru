using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Clients.Repositories.Countries;
using Seller.Web.Areas.Clients.Repositories.Groups;
using Seller.Web.Areas.Clients.ViewModels;
using Seller.Web.Shared.Repositories.Clients;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.ModelBuilders
{
    public class ClientCountryFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ClientCountryFormViewModel>
    {
        private readonly IClientCountriesRepository clientCountriesRepository;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<ClientResources> clientLocalizer;
        private readonly LinkGenerator linkGenerator;

        public ClientCountryFormModelBuilder(
            IClientCountriesRepository clientCountriesRepository,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ClientResources> clientLocalizer,
            LinkGenerator linkGenerator)
        {
            this.globalLocalizer = globalLocalizer;
            this.clientLocalizer = clientLocalizer;
            this.linkGenerator = linkGenerator;
            this.clientCountriesRepository = clientCountriesRepository;
        }

        public async Task<ClientCountryFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new ClientCountryFormViewModel
            {
                Title = this.clientLocalizer.GetString("EditGroup"),
                IdLabel = this.globalLocalizer.GetString("Id"),
                NameLabel = this.globalLocalizer.GetString("Name"),
                SaveText = this.globalLocalizer.GetString("SaveText"),
                FieldRequiredErrorMessage = this.globalLocalizer.GetString("FieldRequiredErrorMessage"),
                GeneralErrorMessage = this.globalLocalizer.GetString("AnErrorOccurred"),
                NavigateToCountries = this.clientLocalizer.GetString("NavigateToCountriesText"),
                SaveUrl = this.linkGenerator.GetPathByAction("Index", "ClientCountriesApi", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name }),
                CountriesUrl = this.linkGenerator.GetPathByAction("Index", "ClientCountries", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name })
            };

            if (componentModel.Id.HasValue)
            {
                var country = await this.clientCountriesRepository.GetAsync(componentModel.Token, componentModel.Language, componentModel.Id);

                if (country is not null)
                {
                    viewModel.Id = componentModel.Id;
                    viewModel.Name = country.Name;
                }
            }

            return viewModel;
        }
    }
}
