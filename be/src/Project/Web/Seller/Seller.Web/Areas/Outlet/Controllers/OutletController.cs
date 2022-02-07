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
using Seller.Web.Areas.Outlet.ViewModel;
using System;

namespace Seller.Web.Areas.Outlet.Controllers
{
    [Area("Outlet")]
    public class OutletController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, OutletPageViewModel> outletPageModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, NewOutletPageViewModel> newOutletPageModelBuilder;

        public OutletController(
            IAsyncComponentModelBuilder<ComponentModelBase, OutletPageViewModel> outletPageModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, NewOutletPageViewModel> newOutletPageModelBuilder)
        {
            this.outletPageModelBuilder = outletPageModelBuilder;
            this.newOutletPageModelBuilder = newOutletPageModelBuilder;
        }

        public async Task<IActionResult> Index()
        {
            var componentModel = new ComponentModelBase
            {
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                SellerId = GuidHelper.ParseNullable((this.User.Identity as ClaimsIdentity).Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim)?.Value)
            };

            var viewModel = await this.outletPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            var componentModel = new ComponentModelBase
            {
                Id = id,
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                SellerId = GuidHelper.ParseNullable((this.User.Identity as ClaimsIdentity).Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim)?.Value)
            };

            var viewModel = await this.newOutletPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
