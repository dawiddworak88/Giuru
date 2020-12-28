using Seller.Web.Areas.Orders.ViewModel;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Microsoft.AspNetCore.Mvc;

namespace Seller.Web.Areas.Orders.Controllers
{
    [Area("Orders")]
    public class OrdersController : BaseController
    {
        private readonly IModelBuilder<OrdersPageViewModel> ordersPageModelBuilder;

        public OrdersController(IModelBuilder<OrdersPageViewModel> ordersPageModelBuilder)
        {
            this.ordersPageModelBuilder = ordersPageModelBuilder;
        }

        public IActionResult Index()
        {
            var viewModel = this.ordersPageModelBuilder.BuildModel();

            return this.View(viewModel);
        }
    }
}
