using Catalog.Api.IntegrationEvents;
using Catalog.Api.ServicesModels.ProductAttributes;
using Foundation.Catalog.Infrastructure;
using Foundation.Catalog.Infrastructure.ProductAttributes.Entities;
using Foundation.EventBus.Abstractions;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Extensions;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Catalog.Api.Services.ProductAttributes
{
    public class ProductAttributesService : IProductAttributesService
    {
        private readonly IEventBus eventBus;
        private readonly CatalogContext context;
        private readonly IStringLocalizer<ProductResources> productLocalizer;

        public ProductAttributesService(
            IEventBus eventBus,
            CatalogContext context,
            IStringLocalizer<ProductResources> productLocalizer)
        {
            this.eventBus = eventBus;
            this.context = context;
            this.productLocalizer = productLocalizer;
        }

        public async Task<PagedResults<IEnumerable<ProductAttributeServiceModel>>> GetAsync(GetProductAttributesServiceModel model)
        {
            var productAttributes = this.context.ProductAttributes.Where(x => x.IsActive);

            if (!string.IsNullOrWhiteSpace(model.SearchTerm))
            {
                productAttributes = productAttributes.Where(x => x.ProductAttributeTranslations.Any(x => x.Name.StartsWith(model.SearchTerm) && x.IsActive));
            }

            productAttributes = productAttributes.ApplySort(model.OrderBy);

            var pagedProductAttributes = productAttributes.PagedIndex(new Pagination(productAttributes.Count(), model.ItemsPerPage), model.PageIndex);

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

            this.context.ProductAttributes.Add(productAttribute.FillCommonProperties());

            var productAttributeTranslation = new ProductAttributeTranslation
            {
                ProductAttributeId = productAttribute.Id,
                Name = model.Name,
                Language = model.Language
            };

            this.context.ProductAttributeTranslations.Add(productAttributeTranslation.FillCommonProperties());

            await this.context.SaveChangesAsync();

            await this.RebuildCategorySchemasAsync(
                model.OrganisationId.Value,
                model.Language,
                model.Username);

            return await this.GetProductAttributeByIdAsync(new GetProductAttributeByIdServiceModel
            { 
                Id = productAttribute.Id,
                Language = model.Language,
                OrganisationId = model.OrganisationId,
                Username = model.Username
            });
        }

        public async Task<ProductAttributeItemServiceModel> CreateProductAttributeItemAsync(CreateUpdateProductAttributeItemServiceModel model)
        {
            var existingProductItemAttribute = this.context.ProductAttributeItems.FirstOrDefault(x => x.ProductAttributeItemTranslations.Any(y => y.Name == model.Name) && x.ProductAttributeId == model.ProductAttributeId && x.SellerId == model.OrganisationId && x.IsActive);

            if (existingProductItemAttribute != null)
            {
                throw new CustomException(this.productLocalizer.GetString("ProductAttributeItemConflict"), (int)HttpStatusCode.Conflict);
            }

            var productAttributeItem = new ProductAttributeItem
            {
                ProductAttributeId = model.ProductAttributeId.Value,
                Order = model.Order,
                SellerId = model.OrganisationId.Value
            };

            this.context.ProductAttributeItems.Add(productAttributeItem.FillCommonProperties());

            var productAttributeItemTranslation = new ProductAttributeItemTranslation
            {
                ProductAttributeItemId = productAttributeItem.Id,
                Name = model.Name,
                Language = model.Language
            };

            this.context.ProductAttributeItemTranslations.Add(productAttributeItemTranslation.FillCommonProperties());

            await this.context.SaveChangesAsync();

            await this.RebuildCategorySchemasAsync(
                model.OrganisationId.Value,
                model.Language,
                model.Username);

            return await this.GetProductAttributeItemByIdAsync(new GetProductAttributeItemByIdServiceModel
            { 
                Id = productAttributeItem.Id,
                Language = model.Language,
                OrganisationId = model.OrganisationId,
                Username = model.Username
            });
        }

        public async Task DeleteProductAttributeAsync(DeleteProductAttributeServiceModel model)
        {
            var existingProductAttribute = this.context.ProductAttributes.FirstOrDefault(x => x.Id == model.Id.Value && x.SellerId == model.OrganisationId && x.IsActive);

            if (existingProductAttribute == null)
            {
                throw new CustomException(this.productLocalizer.GetString("ProductAttributeNotFound"), (int)HttpStatusCode.NotFound);
            }

            if (existingProductAttribute.ProductAttributeItems.Any(x => x.IsActive))
            {
                throw new CustomException(this.productLocalizer.GetString("ProductAttributeNotEmpty"), (int)HttpStatusCode.NotFound);
            }

            existingProductAttribute.IsActive = false;

            await this.context.SaveChangesAsync();

            await this.RebuildCategorySchemasAsync(
                model.OrganisationId.Value,
                model.Language,
                model.Username);
        }

        public async Task DeleteProductAttributeItemAsync(DeleteProductAttributeItemServiceModel model)
        {
            var existingProductAttributeItem = this.context.ProductAttributeItems.FirstOrDefault(x => x.Id == model.Id && x.SellerId == model.OrganisationId && x.IsActive);

            if (existingProductAttributeItem == null)
            {
                throw new CustomException(this.productLocalizer.GetString("ProductAttributeItemNotFound"), (int)HttpStatusCode.NotFound);
            }

            existingProductAttributeItem.IsActive = false;

            await this.context.SaveChangesAsync();

            await this.RebuildCategorySchemasAsync(
                model.OrganisationId.Value,
                model.Language,
                model.Username);
        }

        public async Task<ProductAttributeServiceModel> GetProductAttributeByIdAsync(GetProductAttributeByIdServiceModel model)
        {
            var productAttribute = await this.context.ProductAttributes.FirstOrDefaultAsync(x => x.Id == model.Id && x.SellerId == model.OrganisationId && x.IsActive);

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

                var productAttributeItems = await this.context.ProductAttributeItems.Where(x => x.ProductAttributeId == productAttribute.Id && x.IsActive).ToListAsync();

                if (productAttributeItems != null)
                {
                    var productAttributeItemServiceModels = new List<ProductAttributeItemServiceModel>();

                    foreach (var productAttributeItem in productAttributeItems)
                    {
                        var productAttributeItemServiceModel = new ProductAttributeItemServiceModel
                        { 
                            Id = productAttributeItem.Id,
                            Order = productAttributeItem.Order,
                            LastModifiedDate = productAttributeItem.LastModifiedDate,
                            CreatedDate = productAttributeItem.CreatedDate
                        };

                        var productAttributeItemTranslations = await this.context.ProductAttributeItemTranslations.FirstOrDefaultAsync(x => x.ProductAttributeItemId == productAttributeItem.Id && x.Language == model.Language && x.IsActive);

                        if (productAttributeItemTranslations == null)
                        {
                            productAttributeItemTranslations = await this.context.ProductAttributeItemTranslations.FirstOrDefaultAsync(x => x.ProductAttributeItemId == productAttributeItem.Id && x.IsActive);
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
            var productAttributeItem = await this.context.ProductAttributeItems.FirstOrDefaultAsync(x => x.Id == model.Id && x.SellerId == model.OrganisationId && x.IsActive);

            if (productAttributeItem != null)
            {
                var productAttributeItemServiceModel = new ProductAttributeItemServiceModel
                {
                    Id = productAttributeItem.Id,
                    Order = productAttributeItem.Order,
                    LastModifiedDate = productAttributeItem.LastModifiedDate,
                    CreatedDate = productAttributeItem.CreatedDate
                };

                var productAttributeTranslations = this.context.ProductAttributeItemTranslations.FirstOrDefault(x => x.ProductAttributeItemId ==productAttributeItem.Id && x.Language == model.Language && x.IsActive);

                if (productAttributeTranslations == null)
                {
                    productAttributeTranslations = this.context.ProductAttributeItemTranslations.FirstOrDefault(x => x.ProductAttributeItemId == productAttributeItem.Id && x.IsActive);
                }

                productAttributeItemServiceModel.Name = productAttributeTranslations?.Name;
                
                return productAttributeItemServiceModel;
            }

            return default;
        }

        public async Task<ProductAttributeServiceModel> UpdateProductAttributeAsync(CreateUpdateProductAttributeServiceModel model)
        {
            var productAttribute = await this.context.ProductAttributes.FirstOrDefaultAsync(x => x.Id == model.Id.Value && x.SellerId == model.OrganisationId.Value && x.IsActive);

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

                    this.context.ProductAttributeTranslations.Add(newProductAttributeTranslation.FillCommonProperties());
                }

                await this.context.SaveChangesAsync();
            }

            await this.RebuildCategorySchemasAsync(
                model.OrganisationId.Value,
                model.Language,
                model.Username);

            return await this.GetProductAttributeByIdAsync(new GetProductAttributeByIdServiceModel 
            {
                Id = model.Id,
                OrganisationId = model.OrganisationId,
                Language = model.Language,
                Username = model.Username
            });
        }

        public async Task<ProductAttributeItemServiceModel> UpdateProductAttributeItemAsync(CreateUpdateProductAttributeItemServiceModel model)
        {
            var productAttributeItem = await this.context.ProductAttributeItems.FirstOrDefaultAsync(x => x.Id == model.Id.Value && x.SellerId == model.OrganisationId.Value && x.IsActive);

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

                    this.context.ProductAttributeItemTranslations.Add(newProductAttributeItemTranslation.FillCommonProperties());
                }

                await this.context.SaveChangesAsync();
            }

            await this.RebuildCategorySchemasAsync(
                model.OrganisationId.Value,
                model.Language,
                model.Username);

            return await this.GetProductAttributeItemByIdAsync(new GetProductAttributeItemByIdServiceModel
            {
                Id = model.Id,
                OrganisationId = model.OrganisationId,
                Language = model.Language,
                Username = model.Username
            });
        }

        public async Task<PagedResults<IEnumerable<ProductAttributeItemServiceModel>>> GetProductAttributeItemsAsync(GetProductAttributeItemsServiceModel model)
        {
            var productAttributesItems = this.context.ProductAttributeItems.Where(x => x.ProductAttributeId == model.ProductAttributeId && x.IsActive);

            if (!string.IsNullOrWhiteSpace(model.SearchTerm))
            {
                productAttributesItems = productAttributesItems.Where(x => x.ProductAttributeItemTranslations.Any(y => y.Name.StartsWith(model.SearchTerm)));
            }

            productAttributesItems = productAttributesItems.ApplySort(model.OrderBy);

            var pagedProductAttributeItems = productAttributesItems.PagedIndex(new Pagination(productAttributesItems.Count(), model.ItemsPerPage), model.PageIndex);

            var pagedProductAttributeItemServiceModels = new PagedResults<IEnumerable<ProductAttributeItemServiceModel>>(pagedProductAttributeItems.Total, pagedProductAttributeItems.PageSize);

            var productAttributeItemServiceModels = new List<ProductAttributeItemServiceModel>();

            foreach (var pagedProductAttributeItem in pagedProductAttributeItems.Data.ToList())
            {
                var productAttributeItemServiceModel = new ProductAttributeItemServiceModel
                {
                    Id = pagedProductAttributeItem.Id,
                    Order = pagedProductAttributeItem.Order,
                    LastModifiedDate = pagedProductAttributeItem.LastModifiedDate,
                    CreatedDate = pagedProductAttributeItem.CreatedDate
                };

                var productAttributeItemTranslations = this.context.ProductAttributeItemTranslations.FirstOrDefault(x => x.ProductAttributeItemId == pagedProductAttributeItem.Id &&  x.Language == model.Language && x.IsActive);

                if (productAttributeItemTranslations == null)
                {
                    productAttributeItemTranslations = this.context.ProductAttributeItemTranslations.FirstOrDefault(x => x.ProductAttributeItemId == pagedProductAttributeItem.Id && x.IsActive);
                }

                productAttributeItemServiceModel.Name = productAttributeItemTranslations?.Name;

                productAttributeItemServiceModels.Add(productAttributeItemServiceModel);
            }

            pagedProductAttributeItemServiceModels.Data = productAttributeItemServiceModels;

            return pagedProductAttributeItemServiceModels;
        }

        private async Task RebuildCategorySchemasAsync(
            Guid? organisationId,
            string language,
            string username)
        {
            var message = new RebuildCategorySchemasIntegrationEvent
            {
                OrganisationId = organisationId,
                Language = language,
                Username = username
            };

            this.eventBus.Publish(message);
        }
    }
}
