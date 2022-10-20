using Buyer.Web.Areas.Orders.ViewModel;
using Buyer.Web.Shared.Definitions.Basket;
using Foundation.Account.Definitions;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.Helpers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Orders.Controllers
{
    [Area("Orders")]
    public class OrderItemController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, OrderItemPageViewModel> orderItemPageModelBuilder;

        public OrderItemController(
            IAsyncComponentModelBuilder<ComponentModelBase, OrderItemPageViewModel> orderItemPageModelBuilder
        )
        {
            this.orderItemPageModelBuilder = orderItemPageModelBuilder;
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            var componentModel = new ComponentModelBase
            {
                Id = id,
                IsAuthenticated = this.User.Identity.IsAuthenticated,
                Language = CultureInfo.CurrentUICulture.Name,
                Name = this.User.Identity.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                SellerId = GuidHelper.ParseNullable((this.User.Identity as ClaimsIdentity).Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim)?.Value),
                BasketId = string.IsNullOrWhiteSpace(this.Request.Cookies[BasketConstants.BasketCookieName]) ? null : Guid.Parse(this.Request.Cookies[BasketConstants.BasketCookieName])
            };

            var viewModel = await this.orderItemPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
