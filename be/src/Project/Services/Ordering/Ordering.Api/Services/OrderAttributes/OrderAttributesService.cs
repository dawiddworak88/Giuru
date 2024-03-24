using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Definitions;
using Foundation.GenericRepository.Extensions;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
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
                throw new CustomException(_orderLocalizer.GetString("DeleteOrderAttributeConflict"), (int)HttpStatusCode.Conflict);
            }

            orderAttribute.IsActive = false;
            orderAttribute.LastModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public PagedResults<IEnumerable<OrderAttributeServiceModel>> Get(GetOrderAttributesServiceModel model)
        {
            var orderAttributes = _context.Attributes
                .Include(x => x.AttributeTranslations)
                .Where(x => x.IsActive)
                .AsSingleQuery();

            if (string.IsNullOrWhiteSpace(model.SearchTerm) is false)
            {
                orderAttributes = orderAttributes.Where(x => x.AttributeTranslations.Any(y => y.Name.StartsWith(model.SearchTerm)));
            }

            if (model.ForOrderItems is not null)
            {
                orderAttributes = orderAttributes.Where(x => x.IsOrderItemAttribute == model.ForOrderItems);
            }

            orderAttributes = orderAttributes.ApplySort(model.OrderBy);

            PagedResults<IEnumerable<Infrastructure.Attributes.Entities.Attribute>> pagedResults;

            if (model.PageIndex.HasValue is false || model.ItemsPerPage.HasValue is false)
            {
                orderAttributes = orderAttributes.Take(Constants.MaxItemsPerPageLimit);

                pagedResults = orderAttributes.PagedIndex(new Pagination(orderAttributes.Count(), Constants.MaxItemsPerPageLimit), Constants.DefaultPageIndex);
            }
            else
            {
                pagedResults = orderAttributes.PagedIndex(new Pagination(orderAttributes.Count(), model.ItemsPerPage.Value), model.PageIndex.Value);
            }

            var optionSetIds = pagedResults.Data.Select(x => x.AttributeOptionSetId).ToList();

            var attributeOptions = _context.AttributeOptions.Where(x => optionSetIds.Contains(x.AttributeOptionSetId)).ToList();
            var attributeOptionsTranslations = _context.AttributeOptionTranslations.Where(x => attributeOptions.Select(y => y.Id).Contains(x.AttributeOptionId)).ToList();

            return new PagedResults<IEnumerable<OrderAttributeServiceModel>>(pagedResults.Total, pagedResults.PageSize)
            {
                Data = pagedResults.Data.OrEmptyIfNull().Select(x => new OrderAttributeServiceModel
                {
                    Id = x.Id,
                    Name = x.AttributeTranslations?.FirstOrDefault(t => t.AttributeId == x.Id && t.Language == model.Language && t.IsActive)?.Name ?? x.AttributeTranslations.FirstOrDefault(t => t.AttributeId == x.Id && t.IsActive)?.Name,
                    Type = x.Type,
                    IsRequired = x.IsRequired,
                    IsOrderItemAttribute = x.IsOrderItemAttribute,
                    OrderAttributeOptions = attributeOptions.Where(y => y.AttributeOptionSetId == x.AttributeOptionSetId).Select(y => new AttributeOptionServiceModel
                    {
                        Name = attributeOptionsTranslations?.FirstOrDefault(t => t.AttributeOptionId == y.Id && t.Language == model.Language)?.Name ?? attributeOptionsTranslations?.FirstOrDefault(t => t.AttributeOptionId == y.Id)?.Name,
                        Value = y.Id
                    }),
                    LastModifiedDate = x.LastModifiedDate,
                    CreatedDate = x.CreatedDate
                })
            };
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

            return new OrderAttributeServiceModel
            {
                Id = orderAttribute.Id,
                Name = orderAttributeTranslation.Name,
                Type = orderAttribute.Type,
                IsRequired = orderAttribute.IsRequired,
                IsOrderItemAttribute = orderAttribute.IsOrderItemAttribute,
                OrderAttributeOptions = orderAttributeOptions.Select(x => new AttributeOptionServiceModel
                {
                    Name = orderAttributeOptionsTranslations.FirstOrDefault(x => x.AttributeOptionId == x.Id && x.Language == model.Language && x.IsActive)?.Name ?? orderAttributeOptionsTranslations?.FirstOrDefault(x => x.AttributeOptionId == x.Id && x.IsActive)?.Name,
                    Value = x.Id
                }),
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

            orderAttribute.Type = model.Type;
            orderAttribute.IsRequired = model.IsRequired;
            orderAttribute.IsOrderItemAttribute = model.IsOrderItemAttribute;
            orderAttribute.LastModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return orderAttribute.Id;
        }
    }
}
