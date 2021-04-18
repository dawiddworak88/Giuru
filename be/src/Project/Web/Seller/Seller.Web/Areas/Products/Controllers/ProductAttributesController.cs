using Foundation.Account.Definitions;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.Helpers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Seller.Web.Areas.Products.ViewModels;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Products.Controllers
{
    [Area("Products")]
    public class ProductAttributesController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ProductAttributesPageViewModel> productAttributesModelBuilder;

        public ProductAttributesController(IAsyncComponentModelBuilder<ComponentModelBase, ProductAttributesPageViewModel> productAttributesModelBuilder)
        {
            this.productAttributesModelBuilder = productAttributesModelBuilder;
        }

        public async Task<IActionResult> Index()
        {
            var componentModel = new ComponentModelBase
            {
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                SellerId = GuidHelper.ParseNullable((this.User.Identity as ClaimsIdentity).Claims.FirstOrDefault(x => x.Type == AccountConstants.OrganisationIdClaim)?.Value)
            };

            var viewModel = await this.productAttributesModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
