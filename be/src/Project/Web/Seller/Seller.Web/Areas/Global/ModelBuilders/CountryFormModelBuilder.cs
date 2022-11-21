using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Global.Repositories;
using Seller.Web.Areas.Global.ViewModels;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Global.ModelBuilders
{
    public class CountryFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, CountryFormViewModel>
    {
        private readonly ICountriesRepository countriesRepository;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<ClientResources> clientLocalizer;
        private readonly LinkGenerator linkGenerator;

        public CountryFormModelBuilder(
            ICountriesRepository countriesRepository,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ClientResources> clientLocalizer,
            LinkGenerator linkGenerator)
        {
            this.globalLocalizer = globalLocalizer;
            this.clientLocalizer = clientLocalizer;
            this.linkGenerator = linkGenerator;
            this.countriesRepository = countriesRepository;
        }

        public async Task<CountryFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new CountryFormViewModel
            {
                Title = this.clientLocalizer.GetString("EditCountry"),
                IdLabel = this.globalLocalizer.GetString("Id"),
                NameLabel = this.globalLocalizer.GetString("Name"),
                SaveText = this.globalLocalizer.GetString("SaveText"),
                FieldRequiredErrorMessage = this.globalLocalizer.GetString("FieldRequiredErrorMessage"),
                GeneralErrorMessage = this.globalLocalizer.GetString("AnErrorOccurred"),
                NavigateToCountries = this.clientLocalizer.GetString("NavigateToCountriesText"),
                SaveUrl = this.linkGenerator.GetPathByAction("Index", "CountriesApi", new { Area = "Global", culture = CultureInfo.CurrentUICulture.Name }),
                CountriesUrl = this.linkGenerator.GetPathByAction("Index", "Countries", new { Area = "Global", culture = CultureInfo.CurrentUICulture.Name })
            };

            if (componentModel.Id.HasValue)
            {
                var country = await this.countriesRepository.GetAsync(componentModel.Token, componentModel.Language, componentModel.Id);

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
