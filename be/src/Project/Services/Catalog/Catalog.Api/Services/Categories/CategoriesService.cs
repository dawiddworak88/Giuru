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

namespace Catalog.Api.Services.Categories
{
    public class CategoriesService : ICategoriesService
    {
        private readonly CatalogContext context;
        private readonly IStringLocalizer<ProductResources> productLocalizer;

        public CategoriesService(
            CatalogContext context,
            IStringLocalizer<ProductResources> productLocalizer)
        {
            this.context = context;
            this.productLocalizer = productLocalizer;
        }

        public async Task<PagedResults<IEnumerable<CategoryServiceModel>>> GetAsync(GetCategoriesServiceModel model)
        {
            var categories = this.context.Categories.Where(x => x.IsActive);

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

            var translations = this.context.CategoryTranslations.Where(x => pagedResults.Data.Select(y => y.Id).Contains(x.CategoryId) && x.IsActive || pagedResults.Data.Select(y => y.Parentid).Contains(x.CategoryId) && x.IsActive).ToList();

            var images = this.context.CategoryImages.Where(x => pagedResults.Data.Select(y => y.Id).Contains(x.CategoryId) && x.IsActive).ToList();

            return new PagedResults<IEnumerable<CategoryServiceModel>>(pagedResults.Total, pagedResults.PageSize)
            {
                Data = pagedResults.Data.OrEmptyIfNull().Select(x => new CategoryServiceModel
                {
                    Id = x.Id,
                    Order = x.Order,
                    Level= x.Level,
                    IsLeaf= x.IsLeaf,
                    ParentId = x.Parentid,
                    LastModifiedDate = x.LastModifiedDate,
                    CreatedDate = x.CreatedDate,
                    Name = translations.FirstOrDefault(t => t.CategoryId == x.Id && t.Language == model.Language) != null ? translations.FirstOrDefault(t => t.CategoryId == x.Id && t.Language == model.Language).Name : translations.FirstOrDefault(t => t.CategoryId == x.Id)?.Name,
                    ParentCategoryName = translations.FirstOrDefault(t => t.CategoryId == x.Parentid && t.Language == model.Language) != null ? translations.FirstOrDefault(t => t.CategoryId == x.Parentid && t.Language == model.Language).Name : translations.FirstOrDefault(t => t.CategoryId == x.Parentid)?.Name,
                    ThumbnailMediaId = images.FirstOrDefault()?.MediaId
                })
            };
        }

        public async Task<CategoryServiceModel> GetAsync(GetCategoryServiceModel model)
        {
            var categoryItem = this.context.Categories.FirstOrDefault(x => x.Id == model.Id && x.IsActive);

            if (categoryItem is not null)
            {
                var category = new CategoryServiceModel
                {
                    Id = categoryItem.Id,
                    Order = categoryItem.Order,
                    Level = categoryItem.Level,
                    IsLeaf = categoryItem.IsLeaf,
                    ParentId = categoryItem.Parentid,
                    LastModifiedDate = categoryItem.LastModifiedDate,
                    CreatedDate = categoryItem.CreatedDate
                };

                var thumbnailMedia = this.context.CategoryImages.FirstOrDefault(x => x.CategoryId == categoryItem.Id && x.IsActive);

                if (thumbnailMedia is not null)
                {
                    category.ThumbnailMediaId = thumbnailMedia.MediaId;
                }

                var categoryItemTranslations = this.context.CategoryTranslations.FirstOrDefault(x => x.Language == model.Language && x.CategoryId == categoryItem.Id && x.IsActive);

                if (categoryItemTranslations is null)
                {
                    categoryItemTranslations = this.context.CategoryTranslations.FirstOrDefault(x => x.CategoryId == categoryItem.Id && x.IsActive);
                }

                category.Name = categoryItemTranslations?.Name;

                if (categoryItem.Parentid.HasValue)
                {
                    var parentCategoryTranslations = this.context.CategoryTranslations.FirstOrDefault(x => x.Language == model.Language && x.CategoryId == categoryItem.Parentid && x.IsActive);

                    if (parentCategoryTranslations is null)
                    {
                        parentCategoryTranslations = this.context.CategoryTranslations.FirstOrDefault(x => x.CategoryId == categoryItem.Parentid && x.IsActive);
                    }

                    category.ParentCategoryName = parentCategoryTranslations?.Name;
                }

                return category;
            }

            return default;
        }

        public async Task DeleteAsync(DeleteCategoryServiceModel model)
        {
            var category = await this.context.Categories.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (category == null)
            {
                throw new CustomException(this.productLocalizer.GetString("CategoryNotFound"), (int)HttpStatusCode.NoContent);
            }

            if (await this.context.Categories.AnyAsync(x => x.Parentid == category.Id && x.IsActive))
            {
                throw new CustomException(this.productLocalizer.GetString("SubcategoriesDeleteCategoryConflict"), (int)HttpStatusCode.Conflict);
            }

            if (await this.context.Products.AnyAsync(x => x.CategoryId == category.Id && x.IsActive))
            {
                throw new CustomException(this.productLocalizer.GetString("ProductsDeleteCategoryConflict"), (int)HttpStatusCode.Conflict);
            }

            category.IsActive = false;

            await this.context.SaveChangesAsync();
        }

