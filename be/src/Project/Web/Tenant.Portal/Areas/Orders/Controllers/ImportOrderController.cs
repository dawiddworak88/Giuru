using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Microsoft.AspNetCore.Mvc;
using Tenant.Portal.Areas.Orders.ViewModel;

namespace Tenant.Portal.Areas.Orders.Controllers
{
    [Area("Orders")]
    public class ImportOrderController : BaseController
    {
        private readonly IModelBuilder<ImportOrderPageViewModel> importOrderPageModelBuilder;

        public ImportOrderController(IModelBuilder<ImportOrderPageViewModel> importOrderPageModelBuilder)
        {
            this.importOrderPageModelBuilder = importOrderPageModelBuilder;
        }

        public IActionResult Index()
        {
            var viewModel = this.importOrderPageModelBuilder.BuildModel();

            return this.View(viewModel);
        }
    }
}