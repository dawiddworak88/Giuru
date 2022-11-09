using Buyer.Web.Areas.Orders.Definitions;
using Buyer.Web.Areas.Orders.DomainModels;
using Buyer.Web.Areas.Orders.Repositories;
using Buyer.Web.Areas.Orders.ViewModel;
using Buyer.Web.Shared.ComponentModels.Files;
using Buyer.Web.Shared.Definitions.Files;
using Buyer.Web.Shared.ViewModels.Files;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.ListItems.ViewModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Orders.ModelBuilders
{
    public class OrderDetailModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, OrderDetailViewModel>
    {
        private readonly IAsyncComponentModelBuilder<FilesComponentModel, FilesViewModel> filesModelBuilder;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<OrderResources> orderLocalizer;
        private readonly LinkGenerator linkGenerator;
        private readonly IOrdersRepository ordersRepository;

        public OrderDetailModelBuilder(
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

        public async Task<OrderDetailViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new OrderDetailViewModel
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
                CancelOrderLabel = this.orderLocalizer.GetString("CancelOrder"),
                GeneralErrorMessage = this.globalLocalizer.GetString("AnErrorOccurred"),
                UpdateOrderStatusUrl = this.linkGenerator.GetPathByAction("Cancel", "OrderStatusApi", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name }),
                CustomOrderLabel = this.globalLocalizer.GetString("CustomOrderLabel"),
                OutletQuantityLabel = this.orderLocalizer.GetString("OutletQuantityLabel"),
                StockQuantityLabel = this.orderLocalizer.GetString("StockQuantityLabel"),
                OrderStatusCommentLabel = this.orderLocalizer.GetString("OrderStatusComment"),
                YesLabel = this.globalLocalizer.GetString("Yes"),
                NoLabel = this.globalLocalizer.GetString("No"),
                CancelationConfirmationDialogLabel = this.orderLocalizer.GetString("CancelationConfirmationDialog"),
                AreYouSureToCancelOrderLabel = this.orderLocalizer.GetString("AreYouSureToCancelOrder"),
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
                    viewModel.ExpectedDelivery = order.ExpectedDeliveryDate;
                    viewModel.CustomOrder = order.MoreInfo;
                    viewModel.EditUrl = this.linkGenerator.GetPathByAction("Edit", "OrderItem", new { Area = "Orders", culture = CultureInfo.CurrentUICulture.Name });
                    viewModel.OrderItems = order.OrderItems.Select(x => new OrderItemViewModel
                    {
                        Id = x.Id,
                        ProductId = x.ProductId,
                        Sku = x.ProductSku,
                        Name = x.ProductName,
                        ProductUrl = this.linkGenerator.GetPathByAction("Index", "Product", new { Area = "Products", culture = CultureInfo.CurrentUICulture.Name, Id = x.ProductId }),
                        Quantity = x.Quantity,
                        StockQuantity = x.StockQuantity,
                        OutletQuantity = x.OutletQuantity,
                        ExternalReference = x.ExternalReference,
                        MoreInfo = x.MoreInfo,
                        OrderItemStatusId = x.OrderItemStatusId,
                        OrderItemStatusName = x.OrderItemStatusName,
                        OrderItemStatusChangeComment = x.OrderItemStatusChangeComment,
                        ProductAttributes = x.ProductAttributes,
                        DeliveryFrom = x.ExpectedDeliveryFrom,
                        DeliveryTo = x.ExpectedDeliveryTo,
                        ImageAlt = x.ProductName,
                        ImageSrc = x.PictureUrl
                    });

                    if (order.OrderStatusId == OrdersConstants.OrderStatuses.NewId)
                    {
                        viewModel.CanCancelOrder = true;
                    }
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
