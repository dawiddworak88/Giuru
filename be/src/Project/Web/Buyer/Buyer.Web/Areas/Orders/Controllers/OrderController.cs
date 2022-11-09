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
using Microsoft.AspNetCore.Http;
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
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, OrderDetailPageViewModel> orderDetailPageModelBuilder;

        public OrderController(
            IAsyncComponentModelBuilder<ComponentModelBase, OrderDetailPageViewModel> orderDetailPageModelBuilder)
        {
            this.orderDetailPageModelBuilder = orderDetailPageModelBuilder;
        }

        public async Task<IActionResult> Detail(Guid? id)
        {
            var componentModel = new ComponentModelBase
            {
                Id = id,
                Language = CultureInfo.CurrentUICulture.Name,
                IsAuthenticated = this.User.Identity.IsAuthenticated,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                BasketId = string.IsNullOrWhiteSpace(this.Request.Cookies[BasketConstants.BasketCookieName]) ? null : Guid.Parse(this.Request.Cookies[BasketConstants.BasketCookieName]),
                SellerId = GuidHelper.ParseNullable((this.User.Identity as ClaimsIdentity).Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim)?.Value),
                Name = this.User.Identity.Name
            };

            var viewModel = await this.orderDetailPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
