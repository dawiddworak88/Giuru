using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Threading.Tasks;
using Seller.Web.Areas.Products.ViewModels;
using Foundation.Extensions.Helpers;
using System.Security.Claims;
using System.Linq;
using Foundation.Account.Definitions;
using Seller.Web.Areas.Products.ComponentModels;

namespace Seller.Web.Areas.Products.Controllers
{
    [Area("Products")]
    public class ProductAttributeItemController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ProductAttributeItemComponentModel, ProductAttributeItemPageViewModel> productAttributeItemPageModelBuilder;

        public ProductAttributeItemController(IAsyncComponentModelBuilder<ProductAttributeItemComponentModel, ProductAttributeItemPageViewModel> productAttributeItemPageModelBuilder)
        {
            this.productAttributeItemPageModelBuilder = productAttributeItemPageModelBuilder;
        }

        public async Task<IActionResult> New(Guid? id)
        {
            var componentModel = new ProductAttributeItemComponentModel
            {
                ProductAttributeId = id,
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                SellerId = GuidHelper.ParseNullable((this.User.Identity as ClaimsIdentity).Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim)?.Value)
            };

            var viewModel = await this.productAttributeItemPageModelBuilder.BuildModelAsync(componentModel);

            return this.View("Edit", viewModel);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            var componentModel = new ProductAttributeItemComponentModel
            {
                Id = id,
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                SellerId = GuidHelper.ParseNullable((this.User.Identity as ClaimsIdentity).Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim)?.Value)
            };

            var viewModel = await this.productAttributeItemPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
