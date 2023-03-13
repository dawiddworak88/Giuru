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
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ProductCardModalViewModel> _productCardModalModelBuilder;
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly IStringLocalizer<ProductResources> _productLocalizer;
        private readonly LinkGenerator _linkGenerator;
        private readonly ICategoriesRepository _categoriesRepository;

        public ProductCardFormModelBuilder(
            IAsyncComponentModelBuilder<ComponentModelBase, ProductCardModalViewModel> productCardModalModelBuilder,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ProductResources> productLocalizer,
            ICategoriesRepository categoriesRepository,
            LinkGenerator linkGenerator)
        {
            _globalLocalizer = globalLocalizer;
            _productLocalizer = productLocalizer;
            _linkGenerator = linkGenerator;
            _categoriesRepository = categoriesRepository;
            _productCardModalModelBuilder = productCardModalModelBuilder;
        }

        public async Task<ProductCardFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new ProductCardFormViewModel
            {
                IdLabel = _globalLocalizer.GetString("Id"),
                Title = _productLocalizer.GetString("EditProductCard"),
                NewText = _productLocalizer.GetString("NewProductCardAttribute"),
                SaveText = _globalLocalizer.GetString("SaveText"),
                DefaultInputName = _productLocalizer.GetString("ProductCardDefaultInputName"),
                GeneralErrorMessage = _globalLocalizer.GetString("AnErrorOccurred"),
                NavigateToProductCardsLabel = _productLocalizer.GetString("NavigateToProductCards"),
                ProductCardsUrl = _linkGenerator.GetPathByAction("Index", "ProductCards", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name }),
                FieldRequiredErrorMessage = _globalLocalizer.GetString("FieldRequiredErrorMessage"),
                ProductCardModal = await _productCardModalModelBuilder.BuildModelAsync(componentModel),
                DefinitionUrl = _linkGenerator.GetPathByAction("Definition", "ProductCardApi", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name }),
                SaveUrl = _linkGenerator.GetPathByAction("Index", "ProductCardApi", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name }),
                YesLabel = _globalLocalizer.GetString("Yes"),
                NoLabel = _globalLocalizer.GetString("No"),
                DeleteConfirmationLabel = _globalLocalizer.GetString("DeleteConfirmationLabel"),
                AreYouSureLabel = _globalLocalizer.GetString("AreYouSureLabel"),
                ProductAttributeExistsMessage = _productLocalizer.GetString("ProductAttributeExists")
            };

            if (componentModel.Id.HasValue)
            {
                var categorySchema = await _categoriesRepository.GetCategorySchemaAsync(componentModel.Token, componentModel.Language, componentModel.Id);

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
