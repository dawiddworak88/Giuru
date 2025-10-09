using Catalog.Api.ServicesModels.Categories;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Foundation.GenericRepository.Paginations;
using Foundation.Extensions.Exceptions;
using System.Net;
using Microsoft.Extensions.Localization;
using Foundation.Localization;
using System;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Extensions;
using Foundation.Catalog.Infrastructure;
using Foundation.Catalog.Infrastructure.Categories.Entites;
using Foundation.Catalog.Infrastructure.Categories.Entities;
using Foundation.GenericRepository.Definitions;
using Foundation.EventBus.Abstractions;
using System.Diagnostics;
using Catalog.Api.IntegrationEvents;
using System.Linq.Dynamic.Core;

namespace Catalog.Api.Services.Categories
{
    public class CategoriesService : ICategoriesService
    {
        private readonly IEventBus _eventBus;
        private readonly CatalogContext _context;
        private readonly IStringLocalizer<ProductResources> _productLocalizer;

        public CategoriesService(
            CatalogContext context,
            IEventBus eventBus,
            IStringLocalizer<ProductResources> productLocalizer)
        {
            _context = context;
            _productLocalizer = productLocalizer;
            _eventBus = eventBus;
        }

        public PagedResults<IEnumerable<CategoryServiceModel>> Get(GetCategoriesServiceModel model)
        {
            var categories = _context.Categories.Where(x => x.IsActive)
                .Include(x => x.Images)
                .Include(x => x.Translations)
                .Include(x => x.ParentCategory)
                .Include(x => x.ParentCategory.Translations)          
                .AsSingleQuery();

            if (!string.IsNullOrWhiteSpace(model.SearchTerm))
            {
                categories = categories.Where(x => x.Translations.Any(x => x.Language == model.Language && x.Name.StartsWith(model.SearchTerm) && x.IsActive));
            }

            if (model.Level.HasValue)
            {
                categories = categories.Where(x => x.Level == model.Level.Value);
            }

            if (model.LeafOnly.HasValue)
            {
                categories = categories.Where(x => x.IsLeaf == model.LeafOnly.Value);
            }

            categories = categories.ApplySort(model.OrderBy);

            PagedResults<IEnumerable<Category>> pagedResults;

            if (model.PageIndex.HasValue is false || model.ItemsPerPage.HasValue is false)
            {
                categories = categories.Take(Constants.MaxItemsPerPageLimit);

                pagedResults = categories.PagedIndex(new Pagination(categories.Count(), Constants.MaxItemsPerPageLimit), Constants.DefaultPageIndex);
            }
            else
            {
                pagedResults = categories.PagedIndex(new Pagination(categories.Count(), model.ItemsPerPage.Value), model.PageIndex.Value);
            }

            return new PagedResults<IEnumerable<CategoryServiceModel>>(pagedResults.Total, pagedResults.PageSize)
            {
                Data = pagedResults.Data.OrEmptyIfNull().Select(x => new CategoryServiceModel
                {
                    Id = x.Id,
                    Order = x.Order,
                    Level = x.Level,
                    IsLeaf = x.IsLeaf,
                    ParentId = x.Parentid,
                    LastModifiedDate = x.LastModifiedDate,
                    CreatedDate = x.CreatedDate,
                    Name = x.Translations.FirstOrDefault(t => t.CategoryId == x.Id && t.Language == model.Language)?.Name ?? x.Translations.FirstOrDefault(t => t.CategoryId == x.Id)?.Name,
                    ParentCategoryName = x.ParentCategory?.Translations?.FirstOrDefault(t => t.CategoryId == x.Parentid && t.Language == model.Language)?.Name ?? x.ParentCategory?.Translations?.FirstOrDefault(t => t.CategoryId == x.Parentid)?.Name,
                    ThumbnailMediaId = x.Images.FirstOrDefault(y => y.CategoryId == x.Id)?.MediaId
                })
            };
        }

