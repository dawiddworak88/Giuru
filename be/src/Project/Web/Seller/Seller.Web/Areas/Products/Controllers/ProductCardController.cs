using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.Helpers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Seller.Web.Areas.Products.ComponentModels;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using Seller.Web.Areas.Products.ViewModels;
using System.Linq;
using Foundation.Account.Definitions;

namespace Seller.Web.Areas.Products.Controllers
{
    [Area("Products")]
    public class ProductCardController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ProductCardPageViewModel> productCardPageModelBuilder;

        public ProductCardController(
            IAsyncComponentModelBuilder<ComponentModelBase, ProductCardPageViewModel> productCardPageModelBuilder)
        {
            this.productCardPageModelBuilder = productCardPageModelBuilder;
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            var componentModel = new ComponentModelBase
            {
                Id = id,
                IsAuthenticated = this.User.Identity.IsAuthenticated,
                Name = this.User.Identity.Name,
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                SellerId = GuidHelper.ParseNullable((this.User.Identity as ClaimsIdentity).Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim)?.Value)
            };

            var viewModel = await this.productCardPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
