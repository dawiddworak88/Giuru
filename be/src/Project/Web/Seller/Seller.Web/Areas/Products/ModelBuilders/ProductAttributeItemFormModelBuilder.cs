using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Products.ComponentModels;
using Seller.Web.Areas.Products.Repositories;
using Seller.Web.Areas.Products.ViewModels;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.ModelBuilders.Products
{
    public class ProductAttributeItemFormModelBuilder : IAsyncComponentModelBuilder<ProductAttributeItemComponentModel, ProductAttributeItemFormViewModel>
    {
        private readonly IProductAttributeItemsRepository productAttributeItemsRepository;
        private readonly IStringLocalizer globalLocalizer;
        private readonly IStringLocalizer productLocalizer;
        private readonly LinkGenerator linkGenerator;

        public ProductAttributeItemFormModelBuilder(
            IProductAttributeItemsRepository productAttributeItemsRepository,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ProductResources> productLocalizer,
            LinkGenerator linkGenerator)
        {
            this.productAttributeItemsRepository = productAttributeItemsRepository;
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
                SaveUrl = this.linkGenerator.GetPathByAction("Index", "ProductAttributeItemsApi", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name }),
                ProductAttributeUrl = this.linkGenerator.GetPathByAction("Edit", "ProductAttribute", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, Id = componentModel.ProductAttributeId }),
                NavigateToProductAttributeLabel = this.productLocalizer.GetString("NavigateToProductAttributeLabel")
            };

            if (componentModel.Id.HasValue)
            {
                var productAttributeItem = await this.productAttributeItemsRepository.GetByIdAsync(
                    componentModel.Token,
                    componentModel.Language,
                    componentModel.Id);

                if (productAttributeItem != null)
                {
                    viewModel.Id = productAttributeItem.Id;
                    viewModel.Name = productAttributeItem.Name;
                }
            }
            else
            {
                viewModel.ProductAttributeId = componentModel.ProductAttributeId;
            }

            return viewModel;
        }
    }
}
