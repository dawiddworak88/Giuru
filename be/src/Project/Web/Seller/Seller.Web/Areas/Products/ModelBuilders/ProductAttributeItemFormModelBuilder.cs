using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Products.ComponentModels;
using Seller.Web.Areas.Products.Repositories;
using Seller.Web.Areas.Products.ViewModels;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Products.ModelBuilders
{
    public class ProductAttributeItemFormModelBuilder : IAsyncComponentModelBuilder<ProductAttributeItemComponentModel, ProductAttributeItemFormViewModel>
    {
        private readonly IProductAttributesRepository productAttributesRepository;
        private readonly IStringLocalizer globalLocalizer;
        private readonly IStringLocalizer productLocalizer;
        private readonly LinkGenerator linkGenerator;

        public ProductAttributeItemFormModelBuilder(
            IProductAttributesRepository productAttributesRepository,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ProductResources> productLocalizer,
            LinkGenerator linkGenerator)
        {
            this.productAttributesRepository = productAttributesRepository;
            this.globalLocalizer = globalLocalizer;
            this.productLocalizer = productLocalizer;
            this.linkGenerator = linkGenerator;
        }

        public async Task<ProductAttributeItemFormViewModel> BuildModelAsync(ProductAttributeItemComponentModel componentModel)
        {
            var viewModel = new ProductAttributeItemFormViewModel
            {
                Title = this.productLocalizer.GetString("EditProductAttributeItem"),
                NameLabel = this.globalLocalizer.GetString("Name"),
                SaveText = this.globalLocalizer.GetString("SaveText"),
                NameRequiredErrorMessage = this.productLocalizer.GetString("EnterProductAttributeItemName"),
                GeneralErrorMessage = this.globalLocalizer.GetString("AnErrorOccurred"),
                SaveUrl = this.linkGenerator.GetPathByAction("Index", "ProductAttributeItemsApi", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name })
            };

            if (componentModel.Id.HasValue)
            {
            }
            else
            {
                viewModel.ProductAttributeId = componentModel.ProductAttributeId;
            }

            return viewModel;
        }
    }
}
