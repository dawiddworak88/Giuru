using Seller.Web.Areas.Orders.ViewModel;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Globalization;
using Foundation.ApiExtensions.Definitions;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Foundation.Extensions.Helpers;
using System.Linq;
using Foundation.Account.Definitions;
using Seller.Web.Areas.Orders.ComponetModels;

namespace Seller.Web.Areas.Orders.Controllers
{
    [Area("Orders")]
    public class OrdersController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<OrdersPageComponentModel, OrdersPageViewModel> _ordersPageModelBuilder;

        public OrdersController(IAsyncComponentModelBuilder<OrdersPageComponentModel, OrdersPageViewModel> ordersPageModelBuilder)
        {
            _ordersPageModelBuilder = ordersPageModelBuilder;
        }

        public async Task<IActionResult> Index(string searchTerm)
        {
            var componentModel = new OrdersPageComponentModel
            {
                IsAuthenticated = User.Identity.IsAuthenticated,
                Name = User.Identity.Name,
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                SellerId = GuidHelper.ParseNullable((User.Identity as ClaimsIdentity).Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim)?.Value),
                SearchTerm = searchTerm
            };

            var viewModel = await _ordersPageModelBuilder.BuildModelAsync(componentModel);

            return View(viewModel);
        }
    }
}
