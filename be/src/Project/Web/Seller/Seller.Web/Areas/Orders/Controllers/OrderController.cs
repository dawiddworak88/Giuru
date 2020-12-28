using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Microsoft.AspNetCore.Mvc;
using Seller.Web.Areas.Orders.ViewModel;

namespace Seller.Web.Areas.Orders.Controllers
{
    [Area("Orders")]
    public class OrderController : BaseController
    {
        private readonly IModelBuilder<OrderPageViewModel> orderPageModelBuilder;

        public OrderController(IModelBuilder<OrderPageViewModel> orderPageModelBuilder)
        {
            this.orderPageModelBuilder = orderPageModelBuilder;
        }

        public IActionResult Edit()
        {
            var viewModel = this.orderPageModelBuilder.BuildModel();

            return this.View(viewModel);
        }
    }
}