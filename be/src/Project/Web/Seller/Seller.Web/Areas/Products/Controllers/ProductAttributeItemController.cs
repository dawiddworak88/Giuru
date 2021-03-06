using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Threading.Tasks;
using Seller.Web.Areas.Products.ViewModels;
using Foundation.PageContent.ComponentModels;
using Foundation.Extensions.Helpers;
using System.Security.Claims;
using System.Linq;
using Foundation.Account.Definitions;

namespace Seller.Web.Areas.Products.Controllers
{
    [Area("Products")]
    public class ProductAttributeItemController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ProductAttributeItemPageViewModel> productAttributeItemPageModelBuilder;

        public ProductAttributeItemController(IAsyncComponentModelBuilder<ComponentModelBase, ProductAttributeItemPageViewModel> productAttributeItemPageModelBuilder)
        {
            this.productAttributeItemPageModelBuilder = productAttributeItemPageModelBuilder;
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            var componentModel = new ComponentModelBase
            {
                Id = id,
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                SellerId = GuidHelper.ParseNullable((this.User.Identity as ClaimsIdentity).Claims.FirstOrDefault(x => x.Type == AccountConstants.OrganisationIdClaim)?.Value)
            };

            var viewModel = await this.productAttributeItemPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
