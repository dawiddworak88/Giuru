using DownloadCenter.Api.Infrastructure;
using DownloadCenter.Api.Infrastructure.Entities.Categories;
using DownloadCenter.Api.ServicesModels.Categories;
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

namespace DownloadCenter.Api.Services.Categories
{
    public class CategoriesService : ICategoriesService
    {
        private readonly IStringLocalizer<DownloadCenterResources> downloadCenterLocalizer;
        private readonly DownloadContext context;

        public CategoriesService(
            IStringLocalizer<DownloadCenterResources> downloadCenterLocalizer,
            DownloadContext context)
        {
            this.downloadCenterLocalizer = downloadCenterLocalizer;
            this.context = context;
        }

        public async Task<Guid> CreateAsync(CreateCategoryServiceModel model)
        {
            var category = new Category
            {
                ParentCategoryId = model.ParentCategoryId
            };

            this.context.Categories.Add(category.FillCommonProperties());

            var categoryTranslation = new CategoryTranslation
            {
                Name = model.Name,
                Language = model.Language,
                CategoryId = category.Id
            };

            this.context.CategoryTranslations.Add(categoryTranslation.FillCommonProperties());
            await this.context.SaveChangesAsync();

            return category.Id;
        }

        public async Task DeleteAsync(DeleteCategoryServiceModel model)
        {
            var category = await this.context.Categories.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (category is null)
            {
                throw new CustomException(this.downloadCenterLocalizer.GetString("CategoryNotFound"), (int)HttpStatusCode.NotFound);
            }

            if (await this.context.Categories.AnyAsync(x => x.ParentCategoryId == category.Id && x.IsActive))
            {
                throw new CustomException(this.downloadCenterLocalizer.GetString("SubcategoriesDeleteCategoryConflict"), (int)HttpStatusCode.Conflict);
            }

            if (await this.context.Downloads.AnyAsync(x => x.CategoryId == category.Id && x.IsActive))
            {
                throw new CustomException(this.downloadCenterLocalizer.GetString("CategoryDeleteDownloadCenterConflict"), (int)HttpStatusCode.Conflict);
            }

            category.IsActive = false;

            await this.context.SaveChangesAsync();
        }

        public async Task<CategoryServiceModel> GetAsync(GetCategoryServiceModel model)
        {
            var category = await this.context.Categories.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (category is null)
            {
                throw new CustomException(this.downloadCenterLocalizer.GetString("CategoryNotFound"), (int)HttpStatusCode.NotFound);
            }

            var item = new CategoryServiceModel
            {
                Id = category.Id,
                ParentCategoryId = category.ParentCategoryId,
                LastModifiedDate = category.LastModifiedDate,
                CreatedDate = category.CreatedDate
            };

            var categoryTranslations = await this.context.CategoryTranslations.FirstOrDefaultAsync(x => x.Language == model.Language && x.CategoryId == category.Id && x.IsActive);

            if (categoryTranslations is null)
            {
                categoryTranslations = await this.context.CategoryTranslations.FirstOrDefaultAsync(x => x.IsActive);
            }

            item.Name = categoryTranslations?.Name;

            if (category.ParentCategoryId.HasValue)
            {
                var categoryParentTranslations = await this.context.CategoryTranslations.FirstOrDefaultAsync(x => x.Language == model.Language && x.CategoryId == category.ParentCategoryId && x.IsActive);

                if (categoryParentTranslations is null)
                {
                    categoryParentTranslations = await this.context.CategoryTranslations.FirstOrDefaultAsync(x => x.CategoryId == category.ParentCategoryId && x.IsActive);
                }

                item.ParentCategoryName = categoryParentTranslations?.Name;
            }

            return item;
        }

        public async Task<PagedResults<IEnumerable<CategoryServiceModel>>> GetAsync(GetCategoriesServiceModel model)
        {
            var categories = this.context.Categories.Where(x => x.IsActive);

            if (string.IsNullOrWhiteSpace(model.SearchTerm) is false)
            {
                categories = categories.Where(x => x.Translations.Any(x => x.Name.StartsWith(model.SearchTerm)));
            }

            categories = categories.ApplySort(model.OrderBy);

            var pagedResults = categories.PagedIndex(new Pagination(categories.Count(), model.ItemsPerPage), model.PageIndex);

            var pagedCategoriesServiceModel = new PagedResults<IEnumerable<CategoryServiceModel>>(pagedResults.Total, pagedResults.PageSize);

            var categoriesItems = new List<CategoryServiceModel>();

            foreach (var categoryItem in pagedResults.Data.ToList())
            {
                var category = new CategoryServiceModel
                {
                    Id = categoryItem.Id,
                    ParentCategoryId = categoryItem.ParentCategoryId,
                    LastModifiedDate = categoryItem.LastModifiedDate,
                    CreatedDate = categoryItem.CreatedDate
                };

                var categoryTranslations = this.context.CategoryTranslations.FirstOrDefault(x => x.Language == model.Language && x.CategoryId == category.Id && x.IsActive);

                if (categoryTranslations is null)
                {
                    categoryTranslations = this.context.CategoryTranslations.FirstOrDefault(x => x.IsActive);
                }

                category.Name = categoryTranslations?.Name;

                if (categoryItem.ParentCategoryId.HasValue)
                {
                    var categoryParentTranslations = this.context.CategoryTranslations.FirstOrDefault(x => x.Language == model.Language && x.CategoryId == category.ParentCategoryId && x.IsActive);

                    if (categoryParentTranslations is null)
                    {
                        categoryParentTranslations = this.context.CategoryTranslations.FirstOrDefault(x => x.IsActive);
                    }

                    category.ParentCategoryName = categoryParentTranslations?.Name;
                }

                categoriesItems.Add(category);
            };

            pagedCategoriesServiceModel.Data = categoriesItems;

            return pagedCategoriesServiceModel;
        }

        public async Task<Guid> UpdateAsync(UpdateCategoryServiceModel model)
        {
            var category = await this.context.Categories.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (category is null)
            {
                throw new CustomException(this.downloadCenterLocalizer.GetString("CategoryNotFound"), (int)HttpStatusCode.NotFound);
            }

            if (model.ParentCategoryId.HasValue)
            {
                var parentCategory = await this.context.Categories.FirstOrDefaultAsync(x => x.Id == model.ParentCategoryId && x.IsActive);

                if (parentCategory is null)
                {
                    throw new CustomException(this.downloadCenterLocalizer.GetString("ParentCategoryNotFound"), (int)HttpStatusCode.NotFound);
                }

                category.ParentCategoryId = model.ParentCategoryId;
            }

            var categoryTranslation =  await this.context.CategoryTranslations.FirstOrDefaultAsync(x => x.CategoryId == model.Id && x.Language == model.Language && x.IsActive);

            if (categoryTranslation is not null)
            {
                categoryTranslation.Name = model.Name;
                categoryTranslation.LastModifiedDate = DateTime.UtcNow;
            }
            else
            {
                var newCategoryTranslation = new CategoryTranslation
                {
                    CategoryId = category.Id,
                    Name = model.Name
                };

                this.context.CategoryTranslations.Add(newCategoryTranslation.FillCommonProperties());
            }

            category.LastModifiedDate = DateTime.UtcNow;

            await this.context.SaveChangesAsync();

            return category.Id;
        }
    }
}
