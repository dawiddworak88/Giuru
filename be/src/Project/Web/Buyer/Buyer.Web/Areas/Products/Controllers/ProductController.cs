using Buyer.Web.Areas.Products.ViewModels.Products;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.Controllers
{
    [Area("Products")]
    public class ProductController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ProductPageViewModel> productPageModelBuilder;

        public ProductController(IAsyncComponentModelBuilder<ComponentModelBase, ProductPageViewModel> productPageModelBuilder)
        {
            this.productPageModelBuilder = productPageModelBuilder;
        }

        public async Task<IActionResult> Index(Guid? productId)
        {
            var componentModel = new ComponentModelBase
            {
                Language = CultureInfo.CurrentUICulture.Name
            };

            var viewModel = await this.productPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
