using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Microsoft.AspNetCore.Mvc;
using Seller.Portal.Areas.Clients.ViewModels;

namespace Seller.Portal.Areas.Clients.Controllers
{
    [Area("Clients")]
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
