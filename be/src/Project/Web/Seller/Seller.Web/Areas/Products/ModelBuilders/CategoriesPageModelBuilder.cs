using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.Headers.ViewModels;
using Foundation.PageContent.MenuTiles.ViewModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Products.ViewModels;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Products.ModelBuilders
{
    public class CategoriesPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, CategoriesPageViewModel>
    {
        private readonly IModelBuilder<HeaderViewModel> headerModelBuilder;
        private readonly IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;
        private readonly IStringLocalizer<ProductResources> productLocalizer;
        private readonly LinkGenerator linkGenerator;

        public CategoriesPageModelBuilder(
            IStringLocalizer<ProductResources> productLocalizer,
            LinkGenerator linkGenerator,
            IModelBuilder<HeaderViewModel> headerModelBuilder,
            IModelBuilder<MenuTilesViewModel> menuTilesModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            this.productLocalizer = productLocalizer;
            this.linkGenerator = linkGenerator;
            this.headerModelBuilder = headerModelBuilder;
            this.menuTilesModelBuilder = menuTilesModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
        }

        public async Task<CategoriesPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new CategoriesPageViewModel
            {
                Locale = CultureInfo.CurrentUICulture.Name,
                Header = headerModelBuilder.BuildModel(),
                MenuTiles = menuTilesModelBuilder.BuildModel(),
                Footer = footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}