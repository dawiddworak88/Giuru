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
using Buyer.Web.Areas.Orders.ViewModel;
using Microsoft.AspNetCore.Authorization;

namespace Buyer.Web.Areas.Orders.Controllers
{
    [Area("Orders")]
    [Authorize]
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
                SellerId = GuidHelper.ParseNullable((this.User.Identity as ClaimsIdentity).Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim)?.Value),
                IsAuthenticated = this.User.Identity.IsAuthenticated,
                Name = this.User.Identity.Name
            };

            var viewModel = await this.ordersPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
