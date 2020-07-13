using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Microsoft.AspNetCore.Mvc;
using Seller.Portal.Areas.Clients.ViewModels;

namespace Seller.Portal.Areas.Clients.Controllers
{
    [Area("Clients")]
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
