using Catalog.Api.ServicesModels.ProductAttributes;
using Foundation.Catalog.Infrastructure;
using Foundation.Catalog.Infrastructure.ProductAttributes.Entities;
using Foundation.EventBus.Abstractions;
using Foundation.EventLog.Repositories;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Extensions;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Catalog.Api.Services.ProductAttributes
{
    public class ProductAttributesService : IProductAttributesService
    {
        private readonly IEventBus eventBus;
        private readonly IEventLogRepository eventLogRepository;
        private readonly CatalogContext context;
        private readonly IStringLocalizer<ProductResources> productLocalizer;

        public ProductAttributesService(
            IEventBus eventBus,
            IEventLogRepository eventLogRepository,
            CatalogContext context,
            IStringLocalizer<ProductResources> productLocalizer)
        {
            this.eventBus = eventBus;
            this.eventLogRepository = eventLogRepository;
            this.context = context;
            this.productLocalizer = productLocalizer;
        }

        public async Task<PagedResults<IEnumerable<ProductAttributeServiceModel>>> GetAsync(GetProductAttributesServiceModel model)
        {
            var productAttributes = from c in this.context.ProductAttributes
                             join t in this.context.ProductAttributeTranslations on c.Id equals t.ProductAttributeId into ct
                             from x in ct.DefaultIfEmpty()
                             where (x.Language == model.Language || model.Language == null) && c.IsActive
                             select new ProductAttributeServiceModel
                             {
                                 Id = c.Id,
                                 Order = c.Order,
                                 Name = x.Name,
                                 LastModifiedDate = c.LastModifiedDate,
                                 CreatedDate = c.CreatedDate
                             };

            if (!string.IsNullOrWhiteSpace(model.SearchTerm))
            {
                productAttributes = productAttributes.Where(x => x.Name.StartsWith(model.SearchTerm));
            }

            productAttributes = productAttributes.ApplySort(model.OrderBy);

            return productAttributes.PagedIndex(new Pagination(productAttributes.Count(), model.ItemsPerPage), model.PageIndex);
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
            var existingProductItemAttribute = this.context.ProductAttributeItems.FirstOrDefault(x => x.ProductAttributeItemTranslations.Any(y => y.Name == model.Name) && x.SellerId == model.OrganisationId && x.IsActive);

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
                ProductAttribtuteItemId = productAttributeItem.Id,
                Name = model.Name,
                Language = model.Language
            };

            this.context.ProductAttributeItemTranslations.Add(productAttributeItemTranslation.FillCommonProperties());

            await this.context.SaveChangesAsync();

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

            existingProductAttribute.IsActive = false;

            await this.context.SaveChangesAsync();
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
        }

        public async Task<ProductAttributeServiceModel> GetProductAttributeByIdAsync(GetProductAttributeByIdServiceModel model)
        {
            var productAttribute = await (from pa in this.context.ProductAttributes
                                   join pat in this.context.ProductAttributeTranslations on pa.Id equals pat.ProductAttributeId into patx
                                   from x in patx.DefaultIfEmpty()
                                   where pa.Id == model.Id && pa.SellerId == model.OrganisationId && x.Language == model.Language && pa.IsActive
                                   select new ProductAttributeServiceModel
                                   {
                                       Id = pa.Id,
                                       Name = x.Name,
                                       Order = pa.Order,
                                       LastModifiedDate = pa.LastModifiedDate,
                                       CreatedDate = pa.CreatedDate
                                   }).FirstOrDefaultAsync();

            if (productAttribute != null)
            {
                productAttribute.ProductAttributeItems = from pai in this.context.ProductAttributeItems
                                                         join pait in this.context.ProductAttributeItemTranslations on pai.Id equals pait.ProductAttribtuteItemId into paitx
                                                         from x in paitx.DefaultIfEmpty()
                                                         where pai.ProductAttributeId == productAttribute.Id && pai.SellerId == model.OrganisationId && x.Language == model.Language && pai.IsActive
                                                         select new ProductAttributeItemServiceModel
                                                         {
                                                            Id = pai.Id,
                                                            Name = x.Name,
                                                            Order = pai.Order,
                                                            LastModifiedDate = pai.LastModifiedDate,
                                                            CreatedDate = pai.CreatedDate
                                                         };
            }

            return productAttribute;
        }

        public async Task<ProductAttributeItemServiceModel> GetProductAttributeItemByIdAsync(GetProductAttributeItemByIdServiceModel model)
        {
            var productAttributeItems = from pai in this.context.ProductAttributeItems
                                       join pait in this.context.ProductAttributeItemTranslations on pai.Id equals pait.ProductAttribtuteItemId into paitx
                                       from x in paitx.DefaultIfEmpty()
                                       where pai.ProductAttributeId == model.Id && pai.SellerId == model.OrganisationId && (x.Language == model.Language || x.Language == null) && pai.IsActive
                                       select new ProductAttributeItemServiceModel
                                       {
                                           Id = pai.Id,
                                           Name = x.Name,
                                           Order = pai.Order,
                                           LastModifiedDate = pai.LastModifiedDate,
                                           CreatedDate = pai.CreatedDate
                                       };

            return await productAttributeItems.FirstOrDefaultAsync();
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

                    this.context.ProductAttributeTranslations.Add(newProductAttributeTranslation);
                }

                await this.context.SaveChangesAsync();
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
                        ProductAttribtuteItemId = productAttributeItem.Id,
                        Language = model.Language,
                        Name = model.Name
                    };

                    this.context.ProductAttributeItemTranslations.Add(newProductAttributeItemTranslation);
                }

                await this.context.SaveChangesAsync();
            }

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
            var productAttributesItems = from c in this.context.ProductAttributeItems
                                        join t in this.context.ProductAttributeItemTranslations on c.Id equals t.ProductAttribtuteItemId into ct
                                        from x in ct.DefaultIfEmpty()
                                        where (x.Language == model.Language || model.Language == null) && c.IsActive
                                        select new ProductAttributeItemServiceModel
                                        {
                                            Id = c.Id,
                                            Order = c.Order,
                                            Name = x.Name,
                                            LastModifiedDate = c.LastModifiedDate,
                                            CreatedDate = c.CreatedDate
                                        };

            if (!string.IsNullOrWhiteSpace(model.SearchTerm))
            {
                productAttributesItems = productAttributesItems.Where(x => x.Name.StartsWith(model.SearchTerm));
            }

            productAttributesItems = productAttributesItems.ApplySort(model.OrderBy);

            return productAttributesItems.PagedIndex(new Pagination(productAttributesItems.Count(), model.ItemsPerPage), model.PageIndex);
        }
    }
}
