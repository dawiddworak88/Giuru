using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.ListItems.ViewModels;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Products.Repositories;
using Seller.Web.Areas.Products.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Products.ModelBuilders
{
    public class ProductCardModalModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ProductCardModalViewModel>
    {
        private readonly IProductAttributesRepository _productAttributesRepository;
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;

        public ProductCardModalModelBuilder(
            IProductAttributesRepository productAttributesRepository,
            IStringLocalizer<GlobalResources> globalLocalizer) 
        { 
            _productAttributesRepository = productAttributesRepository;
            _globalLocalizer = globalLocalizer;
        }

        public async Task<ProductCardModalViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new ProductCardModalViewModel
            {
                NameLabel = _globalLocalizer.GetString("Name"),
                SelectDefinitionLabel = "asdasd",
                SaveText = _globalLocalizer.GetString("SaveText"),
                CancelText = _globalLocalizer.GetString("Cancel"),
                InputTypeLabel = _globalLocalizer.GetString("Type"),
                DisplayNameLabel = _globalLocalizer.GetString("DisplayName"),
                DefinitionLabel = _globalLocalizer.GetString("Definition"),
                InputTypes = new List<string>
                {
                    "String",
                    "Array",
                    "Boolean",
                    "Reference"
                }
            };

            var productAttributes = await _productAttributesRepository.GetAsync(componentModel.Token, componentModel.Language);

            if (productAttributes is not null)
            {
                viewModel.DefinitionsOptions = productAttributes.OrEmptyIfNull().Select(x => new ListItemViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                });
            }

            return viewModel;
        }
    }
}
