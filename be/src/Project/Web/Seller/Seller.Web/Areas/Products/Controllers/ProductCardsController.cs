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
    public class ProductCardsController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ProductCardsPageViewModel> _productCardsPageModelBuilder;

        public ProductCardsController(IAsyncComponentModelBuilder<ComponentModelBase, ProductCardsPageViewModel> productCardsPageModelBuilder)
        {
            _productCardsPageModelBuilder = productCardsPageModelBuilder;
        }

        public async Task<IActionResult> Index()
        {
            var componentModel = new ComponentModelBase
            {
                IsAuthenticated = User.Identity.IsAuthenticated,
                Name = User.Identity.Name,
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                SellerId = GuidHelper.ParseNullable((User.Identity as ClaimsIdentity).Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim)?.Value)
            };

            var viewModel = await _productCardsPageModelBuilder.BuildModelAsync(componentModel);

            return View(viewModel);
        }
    }
}
