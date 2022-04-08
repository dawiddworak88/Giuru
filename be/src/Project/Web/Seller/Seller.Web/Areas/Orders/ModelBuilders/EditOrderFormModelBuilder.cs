using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.ListItems.ViewModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Orders.Repositories.Orders;
using Seller.Web.Areas.Orders.ViewModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Orders.ModelBuilders
{
    public class EditOrderFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, EditOrderFormViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<OrderResources> orderLocalizer;
        private readonly LinkGenerator linkGenerator;
        private readonly IOrdersRepository ordersRepository;

        public EditOrderFormModelBuilder(
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

        public async Task<EditOrderFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new EditOrderFormViewModel
            {
                IdLabel = this.globalLocalizer.GetString("Id"),
                Title = this.orderLocalizer.GetString("EditOrder"),
                DeliveryFromLabel = this.orderLocalizer.GetString("DeliveryFrom"),
                DeliveryToLabel = this.orderLocalizer.GetString("DeliveryTo"),
                MoreInfoLabel = this.orderLocalizer.GetString("MoreInfoLabel"),
                NameLabel = this.orderLocalizer.GetString("NameLabel"),
                OrderItemsLabel = this.orderLocalizer.GetString("OrderItemsLabel"),
                QuantityLabel = this.orderLocalizer.GetString("QuantityLabel"),
                ExternalReferenceLabel = this.orderLocalizer.GetString("ExternalReferenceLabel"),
                SkuLabel = this.orderLocalizer.GetString("SkuLabel"),
                GeneralErrorMessage = this.globalLocalizer.GetString("AnErrorOccurred"),
                OrderStatusLabel = this.orderLocalizer.GetString("OrderStatus"),
                SaveText = this.orderLocalizer.GetString("UpdateOrderStatus"),
                ClientLabel = this.globalLocalizer.GetString("Client")
            };

            var orderStatuses = await this.ordersRepository.GetOrderStatusesAsync(componentModel.Token, componentModel.Language);

            if (orderStatuses != null)
            {
                viewModel.OrderStatuses = orderStatuses.Select(x => new ListItemViewModel { Id = x.Id, Name = x.Name });
            }

            var order = await this.ordersRepository.GetOrderAsync(componentModel.Token, componentModel.Language, componentModel.Id);

            if (order != null)
            {
                viewModel.Id = order.Id;
                viewModel.OrderStatusId = order.OrderStatusId;
                viewModel.ClientUrl = this.linkGenerator.GetPathByAction("Edit", "Client", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name, Id = order.ClientId });
                viewModel.ClientName = order.ClientName;
                viewModel.UpdateOrderStatusUrl = this.linkGenerator.GetPathByAction("Index", "OrderStatusApi", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name, Id = order.ClientId });
                viewModel.OrderItems = order.OrderItems.Select(x => new OrderItemViewModel
                {
                    ProductId = x.ProductId,
                    Sku = x.ProductSku,
                    Name = x.ProductName,
                    ProductUrl = this.linkGenerator.GetPathByAction("Edit", "Product", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, Id = x.ProductId }),
                    Quantity = x.Quantity,
                    ExternalReference = x.ExternalReference,
                    MoreInfo = x.MoreInfo,
                    DeliveryFrom = x.ExpectedDeliveryFrom,
                    DeliveryTo = x.ExpectedDeliveryTo,
                    ImageAlt = x.ProductName,
                    ImageSrc = x.PictureUrl
                });
            }

            return viewModel;
        }
    }
}