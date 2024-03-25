using Foundation.Extensions.Exceptions;
using Foundation.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Ordering.Api.Infrastructure;
using System.Net;
using System;
using System.Threading.Tasks;
using System.Linq;
using Foundation.Extensions.ExtensionMethods;
using System.Collections.Generic;
using Ordering.Api.Infrastructure.Orders.Entities;
using Foundation.GenericRepository.Extensions;
using Ordering.Api.Services.Orders;
using Ordering.Api.ServicesModels.OrderItems;

namespace Ordering.Api.Services.OrderItems
{
    public class OrderItemsService : IOrderItemsService
    {
        private readonly OrderingContext _context;
        private readonly IStringLocalizer<OrderResources> _orderLocalizer;
        private readonly IOrdersService _ordersService;

        public OrderItemsService(
            OrderingContext context, 
            IStringLocalizer<OrderResources> orderLocalizer,
            IOrdersService ordersService)
        {
            _context = context;
            _orderLocalizer = orderLocalizer;
            _ordersService = ordersService;
        }

        public async Task<OrderItemServiceModel> GetAsync(GetOrderItemServiceModel model)
        {
            var existingOrderItem = await _context.OrderItems.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (existingOrderItem is null)
            {
                throw new CustomException(_orderLocalizer.GetString("OrderItemNotFound"), (int)HttpStatusCode.NotFound);
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
                    throw new CustomException(_orderLocalizer.GetString("LastOrderItemStatusNotFound"), (int)HttpStatusCode.NotFound);
                }

                orderItem.OrderItemStateId = lastOrderItemStatus.OrderItemStateId;
                orderItem.OrderItemStatusId = lastOrderItemStatus.OrderItemStatusId;

                var orderItemStatusTranslation = await _context.OrderStatusTranslations.FirstOrDefaultAsync(x => x.OrderStatusId == lastOrderItemStatus.OrderItemStatusId && x.Language == model.Language && x.IsActive);

                if (orderItemStatusTranslation is null)
                {
                    orderItemStatusTranslation = await _context.OrderStatusTranslations.FirstOrDefaultAsync(x => x.OrderStatusId == lastOrderItemStatus.OrderItemStatusId && x.IsActive);
                }

                var orderItemStatusChangeCommentTranslation = _context.OrderItemStatusChangesCommentTranslations.FirstOrDefault(x => x.OrderItemStatusChangeId == lastOrderItemStatus.Id && x.Language == model.Language && x.IsActive);

                if (orderItemStatusChangeCommentTranslation is null)
                {
                    orderItemStatusChangeCommentTranslation = _context.OrderItemStatusChangesCommentTranslations.FirstOrDefault(x => x.OrderItemStatusChangeId == lastOrderItemStatus.Id && x.IsActive);
                }

                orderItem.OrderItemStatusChangeComment = orderItemStatusChangeCommentTranslation?.OrderItemStatusChangeComment;
                orderItem.OrderItemStatusName = orderItemStatusTranslation?.Name;
            }

            return orderItem;
        }

        public async Task<OrderItemStatusChangesServiceModel> GetStatusChangesAsync(GetOrderItemStatusChangesServiceModel model)
        {
            var orderItem = await _context.OrderItems.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (orderItem is null)
            {
                throw new CustomException(_orderLocalizer.GetString("OrderItemNotFound"), (int)HttpStatusCode.NotFound);
            }

            var orderItemStatusesHistory = new OrderItemStatusChangesServiceModel
            {
                OrderItemId = orderItem.Id,
            };

            var orderItemStatuses = _context.OrderItemStatusChanges.Where(x => x.OrderItemId == model.Id && x.IsActive).OrderByDescending(x => x.CreatedDate).ToList();

            var orderItemStatusChanges = new List<OrderItemStatusChangeServiceModel>();

            foreach (var orderItemStatus in orderItemStatuses.OrEmptyIfNull())
            {
                var orderItemStatusChange = new OrderItemStatusChangeServiceModel
                {
                    OrderItemStateId = orderItemStatus.OrderItemStateId,
                    OrderItemStatusId = orderItemStatus.OrderItemStatusId,
                    CreatedDate = orderItemStatus.CreatedDate
                };

                var orderItemStatusTranslation = _context.OrderStatusTranslations.FirstOrDefault(x => x.OrderStatusId == orderItemStatus.OrderItemStatusId && x.Language == model.Language && x.IsActive);

                if (orderItemStatusTranslation is null)
                {
                    orderItemStatusTranslation = _context.OrderStatusTranslations.FirstOrDefault(x => x.OrderStatusId == orderItemStatus.OrderItemStatusId && x.IsActive);
                }

                orderItemStatusChange.OrderItemStatusName = orderItemStatusTranslation?.Name;

                var orderItemStatusChangeCommentTranslation = _context.OrderItemStatusChangesCommentTranslations.FirstOrDefault(x => x.OrderItemStatusChangeId == orderItemStatus.Id && x.Language == model.Language && x.IsActive);

                if (orderItemStatusChangeCommentTranslation is null)
                {
                    orderItemStatusChangeCommentTranslation = _context.OrderItemStatusChangesCommentTranslations.FirstOrDefault(x => x.OrderItemStatusChangeId == orderItemStatus.Id && x.IsActive);
                }

                orderItemStatusChange.OrderItemStatusChangeComment = orderItemStatusChangeCommentTranslation?.OrderItemStatusChangeComment;

                orderItemStatusChanges.Add(orderItemStatusChange);
            }

            orderItemStatusesHistory.OrderItemStatusChanges = orderItemStatusChanges;

            return orderItemStatusesHistory;
        }

        public async Task UpdateStatusAsync(UpdateOrderItemStatusServiceModel model)
        {
            var orderItem = await _context.OrderItems.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (orderItem is null)
            {
                throw new CustomException(_orderLocalizer.GetString("OrderItemNotFound"), (int)HttpStatusCode.NoContent);
            }

            var newOrderItemStatus = await _context.OrderStatuses.FirstOrDefaultAsync(x => x.Id == model.OrderItemStatusId && x.IsActive);

            if (newOrderItemStatus is null)
            {
                throw new CustomException(_orderLocalizer.GetString("OrderStatusNotFound"), (int)HttpStatusCode.NoContent);
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
            await _ordersService.MapStatusesToOrderStatusId(orderItem.OrderId);
        }
    }
}
