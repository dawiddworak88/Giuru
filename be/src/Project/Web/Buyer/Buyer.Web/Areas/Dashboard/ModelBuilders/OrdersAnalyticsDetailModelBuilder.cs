using Buyer.Web.Areas.Dashboard.ViewModels;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.ComponentModels;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Dashboard.ModelBuilders
{
    public class OrdersAnalyticsDetailModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, OrdersAnalyticsDetailViewModel>
    {
        public OrdersAnalyticsDetailModelBuilder()
        {

        }

        public async Task<OrdersAnalyticsDetailViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new OrdersAnalyticsDetailViewModel
            {

            };

            return viewModel;
        }
    }
}
