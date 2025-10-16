using Catalog.Api.IntegrationEvents;
using Catalog.Api.ServicesModels.ProductAttributes;
using Foundation.Catalog.Infrastructure;
using Foundation.Catalog.Infrastructure.ProductAttributes.Entities;
using Foundation.EventBus.Abstractions;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Definitions;
using Foundation.GenericRepository.Extensions;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Api.Services.ProductAttributes
{
    public class ProductAttributesService : IProductAttributesService
    {
        private readonly IEventBus _eventBus;
        private readonly CatalogContext _context;
        private readonly IStringLocalizer<ProductResources> _productLocalizer;

        public ProductAttributesService(
            IEventBus eventBus,
            CatalogContext context,
            IStringLocalizer<ProductResources> productLocalizer)
        {
            _eventBus = eventBus;
            _context = context;
            _productLocalizer = productLocalizer;
        }

        public async Task<PagedResults<IEnumerable<ProductAttributeServiceModel>>> GetAsync(GetProductAttributesServiceModel model)
        {
            var productAttributes = _context.ProductAttributes.Where(x => x.IsActive);

            if (!string.IsNullOrWhiteSpace(model.SearchTerm))
            {
                productAttributes = productAttributes.Where(x => x.ProductAttributeTranslations.Any(x => x.Name.StartsWith(model.SearchTerm) && x.IsActive));
            }

            productAttributes = productAttributes.ApplySort(model.OrderBy);

            PagedResults<IEnumerable<ProductAttribute>> pagedProductAttributes;

            if (model.PageIndex.HasValue is false || model.ItemsPerPage.HasValue is false)
            {
                productAttributes = productAttributes.Take(Constants.MaxItemsPerPageLimit);

                pagedProductAttributes = productAttributes.PagedIndex(new Pagination(productAttributes.Count(), Constants.MaxItemsPerPageLimit), Constants.DefaultPageIndex);
            }
            else
            {
                pagedProductAttributes = productAttributes.PagedIndex(new Pagination(productAttributes.Count(), model.ItemsPerPage.Value), model.PageIndex.Value);
            }

            var pagedProductAttributeServiceModels = new PagedResults<IEnumerable<ProductAttributeServiceModel>>(pagedProductAttributes.Total, pagedProductAttributes.PageSize);

            var productAttributeServiceModels = new List<ProductAttributeServiceModel>();

            foreach (var pagedProductAttribute in pagedProductAttributes.Data.ToList())
            {
                var productAttributeServiceModel = new ProductAttributeServiceModel
                {
                    Id = pagedProductAttribute.Id,
                    Order = pagedProductAttribute.Order,
                    LastModifiedDate = pagedProductAttribute.LastModifiedDate,
                    CreatedDate = pagedProductAttribute.CreatedDate
                };

                var productAttributeTranslations = pagedProductAttribute.ProductAttributeTranslations.FirstOrDefault(x => x.Language == model.Language && x.IsActive);

                if (productAttributeTranslations == null)
                {
                    productAttributeTranslations = pagedProductAttribute.ProductAttributeTranslations.FirstOrDefault(x => x.IsActive);
                }

                productAttributeServiceModel.Name = productAttributeTranslations?.Name;

                productAttributeServiceModels.Add(productAttributeServiceModel);
            }

            pagedProductAttributeServiceModels.Data = productAttributeServiceModels;

            return pagedProductAttributeServiceModels;
        }

        public async Task<ProductAttributeServiceModel> CreateProductAttributeAsync(CreateUpdateProductAttributeServiceModel model)
        {
            var productAttribute = new ProductAttribute
            { 
                Order = model.Order,
                SellerId = model.OrganisationId.Value
            };

            _context.ProductAttributes.Add(productAttribute.FillCommonProperties());

            var productAttributeTranslation = new ProductAttributeTranslation
            {
                ProductAttributeId = productAttribute.Id,
                Name = model.Name,
                Language = model.Language
            };

            _context.ProductAttributeTranslations.Add(productAttributeTranslation.FillCommonProperties());

            await _context.SaveChangesAsync();

            RebuildCategorySchemas(
                model.OrganisationId.Value,
                model.Language,
                model.Username);

            return await GetProductAttributeByIdAsync(new GetProductAttributeByIdServiceModel
            { 
                Id = productAttribute.Id,
                Language = model.Language,
                OrganisationId = model.OrganisationId,
                Username = model.Username
            });
        }

        public async Task<ProductAttributeItemServiceModel> CreateProductAttributeItemAsync(CreateUpdateProductAttributeItemServiceModel model)
        {
            var existingProductItemAttribute = _context.ProductAttributeItems.FirstOrDefault(x => x.ProductAttributeItemTranslations.Any(y => y.Name == model.Name) && x.ProductAttributeId == model.ProductAttributeId && x.SellerId == model.OrganisationId && x.IsActive);

            if (existingProductItemAttribute != null)
            {
                throw new ConflictException(_productLocalizer.GetString("ProductAttributeItemConflict"));
            }

            var productAttributeItem = new ProductAttributeItem
            {
                ProductAttributeId = model.ProductAttributeId.Value,
                Order = model.Order,
                SellerId = model.OrganisationId.Value
            };

            _context.ProductAttributeItems.Add(productAttributeItem.FillCommonProperties());

            var productAttributeItemTranslation = new ProductAttributeItemTranslation
            {
                ProductAttributeItemId = productAttributeItem.Id,
                Name = model.Name,
                Language = model.Language
            };

            _context.ProductAttributeItemTranslations.Add(productAttributeItemTranslation.FillCommonProperties());

            await _context.SaveChangesAsync();

            RebuildCategorySchemas(
                model.OrganisationId.Value,
                model.Language,
                model.Username);

            return await GetProductAttributeItemByIdAsync(new GetProductAttributeItemByIdServiceModel
            { 
                Id = productAttributeItem.Id,
                Language = model.Language,
                OrganisationId = model.OrganisationId,
                Username = model.Username
            });
        }

        public async Task DeleteProductAttributeAsync(DeleteProductAttributeServiceModel model)
        {
            var existingProductAttribute = _context.ProductAttributes.FirstOrDefault(x => x.Id == model.Id.Value && x.SellerId == model.OrganisationId && x.IsActive);

            if (existingProductAttribute == null)
            {
                throw new NotFoundException(_productLocalizer.GetString("ProductAttributeNotFound"));
            }

            if (existingProductAttribute.ProductAttributeItems.Any(x => x.IsActive))
            {
                throw new ConflictException(_productLocalizer.GetString("ProductAttributeNotEmpty"));
            }

            AssertCategorySchemaReference(existingProductAttribute, model.Language);

            existingProductAttribute.IsActive = false;

            await _context.SaveChangesAsync();

            RebuildCategorySchemas(
                model.OrganisationId.Value,
                model.Language,
                model.Username);
        }

        public async Task DeleteProductAttributeItemAsync(DeleteProductAttributeItemServiceModel model)
        {
            var existingProductAttributeItem = _context.ProductAttributeItems.FirstOrDefault(x => x.Id == model.Id && x.SellerId == model.OrganisationId && x.IsActive);

            if (existingProductAttributeItem == null)
            {
                throw new NotFoundException(_productLocalizer.GetString("ProductAttributeItemNotFound"));
            }

            AssertCategorySchemaReference(existingProductAttributeItem, model.Language);

            existingProductAttributeItem.IsActive = false;

            await _context.SaveChangesAsync();

            RebuildCategorySchemas(
                model.OrganisationId.Value,
                model.Language,
                model.Username);
        }

        public async Task<ProductAttributeServiceModel> GetProductAttributeByIdAsync(GetProductAttributeByIdServiceModel model)
        {
            var productAttribute = await _context.ProductAttributes.FirstOrDefaultAsync(x => x.Id == model.Id && x.SellerId == model.OrganisationId && x.IsActive);

            if (productAttribute != null)
            {
                var productAttributeServiceModel = new ProductAttributeServiceModel
                {
                    Id = productAttribute.Id,
                    Order = productAttribute.Order,
                    LastModifiedDate = productAttribute.LastModifiedDate,
                    CreatedDate = productAttribute.CreatedDate
                };

                var productAttributeTranslations = productAttribute.ProductAttributeTranslations.FirstOrDefault(x => x.Language == model.Language && x.IsActive);

                if (productAttributeTranslations == null)
                {
                    productAttributeTranslations = productAttribute.ProductAttributeTranslations.FirstOrDefault(x => x.IsActive);
                }

                productAttributeServiceModel.Name = productAttributeTranslations?.Name;

                var productAttributeItems = await _context.ProductAttributeItems.Where(x => x.ProductAttributeId == productAttribute.Id && x.IsActive).ToListAsync();

                if (productAttributeItems != null)
                {
                    var productAttributeItemServiceModels = new List<ProductAttributeItemServiceModel>();

                    foreach (var productAttributeItem in productAttributeItems)
                    {
                        var productAttributeItemServiceModel = new ProductAttributeItemServiceModel
                        { 
                            Id = productAttributeItem.Id,
                            ProductAttributeId = productAttributeItem.ProductAttributeId,
                            Order = productAttributeItem.Order,
                            LastModifiedDate = productAttributeItem.LastModifiedDate,
                            CreatedDate = productAttributeItem.CreatedDate
                        };

                        var productAttributeItemTranslations = await _context.ProductAttributeItemTranslations.FirstOrDefaultAsync(x => x.ProductAttributeItemId == productAttributeItem.Id && x.Language == model.Language && x.IsActive);

                        if (productAttributeItemTranslations == null)
                        {
                            productAttributeItemTranslations = await _context.ProductAttributeItemTranslations.FirstOrDefaultAsync(x => x.ProductAttributeItemId == productAttributeItem.Id && x.IsActive);
                        }

                        productAttributeItemServiceModel.Name = productAttributeItemTranslations?.Name;

                        productAttributeItemServiceModels.Add(productAttributeItemServiceModel);
                    }

                    productAttributeServiceModel.ProductAttributeItems = productAttributeItemServiceModels;
                }

                return productAttributeServiceModel;
            }

            return default;
        }

        public async Task<ProductAttributeItemServiceModel> GetProductAttributeItemByIdAsync(GetProductAttributeItemByIdServiceModel model)
        {
            var productAttributeItem = await _context.ProductAttributeItems.FirstOrDefaultAsync(x => x.Id == model.Id && x.SellerId == model.OrganisationId && x.IsActive);

            if (productAttributeItem != null)
            {
                var productAttributeItemServiceModel = new ProductAttributeItemServiceModel
                {
                    Id = productAttributeItem.Id,
                    ProductAttributeId = productAttributeItem.ProductAttributeId,
                    Order = productAttributeItem.Order,
                    LastModifiedDate = productAttributeItem.LastModifiedDate,
                    CreatedDate = productAttributeItem.CreatedDate
                };

                var productAttributeTranslations = _context.ProductAttributeItemTranslations.FirstOrDefault(x => x.ProductAttributeItemId ==productAttributeItem.Id && x.Language == model.Language && x.IsActive);

                if (productAttributeTranslations == null)
                {
                    productAttributeTranslations = _context.ProductAttributeItemTranslations.FirstOrDefault(x => x.ProductAttributeItemId == productAttributeItem.Id && x.IsActive);
                }

                productAttributeItemServiceModel.Name = productAttributeTranslations?.Name;
                
                return productAttributeItemServiceModel;
            }

            return default;
        }

        public async Task<ProductAttributeServiceModel> UpdateProductAttributeAsync(CreateUpdateProductAttributeServiceModel model)
        {
            var productAttribute = await _context.ProductAttributes.FirstOrDefaultAsync(x => x.Id == model.Id.Value && x.SellerId == model.OrganisationId.Value && x.IsActive);

            if (productAttribute != null)
            {
                productAttribute.Order = model.Order;

                var productAttributeTranslation = productAttribute.ProductAttributeTranslations.FirstOrDefault(x => x.Language == model.Language && x.IsActive);

                if (productAttributeTranslation != null)
                {
                    productAttributeTranslation.Name = model.Name;
                }
                else
                {
                    var newProductAttributeTranslation = new ProductAttributeTranslation
                    {
                        ProductAttributeId = productAttribute.Id,
                        Language = model.Language,
                        Name = model.Name
                    };

                    _context.ProductAttributeTranslations.Add(newProductAttributeTranslation.FillCommonProperties());
                }

                await _context.SaveChangesAsync();
            }

            RebuildCategorySchemas(
                model.OrganisationId.Value,
                model.Language,
                model.Username);

            return await GetProductAttributeByIdAsync(new GetProductAttributeByIdServiceModel 
            {
                Id = model.Id,
                OrganisationId = model.OrganisationId,
                Language = model.Language,
                Username = model.Username
            });
        }

        public async Task<ProductAttributeItemServiceModel> UpdateProductAttributeItemAsync(CreateUpdateProductAttributeItemServiceModel model)
        {
            var productAttributeItem = await _context.ProductAttributeItems.FirstOrDefaultAsync(x => x.Id == model.Id.Value && x.SellerId == model.OrganisationId.Value && x.IsActive);

            if (productAttributeItem != null)
            {
                productAttributeItem.Order = model.Order;

                var productAttributeTranslation = productAttributeItem.ProductAttributeItemTranslations.FirstOrDefault(x => x.Language == model.Language && x.IsActive);

                if (productAttributeTranslation != null)
                {
                    productAttributeTranslation.Name = model.Name;
                }
                else
                {
                    var newProductAttributeItemTranslation = new ProductAttributeItemTranslation
                    {
                        ProductAttributeItemId = productAttributeItem.Id,
                        Language = model.Language,
                        Name = model.Name
                    };

                    _context.ProductAttributeItemTranslations.Add(newProductAttributeItemTranslation.FillCommonProperties());
                }

                await _context.SaveChangesAsync();
            }

            RebuildCategorySchemas(
                model.OrganisationId.Value,
                model.Language,
                model.Username);

            return await GetProductAttributeItemByIdAsync(new GetProductAttributeItemByIdServiceModel
            {
                Id = model.Id,
                OrganisationId = model.OrganisationId,
                Language = model.Language,
                Username = model.Username
            });
        }

        public async Task<PagedResults<IEnumerable<ProductAttributeItemServiceModel>>> GetProductAttributeItemsAsync(GetProductAttributeItemsServiceModel model)
        {
            var productAttributesItems = _context.ProductAttributeItems.Where(x => x.ProductAttributeId == model.ProductAttributeId && x.IsActive);

            if (!string.IsNullOrWhiteSpace(model.SearchTerm))
            {
                productAttributesItems = productAttributesItems.Where(x => x.ProductAttributeItemTranslations.Any(y => y.Name.StartsWith(model.SearchTerm)));
            }

            productAttributesItems = productAttributesItems.ApplySort(model.OrderBy);

            PagedResults<IEnumerable<ProductAttributeItem>> pagedProductAttributeItems;

            if (model.PageIndex.HasValue is false || model.ItemsPerPage.HasValue is false)
            {
                productAttributesItems = productAttributesItems.Take(Constants.MaxItemsPerPageLimit);

                pagedProductAttributeItems = productAttributesItems.PagedIndex(new Pagination(productAttributesItems.Count(), Constants.MaxItemsPerPageLimit), Constants.DefaultPageIndex);
            }
            else
            {
                pagedProductAttributeItems = productAttributesItems.PagedIndex(new Pagination(productAttributesItems.Count(), model.ItemsPerPage.Value), model.PageIndex.Value);
            }

            var pagedProductAttributeItemServiceModels = new PagedResults<IEnumerable<ProductAttributeItemServiceModel>>(pagedProductAttributeItems.Total, pagedProductAttributeItems.PageSize);

            var productAttributeItemServiceModels = new List<ProductAttributeItemServiceModel>();

            foreach (var pagedProductAttributeItem in pagedProductAttributeItems.Data.ToList())
            {
                var productAttributeItemServiceModel = new ProductAttributeItemServiceModel
                {
                    Id = pagedProductAttributeItem.Id,
                    ProductAttributeId = pagedProductAttributeItem.ProductAttributeId,
                    Order = pagedProductAttributeItem.Order,
                    LastModifiedDate = pagedProductAttributeItem.LastModifiedDate,
                    CreatedDate = pagedProductAttributeItem.CreatedDate
                };

                var productAttributeItemTranslations = _context.ProductAttributeItemTranslations.FirstOrDefault(x => x.ProductAttributeItemId == pagedProductAttributeItem.Id &&  x.Language == model.Language && x.IsActive);

                if (productAttributeItemTranslations == null)
                {
                    productAttributeItemTranslations = _context.ProductAttributeItemTranslations.FirstOrDefault(x => x.ProductAttributeItemId == pagedProductAttributeItem.Id && x.IsActive);
                }

                productAttributeItemServiceModel.Name = productAttributeItemTranslations?.Name;

                productAttributeItemServiceModels.Add(productAttributeItemServiceModel);
            }

            pagedProductAttributeItemServiceModels.Data = productAttributeItemServiceModels;

            return pagedProductAttributeItemServiceModels;
        }

        private void AssertCategorySchemaReference(ProductAttribute existingProductAttribute, string language)
        {
            var attributeRef = $"#/definitions/{existingProductAttribute.Id}";

            var categories = _context.Database.SqlQueryRaw<LocalizedCategorySchema>(
              @"SELECT ct.CategoryId, ct.Name, ct.Language
              FROM CategorySchemas cs
              INNER JOIN CategoryTranslations ct ON cs.CategoryId = ct.CategoryId
              INNER JOIN Categories c ON c.Id = ct.CategoryId
              CROSS APPLY OPENJSON([Schema], '$.properties') AS properties
              WHERE c.IsActive AND ISJSON(cs.[Schema]) > 0 AND JSON_VALUE(properties.value, '$.""$ref""') = @AttributeRef",
                    new SqlParameter("@AttributeRef", attributeRef))
                .AsNoTracking()
                .ToList();

            HandleCategorySchemaReferences(language, "ProductAttributeInUseByCategories", categories);
        }

        private void AssertCategorySchemaReference(ProductAttributeItem existingProductAttributeItem, string language)
        {
            var valuesList = existingProductAttributeItem.ProductAttributeItemTranslations
                .Where(x => x.IsActive)
                .Select(x => x.Name).ToList();

            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@attributeId", existingProductAttributeItem.ProductAttributeId)
            };

            for (int i = 0; i < valuesList.Count; i++)
            {
                parameters.Add(new SqlParameter($"@value{i}", valuesList[i]));
            }

            string inClause = string.Join(", ", valuesList.Select((_, i) => $"@value{i}"));

            string query = $@"
                SELECT ct.CategoryId, ct.Name, ct.Language
                FROM CategorySchemas cs
                INNER JOIN CategoryTranslations ct ON cs.CategoryId = ct.CategoryId
                INNER JOIN Categories c ON c.Id = ct.CategoryId
                CROSS APPLY OPENJSON(cs.[Schema], '$.definitions') AS def
                WHERE ISJSON(cs.[Schema]) > 0 AND def.[key] = @attributeId
                  AND EXISTS (
                    SELECT 1
                    FROM OPENJSON(def.[value], '$.anyOf') AS anyOfItems
                    WHERE JSON_VALUE(anyOfItems.[value], '$.title') IN ({inClause}))";

            var categories = _context.Database.SqlQueryRaw<LocalizedCategorySchema>(query, parameters.ToArray()).ToList();

            HandleCategorySchemaReferences(language, "ProductAttributeInUseByCategories", categories);
        }

        private void HandleCategorySchemaReferences(
            string language,
            string localizationKey,
            IEnumerable<LocalizedCategorySchema> categories)
        {
            if (categories.Any())
            {
                var categoryNames = categories
                    .GroupBy(c => c.CategoryId)
                    .Select(group =>
                    {
                        var localizedCategory = group.FirstOrDefault(c => c.Language == language);
                        var categoryName = localizedCategory?.Name ?? group.First().Name;
                        var languages = string.Join(", ", group.Select(c => c.Language).Distinct());
                        return $"{categoryName} ({languages})";
                    });

                var message = $"{_productLocalizer.GetString(localizationKey)} {string.Join(", ", categoryNames)}";

                throw new ConflictException(message);
            }
        }

        private void RebuildCategorySchemas(
            Guid? organisationId,
            string language,
            string username)
        {
            using var source = new ActivitySource(GetType().Name);
            
            var message = new RebuildCategorySchemasIntegrationEvent
            {
                OrganisationId = organisationId,
                Language = language,
                Username = username
            };

            using var activity = source.StartActivity($"{System.Reflection.MethodBase.GetCurrentMethod().Name} {message.GetType().Name}");
            _eventBus.Publish(message);
        }

        public class LocalizedCategorySchema
        {
            public Guid CategoryId { get; set; }
            public string Name { get; set; }
            public string Language { get; set; }
        }
    }
}
