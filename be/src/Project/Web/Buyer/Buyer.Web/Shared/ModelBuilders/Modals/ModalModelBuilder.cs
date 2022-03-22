using Buyer.Web.Shared.ViewModels.Modals;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.ModelBuilders.Modal
{
    public class ModalModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, ModalViewModel>
    {
        private readonly IStringLocalizer<OrderResources> orderLocalizer;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;

        public ModalModelBuilder(
            IStringLocalizer<OrderResources> orderLocalizer,
            IStringLocalizer<GlobalResources> globalLocalizer)
        {
            this.orderLocalizer = orderLocalizer;
            this.globalLocalizer = globalLocalizer;
        }

        public async Task<ModalViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new ModalViewModel
            {
                Title = this.orderLocalizer.GetString("AddingToCart"),
                OkLabel = this.globalLocalizer.GetString("Ok"),
                CancelLabel = this.globalLocalizer.GetString("Cancel"),
                MoreInfoLabel = this.orderLocalizer.GetString("MoreInfoLabel"),
                ExternalReferenceLabel = this.orderLocalizer.GetString("ExternalReferenceLabel"),
                DeliveryFromLabel = this.orderLocalizer.GetString("DeliveryFrom"),
                DeliveryToLabel = this.orderLocalizer.GetString("DeliveryTo"),
                QuantityLabel = this.orderLocalizer.GetString("QuantityLabel"),
                AddText = this.orderLocalizer.GetString("AddOrderItem"),
                StockQuantityLabel = this.orderLocalizer.GetString("StockQuantityLabel"),
                OutletQuantityLabel = this.orderLocalizer.GetString("OutletQuantityLabel")
            };

            return viewModel;
        }
    }
}
