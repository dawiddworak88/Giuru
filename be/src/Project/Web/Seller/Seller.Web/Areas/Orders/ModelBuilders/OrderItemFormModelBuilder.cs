using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Orders.Repositories.Orders;
using Seller.Web.Areas.Orders.ViewModel;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Orders.ModelBuilders
{
    public class OrderItemFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, OrderItemFormViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<OrderResources> orderLocalizer;
        private readonly LinkGenerator linkGenerator;
        private readonly IOrdersRepository ordersRepository;

        public OrderItemFormModelBuilder
        (
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<OrderResources> orderLocalizer,
            LinkGenerator linkGenerator,
            IOrdersRepository ordersRepository)
        {
            this.globalLocalizer = globalLocalizer;
            this.orderLocalizer = orderLocalizer;
            this.linkGenerator = linkGenerator;
            this.ordersRepository = ordersRepository;
        }

        public async Task<OrderItemFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new OrderItemFormViewModel
            {
                IdLabel = this.globalLocalizer.GetString("Id"),
                Title = this.orderLocalizer.GetString("EditOrder"),
            };
            
            return viewModel;
        }
    }
}
