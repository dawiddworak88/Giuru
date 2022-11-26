using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Products.Repositories;
using Seller.Web.Areas.Products.ViewModels;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Products.ModelBuilders
{
    public class ProductCardFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ProductCardFormViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<ProductResources> productLocalizer;
        private readonly LinkGenerator linkGenerator;
        private readonly ICategoriesRepository categoriesRepository;

        public ProductCardFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ProductResources> productLocalizer,
            ICategoriesRepository categoriesRepository,
            LinkGenerator linkGenerator)
        {
            this.globalLocalizer = globalLocalizer;
            this.productLocalizer = productLocalizer;
            this.linkGenerator = linkGenerator;
            this.categoriesRepository = categoriesRepository;
        }

        public async Task<ProductCardFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new ProductCardFormViewModel
            {
                IdLabel = this.globalLocalizer.GetString("Id"),
                Title = this.productLocalizer.GetString("EditProductCard"),
                SaveText = this.globalLocalizer.GetString("SaveText"),
                GeneralErrorMessage = this.globalLocalizer.GetString("AnErrorOccurred"),
                NavigateToProductCardsLabel = this.productLocalizer.GetString("NavigateToProductCards"),
                ProductCardsUrl = this.linkGenerator.GetPathByAction("Index", "ProductCards", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name })
            };

            if (componentModel.Id.HasValue)
            {
                var categorySchema = await this.categoriesRepository.GetCategorySchemaAsync(componentModel.Token, componentModel.Language, componentModel.Id);

                if (categorySchema is not null)
                {
                    viewModel.Id = componentModel.Id;
                    viewModel.Schema = categorySchema.Schema;
                    viewModel.UiSchema = categorySchema.UiSchema;
                }
            }

            return viewModel;
        }
    }
}
