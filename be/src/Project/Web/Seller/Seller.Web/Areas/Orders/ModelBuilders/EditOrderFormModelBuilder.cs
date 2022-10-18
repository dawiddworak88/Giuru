using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.ListItems.ViewModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.Orders.Definitions;
using Microsoft.Extensions.Options;
using Seller.Web.Areas.Orders.DomainModels;
using Seller.Web.Areas.Orders.Repositories.Orders;
using Seller.Web.Areas.Orders.ViewModel;
using Seller.Web.Shared.ComponentModels.Files;
using Seller.Web.Shared.Definitions;
using Seller.Web.Shared.ViewModels;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Orders.ModelBuilders
{
    public class EditOrderFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, EditOrderFormViewModel>
    {
        private readonly IAsyncComponentModelBuilder<FilesComponentModel, FilesViewModel> filesModelBuilder;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<OrderResources> orderLocalizer;
        private readonly LinkGenerator linkGenerator;
        private readonly IOrdersRepository ordersRepository;

        public EditOrderFormModelBuilder
        (
            IAsyncComponentModelBuilder<FilesComponentModel, FilesViewModel> filesModelBuilder,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<OrderResources> orderLocalizer,
            LinkGenerator linkGenerator,
            IOrdersRepository ordersRepository)
        {
            this.globalLocalizer = globalLocalizer;
            this.orderLocalizer = orderLocalizer;
            this.linkGenerator = linkGenerator;
            this.ordersRepository = ordersRepository;
            this.filesModelBuilder = filesModelBuilder;
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
                ClientLabel = this.globalLocalizer.GetString("Client"),
                CancelOrderLabel = this.orderLocalizer.GetString("CancelOrder"),
                CancelOrderStatusUrl = this.linkGenerator.GetPathByAction("Cancel", "OrderStatusApi", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name }),
                OutletQuantityLabel = this.orderLocalizer.GetString("OutletQuantityLabel"),
                StockQuantityLabel = this.orderLocalizer.GetString("StockQuantityLabel"),
                CustomOrderLabel = this.globalLocalizer.GetString("CustomOrderLabel"),
                OrderStatusCommentLabel = this.orderLocalizer.GetString("OrderStatusComment"),
                UpdateOrderItemStatusUrl = this.linkGenerator.GetPathByAction("Item", "OrderStatusApi", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name }),
                OrdersUrl = this.linkGenerator.GetPathByAction("Index", "Orders", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name }),
                NavigateToOrders = this.orderLocalizer.GetString("NavigateToOrdersList")
            };

            var orderStatuses = await this.ordersRepository.GetOrderStatusesAsync(componentModel.Token, componentModel.Language);

            if (orderStatuses is not null)
            {
                viewModel.OrderStatuses = orderStatuses.Select(x => new ListItemViewModel { Id = x.Id, Name = x.Name });
            }
            
            if (componentModel.Id.HasValue)
            {
                var order = await this.ordersRepository.GetOrderAsync(componentModel.Token, componentModel.Language, componentModel.Id);

                if (order is not null)
                {
                    viewModel.Id = order.Id;
                    viewModel.OrderStatusId = order.OrderStatusId;
                    viewModel.ClientUrl = this.linkGenerator.GetPathByAction("Edit", "Client", new { Area = "Clients", culture = CultureInfo.CurrentUICulture.Name, Id = order.ClientId });
                    viewModel.ClientName = order.ClientName;
                    viewModel.UpdateOrderStatusUrl = this.linkGenerator.GetPathByAction("Index", "OrderStatusApi", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name, Id = order.ClientId });
                    viewModel.EditUrl = this.linkGenerator.GetPathByAction("Edit", "OrderItem", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name });
                    viewModel.OrderItems = order.OrderItems.Select(x => new OrderItemViewModel
                    {
                        Id = x.Id,
                        ProductId = x.ProductId,
                        Sku = x.ProductSku,
                        Name = x.ProductName,
                        ProductUrl = this.linkGenerator.GetPathByAction("Edit", "Product", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, Id = x.ProductId }),
                        Quantity = x.Quantity,
                        StockQuantity = x.StockQuantity,
                        OutletQuantity = x.OutletQuantity,
                        ExternalReference = x.ExternalReference,
                        MoreInfo = x.MoreInfo,
                        OrderItemStatusId = x.OrderItemStatusId,
                        OrderItemStatusName = x.OrderItemStatusName,
                        OrderItemStatusChangeComment = x.OrderItemStatusChangeComment,
                        DeliveryFrom = x.ExpectedDeliveryFrom,
                        DeliveryTo = x.ExpectedDeliveryTo,
                        ImageAlt = x.ProductName,
                        ImageSrc = x.PictureUrl
                    });
                    viewModel.CustomOrder = order.MoreInfo;

                    if (order.OrderStatusId == OrdersConstants.OrderStatuses.NewId)
                    {
                        viewModel.CanCancelOrder = true;
                    }

                    var orderItemsStatuses = new List<OrderItemStatusViewModel>();

                    foreach (var orderItem in order.OrderItems.OrEmptyIfNull())
                    {
                        orderItemsStatuses.Add(new OrderItemStatusViewModel
                        {
                            Id = orderItem.Id,
                            OrderStatusId = orderItem.OrderItemStatusId
                        });
                    }

                    viewModel.OrderItemsStatuses = orderItemsStatuses;
                }

                var orderFiles = await this.ordersRepository.GetOrderFilesAsync(componentModel.Token, componentModel.Language, componentModel.Id, FilesConstants.DefaultPageIndex, FilesConstants.DefaultPageSize, null, $"{nameof(OrderFile.CreatedDate)} desc");

                if (orderFiles is not null)
                {
                    var filesComponentModel = new FilesComponentModel
                    {
                        Id = componentModel.Id,
                        IsAuthenticated = componentModel.IsAuthenticated,
                        Language = componentModel.Language,
                        Token = componentModel.Token,
                        SearchApiUrl = this.linkGenerator.GetPathByAction("GetFiles", "OrderFileApi", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name }),
                        Files = orderFiles.Data.OrEmptyIfNull().Select(x => x.Id)
                    };

                    viewModel.Attachments = await this.filesModelBuilder.BuildModelAsync(filesComponentModel);
                }
            }

            return viewModel;
        }
    }
} 