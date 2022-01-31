using Foundation.Account.Definitions;
using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.Helpers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Seller.Web.Areas.Media.ViewModel;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Media.Controllers
{
    [Area("Media")]
    public class EditController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, EditMediaPageViewModel> editMediaPageModelBuilder;

        public EditController(
            IAsyncComponentModelBuilder<ComponentModelBase, EditMediaPageViewModel> editMediaPageModelBuilder
        )
        {
            this.editMediaPageModelBuilder = editMediaPageModelBuilder;
        }

        public async Task<IActionResult> Index()
        {
            var componentModel = new ComponentModelBase
            {
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName),
                SellerId = GuidHelper.ParseNullable((this.User.Identity as ClaimsIdentity).Claims.FirstOrDefault(x => x.Type == AccountConstants.OrganisationIdClaim)?.Value)
            };

            var viewModel = await this.editMediaPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
