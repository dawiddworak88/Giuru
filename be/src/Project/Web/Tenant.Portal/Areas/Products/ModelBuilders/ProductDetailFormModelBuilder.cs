using Feature.Product.Resources;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Microsoft.Extensions.Localization;
using Tenant.Portal.Areas.Products.ViewModels;

namespace Tenant.Portal.Areas.Products.ModelBuilders
{
    public class ProductDetailFormModelBuilder : IModelBuilder<ProductDetailFormViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<ProductResources> productLocalizer;

        public ProductDetailFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ProductResources> productLocalizer)
        {
            this.globalLocalizer = globalLocalizer;
            this.productLocalizer = productLocalizer;
        }

        public ProductDetailFormViewModel BuildModel()
        {
            return new ProductDetailFormViewModel
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
                SaveUrl = "#"
            };
        }
    }
}
