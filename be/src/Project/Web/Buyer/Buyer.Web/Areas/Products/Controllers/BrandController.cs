using Buyer.Web.Areas.Products.ViewModels.Brands;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Brands.Controllers
{
    [Area("Products")]
    public class BrandController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, BrandPageViewModel> brandPageModelBuilder;

        public BrandController(IAsyncComponentModelBuilder<ComponentModelBase, BrandPageViewModel> brandPageModelBuilder)
        {
            this.brandPageModelBuilder = brandPageModelBuilder;
        }

        public async Task<IActionResult> Index(Guid? id)
        {
            var componentModel = new ComponentModelBase
            {
                Id = id,
                Language = CultureInfo.CurrentUICulture.Name,
                IsAuthenticated = this.User.Identity.IsAuthenticated,
                Name = this.User.Identity.Name
            };

            var viewModel = await this.brandPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
