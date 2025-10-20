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
using Newtonsoft.Json;
using Ordering.Api.Configurations;
using Ordering.Api.Definitions;
using Ordering.Api.Infrastructure;
using Ordering.Api.Infrastructure.Orders.Entities;
using Ordering.Api.IntegrationEvents;
using Ordering.Api.IntegrationEventsModels;
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
        private readonly OrderingContext _context;
        private readonly IEventBus _eventBus;
        private readonly IStringLocalizer<OrderResources> _orderLocalizer;
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;
        private readonly IOptionsMonitor<AppSettings> _orderingOptions;
        private readonly IMailingService _mailingService;
        private readonly IOptions<AppSettings> _configuration;
        private readonly IMediaService _mediaService;
        private readonly ILogger _logger;

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
            _context = context;
            _eventBus = eventBus;
            _orderLocalizer = orderLocalizer;
            _mailingService = mailingService;
            _configuration = configuration;
            _globalLocalizer = globalLocalizer;
            _orderingOptions = orderingOptions;
            _mediaService = mediaService;
            _logger = logger;
        }

        public async Task CheckoutAsync(CheckoutBasketServiceModel serviceModel)
        {
            using var source = new ActivitySource(GetType().Name);

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
                BillingCountryId = serviceModel.BillingCountryId,
                BillingFirstName = serviceModel.BillingFirstName,
                BillingLastName = serviceModel.BillingLastName,
                BillingPhoneNumber = serviceModel.BillingPhoneNumber,
                BillingPostCode = serviceModel.BillingPostCode,
                BillingRegion = serviceModel.BillingRegion,
                BillingStreet = serviceModel.BillingStreet,
                ShippingAddressId = serviceModel.ShippingAddressId,
                ShippingCity = serviceModel.ShippingCity,
                ShippingCompany = serviceModel.ShippingCompany,
                ShippingCountryId = serviceModel.ShippingCountryId,
                ShippingFirstName = serviceModel.ShippingFirstName,
                ShippingLastName = serviceModel.ShippingLastName,
                ShippingPhoneNumber = serviceModel.ShippingPhoneNumber,
                ShippingPostCode = serviceModel.ShippingPostCode,
                ShippingRegion = serviceModel.ShippingRegion,
                ShippingStreet = serviceModel.ShippingStreet,
                ExternalReference = serviceModel.ExternalReference,
                MoreInfo = serviceModel.MoreInfo,
                IpAddress = serviceModel.IpAddress
            };

            _context.Orders.Add(order.FillCommonProperties());

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
                    UnitPrice = basketItem.UnitPrice,
                    Price = basketItem.Price,
                    Currency = basketItem.Currency,
                    ExternalReference = basketItem.ExternalReference,
                    MoreInfo = basketItem.MoreInfo
                };

                _context.OrderItems.Add(orderItem.FillCommonProperties());

                var orderItemStatusChange = new OrderItemStatusChange
                {
                    OrderItemId = orderItem.Id,
                    OrderItemStateId = OrderItemStatesConstants.NewId,
                    OrderItemStatusId = OrderItemStatusConstants.NewId,
                };

                _context.OrderItemStatusChanges.Add(orderItemStatusChange.FillCommonProperties());

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

                    attachments.Add(_mediaService.GetMediaUrl(attachmentId));

                    await _context.OrderAttachments.AddAsync(newAttachment.FillCommonProperties());
                }

                Thread.CurrentThread.CurrentCulture = new CultureInfo(serviceModel.Language);
                Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

                if (CanSend(_configuration.Value.SenderEmail, _configuration.Value.SenderName, _orderingOptions.CurrentValue.ActionSendGridCustomOrderTemplateId))
                {
                    await _mailingService.SendTemplateAsync(new TemplateEmail
                    {
                        RecipientEmailAddress = _configuration.Value.SenderEmail,
                        RecipientName = _configuration.Value.SenderName,
                        SenderEmailAddress = _configuration.Value.SenderEmail,
                        SenderName = _configuration.Value.SenderName,
                        TemplateId = _orderingOptions.CurrentValue.ActionSendGridCustomOrderTemplateId,
                        DynamicTemplateData = new
                        {
                            attachmentsLabel = _globalLocalizer.GetString("AttachedAttachments").Value,
                            attachments = attachments,
                            subject = _orderLocalizer.GetString("CustomOrderSubject").Value + " " + serviceModel.ClientName + " (" + order.Id + ")",
                            text = serviceModel.MoreInfo
                        }
                    });
                }
            }

            await _context.SaveChangesAsync();

            var message = new OrderStartedIntegrationEvent
            {
                BasketId = serviceModel.BasketId,
                ClientId = serviceModel.ClientId,
                OrderItems = serviceModel.Items.OrEmptyIfNull().Select(x => new OrderItemStartedEventModel
                {
                    Id = x.ProductId,
                    Quantity = x.Quantity,
                    StockQuantity = x.StockQuantity,
                    OutletQuantity = x.OutletQuantity,
                    Price = x.Price,
                    Currency = x.Currency
                }),
                CreatedDate = order.CreatedDate
            };

            using var activity = source.StartActivity($"{System.Reflection.MethodBase.GetCurrentMethod().Name} {message.GetType().Name}");
            _eventBus.Publish(message);

            if (serviceModel.HasApprovalToSendEmail && CanSend(serviceModel.ClientEmail, serviceModel.ClientName, _configuration.Value.SenderEmail, _configuration.Value.SenderName, _configuration.Value.ActionSendGridConfirmationOrderTemplateId))
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(serviceModel.Language);
                Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

                await _mailingService.SendTemplateAsync(new TemplateEmail
                {
                    RecipientEmailAddress = serviceModel.ClientEmail,
                    RecipientName = serviceModel.ClientName,
                    SenderEmailAddress = _configuration.Value.SenderEmail,
                    SenderName = _configuration.Value.SenderName,
                    TemplateId = _configuration.Value.ActionSendGridConfirmationOrderTemplateId,
                    DynamicTemplateData = new
                    {
                        oc_subject = _orderLocalizer.GetString("oc_subject").Value,
                        oc_preheader = _orderLocalizer.GetString("oc_preheader").Value,
                        oc_title = _orderLocalizer.GetString("oc_title").Value,
                        oc_text = _orderLocalizer.GetString("oc_text").Value,
                        oc_orderedProducts = _orderLocalizer.GetString("oc_orderedProducts").Value,
                        oc_name = _orderLocalizer.GetString("sh_nameLabel").Value,
                        oc_quantity = _orderLocalizer.GetString("sh_quantityLabel").Value,
                        oc_stockQuantity = _orderLocalizer.GetString("sh_stockQuantityLabel").Value,
                        oc_outletQuantity = _orderLocalizer.GetString("sh_outletQuantityLabel").Value,
                        oc_externalReference= _orderLocalizer.GetString("sh_externalReferenceLabel").Value,
                        oc_products = serviceModel.Items.OrEmptyIfNull().Select(x => new
                        {
                            pictureUrl = x.PictureUrl,
                            name = $"{x.ProductName} ({x.ProductSku})",
                            quantity = x.Quantity,
                            stockQuantity = x.StockQuantity,
                            outletQuantity = x.OutletQuantity,
                            externalReference = x.ExternalReference
                        })
                    }
                });
            }
        }

        public async Task<PagedResults<IEnumerable<OrderServiceModel>>> GetAsync(GetOrdersServiceModel model)
        {
            var orders = _context.Orders.AsNoTracking().Where(x => x.IsActive);

            if (model.IsSeller is false)
            {
                orders = orders.Where(x => x.SellerId == model.OrganisationId.Value);
            }

            if (string.IsNullOrWhiteSpace(model.SearchTerm) is false)
            {
                var searchTermLower = model.SearchTerm.ToLower();
                orders = orders.Where(x => x.ClientName.ToLower().StartsWith(searchTermLower)
                || x.OrderItems.Any(y => y.ExternalReference.ToLower().StartsWith(searchTermLower))
                || x.Id.ToString().ToLower() == searchTermLower);
            }

            if (model.CreatedDateGreaterThan.HasValue)
            {
                orders = orders.Where(x => x.CreatedDate >= model.CreatedDateGreaterThan);
            }

            if (model.OrderStatusId.HasValue)
            {
                orders = orders.Where(x => x.OrderStatusId == model.OrderStatusId.Value);
            }

            orders = orders.ApplySort(model.OrderBy);

            int pageSize = model.ItemsPerPage ?? Constants.MaxItemsPerPageLimit;
            int pageIndex = model.PageIndex ?? Constants.DefaultPageIndex;
            
            // Get total count asynchronously
            var totalCount = await orders.CountAsync();
            
            // Apply pagination at the database level
            var pagedOrders = await orders
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var pagedResults = new PagedResults<IEnumerable<Order>>(totalCount, pageSize)
            {
                Data = pagedOrders
            };

            return await GetOrdersDetailsAsync(model.Language, pagedResults);
        }

        public async Task<OrderServiceModel> GetAsync(GetOrderServiceModel model)
        {
            var existingOrder = await _context.Orders.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (existingOrder is null)
            {
                throw new NotFoundException(_orderLocalizer.GetString("OrderNotFound"));
            }

            var orderStatusTranslations = await _context.OrderStatusTranslations
                .AsNoTracking()
                .Where(x => x.IsActive)
                .ToListAsync();

            var orderItems = await _context.OrderItems
                .AsNoTracking()
                .Where(x => x.OrderId == existingOrder.Id && x.IsActive)
                .ToListAsync();

            var orderItemIds = orderItems.Select(x => x.Id).ToList();
            var lastOrderItemStatusChanges = await _context.OrderItemStatusChanges
                .AsNoTracking()
                .Where(x => orderItemIds.Contains(x.OrderItemId) && x.IsActive)
                .ToListAsync();

            var statusChangeIds = lastOrderItemStatusChanges.Select(x => x.Id).ToList();
            var orderItemStatusChangesCommentTranslations = await _context.OrderItemStatusChangesCommentTranslations
                .AsNoTracking()
                .Where(x => statusChangeIds.Contains(x.OrderItemStatusChangeId) && x.IsActive)
                .ToListAsync();

            var order = new OrderServiceModel
            {
                Id = existingOrder.Id,
                SellerId = existingOrder.SellerId,
                ClientId = existingOrder.ClientId.Value,
                ClientName = existingOrder.ClientName,
                BillingAddressId = existingOrder.BillingAddressId,
                BillingCity = existingOrder.BillingCity,
                BillingCompany = existingOrder.BillingCompany,
                BillingCountryId = existingOrder.BillingCountryId,
                BillingFirstName = existingOrder.BillingFirstName,
                BillingLastName = existingOrder.BillingLastName,
                BillingPhoneNumber = existingOrder.BillingPhoneNumber,
                BillingPostCode = existingOrder.BillingPostCode,
                BillingRegion = existingOrder.BillingRegion,
                BillingStreet = existingOrder.BillingStreet,
                ShippingAddressId = existingOrder.ShippingAddressId,
                ShippingCity = existingOrder.ShippingCity,
                ShippingCompany = existingOrder.ShippingCompany,
                ShippingCountryId = existingOrder.ShippingCountryId,
                ShippingFirstName = existingOrder.ShippingFirstName,
                ShippingLastName = existingOrder.ShippingLastName,
                ShippingPhoneNumber = existingOrder.ShippingPhoneNumber,
                ShippingPostCode = existingOrder.ShippingPostCode,
                ShippingRegion = existingOrder.ShippingRegion,
                ShippingStreet = existingOrder.ShippingStreet,
                ExternalReference = existingOrder.ExternalReference,
                MoreInfo = existingOrder.MoreInfo,
                Reason = existingOrder.Reason,
                OrderStateId = existingOrder.OrderStateId,
                OrderStatusId = existingOrder.OrderStatusId,
                OrderStatusName = orderStatusTranslations.FirstOrDefault(y => y.OrderStatusId == existingOrder.OrderStatusId && y.Language == model.Language)?.Name ?? orderStatusTranslations.FirstOrDefault(y => y.OrderStatusId == existingOrder.OrderStatusId)?.Name
            };

            var orderItemsList = new List<OrderItemServiceModel>();

            foreach (var orderItem in orderItems.OrEmptyIfNull())
            {
                var newOrderItem = new OrderItemServiceModel
                {
                    Id = orderItem.Id,
                    OrderId = orderItem.OrderId,
                    ProductId = orderItem.ProductId,
                    ProductSku = orderItem.ProductSku,
                    ProductName = orderItem.ProductName,
                    PictureUrl = orderItem.PictureUrl,
                    Quantity = orderItem.Quantity,
                    StockQuantity = orderItem.StockQuantity,
                    OutletQuantity = orderItem.OutletQuantity,
                    UnitPrice = orderItem.UnitPrice,
                    Price = orderItem.Price,
                    Currency = orderItem.Currency,
                    ExternalReference = orderItem.ExternalReference,
                    MoreInfo = orderItem.MoreInfo,
                    LastOrderItemStatusChangeId = orderItem.LastOrderItemStatusChangeId,
                    LastModifiedDate = orderItem.LastModifiedDate,
                    CreatedDate = orderItem.CreatedDate
                };

                var lastOrderItemStatus = lastOrderItemStatusChanges.FirstOrDefault(x => x.Id == orderItem.LastOrderItemStatusChangeId);

                if (lastOrderItemStatus is not null)
                {
                    newOrderItem.OrderItemStatusId = lastOrderItemStatus.OrderItemStatusId;
                    newOrderItem.OrderItemStateId = lastOrderItemStatus.OrderItemStateId;
                }

                var orderItemStatusTranslation = orderStatusTranslations.FirstOrDefault(x => x.OrderStatusId == lastOrderItemStatus.OrderItemStatusId && x.Language == model.Language && x.IsActive);

                if (orderItemStatusTranslation is null)
                {
                    orderItemStatusTranslation = orderStatusTranslations.FirstOrDefault(x => x.OrderStatusId == lastOrderItemStatus.OrderItemStatusId && x.IsActive);
                }

                newOrderItem.OrderItemStatusName = orderItemStatusTranslation?.Name;

                var orderItemStatusChangeCommentTranslation = orderItemStatusChangesCommentTranslations.FirstOrDefault(x => x.OrderItemStatusChangeId == lastOrderItemStatus.Id && x.Language == model.Language && x.IsActive);

                if (orderItemStatusChangeCommentTranslation is null)
                {
                    orderItemStatusChangeCommentTranslation = orderItemStatusChangesCommentTranslations.FirstOrDefault(x => x.OrderItemStatusChangeId == lastOrderItemStatus.Id && x.IsActive);
                }

                newOrderItem.OrderItemStatusChangeComment = orderItemStatusChangeCommentTranslation?.OrderItemStatusChangeComment;

                orderItemsList.Add(newOrderItem);
            }

            order.OrderItems = orderItemsList;

            return order;
        }

        public async Task<OrderItemServiceModel> GetAsync(GetOrderItemServiceModel model)
        {
            var existingOrderItem = await _context.OrderItems.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (existingOrderItem is null)
            {
                throw new NotFoundException(_orderLocalizer.GetString("OrderItemNotFound"));
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
                UnitPrice = existingOrderItem.UnitPrice,
                Price = existingOrderItem.Price,
                Currency = existingOrderItem.Currency,
                ExternalReference = existingOrderItem.ExternalReference,
                LastOrderItemStatusChangeId = existingOrderItem.LastOrderItemStatusChangeId,
                MoreInfo = existingOrderItem.MoreInfo,
                LastModifiedDate = existingOrderItem.LastModifiedDate,
                CreatedDate = existingOrderItem.CreatedDate
            };

            if (existingOrderItem.LastOrderItemStatusChangeId is not null && existingOrderItem.LastOrderItemStatusChangeId != Guid.Empty)
            {
                var lastOrderItemStatus = await _context.OrderItemStatusChanges.FirstOrDefaultAsync(x => x.Id == existingOrderItem.LastOrderItemStatusChangeId && x.IsActive);

                if (lastOrderItemStatus is null)
                {
                    throw new NotFoundException(_orderLocalizer.GetString("LastOrderItemStatusNotFound"));
                }

                orderItem.OrderItemStateId = lastOrderItemStatus.OrderItemStateId;
                orderItem.OrderItemStatusId = lastOrderItemStatus.OrderItemStatusId;

                var orderItemStatusTranslation = await _context.OrderStatusTranslations.FirstOrDefaultAsync(x => x.OrderStatusId == lastOrderItemStatus.OrderItemStatusId && x.Language == model.Language && x.IsActive);

                if (orderItemStatusTranslation is null)
                {
                    orderItemStatusTranslation = await _context.OrderStatusTranslations.FirstOrDefaultAsync(x => x.OrderStatusId == lastOrderItemStatus.OrderItemStatusId && x.IsActive);
                }

                var orderItemStatusChangeCommentTranslation = await _context.OrderItemStatusChangesCommentTranslations
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.OrderItemStatusChangeId == lastOrderItemStatus.Id && x.Language == model.Language && x.IsActive);

                if (orderItemStatusChangeCommentTranslation is null)
                {
                    orderItemStatusChangeCommentTranslation = await _context.OrderItemStatusChangesCommentTranslations
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.OrderItemStatusChangeId == lastOrderItemStatus.Id && x.IsActive);
                }

                orderItem.OrderItemStatusChangeComment = orderItemStatusChangeCommentTranslation?.OrderItemStatusChangeComment;

                orderItem.OrderItemStatusName = orderItemStatusTranslation?.Name;
            }

            return orderItem;
        }

        public async Task<OrderItemStatusChangesServiceModel> GetAsync(GetOrderItemStatusChangesServiceModel model)
        {
            var orderItem = await _context.OrderItems
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (orderItem is null)
            {
                throw new NotFoundException(_orderLocalizer.GetString("OrderItemNotFound"));
            }

            var orderItemStatusesHistory = new OrderItemStatusChangesServiceModel
            {
                OrderItemId = orderItem.Id,
            };

            var orderItemStatuses = await _context.OrderItemStatusChanges
                .AsNoTracking()
                .Where(x => x.OrderItemId == model.Id && x.IsActive)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();

            // Get all status IDs for translations lookup
            var statusIds = orderItemStatuses.Select(x => x.OrderItemStatusId).Distinct().ToList();
            var statusChangeIds = orderItemStatuses.Select(x => x.Id).ToList();

            // Get translations in a single query
            var orderStatusTranslations = await _context.OrderStatusTranslations
                .AsNoTracking()
                .Where(x => statusIds.Contains(x.OrderStatusId) && x.IsActive)
                .ToListAsync();

            // Get comment translations in a single query
            var orderItemStatusChangesCommentTranslations = await _context.OrderItemStatusChangesCommentTranslations
                .AsNoTracking()
                .Where(x => statusChangeIds.Contains(x.OrderItemStatusChangeId) && x.IsActive)
                .ToListAsync();

            var orderItemStatusChanges = new List<OrderItemStatusChangeServiceModel>();

            foreach (var orderItemStatus in orderItemStatuses.OrEmptyIfNull())
            {
                var orderItemStatusChange = new OrderItemStatusChangeServiceModel
                {
                    OrderItemStateId = orderItemStatus.OrderItemStateId,
                    OrderItemStatusId = orderItemStatus.OrderItemStatusId,
                    CreatedDate = orderItemStatus.CreatedDate
                };

                var orderItemStatusTranslation = orderStatusTranslations
                    .FirstOrDefault(x => x.OrderStatusId == orderItemStatus.OrderItemStatusId && 
                                        x.Language == model.Language);

                if (orderItemStatusTranslation is null)
                {
                    orderItemStatusTranslation = orderStatusTranslations
                        .FirstOrDefault(x => x.OrderStatusId == orderItemStatus.OrderItemStatusId);
                }

                orderItemStatusChange.OrderItemStatusName = orderItemStatusTranslation?.Name;

                var orderItemStatusChangeCommentTranslation = orderItemStatusChangesCommentTranslations
                    .FirstOrDefault(x => x.OrderItemStatusChangeId == orderItemStatus.Id && 
                                        x.Language == model.Language);

                if (orderItemStatusChangeCommentTranslation is null)
                {
                    orderItemStatusChangeCommentTranslation = orderItemStatusChangesCommentTranslations
                        .FirstOrDefault(x => x.OrderItemStatusChangeId == orderItemStatus.Id);
                }

                orderItemStatusChange.OrderItemStatusChangeComment = orderItemStatusChangeCommentTranslation?.OrderItemStatusChangeComment;

                orderItemStatusChanges.Add(orderItemStatusChange);
            }

            orderItemStatusesHistory.OrderItemStatusChanges = orderItemStatusChanges;

            return orderItemStatusesHistory;
        }

        public async Task<PagedResults<IEnumerable<OrderServiceModel>>> GetAsync(GetOrdersByIdsServiceModel model)
        {
            var orders = _context.Orders.AsNoTracking().Where(x => x.IsActive && model.Ids.Contains(x.Id));

            if (model.IsSeller is false)
            {
                orders = orders.Where(x => x.SellerId == model.OrganisationId.Value);
            }

            if (string.IsNullOrWhiteSpace(model.SearchTerm) is false)
            {
                var searchTermLower = model.SearchTerm.ToLower();
                orders = orders.Where(x => x.ClientName.ToLower().StartsWith(searchTermLower)
                || x.OrderItems.Any(y => y.ExternalReference.ToLower().StartsWith(searchTermLower))
                || x.Id.ToString().ToLower() == searchTermLower);
            }

            orders = orders.ApplySort(model.OrderBy);

            int pageSize = model.ItemsPerPage ?? Constants.MaxItemsPerPageLimit;
            int pageIndex = model.PageIndex ?? Constants.DefaultPageIndex;
            
            // Get total count asynchronously
            var totalCount = await orders.CountAsync();
            
            // Apply pagination at the database level
            var pagedOrders = await orders
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var pagedResults = new PagedResults<IEnumerable<Order>>(totalCount, pageSize)
            {
                Data = pagedOrders
            };

            return await GetOrdersDetailsAsync(model.Language, pagedResults);
        }

        public async Task<PagedResults<IEnumerable<OrderFileServiceModel>>> GetOrderFilesAsync(GetOrderFilesServiceModel model)
        {
            var orderFiles = from f in _context.OrderAttachments.AsNoTracking()
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

            int pageSize = model.ItemsPerPage ?? Constants.MaxItemsPerPageLimit;
            int pageIndex = model.PageIndex ?? Constants.DefaultPageIndex;
            
            // Get total count asynchronously
            var totalCount = await orderFiles.CountAsync();
            
            // Apply pagination at the database level
            var pagedFiles = await orderFiles
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResults<IEnumerable<OrderFileServiceModel>>(totalCount, pageSize)
            {
                Data = pagedFiles
            };
        }

        public async Task<IEnumerable<OrderStatusServiceModel>> GetOrderStatusesAsync(GetOrderStatusesServiceModel serviceModel)
        {
            var orderStatuses = from orderstatus in _context.OrderStatuses.AsNoTracking()
                                join orderstate in _context.OrderStates.AsNoTracking() on orderstatus.OrderStateId equals orderstate.Id
                                join orderstatustranslation in _context.OrderStatusTranslations.AsNoTracking() on orderstatus.Id equals orderstatustranslation.OrderStatusId
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

            return await orderStatuses.ToListAsync();
        }

        public async Task<OrderServiceModel> SaveOrderStatusAsync(UpdateOrderStatusServiceModel serviceModel)
        {
            var orders = _context.Orders.Where(x => x.Id == serviceModel.OrderId && x.IsActive);

            if (serviceModel.IsSeller is false)
            {
                orders = orders.Where(x => x.SellerId == serviceModel.OrganisationId.Value);
            }

            var order = await orders.FirstOrDefaultAsync();

            if (order is null)
            {
                throw new NotFoundException(_orderLocalizer.GetString("OrderNotFound"));
            }

            var newOrderStatus = await _context.OrderStatuses.FirstOrDefaultAsync(x => x.Id == serviceModel.OrderStatusId && x.IsActive);

            if (newOrderStatus is null)
            {
                throw new NotFoundException(_orderLocalizer.GetString("OrderStatusNotFound"));
            }

            order.OrderStatusId = newOrderStatus.Id;
            order.OrderStateId = newOrderStatus.OrderStateId;

            if (serviceModel.OrderStatusId == OrderStatusesConstants.CancelledId)
            {
                foreach (var orderItem in order.OrderItems.OrEmptyIfNull())
                {
                    var newOrderItemStatusChange = new OrderItemStatusChange
                    {
                        OrderItemId = orderItem.Id,
                        OrderItemStateId = OrderStatesConstants.CancelledId,
                        OrderItemStatusId = OrderStatusesConstants.CancelledId
                    };

                    _context.OrderItemStatusChanges.Add(newOrderItemStatusChange.FillCommonProperties());

                    orderItem.LastOrderItemStatusChangeId = newOrderItemStatusChange.Id;
                }
            }

            await _context.SaveChangesAsync();

            if (serviceModel.OrderStatusId == OrderStatusesConstants.CancelledId && CanSend(_configuration.Value.SenderEmail, _configuration.Value.SenderName, _configuration.Value.ActionSendGridCancelOrderTemplateId))
            {
                await _mailingService.SendTemplateAsync(new TemplateEmail
                {
                    RecipientEmailAddress = _configuration.Value.SenderEmail,
                    RecipientName = _configuration.Value.SenderName,
                    SenderEmailAddress = _configuration.Value.SenderEmail,
                    SenderName = _configuration.Value.SenderName,
                    TemplateId = _configuration.Value.ActionSendGridCancelOrderTemplateId,
                    DynamicTemplateData = new
                    {
                        co_subject = _orderLocalizer.GetString("co_subject").Value,
                        co_preheader = _orderLocalizer.GetString("co_preheader").Value,
                        co_clientName = order.ClientName,
                        co_clientNameLabel = _orderLocalizer.GetString("sh_clientNameLabel").Value,
                        co_buyerUrl = _configuration.Value.BuyerUrl,
                        co_orderLink = $"{CultureInfo.CurrentCulture.Name}/{OrderCancelationConstants.OrderDetailsUrl}/{order.Id}",
                        co_orderLinkLabel = _orderLocalizer.GetString("sh_orderLinkLabel").Value,
                        co_cancelOrderItemsLabel = _orderLocalizer.GetString("co_cancelOrderItemsLabel").Value,
                        co_name = _orderLocalizer.GetString("sh_nameLabel").Value,
                        co_quantity = _orderLocalizer.GetString("sh_quantityLabel").Value,
                        co_stockQuantity = _orderLocalizer.GetString("sh_stockQuantityLabel").Value,
                        co_outletQuantity = _orderLocalizer.GetString("sh_outletQuantityLabel").Value,
                        co_products = order.OrderItems.OrEmptyIfNull().Select(x => new
                        {
                            pictureUrl = x.PictureUrl,
                            name = $"{x.ProductName} ({x.ProductSku})",
                            quantity = x.Quantity,
                            stockQuantity = x.StockQuantity,
                            outletQuantity = x.OutletQuantity,
                            unitPrice = x.UnitPrice,
                            price = x.Price,
                            currency = x.Currency
                        })
                    }
                });
            }

            return await GetAsync(new GetOrderServiceModel
            {
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
                var orderItem = await _context.OrderItems.FirstOrDefaultAsync(x => x.Id == item.Id && x.IsActive);

                if (orderItem is not null)
                {
                    var newOrderItemStatus = await _context.OrderStatuses.FirstOrDefaultAsync(x => x.Id == item.StatusId && x.IsActive);

                    if (newOrderItemStatus is not null)
                    {
                        var orderItemStatusChange = new OrderItemStatusChange
                        {
                            OrderItemId = item.Id,
                            OrderItemStateId = newOrderItemStatus.OrderStateId,
                            OrderItemStatusId = newOrderItemStatus.Id
                        };

                        await _context.OrderItemStatusChanges.AddAsync(orderItemStatusChange.FillCommonProperties());

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

                            await _context.OrderItemStatusChangesCommentTranslations.AddAsync(orderItemStatusChangeTranslation.FillCommonProperties());
                        }

                        await _context.SaveChangesAsync();
                        await MapStatusesToOrderStatusId(orderItem.OrderId);
                    }
                }
            };
        }

        public async Task SyncOrderLinesStatusesAsync(UpdateOrderLinesStatusesServiceModel model)
        {
            foreach (var item in model.OrderItems.OrEmptyIfNull())
            {
                var orderItem = await _context.OrderItems.Where(x => x.OrderId == item.Id && x.IsActive).Skip(item.OrderLineIndex - 1).FirstOrDefaultAsync();

                if (orderItem is not null)
                {
                    var newOrderItemStatus = await _context.OrderStatuses.FirstOrDefaultAsync(x => x.Id == item.StatusId && x.IsActive);

                    if (newOrderItemStatus is not null)
                    {
                        if (newOrderItemStatus.Id == OrderStatusesConstants.NewId)
                        {
                            _logger.LogError($"OrdersService New item: {JsonConvert.SerializeObject(item)}");
                            _logger.LogError($"OrdersService New orderItem: {JsonConvert.SerializeObject(orderItem)}");
                            _logger.LogError($"OrdersService New newOrderItemStatus: {JsonConvert.SerializeObject(newOrderItemStatus)}");
                        }

                        var orderItemStatusChange = new OrderItemStatusChange
                        {
                            OrderItemId = orderItem.Id,
                            OrderItemStateId = newOrderItemStatus.OrderStateId,
                            OrderItemStatusId = newOrderItemStatus.Id
                        };

                        await _context.OrderItemStatusChanges.AddAsync(orderItemStatusChange.FillCommonProperties());

                        await _context.SaveChangesAsync();

                        orderItem.LastOrderItemStatusChangeId = orderItemStatusChange.Id;
                        orderItem.LastModifiedDate = DateTime.UtcNow;

                        await _context.SaveChangesAsync();

                        foreach (var commentItem in item.CommentTranslations.OrEmptyIfNull().Where(x => string.IsNullOrWhiteSpace(x.Text) is false))
                        {
                            var orderItemStatusChangeTranslation = new OrderItemStatusChangeCommentTranslation
                            {
                                OrderItemStatusChangeComment = commentItem.Text,
                                Language = commentItem.Language,
                                OrderItemStatusChangeId = orderItemStatusChange.Id
                            };

                            await _context.OrderItemStatusChangesCommentTranslations.AddAsync(orderItemStatusChangeTranslation.FillCommonProperties());

                            await _context.SaveChangesAsync();
                        }

                        await MapStatusesToOrderStatusId(orderItem.OrderId);
                    }
                }
            };
        }

        public async Task UpdateOrderItemStatusAsync(UpdateOrderItemStatusServiceModel model)
        {
            var orderItem = await _context.OrderItems.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (orderItem is null)
            {
                throw new NotFoundException(_orderLocalizer.GetString("OrderItemNotFound"));
            }

            var newOrderItemStatus = await _context.OrderStatuses.FirstOrDefaultAsync(x => x.Id == model.OrderItemStatusId && x.IsActive);

            if (newOrderItemStatus is null)
            {
                throw new NotFoundException(_orderLocalizer.GetString("OrderStatusNotFound"));
            }

            var orderItemStatusChange = new OrderItemStatusChange
            {
                OrderItemId = orderItem.Id,
                OrderItemStateId = newOrderItemStatus.OrderStateId,
                OrderItemStatusId = newOrderItemStatus.Id
            };

            await _context.OrderItemStatusChanges.AddAsync(orderItemStatusChange.FillCommonProperties());

            if (model.OrderItemStatusChangeComment is not null)
            {
                var orderItemStatusChangeTranslation = new OrderItemStatusChangeCommentTranslation
                {
                    OrderItemStatusChangeComment = model.OrderItemStatusChangeComment,
                    Language = model.Language,
                    OrderItemStatusChangeId = orderItemStatusChange.Id
                };

                await _context.OrderItemStatusChangesCommentTranslations.AddAsync(orderItemStatusChangeTranslation.FillCommonProperties());
            }

            orderItem.LastOrderItemStatusChangeId = orderItemStatusChange.Id;
            orderItem.LastModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            if (model.OrderItemStatusId.Equals(OrderStatusesConstants.CancelledId) && CanSend(_configuration.Value.SenderEmail, _configuration.Value.SenderName, _configuration.Value.ActionSendGridCancelOrderItemTemplateId))
            {
                var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id.Equals(orderItem.OrderId) && x.IsActive);

                await _mailingService.SendTemplateAsync(new TemplateEmail
                {
                    RecipientEmailAddress = _configuration.Value.SenderEmail,
                    RecipientName = _configuration.Value.SenderName,
                    SenderEmailAddress = _configuration.Value.SenderEmail,
                    SenderName = _configuration.Value.SenderName,
                    TemplateId = _configuration.Value.ActionSendGridCancelOrderItemTemplateId,
                    DynamicTemplateData = new
                    {
                        coi_subject = _orderLocalizer.GetString("coi_subject").Value,
                        coi_preheader = _orderLocalizer.GetString("coi_preheader").Value,
                        coi_clientNameLabel = _orderLocalizer.GetString("sh_clientNameLabel").Value,
                        coi_clientName = order.ClientName,
                        coi_buyerUrl = _configuration.Value.BuyerUrl,
                        coi_orderItemLink = $"{CultureInfo.CurrentCulture.Name}/{OrderCancelationConstants.OrderItemDetailsUrl}/{orderItem.Id}",
                        coi_orderItemLinkLabel = _orderLocalizer.GetString("sh_orderLinkLabel").Value,
                        coi_cancelOrderItemLabel = _orderLocalizer.GetString("coi_cancelOrderItemLabel").Value,
                        coi_name = _orderLocalizer.GetString("sh_nameLabel").Value,
                        coi_quantity = _orderLocalizer.GetString("sh_quantityLabel").Value,
                        coi_stockQuantity = _orderLocalizer.GetString("sh_stockQuantityLabel").Value,
                        coi_outletQuantity = _orderLocalizer.GetString("sh_outletQuantityLabel").Value,
                        coi_productName = orderItem.ProductName,
                        coi_productPictureUrl = orderItem.PictureUrl,
                        coi_productQuantity = orderItem.Quantity,
                        coi_productStockQuantity = orderItem.StockQuantity,
                        coi_productOutletQuantity = orderItem.OutletQuantity
                    }
                });
            }

            await MapStatusesToOrderStatusId(orderItem.OrderId);
        }

        private async Task<PagedResults<IEnumerable<OrderServiceModel>>> GetOrdersDetailsAsync(string language, PagedResults<IEnumerable<Order>> pagedResults)
        {
            if (pagedResults.Data == null || !pagedResults.Data.Any())
            {
                return new PagedResults<IEnumerable<OrderServiceModel>>(pagedResults.Total, pagedResults.PageSize)
                {
                    Data = Enumerable.Empty<OrderServiceModel>()
                };
            }

            var orderIdSet = new HashSet<Guid>(pagedResults.Data.OrEmptyIfNull().Select(x => x.Id));
            
            // Execute database queries sequentially to avoid concurrency issues
            var orderStatusTranslations = await _context.OrderStatusTranslations
                .AsNoTracking()
                .ToListAsync();
                
            var orderItems = await _context.OrderItems
                .AsNoTracking()
                .Where(x => orderIdSet.Contains(x.OrderId) && x.IsActive)
                .ToListAsync();
            
            var orderItemIdSet = orderItems.Select(x => x.Id).ToHashSet();
            
            // Get the latest status change for each order item
            var lastOrderItemStatusChanges = await _context.OrderItemStatusChanges
                .AsNoTracking()
                .Where(x => orderItemIdSet.Contains(x.OrderItemId) && x.IsActive)
                .ToListAsync();
                
            // Process in memory to find the latest status change per order item
            var latestStatusChanges = lastOrderItemStatusChanges
                .GroupBy(x => x.OrderItemId)
                .ToDictionary(
                    g => g.Key,
                    g => g.OrderByDescending(x => x.CreatedDate).First()
                );
                
            var orderItemStatusChangeIds = latestStatusChanges.Values.Select(x => x.Id).ToHashSet();
            
            // Get comment translations - sequential to avoid concurrency issues
            var orderItemStatusChangesCommentTranslations = await _context.OrderItemStatusChangesCommentTranslations
                .AsNoTracking()
                .Where(x => orderItemStatusChangeIds.Contains(x.OrderItemStatusChangeId) && x.IsActive)
                .ToDictionaryAsync(x => x.OrderItemStatusChangeId, x => x);

            // Process the data in memory
            var resultData = pagedResults.Data.OrEmptyIfNull().Select(order =>
            {
                var orderItemsModels = orderItems
                    .Where(item => item.OrderId == order.Id)
                    .Select(item =>
                    {
                        var orderItemModel = new OrderItemServiceModel
                        {
                            Id = item.Id,
                            OrderId = item.OrderId,
                            ProductId = item.ProductId,
                            ProductSku = item.ProductSku,
                            ProductName = item.ProductName,
                            PictureUrl = item.PictureUrl,
                            Quantity = item.Quantity,
                            StockQuantity = item.StockQuantity,
                            OutletQuantity = item.OutletQuantity,
                            UnitPrice = item.UnitPrice,
                            Price = item.Price,
                            Currency = item.Currency,
                            ExternalReference = item.ExternalReference,
                            MoreInfo = item.MoreInfo,
                            LastOrderItemStatusChangeId = item.LastOrderItemStatusChangeId,
                            LastModifiedDate = item.LastModifiedDate,
                            CreatedDate = item.CreatedDate
                        };

                        if (latestStatusChanges.TryGetValue(item.Id, out var statusChange))
                        {
                            orderItemModel.OrderItemStateId = statusChange.OrderItemStateId;
                            orderItemModel.OrderItemStatusId = statusChange.OrderItemStatusId;
                            
                            // Find the appropriate translation
                            var translation = orderStatusTranslations.FirstOrDefault(y => 
                                y.OrderStatusId == statusChange.OrderItemStatusId && 
                                y.Language == language && 
                                y.IsActive);
                                
                            if (translation == null)
                            {
                                translation = orderStatusTranslations.FirstOrDefault(y => 
                                    y.OrderStatusId == statusChange.OrderItemStatusId && 
                                    y.IsActive);
                            }
                            
                            orderItemModel.OrderItemStatusName = translation?.Name;
                        }

                        if (item.LastOrderItemStatusChangeId.HasValue && 
                            orderItemStatusChangesCommentTranslations.TryGetValue(item.LastOrderItemStatusChangeId.Value, out var commentTranslation))
                        {
                            orderItemModel.OrderItemStatusChangeComment = commentTranslation.OrderItemStatusChangeComment;
                        }

                        return orderItemModel;
                    })
                    .ToList();

                return new OrderServiceModel
                {
                    Id = order.Id,
                    SellerId = order.SellerId,
                    ClientId = order.ClientId.Value,
                    ClientName = order.ClientName,
                    BillingAddressId = order.BillingAddressId,
                    BillingCity = order.BillingCity,
                    BillingCompany = order.BillingCompany,
                    BillingCountryId = order.BillingCountryId,
                    BillingFirstName = order.BillingFirstName,
                    BillingLastName = order.BillingLastName,
                    BillingPhoneNumber = order.BillingPhoneNumber,
                    BillingPostCode = order.BillingPostCode,
                    BillingRegion = order.BillingRegion,
                    BillingStreet = order.BillingStreet,
                    ShippingAddressId = order.ShippingAddressId,
                    ShippingCity = order.ShippingCity,
                    ShippingCompany = order.ShippingCompany,
                    ShippingCountryId = order.ShippingCountryId,
                    ShippingFirstName = order.ShippingFirstName,
                    ShippingLastName = order.ShippingLastName,
                    ShippingPhoneNumber = order.ShippingPhoneNumber,
                    ShippingPostCode = order.ShippingPostCode,
                    ShippingRegion = order.ShippingRegion,
                    ShippingStreet = order.ShippingStreet,
                    ExternalReference = order.ExternalReference,
                    MoreInfo = order.MoreInfo,
                    Reason = order.Reason,
                    OrderStateId = order.OrderStateId,
                    OrderStatusId = order.OrderStatusId,
                    OrderStatusName = orderStatusTranslations.FirstOrDefault(y => 
                        y.OrderStatusId == order.OrderStatusId && 
                        y.Language == language)?.Name ?? 
                        orderStatusTranslations.FirstOrDefault(y => 
                            y.OrderStatusId == order.OrderStatusId)?.Name,
                    OrderItems = orderItemsModels,
                    LastModifiedDate = order.LastModifiedDate,
                    CreatedDate = order.CreatedDate
                };
            }).ToList();

            return new PagedResults<IEnumerable<OrderServiceModel>>(pagedResults.Total, pagedResults.PageSize)
            {
                Data = resultData
            };
        }

        private async Task MapStatusesToOrderStatusId(Guid? orderId)
        {
            // Get the order with its items in a single query
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(x => x.Id == orderId && x.IsActive);

            if (order is not null)
            {
                var orderItemIds = order.OrderItems.Select(x => x.Id).ToList();
                var orderItemsLastStatusChangeIds = order.OrderItems
                    .Where(x => x.LastOrderItemStatusChangeId.HasValue)
                    .Select(x => x.LastOrderItemStatusChangeId.Value)
                    .ToList();

                // Get all status changes in a single query
                var statusChanges = await _context.OrderItemStatusChanges
                    .AsNoTracking()
                    .Where(x => orderItemsLastStatusChangeIds.Contains(x.Id) && x.IsActive)
                    .ToListAsync();

                var lastOrderItemStatusChanges = new List<OrderItemStatusServiceModel>();

                foreach (var statusChange in statusChanges)
                {
                    lastOrderItemStatusChanges.Add(new OrderItemStatusServiceModel
                    {
                        OrderItemStateId = statusChange.OrderItemStateId,
                        OrderItemStatusId = statusChange.OrderItemStatusId
                    });
                }

                if (lastOrderItemStatusChanges.Any())
                {
                    bool isSameStatus = lastOrderItemStatusChanges.DistinctBy(x => x.OrderItemStatusId).Count() == 1;
                    var lastOrderItemStatus = lastOrderItemStatusChanges.FirstOrDefault();

                    if (isSameStatus && lastOrderItemStatus != null)
                    {
                        order.OrderStatusId = lastOrderItemStatus.OrderItemStatusId;
                        order.OrderStateId = lastOrderItemStatus.OrderItemStateId;
                        order.LastModifiedDate = DateTime.UtcNow;
                    }
                    else if (lastOrderItemStatus != null && 
                             lastOrderItemStatus.OrderItemStatusId != OrderStatusesConstants.CancelledId)
                    {
                        order.OrderStatusId = OrderStatusesConstants.ProcessingId;
                        order.OrderStateId = OrderStatesConstants.ProcessingId;
                        order.LastModifiedDate = DateTime.UtcNow;
                    }

                    await _context.SaveChangesAsync();
                }
            }
        }

        private bool CanSend(params string[] values)
        {
            return values.Any(string.IsNullOrWhiteSpace) is false;
        }
    }
}
