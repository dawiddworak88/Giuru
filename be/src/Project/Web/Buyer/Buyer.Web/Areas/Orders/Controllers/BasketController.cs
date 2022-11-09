using Buyer.Web.Areas.Orders.ViewModel;
using Buyer.Web.Shared.Definitions.Basket;
using Foundation.Account.Definitions;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.Helpers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Authentication;
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
    public class BasketController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, BasketPageViewModel> basketPageModelBuilder;

        public BasketController(
            IAsyncComponentModelBuilder<ComponentModelBase, BasketPageViewModel> basketPageModelBuilder)
        {
            this.basketPageModelBuilder = basketPageModelBuilder;
        }

        public async Task<IActionResult> Index()
        {
            var componentModel = new ComponentModelBase
            {
                IsAuthenticated = this.User.Identity.IsAuthenticated,
                Name = this.User.Identity.Name,
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                SellerId = GuidHelper.ParseNullable((this.User.Identity as ClaimsIdentity).Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim)?.Value),
                BasketId = string.IsNullOrWhiteSpace(this.Request.Cookies[BasketConstants.BasketCookieName]) ? null : Guid.Parse(this.Request.Cookies[BasketConstants.BasketCookieName])
            };

            var viewModel = await this.basketPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