        public CategoryServiceModel Get(GetCategoryServiceModel model)
        {
            var categoryItem = _context.Categories.FirstOrDefault(x => x.Id == model.Id && x.IsActive);

            if (categoryItem is null)
            {
                throw new NotFoundException(_productLocalizer.GetString("CategoryNotFound"));
            }

            var translations = _context.CategoryTranslations.Where(x => x.CategoryId == categoryItem.Id && x.IsActive || x.CategoryId == categoryItem.Parentid && x.IsActive).ToList();

            return new CategoryServiceModel
            {
                Id = categoryItem.Id,
                Name = translations.FirstOrDefault(t => t.CategoryId == categoryItem.Id && t.Language == model.Language)?.Name ?? translations.FirstOrDefault(t => t.CategoryId == categoryItem.Id)?.Name,
                Order = categoryItem.Order,
                Level = categoryItem.Level,
                IsLeaf = categoryItem.IsLeaf,
                ThumbnailMediaId = _context.CategoryImages.FirstOrDefault(x => x.CategoryId == categoryItem.Id && x.IsActive)?.MediaId ?? null,
                ParentId = categoryItem.Parentid,
                ParentCategoryName = translations.FirstOrDefault(t => t.CategoryId == categoryItem.Parentid && t.Language == model.Language)?.Name ?? translations.FirstOrDefault(t => t.CategoryId == categoryItem.Parentid)?.Name,
                LastModifiedDate = categoryItem.LastModifiedDate,
                CreatedDate = categoryItem.CreatedDate
            };
        }

        public async Task DeleteAsync(DeleteCategoryServiceModel model)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (category is null)
            {
                throw new NotFoundException(_productLocalizer.GetString("CategoryNotFound"));
            }

            if (await _context.Categories.AnyAsync(x => x.Parentid == category.Id && x.IsActive))
            {
                throw new ConflictException(_productLocalizer.GetString("SubcategoriesDeleteCategoryConflict"));
            }

            if (await _context.Products.AnyAsync(x => x.CategoryId == category.Id && x.IsActive))
            {
                throw new ConflictException(_productLocalizer.GetString("ProductsDeleteCategoryConflict"));
            }

            category.IsActive = false;

            await _context.SaveChangesAsync();
        }

        public async Task<CategoryServiceModel> UpdateAsync(UpdateCategoryServiceModel model)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (category is null)
            {
                throw new NotFoundException(_productLocalizer.GetString("CategoryNotFound"));
            }

            if (model.ParentId.HasValue)
            {
                var parentCategory = await _context.Categories.FirstOrDefaultAsync(x => x.Id == model.ParentId && x.IsActive);

                if (parentCategory is null)
                {
                    throw new NotFoundException(_productLocalizer.GetString("ParentCategoryNotFound"));
                }

                category.Parentid = model.ParentId;
                category.Level = parentCategory.Level + 1;
            }

            if (model.Order != 0)
            {
                await OrderingCategoriesAsync(category.Order, model.Order);
                category.Order = model.Order;
            }

            category.Parentid = model.ParentId;
            category.IsLeaf = !await _context.Categories.AnyAsync(x => x.Parentid == category.Id && x.IsActive);
            category.LastModifiedDate = DateTime.UtcNow;

            var categoryTranslation = await _context.CategoryTranslations.FirstOrDefaultAsync(x => x.CategoryId == model.Id && x.Language == model.Language && x.IsActive);

            if (categoryTranslation is not null)
            {
                categoryTranslation.Name = model.Name;
                categoryTranslation.LastModifiedDate = DateTime.UtcNow;
            }
            else
            {
                var newCategoryTranslation = new CategoryTranslation
                {
                    Language = model.Language,
                    CategoryId = category.Id,
                    Name = model.Name
                };

                _context.CategoryTranslations.Add(newCategoryTranslation.FillCommonProperties());
            }

            var categoryImages = _context.CategoryImages.Where(x => x.CategoryId == model.Id);

            foreach (var categoryImage in categoryImages)
            {
                _context.CategoryImages.Remove(categoryImage);
            }

            var images = new List<CategoryImage>();

            foreach (var file in model.Files.OrEmptyIfNull())
            {
                var categoryImage = new CategoryImage
                {
                    CategoryId = model.Id.Value,
                    MediaId = file
                };

                images.Add(categoryImage.FillCommonProperties());
            }

            await _context.CategoryImages.AddRangeAsync(images);

            await _context.SaveChangesAsync();

