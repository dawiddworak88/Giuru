using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Microsoft.AspNetCore.Mvc;
using Seller.Web.Areas.Orders.ViewModel;

namespace Seller.Web.Areas.Orders.Controllers
{
    [Area("Orders")]
    public class OrderDetailController : BaseController
    {
        private readonly IModelBuilder<OrderDetailPageViewModel> orderDetailPageModelBuilder;

        public OrderDetailController(IModelBuilder<OrderDetailPageViewModel> orderDetailPageModelBuilder)
        {
            this.orderDetailPageModelBuilder = orderDetailPageModelBuilder;
        }

        public IActionResult Index()
        {
            var viewModel = this.orderDetailPageModelBuilder.BuildModel();

            return this.View(viewModel);
        }
    }
}