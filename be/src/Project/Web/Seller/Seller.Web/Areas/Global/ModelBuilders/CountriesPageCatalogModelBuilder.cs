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
    public class CountriesPageCatalogModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<Country>>
    {
        private readonly ICatalogModelBuilder catalogModelBuilder;
        private readonly IStringLocalizer globalLocalizer;
        private readonly IStringLocalizer clientLocalizer;
        private readonly LinkGenerator linkGenerator;
        private readonly ICountriesRepository countriesRepository;

        public CountriesPageCatalogModelBuilder(
            ICatalogModelBuilder catalogModelBuilder,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ClientResources> clientLocalizer,
            ICountriesRepository countriesRepository,
            LinkGenerator linkGenerator)
        {
            this.catalogModelBuilder = catalogModelBuilder;
            this.globalLocalizer = globalLocalizer;
            this.clientLocalizer = clientLocalizer;
            this.linkGenerator = linkGenerator;
            this.countriesRepository = countriesRepository;
        }

        public async Task<CatalogViewModel<Country>> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = this.catalogModelBuilder.BuildModel<CatalogViewModel<Country>, Country>();

            viewModel.Title = this.globalLocalizer.GetString("Countries");
            viewModel.DefaultItemsPerPage = Constants.DefaultItemsPerPage;

            viewModel.NewText = this.clientLocalizer.GetString("NewCountry");
            viewModel.NewUrl = this.linkGenerator.GetPathByAction("Edit", "Country", new { Area = "Global", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.EditUrl = this.linkGenerator.GetPathByAction("Edit", "Country", new { Area = "Global", culture = CultureInfo.CurrentUICulture.Name });

            viewModel.DeleteApiUrl = this.linkGenerator.GetPathByAction("Delete", "CountriesApi", new { Area = "Global", culture = CultureInfo.CurrentUICulture.Name });
            viewModel.SearchApiUrl = this.linkGenerator.GetPathByAction("Get", "CountriesApi", new { Area = "Global", culture = CultureInfo.CurrentUICulture.Name });

            viewModel.OrderBy = $"{nameof(Country.CreatedDate)} desc";

            viewModel.Table = new CatalogTableViewModel
            {
                Labels = new string[]
                {
                    globalLocalizer.GetString("Name"),
                    globalLocalizer.GetString("LastModifiedDate"),
                    globalLocalizer.GetString("CreatedDate")
                },
                Actions = new List<CatalogActionViewModel>
                {
                    new CatalogActionViewModel
                    {
                        IsEdit = true
                    },
                    new CatalogActionViewModel
                    {
                        IsDelete = true
                    }
                },
                Properties = new List<CatalogPropertyViewModel>
                {
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(Country.Name).ToCamelCase(),
                        IsDateTime = false
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(Country.LastModifiedDate).ToCamelCase(),
                        IsDateTime = true
                    },
                    new CatalogPropertyViewModel
                    {
                        Title = nameof(Country.CreatedDate).ToCamelCase(),
                        IsDateTime = true
                    }
                }
            };

            viewModel.PagedItems = await this.countriesRepository.GetAsync(componentModel.Token, componentModel.Language, null, Constants.DefaultPageIndex, Constants.DefaultItemsPerPage, $"{nameof(Country.CreatedDate)} desc");

            return viewModel;
        }
    }
}