            return Get(new GetCategoryServiceModel { Id = category.Id, Language = model.Language, OrganisationId = model.OrganisationId, Username = model.Username });
        }

        private async Task OrderingCategoriesAsync(int source, int destination)
        {
            if(source > destination)
            {
                var categories = _context.Categories.Where(x => x.IsActive && x.Order >= destination && x.Order < source);

                foreach (var category in categories.OrEmptyIfNull()) 
                { 
                    category.Order += 1;
                }
            }
            else
            {
                var categories = _context.Categories.Where(x => x.IsActive && x.Order <= destination && x.Order > source);

                foreach (var category in categories.OrEmptyIfNull())
                {
                    category.Order -= 1;
                }
            }
            
            await _context.SaveChangesAsync();
        }

        public async Task<CategoryServiceModel> CreateAsync(CreateCategoryServiceModel model)
        {
            var category = new Category
            {
                Parentid = model.ParentId,
                IsLeaf = true
            };

            var parentCategory = await _context.Categories.FirstOrDefaultAsync(x => x.Id == model.ParentId && x.IsActive);

            if (parentCategory is not null)
            {
                category.Level = parentCategory.Level + 1;
            }

            _context.Categories.Add(category.FillCommonProperties());

            var categoryTranslation = new CategoryTranslation
            {
                CategoryId = category.Id,
                Name = model.Name,
                Language = model.Language
            };
            
            await OrderingCategoriesAsync(0, 1);
            category.Order = 1;

            _context.CategoryTranslations.Add(categoryTranslation.FillCommonProperties());

            var images = new List<CategoryImage>();

            foreach (var file in model.Files.OrEmptyIfNull())
            {
                var categoryImage = new CategoryImage
                {
                    CategoryId = category.Id,
                    MediaId = file,
                };

                images.Add(categoryImage.FillCommonProperties());
            }

            await _context.CategoryImages.AddRangeAsync(images);

            var schemas = new List<CategorySchema>();

            foreach (var schema in model.Schemas.OrEmptyIfNull())
            {
                var categorySchema = new CategorySchema
                {
                    Schema = schema.Schema,
                    UiSchema = schema.UiSchema,
                    CategoryId = category.Id,
                    Language = schema.Language
                };

                schemas.Add(categorySchema.FillCommonProperties());
            }

            await _context.CategorySchemas.AddRangeAsync(schemas);

            await _context.SaveChangesAsync();

            return Get(new GetCategoryServiceModel { Id = category.Id, Language = model.Language, OrganisationId = model.OrganisationId, Username = model.Username });
        }

        public async Task<CategorySchemasServiceModel> UpdateCategorySchemaAsync(UpdateCategorySchemaServiceModel model)
        {
            var categorySchema = await _context.CategorySchemas.FirstOrDefaultAsync(x => x.CategoryId == model.Id && x.Language == model.Language && x.IsActive);

            if (categorySchema is not null)
            {
                categorySchema.Schema = model.Schema;
                categorySchema.UiSchema = model.UiSchema;
                categorySchema.LastModifiedDate = DateTime.UtcNow;
            }
            else
            {
                var newCategorySchema = new CategorySchema
                {
                    CategoryId = model.Id.Value,
                    Language = model.Language,
                    Schema = model.Schema,
                    UiSchema = model.UiSchema
                };

                _context.CategorySchemas.Add(newCategorySchema.FillCommonProperties());
            }

            await _context.SaveChangesAsync();

            TriggerCategoryProductsIndexRebuild(new RebuildCategoryProductsIndexServiceModel
            {
                CategoryId = model.Id,
                Language = model.Language,
                OrganisationId = model.OrganisationId,
                Username = model.Username
            });

            return await GetCategorySchemasAsync(new GetCategorySchemasServiceModel
            {
                Id = model.Id,
                Language = model.Language,
                OrganisationId = model.OrganisationId,
                Username = model.Username
            });
        }

        public async Task<CategorySchemasServiceModel> GetCategorySchemasAsync(GetCategorySchemasServiceModel model)
        {
            var categorySchema = await _context.Categories
                .Include(x => x.Schemas)
                .AsSingleQuery()
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            if (categorySchema is null)
            {
                throw new NotFoundException(_productLocalizer.GetString("CategoryNotFound")); 
            }

            return new CategorySchemasServiceModel
            {
                Id = categorySchema.Id,
                Schemas = categorySchema.Schemas.Select(x => new CategorySchemaServiceModel
                {
                    Id = x.Id,
                    Schema = x.Schema,
                    UiSchema = x.UiSchema,
                    Language = x.Language
                }),
                LastModifiedDate = categorySchema.LastModifiedDate,
                CreatedDate = categorySchema.CreatedDate
            };
        }

        private void TriggerCategoryProductsIndexRebuild(RebuildCategoryProductsIndexServiceModel model)
        {
            using var source = new ActivitySource(GetType().Name);

            var message = new RebuildCategoryProductsIntegrationEvent
            {
                OrganisationId = model.OrganisationId,
                Language = model.Language,
                Username = model.Username,
                CategoryId = model.CategoryId
            };

            using var activity = source.StartActivity($"{System.Reflection.MethodBase.GetCurrentMethod().Name} {message.GetType().Name}");

            _eventBus.Publish(message);
        }
    }
}
