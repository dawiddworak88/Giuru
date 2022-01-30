using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Seller.Web.Areas.Products.ViewModels;
using Foundation.PageContent.ComponentModels;
using System;
using System.Globalization;
using System.Security.Claims;
using System.Linq;
using Foundation.Account.Definitions;
using Foundation.Extensions.Helpers;

namespace Seller.Web.Areas.Products.Controllers
{
    [Area("Products")]
    public class ProductsController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ProductsPageViewModel> productsPageModelBuilder;

        public ProductsController(IAsyncComponentModelBuilder<ComponentModelBase, ProductsPageViewModel> productsPageModelBuilder)
        {
            this.productsPageModelBuilder = productsPageModelBuilder;
        }

        public async Task<IActionResult> Index()
        {
            var componentModel = new ComponentModelBase
            {
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                SellerId = GuidHelper.ParseNullable((this.User.Identity as ClaimsIdentity).Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim)?.Value)
            };

            var viewModel = await this.productsPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
