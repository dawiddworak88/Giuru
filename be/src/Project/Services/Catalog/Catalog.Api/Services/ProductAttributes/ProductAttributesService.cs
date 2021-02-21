using Catalog.Api.ServicesModels.ProductAttributes;
using Foundation.Catalog.Infrastructure;
using Foundation.Catalog.Infrastructure.ProductAttributes.Entities;
using Foundation.EventBus.Abstractions;
using Foundation.EventLog.Repositories;
using Foundation.Extensions.Exceptions;
using Foundation.GenericRepository.Extensions;
using Foundation.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Catalog.Api.Services.ProductAttributes
{
    public class ProductAttributesService : IProductAttributesService
    {
        private readonly IEventBus eventBus;
        private readonly IEventLogRepository eventLogRepository;
        private readonly CatalogContext catalogContext;
        private readonly IStringLocalizer<ProductResources> productLocalizer;

        public ProductAttributesService(
            IEventBus eventBus,
            IEventLogRepository eventLogRepository,
            CatalogContext catalogContext,
            IStringLocalizer<ProductResources> productLocalizer)
        {
            this.eventBus = eventBus;
            this.eventLogRepository = eventLogRepository;
            this.catalogContext = catalogContext;
            this.productLocalizer = productLocalizer;
        }

        public async Task<ProductAttributeServiceModel> CreateProductAttributeAsync(CreateUpdateProductAttributeServiceModel model)
        {
            var existingProductAttribute = this.catalogContext.ProductAttributes.FirstOrDefault(x => x.Key == model.Key && x.SellerId == model.OrganisationId && x.IsActive);

            if (existingProductAttribute != null)
            {
                throw new CustomException(this.productLocalizer.GetString("ProductAttributeKeyConflict"), (int)HttpStatusCode.Conflict);
            }

            var productAttribute = new ProductAttribute
            { 
                Key = model.Key,
                Order = model.Order,
                SellerId = model.OrganisationId.Value,
                Tags = model.Tags
            };

            this.catalogContext.ProductAttributes.Add(productAttribute.FillCommonProperties());

            var productAttributeTranslation = new ProductAttributeTranslation
            {
                ProductAttributeId = productAttribute.Id,
                Name = model.Name,
                Language = model.Language
            };

            this.catalogContext.ProductAttributeTranslations.Add(productAttributeTranslation.FillCommonProperties());

            await this.catalogContext.SaveChangesAsync();

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
            var existingProductItemAttribute = this.catalogContext.ProductAttributeItems.FirstOrDefault(x => x.ProductAttributeItemTranslations.Any(y => y.Name == model.Name) && x.SellerId == model.OrganisationId && x.IsActive);

            if (existingProductItemAttribute != null)
            {
                throw new CustomException(this.productLocalizer.GetString("ProductAttributeItemConflict"), (int)HttpStatusCode.Conflict);
            }

            var productAttributeItem = new ProductAttributeItem
            {
                ProductAttributeId = model.ProductAttributeId.Value,
                Order = model.Order,
                SellerId = model.OrganisationId.Value,
                Tags = model.Tags
            };

            this.catalogContext.ProductAttributeItems.Add(productAttributeItem.FillCommonProperties());

            var productAttributeItemTranslation = new ProductAttributeItemTranslation
            {
                ProductAttribtuteItemId = productAttributeItem.Id,
                Name = model.Name,
                Language = model.Language
            };

            this.catalogContext.ProductAttributeItemTranslations.Add(productAttributeItemTranslation.FillCommonProperties());

            await this.catalogContext.SaveChangesAsync();

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
            var existingProductAttribute = this.catalogContext.ProductAttributes.FirstOrDefault(x => x.Id == model.Id.Value && x.SellerId == model.OrganisationId && x.IsActive);

            if (existingProductAttribute == null)
            {
                throw new CustomException(this.productLocalizer.GetString("ProductAttributeNotFound"), (int)HttpStatusCode.NotFound);
            }

            existingProductAttribute.IsActive = false;

            await this.catalogContext.SaveChangesAsync();
        }

        public async Task DeleteProductAttributeItemAsync(DeleteProductAttributeItemServiceModel model)
        {
            var existingProductAttributeItem = this.catalogContext.ProductAttributeItems.FirstOrDefault(x => x.Id == model.Id && x.SellerId == model.OrganisationId && x.IsActive);

            if (existingProductAttributeItem == null)
            {
                throw new CustomException(this.productLocalizer.GetString("ProductAttributeItemNotFound"), (int)HttpStatusCode.NotFound);
            }

            existingProductAttributeItem.IsActive = false;

            await this.catalogContext.SaveChangesAsync();
        }

        public async Task<ProductAttributeServiceModel> GetProductAttributeByIdAsync(GetProductAttributeByIdServiceModel model)
        {
            var productAttribute = await (from pa in this.catalogContext.ProductAttributes
                                   join pat in this.catalogContext.ProductAttributeTranslations on pa.Id equals pat.ProductAttributeId into patx
                                   from x in patx.DefaultIfEmpty()
                                   where pa.Id == model.Id && pa.SellerId == model.OrganisationId && x.Language == model.Language && pa.IsActive
                                   select new ProductAttributeServiceModel
                                   {
                                       Id = pa.Id,
                                       Name = x.Name,
                                       Order = pa.Order,
                                       Key = pa.Key,
                                       Tags = pa.Tags,
                                       LastModifiedDate = pa.LastModifiedDate,
                                       CreatedDate = pa.CreatedDate
                                   }).FirstOrDefaultAsync();

            if (productAttribute != null)
            {
                productAttribute.ProductAttributeItems = from pai in this.catalogContext.ProductAttributeItems
                                                         join pait in this.catalogContext.ProductAttributeItemTranslations on pai.Id equals pait.ProductAttribtuteItemId into paitx
                                                         from x in paitx.DefaultIfEmpty()
                                                         where pai.ProductAttributeId == productAttribute.Id && pai.SellerId == model.OrganisationId && x.Language == model.Language && pai.IsActive
                                                         select new ProductAttributeItemServiceModel
                                                         {
                                                            Id = pai.Id,
                                                            Name = x.Name,
                                                            Order = pai.Order,
                                                            Tags = pai.Tags,
                                                            LastModifiedDate = pai.LastModifiedDate,
                                                            CreatedDate = pai.CreatedDate
                                                         };
            }

            return productAttribute;
        }

        public async Task<ProductAttributeItemServiceModel> GetProductAttributeItemByIdAsync(GetProductAttributeItemByIdServiceModel model)
        {
            var productAttributeItem = await (from pai in this.catalogContext.ProductAttributeItems
                                       join pait in this.catalogContext.ProductAttributeItemTranslations on pai.Id equals pait.ProductAttribtuteItemId into paitx
                                       from x in paitx.DefaultIfEmpty()
                                       where pai.ProductAttributeId == model.Id && pai.SellerId == model.OrganisationId && x.Language == model.Language && pai.IsActive
                                       select new ProductAttributeItemServiceModel
                                       {
                                           Id = pai.Id,
                                           Name = x.Name,
                                           Order = pai.Order,
                                           Tags = pai.Tags,
                                           LastModifiedDate = pai.LastModifiedDate,
                                           CreatedDate = pai.CreatedDate
                                       }).FirstOrDefaultAsync();

            return productAttributeItem;
        }

        public async Task<ProductAttributeServiceModel> UpdateProductAttributeAsync(CreateUpdateProductAttributeServiceModel model)
        {
            var productAttribute = await this.catalogContext.ProductAttributes.FirstOrDefaultAsync(x => x.Id == model.Id.Value && x.SellerId == model.OrganisationId.Value && x.IsActive);

            if (productAttribute != null)
            {
                productAttribute.Key = model.Key;
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

                    this.catalogContext.ProductAttributeTranslations.Add(newProductAttributeTranslation);
                }

                await this.catalogContext.SaveChangesAsync();
            }

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
            var productAttributeItem = await this.catalogContext.ProductAttributeItems.FirstOrDefaultAsync(x => x.Id == model.Id.Value && x.SellerId == model.OrganisationId.Value && x.IsActive);

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
                        ProductAttribtuteItemId = productAttributeItem.Id,
                        Language = model.Language,
                        Name = model.Name
                    };

                    this.catalogContext.ProductAttributeItemTranslations.Add(newProductAttributeItemTranslation);
                }

                await this.catalogContext.SaveChangesAsync();
            }

            return await this.GetProductAttributeItemByIdAsync(new GetProductAttributeItemByIdServiceModel
            {
                Id = model.Id,
                OrganisationId = model.OrganisationId,
                Language = model.Language,
                Username = model.Username
            });
        }
    }
}
