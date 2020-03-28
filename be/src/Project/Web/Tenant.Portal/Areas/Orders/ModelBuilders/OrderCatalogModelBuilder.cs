using Feature.Order;
using Foundation.Extensions.ModelBuilders;
using Microsoft.Extensions.Localization;
using Tenant.Portal.Areas.Orders.ViewModel;

namespace Tenant.Portal.Areas.Orders.ModelBuilders
{
    public class OrderCatalogModelBuilder : IModelBuilder<OrderCatalogViewModel>
    {
        private readonly IStringLocalizer<OrderResources> orderLocalizer;

        public OrderCatalogModelBuilder(IStringLocalizer<OrderResources> orderLocalizer)
        {
            this.orderLocalizer = orderLocalizer;
        }

        public OrderCatalogViewModel BuildModel()
        {
            return new OrderCatalogViewModel
            { 
                Title = this.orderLocalizer["Orders"],
                ShowNew = true,
                NewText = this.orderLocalizer["NewOrder"],
                NewUrl = "#"
            };
        }
    }
}
