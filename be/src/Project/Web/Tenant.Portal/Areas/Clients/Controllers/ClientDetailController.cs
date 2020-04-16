using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tenant.Portal.Areas.Clients.ViewModels;

namespace Tenant.Portal.Areas.Clients.Controllers
{
    [Area("Clients")]
    [Authorize]
    public class ClientDetailController : BaseController
    {
        private readonly IModelBuilder<ClientDetailPageViewModel> clientDetailPageModelBuilder;

        public ClientDetailController(IModelBuilder<ClientDetailPageViewModel> clientDetailPageModelBuilder)
        {
            this.clientDetailPageModelBuilder = clientDetailPageModelBuilder;
        }

        public IActionResult Index()
        {
            var viewModel = this.clientDetailPageModelBuilder.BuildModel();

            return this.View(viewModel);
        }
    }
}
