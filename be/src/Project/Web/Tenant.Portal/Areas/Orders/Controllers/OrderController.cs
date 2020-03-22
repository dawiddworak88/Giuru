using Tenant.Portal.Areas.Orders.ViewModel;
using Foundation.Extensions.Controllers;
using Foundation.Extensions.ModelBuilders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Tenant.Portal.Areas.Orders.Controllers
{
    [Area("Orders")]
    [AllowAnonymous]
    public class OrderController : BaseController
    {
        private readonly IModelBuilder<OrderPageViewModel> orderPageModelBuilder;

        public OrderController(IModelBuilder<OrderPageViewModel> homePageModelBuilder)
        {
            this.orderPageModelBuilder = homePageModelBuilder;
        }

        public IActionResult Index()
        {
            var viewModel = this.orderPageModelBuilder.BuildModel();

            return this.View(viewModel);
        }
    }
}
