using Catalog.Api.Infrastructure;
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
            var categories = from c in this.context.Categories
                             join t in this.context.CategoryTranslations on c.Id equals t.CategoryId into ct
                             from x in ct.DefaultIfEmpty()
                             join m in this.context.CategoryImages on c.Id equals m.CategoryId into cm
                             from y in cm.DefaultIfEmpty()
                             join pct in this.context.CategoryTranslations on c.Parentid equals pct.CategoryId into pctg
                             from w in pctg.DefaultIfEmpty()
                             where x.Language == model.Language && (w.Language == model.Language || w.Language == null) && c.IsActive
                             select new CategoryServiceModel
                             {
                                 Id = c.Id,
                                 Order = c.Order,
                                 Level = c.Level,
                                 IsLeaf = c.IsLeaf,
                                 ParentId = c.Parentid,
                                 ParentCategoryName = w.Name,
                                 Name = x.Name,
                                 ThumbnailMediaId = y.MediaId,
                                 LastModifiedDate = c.LastModifiedDate,
                                 CreatedDate = c.CreatedDate
                             };

            if (!string.IsNullOrWhiteSpace(model.SearchTerm))
            {
                categories = categories.Where(x => x.Name.StartsWith(model.SearchTerm));
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

            return categories.PagedIndex(new Pagination(categories.Count(), model.ItemsPerPage), model.PageIndex);
        }

        public async Task<CategoryServiceModel> GetAsync(GetCategoryServiceModel model)
        {
            var categories = from c in this.context.Categories
                             join t in this.context.CategoryTranslations on c.Id equals t.CategoryId into ct
                             from x in ct.DefaultIfEmpty()
                             join m in this.context.CategoryImages on c.Id equals m.CategoryId into cm
                             from y in cm.DefaultIfEmpty()
                             where x.Language == model.Language && c.Id == model.Id && c.IsActive
                             orderby c.Order
                             select new CategoryServiceModel
                             {
                                 Id = c.Id,
                                 Order = c.Order,
                                 Level = c.Level,
                                 IsLeaf = c.IsLeaf,
                                 ParentId = c.Parentid,
                                 Name = x.Name,
                                 ThumbnailMediaId = y.MediaId
                             };

            return await categories.FirstOrDefaultAsync();
        }

        public async Task DeleteAsync(DeleteCategoryServiceModel model)
        {
            var category = await this.context.Categories.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (category == null)
            {
                throw new CustomException(this.productLocalizer.GetString("CategoryNotFound"), (int)HttpStatusCode.NotFound);
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

        public async Task<CategoryServiceModel> UpdateAsync(UpdateCategoryServiceModel serviceModel)
        {
            var category = await this.context.Categories.FirstOrDefaultAsync(x => x.Id == serviceModel.Id && x.IsActive);

            if (category == null)
            {
                throw new CustomException(this.productLocalizer.GetString("CategoryNotFound"), (int)HttpStatusCode.NotFound);
            }

            var parentCategory = await this.context.Categories.FirstOrDefaultAsync(x => x.Id == serviceModel.ParentId && x.IsActive);

            if (parentCategory == null)
            {
                throw new CustomException(this.productLocalizer.GetString("ParentCategoryNotFound"), (int)HttpStatusCode.NotFound);
            }

            category.Parentid = serviceModel.ParentId;
            category.Level = parentCategory.Level + 1;
            category.IsLeaf = !await this.context.Categories.AnyAsync(x => x.Parentid == category.Id && x.IsActive);
            category.LastModifiedDate = DateTime.UtcNow;

            var categoryTranslation = await this.context.CategoryTranslations.FirstOrDefaultAsync(x => x.CategoryId == serviceModel.Id && x.Language == serviceModel.Language && x.IsActive);

            if (categoryTranslation != null)
            {
                categoryTranslation.Name = serviceModel.Name;
            }
            else
            {
                var newCategoryTranslation = new CategoryTranslation
                {
                    CategoryId = category.Id,
                    Name = serviceModel.Name
                };

                this.context.CategoryTranslations.Add(newCategoryTranslation.FillCommonProperties());
            }
            
            categoryTranslation.LastModifiedDate = DateTime.UtcNow;

            var categoryImages = this.context.CategoryImages.Where(x => x.CategoryId == serviceModel.Id);

            foreach (var categoryImage in categoryImages)
            {
                this.context.CategoryImages.Remove(categoryImage);
            }

            if (serviceModel.Files != null)
            {
                foreach (var file in serviceModel.Files)
                {
                    var categoryImage = new CategoryImage
                    { 
                        CategoryId = serviceModel.Id.Value,
                        MediaId = file
                    };

                    this.context.CategoryImages.Add(categoryImage.FillCommonProperties());
                }
            }

            await this.context.SaveChangesAsync();

            return await this.GetAsync(new GetCategoryServiceModel { Id = category.Id, Language = serviceModel.Language, OrganisationId = serviceModel.OrganisationId, Username = serviceModel.Username });
        }

        public async Task<CategoryServiceModel> CreateAsync(CreateCategoryServiceModel serviceModel)
        {
            var category = new Category
            {
                Parentid = serviceModel.ParentId,
                IsLeaf = true
            };

            var parentCategory = await this.context.Categories.FirstOrDefaultAsync(x => x.Id == serviceModel.ParentId && x.IsActive);
            category.Level = parentCategory.Level + 1;

            this.context.Categories.Add(category.FillCommonProperties());

            var categoryTranslation = new CategoryTranslation
            { 
                CategoryId = category.Id,
                Name = serviceModel.Name,
                Language = serviceModel.Language
            };

            this.context.CategoryTranslations.Add(categoryTranslation.FillCommonProperties());

            foreach (var file in serviceModel.Files.OrEmptyIfNull())
            {
                var categoryImage = new CategoryImage
                {
                    CategoryId = category.Id,
                    MediaId = file,
                };

                this.context.CategoryImages.Add(categoryImage.FillCommonProperties());
            }

            await this.context.SaveChangesAsync();

            return await this.GetAsync(new GetCategoryServiceModel { Id = category.Id, Language = serviceModel.Language, OrganisationId = serviceModel.OrganisationId, Username = serviceModel.Username });
        }
    }
}
