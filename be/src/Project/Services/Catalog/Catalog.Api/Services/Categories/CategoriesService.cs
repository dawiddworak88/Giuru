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
        private readonly CatalogContext _context;
        private readonly IStringLocalizer<ProductResources> _productLocalizer;

        public CategoriesService(
            CatalogContext context,
            IStringLocalizer<ProductResources> productLocalizer)
        {
            _context = context;
            _productLocalizer = productLocalizer;
        }

        public PagedResults<IEnumerable<CategoryServiceModel>> Get(GetCategoriesServiceModel model)
        {
            var categories = _context.Categories.Where(x => x.IsActive)
                .Include(x => x.Images)
                .Include(x => x.Translations)
                .Include(x => x.ParentCategory)
                .Include(x => x.ParentCategory.Translations)
                .Include(x => x.ClientsGroups)
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
                    Name =  x.Translations.FirstOrDefault(t => t.CategoryId == x.Id && t.Language == model.Language)?.Name ?? x.Translations.FirstOrDefault(t => t.CategoryId == x.Id)?.Name,
                    ParentCategoryName = x.ParentCategory?.Translations?.FirstOrDefault(t => t.CategoryId == x.Parentid && t.Language == model.Language)?.Name ?? x.ParentCategory?.Translations?.FirstOrDefault(t => t.CategoryId == x.Parentid)?.Name,
                    ClientGroupIds = x.ClientsGroups.Select(x => x.GroupId),
                    ThumbnailMediaId = x.Images.FirstOrDefault(y => y.CategoryId == x.Id)?.MediaId
                })
            };
        }

        public CategoryServiceModel Get(GetCategoryServiceModel model)
        {
            var categoryItem = _context.Categories.FirstOrDefault(x => x.Id == model.Id && x.IsActive);

            if (categoryItem is null) { 
                throw new CustomException(_productLocalizer.GetString("CategoryNotFound"), (int)HttpStatusCode.NoContent);
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

    /*  var category = new CategoryServiceModel
      {
          Id = categoryItem.Id,
          Order = categoryItem.Order,
          Level = categoryItem.Level,
          IsLeaf = categoryItem.IsLeaf,
          ParentId = categoryItem.Parentid,
          LastModifiedDate = categoryItem.LastModifiedDate,
          CreatedDate = categoryItem.CreatedDate
      };

      var clientGroups = _context.CategoriesGroups.Where(x => x.CategoryId == categoryItem.Id && x.IsActive).Select(x => x.GroupId);

      if (clientGroups is not null)
      {
          category.ClientGroupIds = clientGroups;
      }

      var thumbnailMedia = _context.CategoryImages.FirstOrDefault(x => x.CategoryId == categoryItem.Id && x.IsActive);

      if (thumbnailMedia is not null)
      {
          category.ThumbnailMediaId = thumbnailMedia.MediaId;
      }

      var categoryItemTranslations = _context.CategoryTranslations.FirstOrDefault(x => x.Language == model.Language && x.CategoryId == categoryItem.Id && x.IsActive);

      if (categoryItemTranslations is null)
      {
          categoryItemTranslations = _context.CategoryTranslations.FirstOrDefault(x => x.CategoryId == categoryItem.Id && x.IsActive);
      }

      category.Name = categoryItemTranslations?.Name;

      if (categoryItem.Parentid.HasValue)
      {
          var parentCategoryTranslations = _context.CategoryTranslations.FirstOrDefault(x => x.Language == model.Language && x.CategoryId == categoryItem.Parentid && x.IsActive);

          if (parentCategoryTranslations is null)
          {
              parentCategoryTranslations = _context.CategoryTranslations.FirstOrDefault(x => x.CategoryId == categoryItem.Parentid && x.IsActive);
          }

          category.ParentCategoryName = parentCategoryTranslations?.Name;
      }

      return category;*/

    public async Task DeleteAsync(DeleteCategoryServiceModel model)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (category is null)
            {
                throw new CustomException(_productLocalizer.GetString("CategoryNotFound"), (int)HttpStatusCode.NoContent);
            }

            if (await _context.Categories.AnyAsync(x => x.Parentid == category.Id && x.IsActive))
            {
                throw new CustomException(_productLocalizer.GetString("SubcategoriesDeleteCategoryConflict"), (int)HttpStatusCode.Conflict);
            }

            if (await _context.Products.AnyAsync(x => x.CategoryId == category.Id && x.IsActive))
            {
                throw new CustomException(_productLocalizer.GetString("ProductsDeleteCategoryConflict"), (int)HttpStatusCode.Conflict);
            }

            category.IsActive = false;

            await _context.SaveChangesAsync();
        }

        public async Task<CategoryServiceModel> UpdateAsync(UpdateCategoryServiceModel model)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (category is null)
            {
                throw new CustomException(_productLocalizer.GetString("CategoryNotFound"), (int)HttpStatusCode.NoContent);
            }

            var parentCategory = await _context.Categories.FirstOrDefaultAsync(x => x.Id == model.ParentId && x.IsActive);

            if (parentCategory is null)
            {
                throw new CustomException(_productLocalizer.GetString("ParentCategoryNotFound"), (int)HttpStatusCode.NoContent);
            }

            category.Parentid = model.ParentId;
            category.Level = parentCategory.Level + 1;
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

            var clientGroups = _context.CategoriesGroups.Where(x => x.CategoryId == model.Id && x.IsActive);

            foreach (var clientGroup in clientGroups.OrEmptyIfNull())
            {
                _context.CategoriesGroups.Remove(clientGroup);
            }

            foreach (var clientGroupId in model.ClientGroupIds.OrEmptyIfNull())
            {
                var group = new CategoriesGroup
                {
                    GroupId = clientGroupId,
                    CategoryId = category.Id
                };

                _context.CategoriesGroups.Add(group.FillCommonProperties());
            }

            await _context.SaveChangesAsync();

            return Get(new GetCategoryServiceModel { Id = category.Id, Language = model.Language, OrganisationId = model.OrganisationId, Username = model.Username });
        }

        public async Task<CategoryServiceModel> CreateAsync(CreateCategoryServiceModel model)
        {
            var category = new Category
            {
                Parentid = model.ParentId,
                IsLeaf = true
            };

            var parentCategory = await _context.Categories.FirstOrDefaultAsync(x => x.Id == model.ParentId && x.IsActive);

            category.Level = parentCategory.Level + 1;

            _context.Categories.Add(category.FillCommonProperties());

            var categoryTranslation = new CategoryTranslation
            { 
                CategoryId = category.Id,
                Name = model.Name,
                Language = model.Language
            };

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

            foreach (var clientGroupId in model.ClientGroupIds.OrEmptyIfNull())
            {
                var group = new CategoriesGroup
                {
                    CategoryId = category.Id,
                    GroupId = clientGroupId
                };

                _context.CategoriesGroups.Add(group.FillCommonProperties());
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

                _context.CategorySchemas.Add(categorySchema.FillCommonProperties());
            }

            await _context.SaveChangesAsync();

            return Get(new GetCategoryServiceModel { Id = category.Id, Language = model.Language, OrganisationId = model.OrganisationId, Username = model.Username });
        }

        public async Task<CategorySchemaServiceModel> UpdateCategorySchemaAsync(UpdateCategorySchemaServiceModel model)
        {
            var categorySchema = await _context.CategorySchemas.FirstOrDefaultAsync(x => x.CategoryId == model.CategoryId && x.Language == model.Language && x.IsActive);

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
                    CategoryId = model.CategoryId.Value,
                    Language = model.Language,
                    Schema = model.Schema,
                    UiSchema = model.UiSchema
                };

                _context.CategorySchemas.Add(newCategorySchema.FillCommonProperties());
            }

            await _context.SaveChangesAsync();

            return await GetCategorySchemaAsync(new GetCategorySchemaServiceModel 
            { 
                CategoryId = model.CategoryId,
                Language = model.Language,
                OrganisationId = model.OrganisationId,
                Username = model.Username
            });
        }

        public async Task<CategorySchemaServiceModel> GetCategorySchemaAsync(GetCategorySchemaServiceModel model)
        {
            var categorySchemas = from c in _context.Categories
                                  join cs in _context.CategorySchemas on c.Id equals cs.CategoryId into csx
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
