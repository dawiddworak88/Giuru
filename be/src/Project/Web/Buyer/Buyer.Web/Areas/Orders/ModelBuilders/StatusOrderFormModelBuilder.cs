using Buyer.Web.Areas.Orders.Repositories;
using Buyer.Web.Areas.Orders.ViewModel;
using Buyer.Web.Areas.Products.DomainModels;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.ListItems.ViewModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Orders.ModelBuilders
{
    public class StatusOrderFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, StatusOrderFormViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<OrderResources> orderLocalizer;
        private readonly LinkGenerator linkGenerator;
        private readonly IOrdersRepository ordersRepository;

        public StatusOrderFormModelBuilder(
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

        public async Task<StatusOrderFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new StatusOrderFormViewModel
            {
                Title = this.orderLocalizer.GetString("Order"),
                DeliveryFromLabel = this.orderLocalizer.GetString("DeliveryFrom"),
                DeliveryToLabel = this.orderLocalizer.GetString("DeliveryTo"),
                MoreInfoLabel = this.orderLocalizer.GetString("MoreInfoLabel"),
                NameLabel = this.orderLocalizer.GetString("NameLabel"),
                OrderItemsLabel = this.orderLocalizer.GetString("OrderItemsLabel"),
                QuantityLabel = this.orderLocalizer.GetString("QuantityLabel"),
                ExternalReferenceLabel = this.orderLocalizer.GetString("ExternalReferenceLabel"),
                SkuLabel = this.orderLocalizer.GetString("SkuLabel"),
                OrderStatusLabel = this.orderLocalizer.GetString("OrderStatus"),
                ExpectedDeliveryLabel = this.orderLocalizer.GetString("ExpectedDeliveryLabel"),
                FabricsLabel = this.orderLocalizer.GetString("FabricsLabel"),
                CustomOrderLabel = this.globalLocalizer.GetString("NonstandardOrderLabel"),
                OutletQuantityLabel = this.orderLocalizer.GetString("OutletQuantityLabel"),
                StockQuantityLabel = this.orderLocalizer.GetString("StockQuantityLabel")
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
                viewModel.ExpectedDelivery = order.ExpectedDeliveryDate;
                viewModel.CustomOrder = order.MoreInfo;
                viewModel.OrderItems = order.OrderItems.Select(x => new OrderItemViewModel
                {
                    ProductId = x.ProductId,
                    Sku = x.ProductSku,
                    Name = x.ProductName,
                    ProductUrl = this.linkGenerator.GetPathByAction("Edit", "Product", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, Id = x.ProductId }),
                    Quantity = x.Quantity,
                    StockQuantity = x.StockQuantity,
                    OutletQuantity = x.OutletQuantity,
                    ExternalReference = x.ExternalReference,
                    MoreInfo = x.MoreInfo,
                    Fabrics = x.ProductAttributes.OrEmptyIfNull().Select(x => new ProductAttribute
                    {
                        Key = x.Key,
                        Values = x.Values,
                        Name = x.Name
                    }),
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
