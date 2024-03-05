using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Extensions;
using Foundation.Localization;
using Google.Protobuf.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Ordering.Api.Infrastructure;
using Ordering.Api.Infrastructure.Attributes.Entities;
using Ordering.Api.ServicesModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Ordering.Api.Services.OrderAttributes
{
    public class OrderAttributesService : IOrderAttributesService
    {
        private readonly OrderingContext _context;
        private readonly IStringLocalizer<OrderResources> _orderLocalizer;

        public OrderAttributesService(
            OrderingContext context, 
            IStringLocalizer<OrderResources> orderLocalizer)
        {
            _context = context;
            _orderLocalizer = orderLocalizer;
        }

        public async Task<Guid> CreateAsync(CreateOrderAttributeServiceModel model)
        {
            var orderAttribute = new Infrastructure.Attributes.Entities.Attribute
            {
                Type = model.Type,
                IsRequired = model.IsRequired,
                IsOrderItemAttribute = model.IsOrderItemAttribute
            };

            await _context.Attributes.AddAsync(orderAttribute.FillCommonProperties());

            var orderAttributeTranslation = new AttributeTranslation
            {
                AttributeId = orderAttribute.Id,
                Name = model.Name,
                Language = model.Language
            };

            await _context.AttributeTranslations.AddAsync(orderAttributeTranslation.FillCommonProperties());
            await _context.SaveChangesAsync();

            return orderAttribute.Id;
        }

        public async Task DeleteAsync(DeleteOrderAttributeServiceModel model)
        {
            var orderAttribute = await _context.Attributes.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (orderAttribute is null)
            {
                throw new CustomException(_orderLocalizer.GetString("OrderAttributeNotFound"), (int)HttpStatusCode.NoContent);
            } 
            else if (_context.AttributeValues.Any(x => x.AttributeId == orderAttribute.Id) is true)
            {
                throw new CustomException(_orderLocalizer.GetString("DeleteOrderAttributeConflict"), (int)HttpStatusCode.NoContent);
            }

            orderAttribute.IsActive = false;
            orderAttribute.LastModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task<OrderAttributeServiceModel> GetAsync(GetOrderAttributeServiceModel model)
        {
            var orderAttribute = await _context.Attributes
                .Include(x => x.AttributeTranslations)
                .AsSingleQuery()
                .FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (orderAttribute is null)
            {
                throw new CustomException(_orderLocalizer.GetString("OrderAttributeNotFound"), (int)HttpStatusCode.NoContent);
            }

            var orderAttributeTranslation = orderAttribute.AttributeTranslations.FirstOrDefault(x => x.Language == model.Language && x.IsActive);

            if (orderAttributeTranslation is null)
            {
                orderAttributeTranslation = orderAttribute.AttributeTranslations.FirstOrDefault(x => x.IsActive);
            }

            var orderAttributeOptions = _context.AttributeOptions.Where(x => x.AttributeOptionSetId == orderAttribute.AttributeOptionSetId).ToList();
            var orderAttributeOptionsTranslations = _context.AttributeOptionTranslations.Where(x => orderAttributeOptions.Select(y => y.Id).Contains(x.AttributeOptionId)).ToList();

            var attributeOptions = new List<OrderAttributeOptionServiceModel>();

            foreach ( var option in orderAttributeOptions.OrEmptyIfNull())
            {
                var orderAttributeOption = new OrderAttributeOptionServiceModel
                {
                    OrderAttributeOptionSetId = option.AttributeOptionSetId,
                    Name = orderAttributeOptionsTranslations?.FirstOrDefault(x => x.Language == model.Language && x.AttributeOptionId == option.Id)?.Name ?? orderAttributeOptionsTranslations?.FirstOrDefault(x => x.AttributeOptionId == option.Id)?.Name,
                    Value = option.Id
                };

                attributeOptions.Add(orderAttributeOption);
            }

            return new OrderAttributeServiceModel
            {
                Id = orderAttribute.Id,
                Name = orderAttributeTranslation.Name,
                Type = orderAttribute.Type,
                IsRequired = orderAttribute.IsRequired,
                OrderAttributeOptions = attributeOptions,
                LastModifiedDate = orderAttribute.LastModifiedDate,
                CreatedDate = orderAttribute.CreatedDate
            };
        }

        public async Task<Guid> UpdateAsync(UpdateOrderAttributeServiceModel model)
        {
            var orderAttribute = await _context.Attributes
                .Include(x => x.AttributeTranslations)
                .AsSingleQuery()
                .FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (orderAttribute is null )
            {
                throw new CustomException(_orderLocalizer.GetString("OrderAttributeNotFound"), (int)HttpStatusCode.NoContent);
            }

            var orderAttributeTranslation = orderAttribute.AttributeTranslations.FirstOrDefault(x => x.AttributeId == orderAttribute.Id && x.Language == model.Language && x.IsActive);

            if (orderAttributeTranslation is not null)
            {
                orderAttributeTranslation.Name = model.Name;
                orderAttributeTranslation.LastModifiedDate = DateTime.UtcNow;
            }
            else
            {
                var newOrderAttributeTranslation = new AttributeTranslation
                {
                    AttributeId = orderAttribute.Id,
                    Name = model.Name,
                    Language = model.Language
                };

                await _context.AttributeTranslations.AddAsync(newOrderAttributeTranslation.FillCommonProperties());
            }

            orderAttribute.IsRequired = model.IsRequired;
            orderAttribute.Type = model.Type;
            orderAttribute.LastModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return orderAttribute.Id;
        }
    }
}
