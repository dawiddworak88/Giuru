using Feature.Product;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Threading.Tasks;
using Tenant.Portal.Areas.Products.Repositories;
using Tenant.Portal.Areas.Products.ViewModels;
using Tenant.Portal.Shared.ComponentModels;

namespace Tenant.Portal.Areas.Products.ModelBuilders
{
    public class ProductDetailFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ProductDetailFormViewModel>
    {
        private readonly IProductRepository productRepository;
        private readonly IProductSchemaRepository productSchemaRepository;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<ProductResources> productLocalizer;
        private readonly LinkGenerator linkGenerator;

        public ProductDetailFormModelBuilder(
            IProductRepository productRepository,
            IProductSchemaRepository productSchemaRepository,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ProductResources> productLocalizer,
            LinkGenerator linkGenerator)
        {
            this.productRepository = productRepository;
            this.productSchemaRepository = productSchemaRepository;
            this.globalLocalizer = globalLocalizer;
            this.productLocalizer = productLocalizer;
            this.linkGenerator = linkGenerator;
        }

        public async Task<ProductDetailFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new ProductDetailFormViewModel
            {
                Schema = await this.productSchemaRepository.GetProductSchemaByEntityTypeIdAsync(componentModel.Token, componentModel.Language, Definitons.Constants.ProductEntityTypeId),
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
                viewModel.Product = await this.productRepository.GetProductAsync(componentModel.Token, componentModel.Language, componentModel.Id);
            }

            return viewModel;
        }
    }
}
