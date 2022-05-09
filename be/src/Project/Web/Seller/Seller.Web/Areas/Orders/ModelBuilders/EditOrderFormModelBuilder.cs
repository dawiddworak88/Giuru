using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.Extensions.Services.MediaServices;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.ListItems.ViewModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Seller.Web.Areas.Orders.Repositories.Orders;
using Seller.Web.Areas.Orders.ViewModel;
using Seller.Web.Areas.Shared.Repositories.Media;
using Seller.Web.Shared.Configurations;
using Seller.Web.Shared.ViewModels;
using System.Collections.Generic;
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
        private readonly IMediaHelperService mediaService;
        private readonly IMediaItemsRepository mediaRepository;
        private readonly IOptions<AppSettings> options;

        public EditOrderFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<OrderResources> orderLocalizer,
            LinkGenerator linkGenerator,
            IOptions<AppSettings> options,
            IMediaHelperService mediaService,
            IMediaItemsRepository mediaRepository,
            IOrdersRepository ordersRepository)
        {
            this.globalLocalizer = globalLocalizer;
            this.orderLocalizer = orderLocalizer;
            this.linkGenerator = linkGenerator;
            this.ordersRepository = ordersRepository;
            this.mediaService = mediaService;
            this.mediaRepository = mediaRepository;
            this.options = options;
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
                OutletQuantityLabel = this.orderLocalizer.GetString("OutletQuantityLabel"),
                StockQuantityLabel = this.orderLocalizer.GetString("StockQuantityLabel"),
                CustomOrderLabel = this.globalLocalizer.GetString("CustomOrderLabel"),
                AttachmentsLabel = this.globalLocalizer.GetString("Attachments")
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
                    StockQuantity = x.StockQuantity,
                    OutletQuantity = x.OutletQuantity,
                    ExternalReference = x.ExternalReference,
                    MoreInfo = x.MoreInfo,
                    DeliveryFrom = x.ExpectedDeliveryFrom,
                    DeliveryTo = x.ExpectedDeliveryTo,
                    ImageAlt = x.ProductName,
                    ImageSrc = x.PictureUrl
                });
                viewModel.CustomOrder = order.MoreInfo;

                var attachments = new List<FileViewModel>();

                foreach (var attachmentItem in order.Attachments.OrEmptyIfNull())
                {
                    var file = await this.mediaRepository.GetMediaItemAsync(componentModel.Token, componentModel.Language, attachmentItem);

                    if (file is not null)
                    {
                        attachments.Add(new FileViewModel
                        {
                            Id = file.Id,
                            Url = this.mediaService.GetFileUrl(this.options.Value.MediaUrl, attachmentItem),
                            Name = file.Name,
                            MimeType = file.MimeType,
                            Filename = file.Filename
                        });
                    };
                };

                viewModel.Attachments = attachments;
            }

            return viewModel;
        }
    }
}