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
using Seller.Web.Areas.Products.ComponentModels;

namespace Seller.Web.Areas.Products.Controllers
{
    [Area("Products")]
    public class ProductController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ProductPageViewModel> editProductPageModelBuilder;
        private readonly IAsyncComponentModelBuilder<DuplicateProductComponentModel, ProductPageViewModel> duplicateProductPageModelBuilder;

        public ProductController(
            IAsyncComponentModelBuilder<ComponentModelBase, ProductPageViewModel> editProductPageModelBuilder,
            IAsyncComponentModelBuilder<DuplicateProductComponentModel, ProductPageViewModel> duplicateProductPageModelBuilder)
        {
            this.editProductPageModelBuilder = editProductPageModelBuilder;
            this.duplicateProductPageModelBuilder = duplicateProductPageModelBuilder;
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            var componentModel = new ComponentModelBase
            {
                Id = id,
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                SellerId = GuidHelper.ParseNullable((this.User.Identity as ClaimsIdentity).Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim)?.Value)
            };

            var viewModel = await this.editProductPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }

        public async Task<IActionResult> Duplicate(Guid? id)
        {
            var componentModel = new DuplicateProductComponentModel
            {
                Id = id,
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                SellerId = GuidHelper.ParseNullable((this.User.Identity as ClaimsIdentity).Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim)?.Value)
            };

            var viewModel = await this.duplicateProductPageModelBuilder.BuildModelAsync(componentModel);

            return this.View("Edit", viewModel);
        }
    }
}
