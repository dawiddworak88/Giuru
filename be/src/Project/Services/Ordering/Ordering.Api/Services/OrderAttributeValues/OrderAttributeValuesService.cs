using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Definitions;
using Foundation.GenericRepository.Extensions;
using Foundation.GenericRepository.Paginations;
using Microsoft.EntityFrameworkCore;
using Ordering.Api.Infrastructure;
using Ordering.Api.Infrastructure.Attributes.Entities;
using Ordering.Api.ServicesModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.Api.Services.OrderAttributeValues
{
    public class OrderAttributeValuesService : IOrderAttributeValuesService
    {
        private readonly OrderingContext _context;

        public OrderAttributeValuesService(
            OrderingContext context)
        {
            _context = context;
        }

        public async Task BatchAsync(CreateBatchOrderAttributeValuesServiceModel model)
        {
            var attributeValues = _context.AttributeValues
                .Include(x => x.AttributeValueTranslations)
                .Where(x => x.OrderId == model.OrderId && model.Values.Select(y => y.AttributeId).Contains(x.AttributeId))
                .AsSingleQuery()
                .ToList();

            foreach (var attributeValue in model.Values.OrEmptyIfNull())
            {
                var existingValue = attributeValues.FirstOrDefault(x => x.AttributeId == attributeValue.AttributeId);

                if (string.IsNullOrWhiteSpace(attributeValue.Value) is true && existingValue is not null)
                {
                    var existingTranslation = existingValue.AttributeValueTranslations.FirstOrDefault(x => x.Language == model.Language);

                    if (existingTranslation is not null)
                    {
                        _context.Remove(existingTranslation);
                    }

                    continue;
                }

                if (existingValue is null)
                {
                    var newValue = new AttributeValue
                    {
                        OrderId = model.OrderId.Value,
                        OrderItemId = attributeValue.OrderItemId,
                        AttributeId = attributeValue.AttributeId.Value,
                    };

                    await _context.AttributeValues.AddAsync(newValue.FillCommonProperties());

                    if (string.IsNullOrWhiteSpace(attributeValue.Value) is false)
                    {
                        var newValueTranslation = new AttributeValueTranslation
                        {
                            AttributeValueId = newValue.Id,
                            Value = attributeValue.Value,
                            Language = model.Language
                        };

                        await _context.AttributeValueTranslations.AddAsync(newValueTranslation.FillCommonProperties());
                    }
                }
                else
                {
                    var existingTranslation = existingValue.AttributeValueTranslations?.FirstOrDefault(x => x.Language == model.Language);

                    if (existingTranslation is null)
                    {
                        var newValueTranslation = new AttributeValueTranslation
                        {
                            AttributeValueId = existingValue.Id,
                            Value = attributeValue.Value,
                            Language = model.Language
                        };

                        await _context.AttributeValueTranslations.AddAsync(newValueTranslation.FillCommonProperties());
                    }
                    else
                    {
                        existingTranslation.Value = attributeValue.Value;
                        existingTranslation.LastModifiedDate = DateTime.UtcNow;
                    }
                }
            }

            await _context.SaveChangesAsync();
        }

        public PagedResults<IEnumerable<OrderAttributeValueServiceModel>> Get(GetOrderAttributeValuesServiceModel model)
        {
            var attributeValues = _context.AttributeValues
                .Include(x => x.AttributeValueTranslations)
                .Where(x => x.IsActive)
                .AsSingleQuery();

            if (model.OrderId.HasValue)
            {
                attributeValues = attributeValues.Where(x => x.OrderId == model.OrderId);
            }

            if (string.IsNullOrWhiteSpace(model.SearchTerm) is false)
            {
                attributeValues = attributeValues.Where(x => x.AttributeValueTranslations.Any(y => y.Value.StartsWith(model.SearchTerm)));
            }

            attributeValues = attributeValues.ApplySort(model.OrderBy);

            PagedResults<IEnumerable<AttributeValue>> pagedResults;

            if (model.PageIndex.HasValue is false || model.ItemsPerPage.HasValue is false)
            {
                attributeValues = attributeValues.Take(Constants.MaxItemsPerPageLimit);

                pagedResults = attributeValues.PagedIndex(new Pagination(attributeValues.Count(), Constants.MaxItemsPerPageLimit), Constants.DefaultPageIndex);
            }
            else
            {
                pagedResults = attributeValues.PagedIndex(new Pagination(attributeValues.Count(), model.ItemsPerPage.Value), model.PageIndex.Value);
            }

            return new PagedResults<IEnumerable<OrderAttributeValueServiceModel>>(pagedResults.Total, pagedResults.PageSize)
            {
                Data = pagedResults.Data.OrEmptyIfNull().Select(x => new OrderAttributeValueServiceModel
                {
                    Id = x.Id,
                    Value = x.AttributeValueTranslations?.FirstOrDefault(t => t.Language == model.Language && t.AttributeValueId == x.Id && t.IsActive)?.Value ?? x.AttributeValueTranslations?.FirstOrDefault(x => x.IsActive)?.Value,
                    AttributeId = x.AttributeId,
                    OrderItemId = x.OrderItemId,
                    LastModifiedDate = x.LastModifiedDate,
                    CreatedDate = x.CreatedDate
                })
            };
        }
    }
}
