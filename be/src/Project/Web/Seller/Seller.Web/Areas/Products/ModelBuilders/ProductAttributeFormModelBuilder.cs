using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Products.Repositories;
using Seller.Web.Areas.Products.ViewModels;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.ModelBuilders.Products
{
    public class ProductAttributeFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ProductAttributeFormViewModel>
    {
        private readonly IProductAttributesRepository productAttributesRepository;
        private readonly IStringLocalizer globalLocalizer;
        private readonly IStringLocalizer productLocalizer;
        private readonly LinkGenerator linkGenerator;

        public ProductAttributeFormModelBuilder(
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

        public async Task<ProductAttributeFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new ProductAttributeFormViewModel
            {
                Title = this.productLocalizer.GetString("EditProductAttribute"),
                NameLabel = this.globalLocalizer.GetString("Name"),
                SaveText = this.globalLocalizer.GetString("SaveText"),
                NameRequiredErrorMessage = this.productLocalizer.GetString("EnterProductAttributeName"),
                GeneralErrorMessage = this.globalLocalizer.GetString("AnErrorOccurred"),
                EditUrl = this.linkGenerator.GetPathByAction("Edit", "ProductAttribute", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name }),
                SaveUrl = this.linkGenerator.GetPathByAction("Index", "ProductAttributesApi", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name }),
                AttributesUrl = this.linkGenerator.GetPathByAction("index", "ProductAttributes", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name }),
                NavigateToAttributesLabel = this.productLocalizer.GetString("NavigateToAttributesLabel")
            };

            if (componentModel.Id.HasValue)
            {
                var productAttribute = await this.productAttributesRepository.GetByIdAsync(
                    componentModel.Token,
                    componentModel.Language,
                    componentModel.Id);

                viewModel.Id = productAttribute.Id;
                viewModel.Name = productAttribute.Name;
            }

            return viewModel;
        }
    }
}
