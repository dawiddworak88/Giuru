using Foundation.ApiExtensions.Definitions;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Seller.Web.Areas.Clients.ViewModels;
using System.Globalization;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Clients.Controllers
{
    [Area("Clients")]
    public class ClientGroupsController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ClientGroupsPageViewModel> groupsPageModelBuilder;

        public ClientGroupsController(
            IAsyncComponentModelBuilder<ComponentModelBase, ClientGroupsPageViewModel> groupsPageModelBuilder)
        {
            this.groupsPageModelBuilder = groupsPageModelBuilder;
        }

        public async Task<IActionResult> Index()
        {
            var componentModel = new ComponentModelBase
            {
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName)
            };

            var viewModel = await this.groupsPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}