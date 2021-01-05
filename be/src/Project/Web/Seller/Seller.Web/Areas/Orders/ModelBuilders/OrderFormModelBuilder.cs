using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Orders.ViewModel;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Orders.ModelBuilders
{
    public class OrderFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, OrderFormViewModel>
    {
        private readonly IStringLocalizer<OrderResources> orderLocalizer;

        public OrderFormModelBuilder(IStringLocalizer<OrderResources> orderLocalizer)
        {
            this.orderLocalizer = orderLocalizer;
        }

        public async Task<OrderFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new OrderFormViewModel
            { 
                Title = this.orderLocalizer.GetString("EditOrder")
            };

            return viewModel;
        }
    }
}
