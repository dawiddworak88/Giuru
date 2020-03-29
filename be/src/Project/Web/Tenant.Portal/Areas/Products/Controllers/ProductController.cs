using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tenant.Portal.Areas.Products.ViewModels;

namespace Tenant.Portal.Areas.Products.Controllers
{
    [Area("Products")]
    [AllowAnonymous]
    public class ProductController : BaseController
    {
        private readonly IModelBuilder<ProductPageViewModel> productPageModelBuilder;

        public ProductController(IModelBuilder<ProductPageViewModel> homePageModelBuilder)
        {
            this.productPageModelBuilder = homePageModelBuilder;
        }

        public IActionResult Index()
        {
            var viewModel = this.productPageModelBuilder.BuildModel();

            return this.View(viewModel);
        }
    }
}
