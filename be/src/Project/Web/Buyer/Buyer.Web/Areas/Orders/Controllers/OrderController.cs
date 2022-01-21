using Buyer.Web.Areas.Orders.ViewModel;
using Buyer.Web.Shared.Definitions.Basket;
using Foundation.Account.Definitions;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.Helpers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Orders.Controllers
{
    [Area("Orders")]
    [Authorize]
    public class OrderController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, StatusOrderPageViewModel> editOrderPageModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, OrderPageViewModel> orderPageModelBuilder;

        public OrderController(
            IAsyncComponentModelBuilder<ComponentModelBase, StatusOrderPageViewModel> editOrderPageModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, OrderPageViewModel> orderPageModelBuilder)
        {
            this.editOrderPageModelBuilder = editOrderPageModelBuilder;
            this.orderPageModelBuilder = orderPageModelBuilder;
        }

        public async Task<IActionResult> Index()
        {
            var reqCookie = this.Request.Cookies[BasketConstants.BasketCookieName];
            var componentModel = new ComponentModelBase
            {
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                SellerId = GuidHelper.ParseNullable((this.User.Identity as ClaimsIdentity).Claims.FirstOrDefault(x => x.Type == AccountConstants.OrganisationIdClaim)?.Value),
                IsAuthenticated = this.User.Identity.IsAuthenticated,
                Name = this.User.Identity.Name
            };

            if (reqCookie != null)
            {
                componentModel.BasketId = Guid.Parse(reqCookie);
            }

            var viewModel = await this.orderPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }

        public async Task<IActionResult> Status(Guid? id)
        {
            var componentModel = new ComponentModelBase
            {
                Id = id,
                Language = CultureInfo.CurrentUICulture.Name,
                IsAuthenticated = this.User.Identity.IsAuthenticated,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                SellerId = GuidHelper.ParseNullable((this.User.Identity as ClaimsIdentity).Claims.FirstOrDefault(x => x.Type == AccountConstants.OrganisationIdClaim)?.Value),
                Name = this.User.Identity.Name
            };

            var viewModel = await this.editOrderPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
