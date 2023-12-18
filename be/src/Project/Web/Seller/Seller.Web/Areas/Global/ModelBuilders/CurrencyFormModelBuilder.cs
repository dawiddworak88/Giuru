using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using OpenTelemetry;
using Seller.Web.Areas.Global.Repositories;
using Seller.Web.Areas.Global.ViewModels;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Global.ModelBuilders
{
    public class CurrencyFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, CurrencyFormViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly LinkGenerator _linkGenerator;
        private readonly ICurrenciesRepository _currenciesRepository;

        public CurrencyFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            LinkGenerator linkGenerator,
            ICurrenciesRepository currenciesRepository) 
        { 
            _globalLocalizer = globalLocalizer;
            _linkGenerator = linkGenerator;
            _currenciesRepository = currenciesRepository;
        }

        public async Task<CurrencyFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new CurrencyFormViewModel
            {
                Title = _globalLocalizer.GetString("EditCurrency"),
                IdLabel = _globalLocalizer.GetString("Id"),
                NameLabel = _globalLocalizer.GetString("Name"),
                SaveText = _globalLocalizer.GetString("SaveText"),
                FieldRequiredErrorMessage = _globalLocalizer.GetString("FieldRequiredErrorMessage"),
                GeneralErrorMessage = _globalLocalizer.GetString("AnErrorOccurred"),
                NavigateToCurrencies = _globalLocalizer.GetString("NavigateToCurrenciesText"),
                SaveUrl = _linkGenerator.GetPathByAction("Index", "CurrenciesApi", new { Area = "Global", culture = CultureInfo.CurrentUICulture.Name }),
                CurrenciesUrl = _linkGenerator.GetPathByAction("Index", "Currencies", new { Area = "Global", culture = CultureInfo.CurrentUICulture.Name }),
                CurrencyCodeLabel = _globalLocalizer.GetString("CurrencyCodeLabel"),
                SymbolLabel = _globalLocalizer.GetString("SymbolLabel"),

            };

            if (componentModel.Id.HasValue)
            {
                var currency = await _currenciesRepository.GetAsync(componentModel.Token, componentModel.Language, componentModel.Id);

                if (currency is not null)
                { 
                    viewModel.Id = componentModel.Id;
                    viewModel.CurrencyCode = currency.CurrencyCode;
                    viewModel.Symbol = currency.Symbol;
                    viewModel.Name = currency.Name;
                }
            }

            return viewModel;
        }
    }
}
