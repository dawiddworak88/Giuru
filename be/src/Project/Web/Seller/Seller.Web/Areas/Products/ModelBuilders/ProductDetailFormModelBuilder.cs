using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Threading.Tasks;
using Seller.Web.Areas.Products.Repositories;
using Seller.Web.Areas.Products.ViewModels;
using Foundation.PageContent.ComponentModels;

namespace Seller.Web.Areas.Products.ModelBuilders
{
    public class ProductDetailFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ProductDetailFormViewModel>
    {
        private readonly IProductsRepository productsRepository;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<ProductResources> productLocalizer;
        private readonly LinkGenerator linkGenerator;

        public ProductDetailFormModelBuilder(
            IProductsRepository productRepository,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ProductResources> productLocalizer,
            LinkGenerator linkGenerator)
        {
            this.productsRepository = productRepository;
            this.globalLocalizer = globalLocalizer;
            this.productLocalizer = productLocalizer;
            this.linkGenerator = linkGenerator;
        }

        public async Task<ProductDetailFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new ProductDetailFormViewModel
            {
                GeneralErrorMessage = this.globalLocalizer["AnErrorOccurred"],
                NameLabel = this.globalLocalizer["NameLabel"],
                NameRequiredErrorMessage = this.globalLocalizer["NameRequiredErrorMessage"],
                EnterNameText = this.globalLocalizer["EnterNameText"],
                SelectSchemaLabel = this.globalLocalizer["SelectSchemaLabel"],
                EnterSkuText = this.productLocalizer["EnterSkuText"],
                ProductDetailText = this.productLocalizer["ProductDetailText"],
                SkuRequiredErrorMessage = this.productLocalizer["SkuRequiredErrorMessage"],
                SkuLabel = this.productLocalizer["SkuLabel"],
                SaveText = this.globalLocalizer["SaveText"],
                SaveUrl = this.linkGenerator.GetPathByAction("Save", "ProductApi", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name })
            };

            if (componentModel.Id.HasValue)
            {
                // viewModel.Product = await this.productsRepository.GetProductAsync(componentModel.Token, componentModel.Language, componentModel.Id);
            }

            return viewModel;
        }
    }
}
