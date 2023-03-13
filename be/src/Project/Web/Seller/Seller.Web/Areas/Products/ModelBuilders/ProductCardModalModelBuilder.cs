using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Products.Repositories;
using Seller.Web.Areas.Products.ViewModels;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Products.ModelBuilders
{
    public class ProductCardModalModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ProductCardModalViewModel>
    {
        private readonly IProductAttributesRepository _productAttributesRepository;
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly IStringLocalizer<ProductResources> _productLocalizer;
        private readonly LinkGenerator _linkGenerator;

        public ProductCardModalModelBuilder(
            IProductAttributesRepository productAttributesRepository,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<ProductResources> productLocalizer,
            LinkGenerator linkGenerator) 
        { 
            _productAttributesRepository = productAttributesRepository;
            _globalLocalizer = globalLocalizer;
            _linkGenerator = linkGenerator;
            _productLocalizer = productLocalizer;
        }

        public async Task<ProductCardModalViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new ProductCardModalViewModel
            {
                NameLabel = _globalLocalizer.GetString("Name"),
                SaveText = _globalLocalizer.GetString("SaveText"),
                CancelText = _globalLocalizer.GetString("Cancel"),
                ToDefinitionText = _productLocalizer.GetString("ToDefinition"),
                InputTypeLabel = _globalLocalizer.GetString("Type"),
                DisplayNameLabel = _globalLocalizer.GetString("DisplayName"),
                DisplayNameErrorMessage = _globalLocalizer.GetString("FieldRequiredErrorMessage"),
                NameErrorMessage = _productLocalizer.GetString("ProductCardNameErrorMessage"),
                DefinitionLabel = _globalLocalizer.GetString("Definition"),
                InputTypes = new List<ProductCardModalInputTypeViewModel>
                {
                    new ProductCardModalInputTypeViewModel { Value = "string", Text = _globalLocalizer.GetString("String") },
                    new ProductCardModalInputTypeViewModel { Value = "array", Text = _globalLocalizer.GetString("Array") },
                    new ProductCardModalInputTypeViewModel { Value = "boolean", Text = _globalLocalizer.GetString("Boolean") },
                    new ProductCardModalInputTypeViewModel { Value = "reference", Text = _globalLocalizer.GetString("Reference") },
                    new ProductCardModalInputTypeViewModel { Value = "number", Text = _globalLocalizer.GetString("Number") }
                }
            };

            var productAttributes = await _productAttributesRepository.GetAsync(componentModel.Token, componentModel.Language);

            if (productAttributes is not null)
            {
                viewModel.DefinitionsOptions = productAttributes.OrEmptyIfNull().Select(x => new ProductCardModalDefinitionViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Url = _linkGenerator.GetPathByAction("Edit", "ProductAttribute", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, Id = x.Id })
                });
            }

            return viewModel;
        }
    }
}
