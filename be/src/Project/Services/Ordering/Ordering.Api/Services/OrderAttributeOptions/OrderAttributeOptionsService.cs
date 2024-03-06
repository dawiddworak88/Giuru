using Foundation.Extensions.Exceptions;
using Foundation.GenericRepository.Extensions;
using Foundation.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Ordering.Api.Infrastructure;
using Ordering.Api.Infrastructure.Attributes.Entities;
using Ordering.Api.ServicesModels;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Ordering.Api.Services.OrderAttributeOptions
{
    public class OrderAttributeOptionsService : IOrderAttributeOptionsService
    {
        private readonly OrderingContext _context;
        private readonly IStringLocalizer<OrderResources> _orderLocalizer;

        public OrderAttributeOptionsService(
            OrderingContext context, 
            IStringLocalizer<OrderResources> orderLocalizer)
        {
            _context = context;
            _orderLocalizer = orderLocalizer;
        }

        public async Task<Guid> CreateAsync(CreateOrderAttributeOptionServiceModel model)
        {
            var orderAttribute = await _context.Attributes.FirstOrDefaultAsync(x => x.Id == model.OrderAttributeId && x.IsActive);

            if (orderAttribute is null)
            {
                throw new CustomException(_orderLocalizer.GetString("OrderAttributeNotFound"), (int)HttpStatusCode.NoContent);
            }
            else if (orderAttribute.Type != "select")
            {
                throw new CustomException(_orderLocalizer.GetString("OrderAttributeSelectTypeConflict"), (int)HttpStatusCode.Conflict);
            }

            var orderAttributeOptionSetId = orderAttribute.AttributeOptionSetId;

            if (orderAttribute.AttributeOptionSetId.HasValue is false)
            {
                var orderAttributeOptionSet = new AttributeOptionSet();

                await _context.AttributeOptionSets.AddAsync(orderAttributeOptionSet.FillCommonProperties());

                orderAttributeOptionSetId = orderAttributeOptionSet.Id;
                orderAttribute.AttributeOptionSetId = orderAttributeOptionSet.Id;
            }

            var orderAttributeOption = new AttributeOption
            {
                AttributeOptionSetId = orderAttributeOptionSetId.Value
            };

            await _context.AttributeOptions.AddAsync(orderAttributeOption.FillCommonProperties());

            var orderAttributeOptionTranslation = new AttributeOptionTranslation
            {
                AttributeOptionId = orderAttributeOption.Id,
                Name = model.Name,
                Language = model.Language
            };

            await _context.AttributeOptionTranslations.AddAsync(orderAttributeOptionTranslation.FillCommonProperties());
            await _context.SaveChangesAsync();

            return orderAttributeOption.Id;
        }

        public async Task DeleteAsync(DeleteOrderAttributeOptionServiceModel model)
        {
            var orderAttributeOption = await _context.AttributeOptions.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (orderAttributeOption is null)
            {
                throw new CustomException(_orderLocalizer.GetString("FieldOptionNotFound"), (int)HttpStatusCode.NoContent);
            }

            orderAttributeOption.IsActive = false;
            orderAttributeOption.LastModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task<OrderAttributeOptionServiceModel> GetAsync(GetOrderAttributeOptionServiceModel model)
        {
            var orderAttributeOption = await _context.AttributeOptions
                .Include(x => x.AttributeOptionTranslations)
                .AsSingleQuery()
                .FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if ( orderAttributeOption is null)
            {
                throw new CustomException(_orderLocalizer.GetString("OrderAttributeOptionNotFound"), (int)HttpStatusCode.NoContent);
            }

            var orderAttribute = await _context.Attributes.FirstOrDefaultAsync(x => x.AttributeOptionSetId == orderAttributeOption.AttributeOptionSetId);

            return new OrderAttributeOptionServiceModel
            {
                Id = model.Id,
                Name = orderAttributeOption.AttributeOptionTranslations?.FirstOrDefault(x => x.Language == model.Language && x.IsActive)?.Name ?? orderAttributeOption.AttributeOptionTranslations.FirstOrDefault(x => x.IsActive)?.Name,
                Value = orderAttributeOption.Id,
                OrderAttributeId = orderAttribute?.Id,
                LastModifiedDate = orderAttributeOption.LastModifiedDate,
                CreatedDate = orderAttributeOption.CreatedDate
            };
        }

        public async Task<Guid> UpdateAsync(UpdateOrderAttributeOptionServiceModel model)
        {
            var orderAttributeOption = await _context.AttributeOptions
                .Include(x => x.AttributeOptionTranslations)
                .AsSingleQuery()
                .FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (orderAttributeOption is null)
            {
                throw new CustomException(_orderLocalizer.GetString("OrderAttributeOptionNotFound"), (int)HttpStatusCode.NoContent);
            }

            var orderAttributeTranslation = orderAttributeOption.AttributeOptionTranslations.FirstOrDefault(x => x.Language == model.Language && x.IsActive);

            if (orderAttributeTranslation is not null)
            {
                orderAttributeTranslation.Name = model.Name;
            }
            else
            {
                var newOrderAttributeOptionTranslation = new AttributeOptionTranslation
                {
                    AttributeOptionId = orderAttributeOption.Id,
                    Name = model.Name,
                    Language = model.Language
                };

                await _context.AttributeOptionTranslations.AddAsync(newOrderAttributeOptionTranslation.FillCommonProperties());
            }

            return orderAttributeOption.Id;
        }
    }
}
