using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.Helpers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ProductCardPageViewModel> _productCardPageModelBuilder;

        public ProductCardController(
            IAsyncComponentModelBuilder<ComponentModelBase, ProductCardPageViewModel> productCardPageModelBuilder)
        {
            _productCardPageModelBuilder = productCardPageModelBuilder;
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            var componentModel = new ComponentModelBase
            {
                Id = id,
                IsAuthenticated = User.Identity.IsAuthenticated,
                Name = User.Identity.Name,
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                SellerId = GuidHelper.ParseNullable((User.Identity as ClaimsIdentity).Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim)?.Value)
            };

            var viewModel = await _productCardPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
