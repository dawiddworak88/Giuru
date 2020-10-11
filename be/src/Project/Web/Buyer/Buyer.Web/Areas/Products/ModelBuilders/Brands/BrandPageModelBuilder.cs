using Buyer.Web.Areas.Products.ViewModels.Brands;
using Buyer.Web.Shared.Headers.ViewModels;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.MainNavigations.ViewModels;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.ModelBuilders.Brands
{
    public class BrandPageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, BrandPageViewModel>
    {
        private readonly IModelBuilder<BuyerHeaderViewModel> headerModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, MainNavigationViewModel> mainNavigationModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, BrandDetailViewModel> brandDetailModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, BrandCatalogViewModel> brandCatalogModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;

        public BrandPageModelBuilder(
            IModelBuilder<BuyerHeaderViewModel> headerModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, MainNavigationViewModel> mainNavigationModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, BrandDetailViewModel> brandDetailModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, BrandCatalogViewModel> brandCatalogModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.mainNavigationModelBuilder = mainNavigationModelBuilder;
            this.brandDetailModelBuilder = brandDetailModelBuilder;
            this.brandCatalogModelBuilder = brandCatalogModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
        }

        public async Task<BrandPageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {

            var viewModel = new BrandPageViewModel
            {
                Header = headerModelBuilder.BuildModel(),
                BrandDetail = await this.brandDetailModelBuilder.BuildModelAsync(componentModel), 
                Catalog = await this.brandCatalogModelBuilder.BuildModelAsync(componentModel),
                MainNavigation = await this.mainNavigationModelBuilder.BuildModelAsync(new ComponentModelBase { Id = componentModel.Id, Token = componentModel.Token, IsAuthenticated = componentModel.IsAuthenticated, Language = componentModel.Language }),
                Footer = footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
