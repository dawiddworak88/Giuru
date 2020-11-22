using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Products.DomainModels;
using Seller.Web.Areas.Products.ModelBuilders;
using Seller.Web.Shared.Catalogs.ModelBuilders;
using Seller.Web.Shared.ViewModels;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Categories.ModelBuilders
{
    public class CategoriesPageCatalogModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, CatalogViewModel<Category>>
    {
        private readonly ICatalogModelBuilder catalogModelBuilder;
        private readonly ICategoriesRepository categoriesRepository;
        private readonly IStringLocalizer globalLocalizer;
        private readonly IStringLocalizer productLocalizer;
        private readonly LinkGenerator linkGenerator;

        public CategoriesPageCatalogModelBuilder(
            ICatalogModelBuilder catalogModelBuilder,
            ICategoriesRepository categoriesRepository,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ProductResources> productLocalizer,
            LinkGenerator linkGenerator)
        {
            this.catalogModelBuilder = catalogModelBuilder;
            this.categoriesRepository = categoriesRepository;
            this.globalLocalizer = globalLocalizer;
            this.productLocalizer = productLocalizer;
            this.linkGenerator = linkGenerator;
        }

        public async Task<CatalogViewModel<Category>> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = this.catalogModelBuilder.BuildModel<CatalogViewModel<Category>, Category>();

            // viewModel.PagedItems = await this.categoriesRepository.GetCategoriesAsync(componentModel.Token, CultureInfo.CurrentUICulture.Name, null, Foundation.GenericRepository.Definitions.Constants.DefaultPageIndex, Foundation.GenericRepository.Definitions.Constants.DefaultItemsPerPage);

            return viewModel;
        }
    }
}
