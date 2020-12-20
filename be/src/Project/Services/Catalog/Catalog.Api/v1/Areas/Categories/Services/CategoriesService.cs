using Catalog.Api.Infrastructure;
using Catalog.Api.v1.Areas.Categories.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Catalog.Api.v1.Areas.Categories.ResultModels;
using Microsoft.EntityFrameworkCore;
using Foundation.GenericRepository.Paginations;
using Foundation.Extensions.Exceptions;
using System.Net;
using Microsoft.Extensions.Localization;
using Foundation.Localization;
using Foundation.Localization.Services;
using Catalog.Api.Infrastructure.Categories.Entities;
using System;
using Catalog.Api.Infrastructure.Categories.Entites;
using Foundation.GenericRepository.Helpers;
using Foundation.Extensions.ExtensionMethods;

namespace Catalog.Api.v1.Areas.Categories.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly CatalogContext context;
        private readonly ICultureService cultureService;
        private readonly IStringLocalizer<ProductResources> productLocalizer;

        public CategoriesService(
            CatalogContext context,
            ICultureService cultureService,
            IStringLocalizer<ProductResources> productLocalizer)
        {
            this.context = context;
            this.cultureService = cultureService;
            this.productLocalizer = productLocalizer;
        }

        public async Task<PagedResults<IEnumerable<CategoryResultModel>>> GetAsync(GetCategoriesModel model)
        {
            var categories = from c in this.context.Categories
                             join t in this.context.CategoryTranslations on c.Id equals t.CategoryId into ct
                             from x in ct.DefaultIfEmpty()
                             join m in this.context.CategoryImages on c.Id equals m.CategoryId into cm
                             from y in cm.DefaultIfEmpty()
                             join pct in this.context.CategoryTranslations on c.Parentid equals pct.CategoryId into pctg
                             from w in pctg.DefaultIfEmpty()
                             where x.Language == model.Language && (w.Language == model.Language || w.Language == null) && c.IsActive
                             orderby c.Order
                             select new CategoryResultModel
                             {
                                 Id = c.Id,
                                 Order = c.Order,
                                 Level = c.Level,
                                 IsLeaf = c.IsLeaf,
                                 ParentId = c.Parentid,
                                 SchemaId = c.SchemaId,
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

            return categories.PagedIndex(new Pagination(categories.Count(), model.ItemsPerPage), model.PageIndex);
        }

        public async Task<CategoryResultModel> GetAsync(GetCategoryModel model)
        {
            var categories = from c in this.context.Categories
                             join t in this.context.CategoryTranslations on c.Id equals t.CategoryId into ct
                             from x in ct.DefaultIfEmpty()
                             join m in this.context.CategoryImages on c.Id equals m.CategoryId into cm
                             from y in cm.DefaultIfEmpty()
                             where x.Language == model.Language && c.Id == model.Id && c.IsActive
                             orderby c.Order
                             select new CategoryResultModel
                             {
                                 Id = c.Id,
                                 Order = c.Order,
                                 Level = c.Level,
                                 IsLeaf = c.IsLeaf,
                                 ParentId = c.Parentid,
                                 SchemaId = c.SchemaId,
                                 Name = x.Name,
                                 ThumbnailMediaId = y.MediaId
                             };

            return await categories.FirstOrDefaultAsync();
        }

        public async Task DeleteAsync(DeleteCategoryModel model)
        {
            this.cultureService.SetCulture(model.Language);

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

        public async Task<CategoryResultModel> UpdateAsync(UpdateCategoryModel serviceModel)
        {
            this.cultureService.SetCulture(serviceModel.Language);

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

            categoryTranslation.Name = serviceModel.Name;
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

                    this.context.CategoryImages.Add(EntityHelper.SeedEntity(categoryImage));
                }
            }

            await this.context.SaveChangesAsync();

            return await this.GetAsync(new GetCategoryModel { Id = category.Id, Language = serviceModel.Language, OrganisationId = serviceModel.OrganisationId, Username = serviceModel.Username });
        }

        public async Task<CategoryResultModel> CreateAsync(CreateCategoryModel serviceModel)
        {
            this.cultureService.SetCulture(serviceModel.Language);

            var category = new Category
            {
                Parentid = serviceModel.ParentId,
                IsLeaf = true
            };

            var parentCategory = await this.context.Categories.FirstOrDefaultAsync(x => x.Id == serviceModel.ParentId && x.IsActive);
            category.Level = parentCategory.Level + 1;

            this.context.Categories.Add(EntityHelper.SeedEntity(category));

            var categoryTranslation = new CategoryTranslation
            { 
                CategoryId = category.Id,
                Name = serviceModel.Name,
                Language = serviceModel.Language
            };

            this.context.CategoryTranslations.Add(EntityHelper.SeedEntity(categoryTranslation));

            foreach (var file in serviceModel.Files.OrEmptyIfNull())
            {
                var categoryImage = new CategoryImage
                {
                    CategoryId = category.Id,
                    MediaId = file,
                };

                this.context.CategoryImages.Add(EntityHelper.SeedEntity(categoryImage));
            }

            await this.context.SaveChangesAsync();

            return await this.GetAsync(new GetCategoryModel { Id = category.Id, Language = serviceModel.Language, OrganisationId = serviceModel.OrganisationId, Username = serviceModel.Username });
        }
    }
}
