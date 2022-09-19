using DownloadCenter.Api.Infrastructure;
using DownloadCenter.Api.Infrastructure.Entities.DownloadCenterCategories;
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
        private readonly DownloadCenterContext context;

        public CategoriesService(
            IStringLocalizer<DownloadCenterResources> downloadCenterLocalizer,
            DownloadCenterContext context)
        {
            this.downloadCenterLocalizer = downloadCenterLocalizer;
            this.context = context;
        }

        public async Task<Guid> CreateAsync(CreateCategoryServiceModel model)
        {
            var category = new DownloadCenterCategory
            {
                ParentCategoryId = model.ParentCategoryId,
                IsVisible = model.IsVisible,
                SellerId = model.OrganisationId.Value,
                Order = 0
            };

            await this.context.DownloadCenterCategories.AddAsync(category.FillCommonProperties());

            var categoryTranslation = new DownloadCenterCategoryTranslation
            {
                Name = model.Name,
                Language = model.Language,
                CategoryId = category.Id
            };

            await this.context.DownloadCenterCategoryTranslations.AddAsync(categoryTranslation.FillCommonProperties());
            await this.context.SaveChangesAsync();

            return category.Id;
        }

        public async Task DeleteAsync(DeleteCategoryServiceModel model)
        {
            var category = await this.context.DownloadCenterCategories.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (category is null)
            {
                throw new CustomException(this.downloadCenterLocalizer.GetString("CategoryNotFound"), (int)HttpStatusCode.NoContent);
            }

            if (await this.context.DownloadCenterCategories.AnyAsync(x => x.ParentCategoryId == category.Id && x.IsActive))
            {
                throw new CustomException(this.downloadCenterLocalizer.GetString("SubcategoriesDeleteCategoryConflict"), (int)HttpStatusCode.Conflict);
            }

            if (await this.context.DownloadCenterCategoryFiles.AnyAsync(x => x.CategoryId == model.Id && x.IsActive))
            {
                throw new CustomException(this.downloadCenterLocalizer.GetString("CategoryFileConflict"), (int)HttpStatusCode.Conflict);
            }

            category.IsActive = false;

            await this.context.SaveChangesAsync();
        }

        public async Task<CategoryServiceModel> GetAsync(GetCategoryServiceModel model)
        {
            var category = await this.context.DownloadCenterCategories.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (category is null)
            {
                throw new CustomException(this.downloadCenterLocalizer.GetString("CategoryNotFound"), (int)HttpStatusCode.NoContent);
            }

            var item = new CategoryServiceModel
            {
                Id = category.Id,
                IsVisible = category.IsVisible,
                ParentCategoryId = category.ParentCategoryId,
                LastModifiedDate = category.LastModifiedDate,
                CreatedDate = category.CreatedDate
            };

            var categoryTranslations = await this.context.DownloadCenterCategoryTranslations.FirstOrDefaultAsync(x => x.Language == model.Language && x.CategoryId == category.Id && x.IsActive);

            if (categoryTranslations is null)
            {
                categoryTranslations = await this.context.DownloadCenterCategoryTranslations.FirstOrDefaultAsync(x => x.IsActive);
            }

            item.Name = categoryTranslations?.Name;

            if (category.ParentCategoryId.HasValue)
            {
                var categoryParentTranslations = await this.context.DownloadCenterCategoryTranslations.FirstOrDefaultAsync(x => x.Language == model.Language && x.CategoryId == category.ParentCategoryId && x.IsActive);

                if (categoryParentTranslations is null)
                {
                    categoryParentTranslations = await this.context.DownloadCenterCategoryTranslations.FirstOrDefaultAsync(x => x.CategoryId == category.ParentCategoryId && x.IsActive);
                }

                item.ParentCategoryName = categoryParentTranslations?.Name;
            }

            return item;
        }

        public async Task<PagedResults<IEnumerable<CategoryServiceModel>>> GetAsync(GetCategoriesServiceModel model)
        {
            var categories = this.context.DownloadCenterCategories.Where(x => x.IsActive);

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

                var categoryTranslations = this.context.DownloadCenterCategoryTranslations.FirstOrDefault(x => x.Language == model.Language && x.CategoryId == category.Id && x.IsActive);

                if (categoryTranslations is null)
                {
                    categoryTranslations = this.context.DownloadCenterCategoryTranslations.FirstOrDefault(x => x.IsActive);
                }

                category.Name = categoryTranslations?.Name;

                if (categoryItem.ParentCategoryId.HasValue)
                {
                    var categoryParentTranslations = this.context.DownloadCenterCategoryTranslations.FirstOrDefault(x => x.Language == model.Language && x.CategoryId == category.ParentCategoryId && x.IsActive);

                    if (categoryParentTranslations is null)
                    {
                        categoryParentTranslations = this.context.DownloadCenterCategoryTranslations.FirstOrDefault(x => x.IsActive);
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
            var category = await this.context.DownloadCenterCategories.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (category is null)
            {
                throw new CustomException(this.downloadCenterLocalizer.GetString("CategoryNotFound"), (int)HttpStatusCode.NoContent);
            }

            var categoryTranslation = await this.context.DownloadCenterCategoryTranslations.FirstOrDefaultAsync(x => x.CategoryId == model.Id && x.Language == model.Language && x.IsActive);

            if (categoryTranslation is not null)
            {
                categoryTranslation.Name = model.Name;
                categoryTranslation.LastModifiedDate = DateTime.UtcNow;
            }
            else
            {
                var newCategoryTranslation = new DownloadCenterCategoryTranslation
                {
                    CategoryId = category.Id,
                    Name = model.Name,
                    Language = model.Language
                };

                this.context.DownloadCenterCategoryTranslations.Add(newCategoryTranslation.FillCommonProperties());
            }

            category.ParentCategoryId = model.ParentCategoryId;
            category.IsVisible = model.IsVisible;
            category.LastModifiedDate = DateTime.UtcNow;

            await this.context.SaveChangesAsync();

            return category.Id;
        }
    }
}
