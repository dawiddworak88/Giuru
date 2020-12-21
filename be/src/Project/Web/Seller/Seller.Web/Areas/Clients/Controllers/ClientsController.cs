using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Microsoft.AspNetCore.Mvc;
using Seller.Web.Areas.Clients.ViewModels;

namespace Seller.Web.Areas.Clients.Controllers
{
    [Area("Clients")]
    public class ClientsController : BaseController
    {
        private readonly IModelBuilder<ClientsPageViewModel> clientPageModelBuilder;

        public ClientsController(IModelBuilder<ClientsPageViewModel> homePageModelBuilder)
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
