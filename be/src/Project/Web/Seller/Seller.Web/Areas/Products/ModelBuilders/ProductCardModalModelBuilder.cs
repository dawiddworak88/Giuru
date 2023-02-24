using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.ListItems.ViewModels;
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

        public ProductCardModalModelBuilder(
            IProductAttributesRepository productAttributesRepository) 
        { 
            _productAttributesRepository = productAttributesRepository;
        }

        public async Task<ProductCardModalViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new ProductCardModalViewModel
            {
                InputTypes = new List<string>
                {
                    "String",
                    "Array",
                    "Boolean"
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
