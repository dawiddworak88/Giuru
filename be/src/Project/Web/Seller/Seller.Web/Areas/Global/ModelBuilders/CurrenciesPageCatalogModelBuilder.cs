using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.GenericRepository.Definitions;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Global.DomainModels;
using Seller.Web.Areas.Global.Repositories;
using Seller.Web.Shared.Catalogs.ModelBuilders;
using Seller.Web.Shared.ViewModels;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Global.ModelBuilders
{
    public class CurrenciesPageCatalogModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<Currency>>
    {
        private readonly ICatalogModelBuilder _catalogModelBuilder;
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly LinkGenerator _linkGenerator;
        private readonly ICurrenciesRepository _currenciesRepository;

        public CurrenciesPageCatalogModelBuilder(
            ICatalogModelBuilder catalogModelBuilder,
            IStringLocalizer<GlobalResources> globalLocalizer,
            LinkGenerator linkGenerator,
            ICurrenciesRepository currenciesRepository)
        {
            _catalogModelBuilder = catalogModelBuilder;
            _globalLocalizer = globalLocalizer;
            _linkGenerator = linkGenerator;
            _currenciesRepository = currenciesRepository;
        }

        public async Task<CatalogViewModel<Currency>> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = _catalogModelBuilder.BuildModel<CatalogViewModel<Currency>, Currency>();

            viewModel.Title = _globalLocalizer.GetString("Currencies");
            viewModel.DefaultItemsPerPage = Constants.DefaultItemsPerPage;

            viewModel.NewText = _globalLocalizer.GetString("NewCurrency");
            viewModel.NewUrl = _linkGenerator.GetPathByAction("Edit", "Currency", new { Area = "Global", culture = CultureInfo.CurrentUICulture.Name});
            viewModel.EditUrl = _linkGenerator.GetPathByAction("Edit", "Currency", new { Area = "Global", culture = CultureInfo.CurrentUICulture.Name});
            viewModel.DeleteApiUrl = _linkGenerator.GetPathByAction("Delete", "CurrenciesApi", new { Areas = "Global", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.SearchApiUrl = _linkGenerator.GetPathByAction("Get", "CurrenciesApi", new { Areas = "Global", culture = CultureInfo.CurrentUICulture.Name });

            viewModel.OrderBy = $"{nameof(Currency.CreatedDate)} desc";

            viewModel.Table = new CatalogTableViewModel
            {
                Labels = new string[]
                {
                    _globalLocalizer.GetString("Name"),
                    _globalLocalizer.GetString("LastModifiedDate"),
                    _globalLocalizer.GetString("CreatedDate")
                },
                Actions = new List<CatalogActionViewModel>
                {
                    new CatalogActionViewModel 
                    {
                        IsEdit = true,
                    },
                    new CatalogActionViewModel
                    {
                        IsDelete = true,
                    }
                },
                Properties = new List<CatalogPropertyViewModel>
                {
                    new CatalogPropertyViewModel 
                    {
                        Title = nameof(Currency.Name).ToCamelCase(),
                        IsDateTime = false,
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(Currency.LastModifiedDate).ToCamelCase(),
                        IsDateTime = true
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(Currency.CreatedDate).ToCamelCase(),
                        IsDateTime = true
                    }
                }
            };

            viewModel.PagedItems = await _currenciesRepository.GetAsync(componentModel.Token, componentModel.Language, null, Constants.DefaultPageIndex, Constants.DefaultItemsPerPage, $"{nameof(Currency.CreatedDate)} desc");

            return viewModel;
        }
    }
}
