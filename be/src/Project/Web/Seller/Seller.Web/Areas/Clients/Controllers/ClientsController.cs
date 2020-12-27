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
    public class ClientsController : BaseController
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, ClientsPageViewModel> clientsPageModelBuilder;

        public ClientsController(IAsyncComponentModelBuilder<ComponentModelBase, ClientsPageViewModel> clientsPageModelBuilder)
        {
            this.clientsPageModelBuilder = clientsPageModelBuilder;
        }

        public async Task<IActionResult> Index()
        {
            var componentModel = new ComponentModelBase
            {
                Language = CultureInfo.CurrentUICulture.Name,
                Token = await HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName)
            };

            var viewModel = await this.clientsPageModelBuilder.BuildModelAsync(componentModel);

            return this.View(viewModel);
        }
    }
}
