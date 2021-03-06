using Seller.Web.Areas.Orders.ViewModel;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Microsoft.AspNetCore.Mvc;
using Foundation.PageContent.ComponentModels;
using System.Threading.Tasks;
using System.Globalization;
using Foundation.ApiExtensions.Definitions;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Foundation.Extensions.Helpers;
using System.Linq;
using Foundation.Account.Definitions;

namespace Seller.Web.Areas.Orders.Controllers
{
    [Area("Orders")]
    public class OrdersController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, OrdersPageViewModel> ordersPageModelBuilder;

        public OrdersController(IAsyncComponentModelBuilder<ComponentModelBase, OrdersPageViewModel> ordersPageModelBuilder)
        {
            this.ordersPageModelBuilder = ordersPageModelBuilder;
        }

        public async Task<IActionResult> Index()
        {
            var componentModel = new ComponentModelBase
            {
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                SellerId = GuidHelper.ParseNullable((this.User.Identity as ClaimsIdentity).Claims.FirstOrDefault(x => x.Type == AccountConstants.OrganisationIdClaim)?.Value)
            };

            var viewModel = await this.ordersPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
