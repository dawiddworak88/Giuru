using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Microsoft.AspNetCore.Mvc;
using Seller.Web.Areas.Clients.ViewModels;

namespace Seller.Web.Areas.Clients.Controllers
{
    [Area("Clients")]
    public class ClientController : BaseController
    {
        private readonly IModelBuilder<ClientPageViewModel> clientDetailPageModelBuilder;

        public ClientController(IModelBuilder<ClientPageViewModel> clientDetailPageModelBuilder)
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
