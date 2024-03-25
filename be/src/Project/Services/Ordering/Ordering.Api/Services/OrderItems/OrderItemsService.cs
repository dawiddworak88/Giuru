using Foundation.Extensions.Exceptions;
using Foundation.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Ordering.Api.Infrastructure;
using Ordering.Api.ServicesModels;
using System.Net;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace Ordering.Api.Services.OrderItems
{
    public class OrderItemsService : IOrderItemsService
    {
        private readonly OrderingContext _context;
        private readonly IStringLocalizer<OrderResources> _orderLocalizer;

        public OrderItemsService(
            OrderingContext context, 
            IStringLocalizer<OrderResources> orderLocalizer)
        {
            _context = context;
            _orderLocalizer = orderLocalizer;
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

        public Task UpdateOrderItemStatusAsync(UpdateOrderItemStatusServiceModel model)
        {
            throw new System.NotImplementedException();
        }
    }
}
