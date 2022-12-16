using Foundation.EventBus.Abstractions;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Definitions;
using Foundation.GenericRepository.Extensions;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Foundation.Mailing.Models;
using Foundation.Mailing.Services;
using Foundation.Media.Services.MediaServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nest;
using Newtonsoft.Json;
using Ordering.Api.Configurations;
using Ordering.Api.Definitions;
using Ordering.Api.Infrastructure;
using Ordering.Api.Infrastructure.Orders.Entities;
using Ordering.Api.IntegrationEvents;
using Ordering.Api.ServicesModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private readonly ILogger logger;

        public OrdersService(
            OrderingContext context,
            IEventBus eventBus,
            IStringLocalizer<OrderResources> orderLocalizer,
            IMailingService mailingService,
            IStringLocalizer<GlobalResources> globalLocalizer,
            IOptionsMonitor<AppSettings> orderingOptions,
            IOptions<AppSettings> configuration,
            IMediaService mediaService,
            ILogger<OrdersService> logger)
        {
            this.context = context;
            this.eventBus = eventBus;
            this.orderLocalizer = orderLocalizer;
            this.mailingService = mailingService;
            this.configuration = configuration;
            this.globalLocalizer = globalLocalizer;
            this.orderingOptions = orderingOptions;
            this.mediaService = mediaService;
            this.logger = logger;
        }

        public async Task CheckoutAsync(CheckoutBasketServiceModel serviceModel)
        {
            using var source = new ActivitySource(this.GetType().Name);

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

                var orderItemStatusChange = new OrderItemStatusChange
                {
                    OrderItemId = orderItem.Id,
                    OrderItemStateId = OrderItemStatesConstants.NewId,
                    OrderItemStatusId = OrderItemStatusConstants.NewId,
                };

                this.context.OrderItemStatusChanges.Add(orderItemStatusChange.FillCommonProperties());

                orderItem.LastOrderItemStatusChangeId = orderItemStatusChange.Id == Guid.Empty ? null : orderItemStatusChange.Id;
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

            using var activity = source.StartActivity($"{System.Reflection.MethodBase.GetCurrentMethod().Name} {message.GetType().Name}");
            this.eventBus.Publish(message);
        }

        public async Task<PagedResults<IEnumerable<OrderServiceModel>>> GetAsync(GetOrdersServiceModel model)
        {
            var orders = this.context.Orders.Where(x => x.IsActive);

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

            PagedResults<IEnumerable<Order>> pagedResults;

            if (model.PageIndex.HasValue is false || model.ItemsPerPage.HasValue is false)
            {
                orders = orders.Take(Constants.MaxItemsPerPageLimit);

                pagedResults = orders.PagedIndex(new Pagination(orders.Count(), Constants.MaxItemsPerPageLimit), Constants.DefaultPageIndex);
            }
            else
            {
                pagedResults = orders.PagedIndex(new Pagination(orders.Count(), model.ItemsPerPage.Value), model.PageIndex.Value);
            }

            var pagedOrdersServiceModel = new PagedResults<IEnumerable<OrderServiceModel>>(pagedResults.Total, pagedResults.PageSize);

            var ordersList = new List<OrderServiceModel>();

            foreach (var item in pagedResults.Data.OrEmptyIfNull().ToList())
            {
                var order = new OrderServiceModel
                {
                    Id = item.Id,
                    SellerId = item.SellerId,
                    ClientId = item.ClientId.Value,
                    ClientName = item.ClientName,
                    BillingAddressId = item.BillingAddressId,
                    BillingCity = item.BillingCity,
                    BillingCompany = item.BillingCompany,
                    BillingCountryCode = item.BillingCountryCode,
                    BillingFirstName = item.BillingFirstName,
                    BillingLastName = item.BillingLastName,
                    BillingPhone = item.BillingPhone,
                    BillingPhonePrefix = item.BillingPhonePrefix,
                    BillingPostCode = item.BillingPostCode,
                    BillingRegion = item.BillingRegion,
                    BillingStreet = item.BillingStreet,
                    ShippingAddressId = item.ShippingAddressId,
                    ShippingCity = item.ShippingCity,
                    ShippingCompany = item.ShippingCompany,
                    ShippingCountryCode = item.ShippingCountryCode,
                    ShippingFirstName = item.ShippingFirstName,
                    ShippingLastName = item.ShippingLastName,
                    ShippingPhone = item.ShippingPhone,
                    ShippingPhonePrefix = item.ShippingPhonePrefix,
                    ShippingPostCode = item.ShippingPostCode,
                    ShippingRegion = item.ShippingRegion,
                    ShippingStreet = item.ShippingStreet,
                    ExpectedDeliveryDate = item.ExpectedDeliveryDate,
                    ExternalReference = item.ExternalReference,
                    MoreInfo = item.MoreInfo,
                    Reason = item.Reason,
                    OrderStateId = item.OrderStateId,
                    OrderStatusId = item.OrderStatusId,
                    LastModifiedDate = item.LastModifiedDate,
                    CreatedDate = item.CreatedDate
                };

                var orderStatusTranslation = this.context.OrderStatusTranslations.FirstOrDefault(x => x.OrderStatusId == item.OrderStatusId && x.IsActive && x.Language == model.Language);

                if (orderStatusTranslation is null)
                {
                    orderStatusTranslation = this.context.OrderStatusTranslations.FirstOrDefault(x => x.OrderStatusId == item.OrderStatusId && x.IsActive);
                }

                order.OrderStatusName = orderStatusTranslation?.Name;

                var orderedItems = this.context.OrderItems.Where(x => x.OrderId == item.Id && x.IsActive);

                var orderItems = new List<OrderItemServiceModel>();

                foreach (var orderedItem in orderedItems.OrEmptyIfNull().ToList())
                {
                    var orderItem = new OrderItemServiceModel
                    {
                        ProductId = orderedItem.ProductId,
                        ProductSku = orderedItem.ProductSku,
                        ProductName = orderedItem.ProductName,
                        PictureUrl = orderedItem.PictureUrl,
                        Quantity = orderedItem.Quantity,
                        StockQuantity = orderedItem.StockQuantity,
                        OutletQuantity = orderedItem.OutletQuantity,
                        ExternalReference = orderedItem.ExternalReference,
                        ExpectedDeliveryFrom = orderedItem.ExpectedDeliveryFrom,
                        ExpectedDeliveryTo = orderedItem.ExpectedDeliveryTo,
                        MoreInfo = orderedItem.MoreInfo,
                        LastModifiedDate = orderedItem.LastModifiedDate,
                        CreatedDate = orderedItem.CreatedDate
                    };

                    var lastOrderItemStatusChange = this.context.OrderItemStatusChanges.FirstOrDefault(x => x.Id == orderedItem.LastOrderItemStatusChangeId && x.IsActive);

                    if (lastOrderItemStatusChange is not null)
                    {
                        orderItem.OrderItemStateId = lastOrderItemStatusChange.OrderItemStateId;
                        orderItem.OrderItemStatusId = lastOrderItemStatusChange.OrderItemStatusId;
                        
                        var orderItemStatusTranslation = this.context.OrderStatusTranslations.FirstOrDefault(x => x.OrderStatusId == lastOrderItemStatusChange.OrderItemStatusId && x.IsActive && x.Language == model.Language);

                        if (orderItemStatusTranslation is null)
                        {
                            orderItemStatusTranslation = this.context.OrderStatusTranslations.FirstOrDefault(x => x.OrderStatusId == lastOrderItemStatusChange.OrderItemStatusId && x.IsActive);
                        }

                        orderItem.OrderItemStatusName = orderItemStatusTranslation?.Name;
                    }

                    orderItems.Add(orderItem);
                }

                order.OrderItems = orderItems;

                ordersList.Add(order);
            }

            pagedOrdersServiceModel.Data = ordersList;

            return pagedOrdersServiceModel;
        }

        public async Task<OrderServiceModel> GetAsync(GetOrderServiceModel model)
        {
            var existingOrder = await this.context.Orders.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (existingOrder is null)
            {
                throw new CustomException(this.orderLocalizer.GetString("OrderNotFound"), (int)HttpStatusCode.NotFound);
            }

            var order = new OrderServiceModel
            {
                Id = existingOrder.Id,
                SellerId = existingOrder.Id,
                ClientId = existingOrder.ClientId.Value,
                ClientName = existingOrder.ClientName,
                BillingAddressId = existingOrder.BillingAddressId,
                BillingCity = existingOrder.BillingCity,
                BillingCompany = existingOrder.BillingCompany,
                BillingCountryCode = existingOrder.BillingCountryCode,
                BillingFirstName = existingOrder.BillingFirstName,
                BillingLastName = existingOrder.BillingLastName,
                BillingPhone = existingOrder.BillingPhone,
                BillingPhonePrefix = existingOrder.BillingPhonePrefix,
                BillingPostCode = existingOrder.BillingPostCode,
                BillingRegion = existingOrder.BillingRegion,
                BillingStreet = existingOrder.BillingStreet,
                ShippingAddressId = existingOrder.ShippingAddressId,
                ShippingCity = existingOrder.ShippingCity,
                ShippingCompany = existingOrder.ShippingCompany,
                ShippingCountryCode = existingOrder.ShippingCountryCode,
                ShippingFirstName = existingOrder.ShippingFirstName,
                ShippingLastName = existingOrder.ShippingLastName,
                ShippingPhone = existingOrder.ShippingPhone,
                ShippingPhonePrefix = existingOrder.ShippingPhonePrefix,
                ShippingPostCode = existingOrder.ShippingPostCode,
                ShippingRegion = existingOrder.ShippingRegion,
                ShippingStreet = existingOrder.ShippingStreet,
                ExternalReference = existingOrder.ExternalReference,
                ExpectedDeliveryDate = existingOrder.ExpectedDeliveryDate,
                MoreInfo = existingOrder.MoreInfo,
                Reason = existingOrder.Reason,
                OrderStateId = existingOrder.OrderStateId,
                OrderStatusId = existingOrder.OrderStatusId,
                LastModifiedDate = existingOrder.LastModifiedDate,
                CreatedDate = existingOrder.CreatedDate
            };

            var orderStatusTranslation = this.context.OrderStatusTranslations.FirstOrDefault(x => x.OrderStatusId == existingOrder.OrderStatusId && x.IsActive && x.Language == model.Language);

            if (orderStatusTranslation is null)
            {
                orderStatusTranslation = this.context.OrderStatusTranslations.FirstOrDefault(x => x.OrderStatusId == existingOrder.OrderStatusId && x.IsActive);
            }

            order.OrderStatusName = orderStatusTranslation?.Name;

            var orderedItems = this.context.OrderItems.Where(x => x.OrderId == existingOrder.Id && x.IsActive);

            var orderItems = new List<OrderItemServiceModel>();

            foreach (var orderedItem in orderedItems.OrEmptyIfNull().ToList())
            {
                var orderItem = new OrderItemServiceModel
                {
                    Id = orderedItem.Id,
                    OrderId = orderedItem.OrderId,
                    ProductId = orderedItem.ProductId,
                    ProductSku = orderedItem.ProductSku,
                    ProductName = orderedItem.ProductName,
                    PictureUrl = orderedItem.PictureUrl,
                    Quantity = orderedItem.Quantity,
                    StockQuantity = orderedItem.StockQuantity,
                    OutletQuantity = orderedItem.OutletQuantity,
                    ExternalReference = orderedItem.ExternalReference,
                    ExpectedDeliveryFrom = orderedItem.ExpectedDeliveryFrom,
                    ExpectedDeliveryTo = orderedItem.ExpectedDeliveryTo,
                    MoreInfo = orderedItem.MoreInfo,
                    LastModifiedDate = orderedItem.LastModifiedDate,
                    CreatedDate = orderedItem.CreatedDate
                };

                var lastOrderStatusChange = this.context.OrderItemStatusChanges.FirstOrDefault(x => x.Id == orderedItem.LastOrderItemStatusChangeId && x.IsActive);

                if (lastOrderStatusChange is not null)
                {
                    orderItem.OrderItemStateId = lastOrderStatusChange.OrderItemStateId;
                    orderItem.OrderItemStatusId = lastOrderStatusChange.OrderItemStatusId;
                    orderItem.LastOrderItemStatusChangeId = lastOrderStatusChange.Id;

                    var orderItemStatusTranslation = this.context.OrderStatusTranslations.FirstOrDefault(x => x.OrderStatusId == lastOrderStatusChange.OrderItemStatusId && x.IsActive && x.Language == model.Language);

                    if (orderItemStatusTranslation is null)
                    {
                        orderItemStatusTranslation = this.context.OrderStatusTranslations.FirstOrDefault(x => x.OrderStatusId == lastOrderStatusChange.OrderItemStatusId && x.IsActive);
                    }

                    orderItem.OrderItemStatusName = orderItemStatusTranslation?.Name;

                    var orderItemStatusCommentTranslation = this.context.OrderItemStatusChangesCommentTranslations.FirstOrDefault(x => x.OrderItemStatusChangeId == lastOrderStatusChange.Id && x.Language == model.Language && x.IsActive);

                    if (orderItemStatusCommentTranslation is null)
                    {
                        orderItemStatusCommentTranslation = this.context.OrderItemStatusChangesCommentTranslations.FirstOrDefault(x => x.OrderItemStatusChangeId == lastOrderStatusChange.Id && x.IsActive);
                    }

                    orderItem.OrderItemStatusChangeComment = orderItemStatusCommentTranslation?.OrderItemStatusChangeComment;
                }

                orderItems.Add(orderItem);
            }

            order.OrderItems = orderItems;

            return order;
        }

        public async Task<OrderItemServiceModel> GetAsync(GetOrderItemServiceModel model)
        {
            var existingOrderItem = await this.context.OrderItems.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (existingOrderItem is null)
            {
                throw new CustomException(this.orderLocalizer.GetString("OrderItemNotFound"), (int)HttpStatusCode.NotFound);
            }

            var orderItem = new OrderItemServiceModel
            {
                Id = existingOrderItem.Id,
                OrderId = existingOrderItem.OrderId,
                ProductId = existingOrderItem.ProductId,
                ProductName = existingOrderItem.ProductName,
                ProductSku = existingOrderItem.ProductSku,
                PictureUrl = existingOrderItem.PictureUrl,
                Quantity = existingOrderItem.Quantity,
                StockQuantity = existingOrderItem.StockQuantity,
                OutletQuantity = existingOrderItem.OutletQuantity,
                ExternalReference = existingOrderItem.ExternalReference,
                ExpectedDeliveryFrom = existingOrderItem.ExpectedDeliveryFrom,
                ExpectedDeliveryTo = existingOrderItem.ExpectedDeliveryTo,
                LastOrderItemStatusChangeId = existingOrderItem.LastOrderItemStatusChangeId,
                MoreInfo = existingOrderItem.MoreInfo,
                LastModifiedDate = existingOrderItem.LastModifiedDate,
                CreatedDate = existingOrderItem.CreatedDate
            };

            if (existingOrderItem.LastOrderItemStatusChangeId is not null && existingOrderItem.LastOrderItemStatusChangeId != Guid.Empty)
            {
                var lastOrderItemStatus = await this.context.OrderItemStatusChanges.FirstOrDefaultAsync(x => x.Id == existingOrderItem.LastOrderItemStatusChangeId && x.IsActive);

                if (lastOrderItemStatus is null)
                {
                    throw new CustomException(this.orderLocalizer.GetString("LastOrderItemStatusNotFound"), (int)HttpStatusCode.NotFound);
                }

                orderItem.OrderItemStateId = lastOrderItemStatus.OrderItemStateId;
                orderItem.OrderItemStatusId = lastOrderItemStatus.OrderItemStatusId;

                var orderItemStatusTranslation = await this.context.OrderStatusTranslations.FirstOrDefaultAsync(x => x.OrderStatusId == lastOrderItemStatus.OrderItemStatusId && x.Language == model.Language && x.IsActive);

                if (orderItemStatusTranslation is null)
                {
                    orderItemStatusTranslation = await this.context.OrderStatusTranslations.FirstOrDefaultAsync(x => x.OrderStatusId == lastOrderItemStatus.OrderItemStatusId && x.IsActive);
                }

                var orderItemStatusChangeCommentTranslation = this.context.OrderItemStatusChangesCommentTranslations.FirstOrDefault(x => x.OrderItemStatusChangeId == lastOrderItemStatus.Id && x.Language == model.Language && x.IsActive);

                if (orderItemStatusChangeCommentTranslation is null)
                {
                    orderItemStatusChangeCommentTranslation = this.context.OrderItemStatusChangesCommentTranslations.FirstOrDefault(x => x.OrderItemStatusChangeId == lastOrderItemStatus.Id && x.IsActive);
                }

                orderItem.OrderItemStatusChangeComment = orderItemStatusChangeCommentTranslation?.OrderItemStatusChangeComment;

                orderItem.OrderItemStatusName = orderItemStatusTranslation?.Name;
            }

            return orderItem;
        }

        public async Task<OrderItemStatusChangesServiceModel> GetAsync(GetOrderItemStatusChangesServiceModel model)
        {
            var orderItem = this.context.OrderItems.FirstOrDefault(x => x.Id == model.Id && x.IsActive);

            if (orderItem is null)
            {
                throw new CustomException(this.orderLocalizer.GetString("OrderItemNotFound"), (int)HttpStatusCode.NotFound);
            }

            var orderItemStatusesHistory = new OrderItemStatusChangesServiceModel
            {
                OrderItemId = orderItem.Id,
            };

            var orderItemStatuses = this.context.OrderItemStatusChanges.Where(x => x.OrderItemId == model.Id && x.IsActive).OrderByDescending(x => x.CreatedDate);

            var orderItemStatusChanges = new List<OrderItemStatusChangeServiceModel>();

            foreach (var orderItemStatus in orderItemStatuses.OrEmptyIfNull().ToList())
            {
                var orderItemStatusChange = new OrderItemStatusChangeServiceModel
                {
                    OrderItemStateId = orderItemStatus.OrderItemStateId,
                    OrderItemStatusId = orderItemStatus.OrderItemStatusId,
                    CreatedDate = orderItemStatus.CreatedDate
                };

                var orderItemStatusTranslation = this.context.OrderStatusTranslations.FirstOrDefault(x => x.OrderStatusId == orderItemStatus.OrderItemStatusId && x.Language == model.Language && x.IsActive);

                if (orderItemStatusTranslation is null)
                {
                    orderItemStatusTranslation = this.context.OrderStatusTranslations.FirstOrDefault(x => x.OrderStatusId == orderItemStatus.OrderItemStatusId && x.IsActive);
                }

                orderItemStatusChange.OrderItemStatusName = orderItemStatusTranslation?.Name;

                var orderItemStatusChangeCommentTranslation = this.context.OrderItemStatusChangesCommentTranslations.FirstOrDefault(x => x.OrderItemStatusChangeId == orderItemStatus.Id && x.Language == model.Language && x.IsActive);

                if (orderItemStatusChangeCommentTranslation is null)
                {
                    orderItemStatusChangeCommentTranslation = this.context.OrderItemStatusChangesCommentTranslations.FirstOrDefault(x => x.OrderItemStatusChangeId == orderItemStatus.Id && x.IsActive);
                }

                orderItemStatusChange.OrderItemStatusChangeComment = orderItemStatusChangeCommentTranslation?.OrderItemStatusChangeComment;

                orderItemStatusChanges.Add(orderItemStatusChange);
            }

            orderItemStatusesHistory.OrderItemStatusChanges = orderItemStatusChanges;

            return orderItemStatusesHistory;
        }

        public async Task<PagedResults<IEnumerable<OrderServiceModel>>> GetAsync(GetOrdersByIdsServiceModel model)
        {
            var orders = this.context.Orders.Where(x => x.IsActive && model.Ids.Contains(x.Id));

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

            orders = orders.ApplySort(model.OrderBy);

            PagedResults<IEnumerable<Order>> pagedResults;

            if (model.PageIndex.HasValue is false || model.ItemsPerPage.HasValue is false)
            {
                orders = orders.Take(Constants.MaxItemsPerPageLimit);

                pagedResults = orders.PagedIndex(new Pagination(orders.Count(), Constants.MaxItemsPerPageLimit), Constants.DefaultPageIndex);
            }
            else
            {
                pagedResults = orders.PagedIndex(new Pagination(orders.Count(), model.ItemsPerPage.Value), model.PageIndex.Value);
            }

            var pagedOrdersServiceModel = new PagedResults<IEnumerable<OrderServiceModel>>(pagedResults.Total, pagedResults.PageSize);

            var ordersList = new List<OrderServiceModel>();

            foreach (var item in pagedResults.Data.OrEmptyIfNull().ToList())
            {
                var order = new OrderServiceModel
                {
                    Id = item.Id,
                    SellerId = item.SellerId,
                    ClientId = item.ClientId.Value,
                    ClientName = item.ClientName,
                    BillingAddressId = item.BillingAddressId,
                    BillingCity = item.BillingCity,
                    BillingCompany = item.BillingCompany,
                    BillingCountryCode = item.BillingCountryCode,
                    BillingFirstName = item.BillingFirstName,
                    BillingLastName = item.BillingLastName,
                    BillingPhone = item.BillingPhone,
                    BillingPhonePrefix = item.BillingPhonePrefix,
                    BillingPostCode = item.BillingPostCode,
                    BillingRegion = item.BillingRegion,
                    BillingStreet = item.BillingStreet,
                    ShippingAddressId = item.ShippingAddressId,
                    ShippingCity = item.ShippingCity,
                    ShippingCompany = item.ShippingCompany,
                    ShippingCountryCode = item.ShippingCountryCode,
                    ShippingFirstName = item.ShippingFirstName,
                    ShippingLastName = item.ShippingLastName,
                    ShippingPhone = item.ShippingPhone,
                    ShippingPhonePrefix = item.ShippingPhonePrefix,
                    ShippingPostCode = item.ShippingPostCode,
                    ShippingRegion = item.ShippingRegion,
                    ShippingStreet = item.ShippingStreet,
                    ExpectedDeliveryDate = item.ExpectedDeliveryDate,
                    ExternalReference = item.ExternalReference,
                    MoreInfo = item.MoreInfo,
                    Reason = item.Reason,
                    OrderStateId = item.OrderStateId,
                    OrderStatusId = item.OrderStatusId,
                    LastModifiedDate = item.LastModifiedDate,
                    CreatedDate = item.CreatedDate
                };

                var orderStatusTranslation = this.context.OrderStatusTranslations.FirstOrDefault(x => x.OrderStatusId == item.OrderStatusId && x.IsActive && x.Language == model.Language);

                if (orderStatusTranslation is null)
                {
                    orderStatusTranslation = this.context.OrderStatusTranslations.FirstOrDefault(x => x.OrderStatusId == item.OrderStatusId && x.IsActive);
                }

                order.OrderStatusName = orderStatusTranslation?.Name;

                var orderedItems = this.context.OrderItems.Where(x => x.OrderId == item.Id && x.IsActive);

                var orderItems = new List<OrderItemServiceModel>();

                foreach (var orderedItem in orderedItems.OrEmptyIfNull().ToList())
                {
                    var orderItem = new OrderItemServiceModel
                    {
                        ProductId = orderedItem.ProductId,
                        ProductSku = orderedItem.ProductSku,
                        ProductName = orderedItem.ProductName,
                        PictureUrl = orderedItem.PictureUrl,
                        Quantity = orderedItem.Quantity,
                        StockQuantity = orderedItem.StockQuantity,
                        OutletQuantity = orderedItem.OutletQuantity,
                        ExternalReference = orderedItem.ExternalReference,
                        ExpectedDeliveryFrom = orderedItem.ExpectedDeliveryFrom,
                        ExpectedDeliveryTo = orderedItem.ExpectedDeliveryTo,
                        MoreInfo = orderedItem.MoreInfo,
                        LastModifiedDate = orderedItem.LastModifiedDate,
                        CreatedDate = orderedItem.CreatedDate
                    };

                    var lastOrderItemStatusChange = this.context.OrderItemStatusChanges.FirstOrDefault(x => x.Id == orderedItem.LastOrderItemStatusChangeId && x.IsActive);

                    if (lastOrderItemStatusChange is not null)
                    {
                        orderItem.OrderItemStateId = lastOrderItemStatusChange.OrderItemStateId;
                        orderItem.OrderItemStatusId = lastOrderItemStatusChange.OrderItemStatusId;

                        var orderItemStatusTranslation = this.context.OrderStatusTranslations.FirstOrDefault(x => x.OrderStatusId == lastOrderItemStatusChange.OrderItemStatusId && x.IsActive && x.Language == model.Language);

                        if (orderItemStatusTranslation is null)
                        {
                            orderItemStatusTranslation = this.context.OrderStatusTranslations.FirstOrDefault(x => x.OrderStatusId == lastOrderItemStatusChange.OrderItemStatusId && x.IsActive);
                        }

                        orderItem.OrderItemStatusName = orderItemStatusTranslation?.Name;
                    }

                    orderItems.Add(orderItem);
                }

                order.OrderItems = orderItems;

                ordersList.Add(order);
            }

            pagedOrdersServiceModel.Data = ordersList;

            return pagedOrdersServiceModel;
        }

        public async Task<PagedResults<IEnumerable<OrderFileServiceModel>>> GetOrderFilesAsync(GetOrderFilesServiceModel model)
        {
            var orderFiles = from f in this.context.OrderAttachments
                                              where f.OrderId == model.Id && f.IsActive
                                              select new OrderFileServiceModel
                                              {
                                                  Id = f.MediaId,
                                                  LastModifiedDate = f.LastModifiedDate,
                                                  CreatedDate = f.CreatedDate
                                              };

            if (string.IsNullOrWhiteSpace(model.SearchTerm) is false)
            {
                orderFiles = orderFiles.Where(x => x.Id.ToString() == model.SearchTerm);
            }

            orderFiles = orderFiles.ApplySort(model.OrderBy);

            if (model.PageIndex.HasValue is false || model.ItemsPerPage.HasValue is false)
            {
                orderFiles = orderFiles.Take(Constants.MaxItemsPerPageLimit);

                return orderFiles.PagedIndex(new Pagination(orderFiles.Count(), Constants.MaxItemsPerPageLimit), Constants.DefaultPageIndex);
            }

            return orderFiles.PagedIndex(new Pagination(orderFiles.Count(), model.ItemsPerPage.Value), model.PageIndex.Value);
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

            if (order is null)
            {
                throw new CustomException(this.orderLocalizer.GetString("OrderNotFound"), (int)HttpStatusCode.NoContent);
            }

            var newOrderStatus = await this.context.OrderStatuses.FirstOrDefaultAsync(x => x.Id == serviceModel.OrderStatusId && x.IsActive);

            if (newOrderStatus is null)
            {
                throw new CustomException(this.orderLocalizer.GetString("OrderStatusNotFound"), (int)HttpStatusCode.NoContent);
            }

            order.OrderStatusId = newOrderStatus.Id;
            order.OrderStateId = newOrderStatus.OrderStateId;

            if (serviceModel.OrderStatusId == OrderStatusesConstants.CanceledId)
            {
                foreach (var orderItem in order.OrderItems.OrEmptyIfNull())
                {
                    var newOrderItemStatusChange = new OrderItemStatusChange
                    {
                        OrderItemId = orderItem.Id,
                        OrderItemStateId = OrderStatesConstants.CanceledId,
                        OrderItemStatusId = OrderStatusesConstants.CanceledId
                    };

                    this.context.OrderItemStatusChanges.Add(newOrderItemStatusChange.FillCommonProperties());

                    orderItem.LastOrderItemStatusChangeId = newOrderItemStatusChange.Id;
                }
            }

            await this.context.SaveChangesAsync();

            return await this.GetAsync(new GetOrderServiceModel {  
                Id = serviceModel.OrderId,
                OrganisationId = serviceModel.OrganisationId,
                Username = serviceModel.Username,
                Language = serviceModel.Language,
                IsSeller = serviceModel.IsSeller
            });
        }

        public async Task SyncOrderItemsStatusesAsync(UpdateOrderItemsStatusesServiceModel model)
        {
            foreach (var item in model.OrderItems.OrEmptyIfNull())
            {
                var orderItem = await this.context.OrderItems.FirstOrDefaultAsync(x => x.Id == item.Id && x.IsActive);

                if (orderItem is not null)
                {
                    var newOrderItemStatus = await this.context.OrderStatuses.FirstOrDefaultAsync(x => x.Id == item.StatusId && x.IsActive);

                    if (newOrderItemStatus is not null)
                    {
                        var orderItemStatusChange = new OrderItemStatusChange
                        {
                            OrderItemId = item.Id,
                            OrderItemStateId = newOrderItemStatus.OrderStateId,
                            OrderItemStatusId = newOrderItemStatus.Id
                        };

                        await this.context.OrderItemStatusChanges.AddAsync(orderItemStatusChange.FillCommonProperties());

                        orderItem.LastOrderItemStatusChangeId = orderItemStatusChange.Id;
                        orderItem.LastModifiedDate = DateTime.UtcNow;

                        if (item.StatusChangeComment is not null)
                        {
                            var orderItemStatusChangeTranslation = new OrderItemStatusChangeCommentTranslation
                            {
                                OrderItemStatusChangeComment = item.StatusChangeComment,
                                Language = item.Language,
                                OrderItemStatusChangeId = orderItemStatusChange.Id
                            };

                            await this.context.OrderItemStatusChangesCommentTranslations.AddAsync(orderItemStatusChangeTranslation.FillCommonProperties());
                        }

                        await this.context.SaveChangesAsync();
                        await this.MapStatusesToOrderStatusId(orderItem.OrderId);
                    }
                }
            };
        }

        public async Task SyncOrderLinesStatusesAsync(UpdateOrderLinesStatusesServiceModel model)
        {
            foreach (var item in model.OrderItems.OrEmptyIfNull())
            {
                var orderItem = await this.context.OrderItems.Where(x => x.OrderId == item.Id && x.IsActive).Skip(item.OrderLineIndex - 1).FirstOrDefaultAsync();

                if (orderItem is not null)
                {
                    var newOrderItemStatus = await this.context.OrderStatuses.FirstOrDefaultAsync(x => x.Id == item.StatusId && x.IsActive);

                    if (newOrderItemStatus is not null)
                    {
                        if (newOrderItemStatus.Id == OrderStatusesConstants.NewId)
                        {
                            this.logger.LogError($"OrdersService New item: {JsonConvert.SerializeObject(item)}");
                            this.logger.LogError($"OrdersService New orderItem: {JsonConvert.SerializeObject(orderItem)}");
                            this.logger.LogError($"OrdersService New newOrderItemStatus: {JsonConvert.SerializeObject(newOrderItemStatus)}");
                        }

                        var orderItemStatusChange = new OrderItemStatusChange
                        {
                            OrderItemId = orderItem.Id,
                            OrderItemStateId = newOrderItemStatus.OrderStateId,
                            OrderItemStatusId = newOrderItemStatus.Id
                        };

                        await this.context.OrderItemStatusChanges.AddAsync(orderItemStatusChange.FillCommonProperties());

                        await this.context.SaveChangesAsync();

                        orderItem.LastOrderItemStatusChangeId = orderItemStatusChange.Id;
                        orderItem.LastModifiedDate = DateTime.UtcNow;

                        await this.context.SaveChangesAsync();

                        foreach (var commentItem in item.CommentTranslations.OrEmptyIfNull().Where(x => string.IsNullOrWhiteSpace(x.Text) is false))
                        {
                            var orderItemStatusChangeTranslation = new OrderItemStatusChangeCommentTranslation
                            {
                                OrderItemStatusChangeComment = commentItem.Text,
                                Language = commentItem.Language,
                                OrderItemStatusChangeId = orderItemStatusChange.Id
                            };

                            await this.context.OrderItemStatusChangesCommentTranslations.AddAsync(orderItemStatusChangeTranslation.FillCommonProperties());

                            await this.context.SaveChangesAsync();
                        }

                        await this.MapStatusesToOrderStatusId(orderItem.OrderId);
                    }
                }
            };
        }

        public async Task UpdateOrderItemStatusAsync(UpdateOrderItemStatusServiceModel model)
        {
            var orderItem = await this.context.OrderItems.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (orderItem is null)
            {
                throw new CustomException(this.orderLocalizer.GetString("OrderItemNotFound"), (int)HttpStatusCode.NoContent);
            }

            var newOrderItemStatus = await this.context.OrderStatuses.FirstOrDefaultAsync(x => x.Id == model.OrderItemStatusId && x.IsActive);

            if (newOrderItemStatus is null)
            {
                throw new CustomException(this.orderLocalizer.GetString("OrderStatusNotFound"), (int)HttpStatusCode.NoContent);
            }

            var orderItemStatusChange = new OrderItemStatusChange
            {
                OrderItemId = orderItem.Id,
                OrderItemStateId = newOrderItemStatus.OrderStateId,
                OrderItemStatusId = newOrderItemStatus.Id
            };

            await this.context.OrderItemStatusChanges.AddAsync(orderItemStatusChange.FillCommonProperties());

            if (model.OrderItemStatusChangeComment is not null)
            {
                var orderItemStatusChangeTranslation = new OrderItemStatusChangeCommentTranslation
                {
                    OrderItemStatusChangeComment = model.OrderItemStatusChangeComment,
                    Language = model.Language,
                    OrderItemStatusChangeId = orderItemStatusChange.Id
                };

                await this.context.OrderItemStatusChangesCommentTranslations.AddAsync(orderItemStatusChangeTranslation.FillCommonProperties());
            }

            orderItem.LastOrderItemStatusChangeId = orderItemStatusChange.Id;
            orderItem.LastModifiedDate = DateTime.UtcNow;

            await this.context.SaveChangesAsync();
            await this.MapStatusesToOrderStatusId(orderItem.OrderId);
        }

        private async Task MapStatusesToOrderStatusId(Guid? orderId)
        {
            var order = await this.context.Orders.FirstOrDefaultAsync(x => x.Id == orderId && x.IsActive);

            if (order is not null)
            {
                var orderItemsLastStatusChangeIds = order.OrderItems.Select(x => x.LastOrderItemStatusChangeId);

                var lastOrderItemStatusChanges = new List<OrderItemStatusServiceModel>();

                foreach (var orderItemLastStatusChangeId in orderItemsLastStatusChangeIds.OrEmptyIfNull())
                {
                    var lastOrderItemStatusChange = this.context.OrderItemStatusChanges.FirstOrDefault(x => x.Id == orderItemLastStatusChangeId && x.IsActive);

                    if (lastOrderItemStatusChange is not null)
                    {
                        lastOrderItemStatusChanges.Add(new OrderItemStatusServiceModel
                        {
                            OrderItemStateId = lastOrderItemStatusChange.OrderItemStateId,
                            OrderItemStatusId = lastOrderItemStatusChange.OrderItemStatusId
                        });
                    }
                }

                bool isSameStatus = lastOrderItemStatusChanges.DistinctBy(x => x.OrderItemStatusId).Count() == 1;

                var lastOrderItemStatus = lastOrderItemStatusChanges.FirstOrDefault();

                if (isSameStatus is true)
                {
                    order.OrderStatusId = lastOrderItemStatus.OrderItemStatusId;
                    order.OrderStateId = lastOrderItemStatus.OrderItemStateId;
                    order.LastModifiedDate = DateTime.UtcNow;
                }
                else if (lastOrderItemStatus.OrderItemStatusId != OrderStatusesConstants.CanceledId && lastOrderItemStatus is not null)
                {
                    order.OrderStatusId = OrderStatusesConstants.ProcessingId;
                    order.OrderStateId = OrderStatesConstants.ProcessingId;
                    order.LastModifiedDate = DateTime.UtcNow;
                }

                await this.context.SaveChangesAsync();
            }
        }
    }
}