        public async Task<CategoryServiceModel> UpdateAsync(UpdateCategoryServiceModel model)
        {
            var category = await this.context.Categories.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (category == null)
            {
                throw new CustomException(this.productLocalizer.GetString("CategoryNotFound"), (int)HttpStatusCode.NoContent);
            }

            var parentCategory = await this.context.Categories.FirstOrDefaultAsync(x => x.Id == model.ParentId && x.IsActive);

            if (parentCategory == null)
            {
                throw new CustomException(this.productLocalizer.GetString("ParentCategoryNotFound"), (int)HttpStatusCode.NoContent);
            }

            category.Parentid = model.ParentId;
            category.Level = parentCategory.Level + 1;
            category.IsLeaf = !await this.context.Categories.AnyAsync(x => x.Parentid == category.Id && x.IsActive);
            category.LastModifiedDate = DateTime.UtcNow;

            var categoryTranslation = await this.context.CategoryTranslations.FirstOrDefaultAsync(x => x.CategoryId == model.Id && x.Language == model.Language && x.IsActive);

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

                this.context.CategoryTranslations.Add(newCategoryTranslation.FillCommonProperties());
            }

            var categoryImages = this.context.CategoryImages.Where(x => x.CategoryId == model.Id);

            foreach (var categoryImage in categoryImages)
            {
                this.context.CategoryImages.Remove(categoryImage);
            }

            foreach (var file in model.Files.OrEmptyIfNull())
            {
                var categoryImage = new CategoryImage
                { 
                    CategoryId = model.Id.Value,
                    MediaId = file
                };

                this.context.CategoryImages.Add(categoryImage.FillCommonProperties());
            }

            await this.context.SaveChangesAsync();

            return await this.GetAsync(new GetCategoryServiceModel { Id = category.Id, Language = model.Language, OrganisationId = model.OrganisationId, Username = model.Username });
        }

        public async Task<CategoryServiceModel> CreateAsync(CreateCategoryServiceModel model)
        {
            var category = new Category
            {
                Parentid = model.ParentId,
                IsLeaf = true
            };

            var parentCategory = await this.context.Categories.FirstOrDefaultAsync(x => x.Id == model.ParentId && x.IsActive);

            category.Level = parentCategory.Level + 1;

            this.context.Categories.Add(category.FillCommonProperties());

            var categoryTranslation = new CategoryTranslation
            { 
                CategoryId = category.Id,
                Name = model.Name,
                Language = model.Language
            };

            this.context.CategoryTranslations.Add(categoryTranslation.FillCommonProperties());

            foreach (var file in model.Files.OrEmptyIfNull())
            {
                var categoryImage = new CategoryImage
                {
                    CategoryId = category.Id,
                    MediaId = file,
                };

                this.context.CategoryImages.Add(categoryImage.FillCommonProperties());
            }

            if (model.Schema is not null)
            {
                var categorySchema = new CategorySchema
                {
                    Schema = model.Schema,
                    UiSchema = model.UiSchema,
                    CategoryId = category.Id,
                    Language = model.Language
                };

                this.context.CategorySchemas.Add(categorySchema.FillCommonProperties());
            }

            await this.context.SaveChangesAsync();

            return await this.GetAsync(new GetCategoryServiceModel { Id = category.Id, Language = model.Language, OrganisationId = model.OrganisationId, Username = model.Username });
        }

        public async Task<CategorySchemaServiceModel> UpdateCategorySchemaAsync(UpdateCategorySchemaServiceModel model)
        {
            var categorySchema = await this.context.CategorySchemas.FirstOrDefaultAsync(x => x.CategoryId == model.CategoryId && x.Language == model.Language && x.IsActive);

            if (categorySchema != null)
            {
                categorySchema.Schema = model.Schema;
                categorySchema.UiSchema = model.UiSchema;
                categorySchema.LastModifiedDate = DateTime.UtcNow;
            }
            else
            {
                var newCategorySchema = new CategorySchema
                { 
                    CategoryId = model.CategoryId.Value,
                    Language = model.Language,
                    Schema = model.Schema,
                    UiSchema = model.UiSchema
                };

                this.context.CategorySchemas.Add(newCategorySchema.FillCommonProperties());
            }

            await this.context.SaveChangesAsync();

            return await this.GetCategorySchemaAsync(new GetCategorySchemaServiceModel 
            { 
                CategoryId = model.CategoryId,
                Language = model.Language,
                OrganisationId = model.OrganisationId,
                Username = model.Username
            });
        }

        public async Task<CategorySchemaServiceModel> GetCategorySchemaAsync(GetCategorySchemaServiceModel model)
        {
            var categorySchemas = from c in this.context.Categories
                                  join cs in this.context.CategorySchemas on c.Id equals cs.CategoryId into csx
                                  from x in csx.DefaultIfEmpty()
                                  where x != null && c.Id == model.CategoryId && (x.Language == model.Language || x.Language == null) && c.IsActive
                                  select new CategorySchemaServiceModel
                                  {
                                      Id = x.Id,
                                      CategoryId = c.Id,
                                      Schema = x.Schema,
                                      UiSchema = x.UiSchema,
                                      LastModifiedDate = x.LastModifiedDate,
                                      CreatedDate = x.CreatedDate
                                  };

            return await categorySchemas.FirstOrDefaultAsync();
        }
    }
}
