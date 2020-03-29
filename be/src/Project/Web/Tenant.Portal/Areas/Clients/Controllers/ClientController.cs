using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tenant.Portal.Areas.Clients.ViewModels;

namespace Tenant.Portal.Areas.Clients.Controllers
{
    [Area("Clients")]
    [AllowAnonymous]
    public class ClientController : BaseController
    {
        private readonly IModelBuilder<ClientPageViewModel> clientPageModelBuilder;

        public ClientController(IModelBuilder<ClientPageViewModel> homePageModelBuilder)
        {
            this.clientPageModelBuilder = homePageModelBuilder;
        }

        public IActionResult Index()
        {
            var viewModel = this.clientPageModelBuilder.BuildModel();

            return this.View(viewModel);
        }
    }
}
