using Foundation.Account.Definitions;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.Helpers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Seller.Web.Areas.Media.ViewModel;
using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Media.Controllers
{
    [Area("Media")]
    public class MediaItemsController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, MediaItemsPageViewModel> mediasPageModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, MediaItemPageViewModel> mediaItemPageModelBuilder;

        public MediaItemsController(
            IAsyncComponentModelBuilder<ComponentModelBase, MediaItemsPageViewModel> mediasPageModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, MediaItemPageViewModel> mediaItemPageModelBuilder
        )
        {
            this.mediasPageModelBuilder = mediasPageModelBuilder;
            this.mediaItemPageModelBuilder = mediaItemPageModelBuilder;
        }

        public async Task<IActionResult> Index()
        {
            var componentModel = new ComponentModelBase
            {
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                SellerId = GuidHelper.ParseNullable((this.User.Identity as ClaimsIdentity).Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim)?.Value)
            };

            var viewModel = await this.mediasPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }

        public async Task<IActionResult> Item(Guid? id)
        {
            var componentModel = new ComponentModelBase
            {
                Id = id,
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                SellerId = GuidHelper.ParseNullable((this.User.Identity as ClaimsIdentity).Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim)?.Value)
            };

            var viewModel = await this.mediaItemPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
