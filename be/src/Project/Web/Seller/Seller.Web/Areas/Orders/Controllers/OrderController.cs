using DocumentFormat.OpenXml.Vml.Spreadsheet;
using Foundation.Account.Definitions;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.Helpers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Seller.Web.Areas.Orders.ViewModel;
using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Orders.Controllers
{
    [Area("Orders")]
    public class OrderController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, OrderPageViewModel> orderPageModelBuilder;

        public OrderController(
            IAsyncComponentModelBuilder<ComponentModelBase, OrderPageViewModel> orderPageModelBuilder)
        {
            this.orderPageModelBuilder = orderPageModelBuilder;
        }

        [HttpGet]
        public async Task<IActionResult> Detail(Guid? id)
        {
            var componentModel = new ComponentModelBase
            {
                Id = id,
                IsAuthenticated = this.User.Identity.IsAuthenticated,
                Name = this.User.Identity.Name,
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                SellerId = GuidHelper.ParseNullable((this.User.Identity as ClaimsIdentity).Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim)?.Value)
            };

            var viewModel = await this.orderPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
