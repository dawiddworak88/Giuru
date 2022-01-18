using Buyer.Web.Areas.Products.ViewModels.AvailableProducts;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.Controllers
{
    [Area("Products")]
    public class AvailableProductsController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, AvailableProductsPageViewModel> availableProductsPageModelBuilder;

        public AvailableProductsController(IAsyncComponentModelBuilder<ComponentModelBase, AvailableProductsPageViewModel> availableProductsPageModelBuilder)
        {
            this.availableProductsPageModelBuilder = availableProductsPageModelBuilder;
        }

        public async Task<IActionResult> Index()
        {
            var reqCookie = this.Request.Cookies["basket"];
            var componentModel = new ComponentModelBase
            { 
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                Language = CultureInfo.CurrentUICulture.Name,
                IsAuthenticated = this.User.Identity.IsAuthenticated,
                Name = this.User.Identity.Name
            };

            if (reqCookie != null)
            {
                componentModel.BasketId = Guid.Parse(reqCookie);
            }

            var viewModel = await this.availableProductsPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
