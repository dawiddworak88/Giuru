using Foundation.EventBus.Abstractions;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Extensions;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Foundation.Mailing.Models;
using Foundation.Mailing.Services;
using Foundation.Media.Services.MediaServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Ordering.Api.Configurations;
using Ordering.Api.Infrastructure;
using Ordering.Api.Infrastructure.Orders.Definitions;
using Ordering.Api.Infrastructure.Orders.Entities;
using Ordering.Api.IntegrationEvents;
using Ordering.Api.ServicesModels;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Api.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly OrderingContext context;
        private readonly IEventBus eventBus;
        private readonly IStringLocalizer<OrderResources> orderLocalizer;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IOptionsMonitor<AppSettings> orderingOptions;
        private readonly IMailingService mailingService;
        private readonly IOptions<AppSettings> configuration;
        private readonly IMediaService mediaService;

        public OrdersService(
            OrderingContext context,
            IEventBus eventBus,
            IStringLocalizer<OrderResources> orderLocalizer,
            IMailingService mailingService,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IOptionsMonitor<AppSettings> orderingOptions,
            IOptions<AppSettings> configuration,
            IMediaService mediaService)
        {
            this.context = context;
            this.eventBus = eventBus;
            this.orderLocalizer = orderLocalizer;
            this.mailingService = mailingService;
            this.configuration = configuration;
            this.globalLocalizer = globalLocalizer;
            this.orderingOptions = orderingOptions;
            this.mediaService = mediaService;
        }

        public async Task CheckoutAsync(CheckoutBasketServiceModel serviceModel)
        {
            var order = new Order
            {
                OrderStateId = OrderStatesConstants.NewId,
                OrderStatusId = OrderStatusesConstants.NewId,
                ClientId = serviceModel.ClientId,
                ClientName = serviceModel.ClientName,
                SellerId = serviceModel.SellerId,
                BillingAddressId = serviceModel.BillingAddressId,
                BillingCity = serviceModel.BillingCity,
                BillingCompany = serviceModel.BillingCompany,
                BillingCountryCode = serviceModel.BillingCountryCode,
                BillingFirstName = serviceModel.BillingFirstName,
                BillingLastName = serviceModel.BillingLastName,
                BillingPhone = serviceModel.BillingPhone,
                BillingPhonePrefix = serviceModel.BillingPhonePrefix,
                BillingPostCode = serviceModel.BillingPostCode,
                BillingRegion = serviceModel.BillingRegion,
                BillingStreet = serviceModel.BillingStreet,
                ShippingAddressId = serviceModel.ShippingAddressId,
                ShippingCity = serviceModel.ShippingCity,
                ShippingCompany = serviceModel.ShippingCompany,
                ShippingCountryCode = serviceModel.ShippingCountryCode,
                ShippingFirstName = serviceModel.ShippingFirstName,
                ShippingLastName = serviceModel.ShippingLastName,
                ShippingPhone = serviceModel.ShippingPhone,
                ShippingPhonePrefix = serviceModel.ShippingPhonePrefix,
                ShippingPostCode = serviceModel.ShippingPostCode,
                ShippingRegion = serviceModel.ShippingRegion,
                ShippingStreet = serviceModel.ShippingStreet,
                ExternalReference = serviceModel.ExternalReference,
                ExpectedDeliveryDate = serviceModel.ExpectedDeliveryDate,
                MoreInfo = serviceModel.MoreInfo,
                IpAddress = serviceModel.IpAddress
            };

            this.context.Orders.Add(order.FillCommonProperties());

            foreach (var basketItem in serviceModel.Items.OrEmptyIfNull())
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    OrderStateId = OrderStatesConstants.NewId,
                    OrderStatusId = OrderStatusesConstants.NewId,
                    ProductId = basketItem.ProductId.Value,
                    ProductSku = basketItem.ProductSku,
                    ProductName = basketItem.ProductName,
                    PictureUrl = basketItem.PictureUrl,
                    Quantity = basketItem.Quantity,
                    StockQuantity = basketItem.StockQuantity,
                    OutletQuantity = basketItem.OutletQuantity,
                    ExternalReference = basketItem.ExternalReference,
                    ExpectedDeliveryFrom = basketItem.ExpectedDeliveryFrom,
                    ExpectedDeliveryTo = basketItem.ExpectedDeliveryTo,
                    MoreInfo = basketItem.MoreInfo
                };
             
                this.context.OrderItems.Add(orderItem.FillCommonProperties());
            };

            if (serviceModel.HasCustomOrder)
            {
                var attachments = new List<string>();

                foreach (var attachmentId in serviceModel.Attachments.OrEmptyIfNull())
                {
                    var newAttachment = new OrderAttachment
                    {
                        OrderId = order.Id,
                        MediaId = attachmentId
                    };

                    attachments.Add(this.mediaService.GetMediaUrl(attachmentId));

                    await this.context.OrderAttachments.AddAsync(newAttachment.FillCommonProperties());
                }

                Thread.CurrentThread.CurrentCulture = new CultureInfo(serviceModel.Language);
                Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

                await this.mailingService.SendTemplateAsync(new TemplateEmail
                {
                    RecipientEmailAddress = this.configuration.Value.SenderEmail,
                    RecipientName = this.configuration.Value.SenderName,
                    SenderEmailAddress = this.configuration.Value.SenderEmail,
                    SenderName = this.configuration.Value.SenderName,
                    TemplateId = this.orderingOptions.CurrentValue.ActionSendGridCustomOrderTemplateId,
                    DynamicTemplateData = new
                    {
                        attachmentsLabel = this.globalLocalizer.GetString("AttachedAttachments").Value,
                        attachments = attachments,
                        subject = this.orderLocalizer.GetString("CustomOrderSubject").Value + " " + serviceModel.ClientName + " (" + order.Id + ")",
                        text = serviceModel.MoreInfo
                    }
                });
            }

            await this.context.SaveChangesAsync();

            var message = new OrderStartedIntegrationEvent
            { 
                BasketId =  serviceModel.BasketId
            };

            this.eventBus.Publish(message);
        }

        public async Task<PagedResults<IEnumerable<OrderServiceModel>>> GetAsync(GetOrdersServiceModel model)
        {
            var orders = from c in this.context.Orders
                         join os in this.context.OrderStatuses on c.OrderStatusId equals os.Id
                         join ost in this.context.OrderStatusTranslations on os.Id equals ost.OrderStatusId
                         where ost.Language == model.Language && c.IsActive
                         select new OrderServiceModel
                         {
                             Id = c.Id,
                             SellerId = c.SellerId,
                             ClientId = c.ClientId.Value,
                             ClientName = c.ClientName,
                             BillingAddressId = c.BillingAddressId,
                             BillingCity = c.BillingCity,
                             BillingCompany = c.BillingCompany,
                             BillingCountryCode = c.BillingCountryCode,
                             BillingFirstName = c.BillingFirstName,
                             BillingLastName = c.BillingLastName,
                             BillingPhone = c.BillingPhone,
                             BillingPhonePrefix = c.BillingPhonePrefix,
                             BillingPostCode = c.BillingPostCode,
                             BillingRegion = c.BillingRegion,
                             BillingStreet = c.BillingStreet,
                             ShippingAddressId = c.ShippingAddressId,
                             ShippingCity = c.ShippingCity,
                             ShippingCompany = c.ShippingCompany,
                             ShippingCountryCode = c.ShippingCountryCode,
                             ShippingFirstName = c.ShippingFirstName,
                             ShippingLastName = c.ShippingLastName,
                             ShippingPhone = c.ShippingPhone,
                             ShippingPhonePrefix = c.ShippingPhonePrefix,
                             ShippingPostCode = c.ShippingPostCode,
                             ShippingRegion = c.ShippingRegion,
                             ShippingStreet = c.ShippingStreet,
                             ExpectedDeliveryDate = c.ExpectedDeliveryDate,
                             ExternalReference = c.ExternalReference,
                             MoreInfo = c.MoreInfo,
                             Reason = c.Reason,
                             OrderStateId = c.OrderStateId,
                             OrderStatusId = c.OrderStatusId,
                             OrderStatusName = ost.Name,
                             OrderItems = c.OrderItems.Select(x => new OrderItemServiceModel
                             {
                                 ProductId = x.ProductId,
                                 ProductSku = x.ProductSku,
                                 ProductName = x.ProductName,
                                 PictureUrl = x.PictureUrl,
                                 Quantity = x.Quantity,
                                 StockQuantity = x.StockQuantity,
                                 OutletQuantity = x.OutletQuantity,
                                 ExternalReference = x.ExternalReference,
                                 ExpectedDeliveryFrom = x.ExpectedDeliveryFrom,
                                 ExpectedDeliveryTo = x.ExpectedDeliveryTo,
                                 MoreInfo = x.MoreInfo,
                                 LastModifiedDate = x.LastModifiedDate,
                                 CreatedDate = x.CreatedDate
                             }),
                             LastModifiedDate = c.LastModifiedDate,
                             CreatedDate = c.CreatedDate
                         };

            if (model.IsSeller is false)
            {
                orders = orders.Where(x => x.SellerId == model.OrganisationId.Value);
            }

            if (string.IsNullOrWhiteSpace(model.SearchTerm) is false)
            {
                orders = orders.Where(x => x.ClientName.ToLower().StartsWith(model.SearchTerm.ToLower()) 
                || x.OrderItems.Any(y => y.ExternalReference.ToLower().StartsWith(model.SearchTerm.ToLower())) 
                || x.Id.ToString().ToLower() == model.SearchTerm.ToLower());
            }

            if (model.CreatedDateGreaterThan.HasValue)
            {
                orders = orders.Where(x => x.CreatedDate >= model.CreatedDateGreaterThan);
            }

            orders = orders.ApplySort(model.OrderBy);

            return orders.PagedIndex(new Pagination(orders.Count(), model.ItemsPerPage), model.PageIndex);
        }

        public async Task<OrderServiceModel> GetAsync(GetOrderServiceModel model)
        {
            var orderItem = await this.context.Orders.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);
            if (orderItem is not null)
            {
                var order = new OrderServiceModel
                {
                    Id = orderItem.Id,
                    SellerId = orderItem.SellerId,
                    ClientId = orderItem.ClientId.Value,
                    ClientName = orderItem.ClientName,
                    BillingAddressId = orderItem.BillingAddressId,
                    BillingCity = orderItem.BillingCity,
                    BillingCompany = orderItem.BillingCompany,
                    BillingCountryCode = orderItem.BillingCountryCode,
                    BillingFirstName = orderItem.BillingFirstName,
                    BillingLastName = orderItem.BillingLastName,
                    BillingPhone = orderItem.BillingPhone,
                    BillingPhonePrefix = orderItem.BillingPhonePrefix,
                    BillingPostCode = orderItem.BillingPostCode,
                    BillingRegion = orderItem.BillingRegion,
                    BillingStreet = orderItem.BillingStreet,
                    ShippingAddressId = orderItem.ShippingAddressId,
                    ShippingCity = orderItem.ShippingCity,
                    ShippingCompany = orderItem.ShippingCompany,
                    ShippingCountryCode = orderItem.ShippingCountryCode,
                    ShippingFirstName = orderItem.ShippingFirstName,
                    ShippingLastName = orderItem.ShippingLastName,
                    ShippingPhone = orderItem.ShippingPhone,
                    ShippingPhonePrefix = orderItem.ShippingPhonePrefix,
                    ShippingPostCode = orderItem.ShippingPostCode,
                    ShippingRegion = orderItem.ShippingRegion,
                    ShippingStreet = orderItem.ShippingStreet,
                    ExternalReference = orderItem.ExternalReference,
                    ExpectedDeliveryDate = orderItem.ExpectedDeliveryDate,
                    MoreInfo = orderItem.MoreInfo,
                    Reason = orderItem.Reason,
                    OrderStateId = orderItem.OrderStateId,
                    OrderStatusId = orderItem.OrderStatusId,
                    LastModifiedDate = orderItem.LastModifiedDate,
                    CreatedDate = orderItem.CreatedDate
                };

                var orderStatusTranslation = await this.context.OrderStatusTranslations.FirstOrDefaultAsync(x => x.OrderStatusId == orderItem.OrderStatusId && x.IsActive && x.Language == model.Language);
                
                if (orderStatusTranslation is null)
                {
                    orderStatusTranslation = await this.context.OrderStatusTranslations.FirstOrDefaultAsync(x => x.OrderStatusId == orderItem.OrderStatusId && x.IsActive);
                }

                order.OrderStatusName = orderStatusTranslation?.Name;

                var attachments = this.context.OrderAttachments.Where(x => x.OrderId == orderItem.Id && x.IsActive);
                
                if (attachments is not null)
                {
                    order.Attachments = attachments.Select(x => x.MediaId);
                }

                var orderItems = this.context.OrderItems.Where(x => x.OrderId == orderItem.Id && x.IsActive);

                if (orderItems is not null)
                {
                    order.OrderItems = orderItems.Select(x => new OrderItemServiceModel
                    {
                        ProductId = x.ProductId,
                        ProductSku = x.ProductSku,
                        ProductName = x.ProductName,
                        PictureUrl = x.PictureUrl,
                        Quantity = x.Quantity,
                        StockQuantity = x.StockQuantity,
                        OutletQuantity = x.OutletQuantity,
                        ExternalReference = x.ExternalReference,
                        ExpectedDeliveryFrom = x.ExpectedDeliveryFrom,
                        ExpectedDeliveryTo = x.ExpectedDeliveryTo,
                        MoreInfo = x.MoreInfo,
                        LastModifiedDate = x.LastModifiedDate,
                        CreatedDate = x.CreatedDate
                    });
                }

                return order;
            }

            return default;
        }

        public async Task<IEnumerable<OrderStatusServiceModel>> GetOrderStatusesAsync(GetOrderStatusesServiceModel serviceModel)
        {
            var orderStatuses = from orderstatus in this.context.OrderStatuses
                                join orderstate in this.context.OrderStates on orderstatus.OrderStateId equals orderstate.Id
                                join orderstatustranslation in this.context.OrderStatusTranslations on orderstatus.Id equals orderstatustranslation.OrderStatusId
                                where orderstatustranslation.Language == serviceModel.Language && orderstatus.IsActive
                                orderby orderstatus.Order
                                select new OrderStatusServiceModel
                                {
                                    Id = orderstatus.Id,
                                    OrderStateId = orderstate.Id,
                                    Name = orderstatustranslation.Name,
                                    LastModifiedDate = orderstatus.LastModifiedDate,
                                    CreatedDate = orderstatus.CreatedDate
                                };

            return orderStatuses;
        }

        public async Task<OrderServiceModel> SaveOrderStatusAsync(UpdateOrderStatusServiceModel serviceModel)
        {
            var orders = this.context.Orders.Where(x => x.Id == serviceModel.OrderId && x.IsActive);

            if (serviceModel.IsSeller is false)
            {
                orders = orders.Where(x => x.SellerId == serviceModel.OrganisationId.Value);
            }

            var order = await orders.FirstOrDefaultAsync();

            if (order == null)
            {
                throw new CustomException(this.orderLocalizer.GetString("OrderNotFound"), (int)HttpStatusCode.NotFound);
            }

            var newOrderOstatus = await this.context.OrderStatuses.FirstOrDefaultAsync(x => x.Id == serviceModel.OrderStatusId && x.IsActive);

            if (newOrderOstatus == null)
            {
                throw new CustomException(this.orderLocalizer.GetString("OrderStatusNotFound"), (int)HttpStatusCode.NotFound);
            }

            order.OrderStatusId = newOrderOstatus.Id;
            order.OrderStateId = newOrderOstatus.OrderStateId;

            await this.context.SaveChangesAsync();

            return await this.GetAsync(new GetOrderServiceModel {  
                Id = serviceModel.OrderId,
                OrganisationId = serviceModel.OrganisationId,
                Username = serviceModel.Username,
                Language = serviceModel.Language,
                IsSeller = serviceModel.IsSeller
            });
        }
    }
}
