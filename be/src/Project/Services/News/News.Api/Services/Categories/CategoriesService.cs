using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Extensions;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using News.Api.Infrastructure;
using News.Api.Infrastructure.Entities.Categories;
using News.Api.ServicesModels.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace News.Api.Services.Categories
{
    public class CategoriesService : ICategoriesService
    {
        private readonly NewsContext newsContext;
        private readonly IStringLocalizer<NewsResources> newsLocalizer;

        public CategoriesService(
            NewsContext newsContext,
            IStringLocalizer<NewsResources> newsLocalizer)
        {
            this.newsContext = newsContext;
            this.newsLocalizer = newsLocalizer;
        }

        public async Task<CategoryServiceModel> CreateAsync(CreateCategoryServiceModel model)
        {
            var category = new Category
            {
                ParentCategoryId = model.ParentCategoryId
            };

            this.newsContext.Categories.Add(category.FillCommonProperties());
            var categoryTranslation = new CategoryTranslation
            {
                Name = model.Name,
                Language = model.Language,
                CategoryId = category.Id
            };

            this.newsContext.CategoryTranslations.Add(categoryTranslation.FillCommonProperties());
            await this.newsContext.SaveChangesAsync();

            return await this.GetAsync(new GetCategoryServiceModel { Id = category.Id, Language = model.Language, Username = model.Username, OrganisationId = model.OrganisationId });
        }

        public async Task DeleteAsync(DeleteCategoryServiceModel model)
        {
            var category = await this.newsContext.Categories.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (category is null)
            {
                throw new CustomException(this.newsLocalizer.GetString("CategoryNotFound"), (int)HttpStatusCode.NotFound);
            }

            if (await this.newsContext.Categories.AnyAsync(x => x.ParentCategoryId == category.Id && x.IsActive))
            {
                throw new CustomException(this.newsLocalizer.GetString("SubcategoriesDeleteCategoryConflict"), (int)HttpStatusCode.Conflict);
            }

            if (await this.newsContext.NewsItems.AnyAsync(x => x.CategoryId == category.Id && x.IsActive))
            {
                throw new CustomException(this.newsLocalizer.GetString("CategoryDeleteNewsConflict"), (int)HttpStatusCode.Conflict);
            }

            category.IsActive = false;

            await this.newsContext.SaveChangesAsync();
        }

        public async Task<CategoryServiceModel> GetAsync(GetCategoryServiceModel model)
        {
            var category = from c in this.newsContext.Categories
                           join t in this.newsContext.CategoryTranslations on c.Id equals t.CategoryId into ct
                           from x in ct.DefaultIfEmpty()
                           join pct in this.newsContext.CategoryTranslations on c.ParentCategoryId equals pct.CategoryId into pctg
                           from w in pctg.DefaultIfEmpty()
                           where x.Language == model.Language && c.Id == model.Id && c.IsActive
                           select new CategoryServiceModel
                           {
                                Id = c.Id,
                                Name = x.Name,
                                ParentCategoryId = c.ParentCategoryId,
                                ParentCategoryName = w.Name,
                                LastModifiedDate = c.LastModifiedDate,
                                CreatedDate = c.CreatedDate
                           };

            return category.FirstOrDefault();
        }

        public async Task<PagedResults<IEnumerable<CategoryServiceModel>>> GetAsync(GetCategoriesServiceModel model)
        {
            var categories = from c in this.newsContext.Categories
                             join t in this.newsContext.CategoryTranslations on c.Id equals t.CategoryId into ct
                             from x in ct.DefaultIfEmpty()
                             join pct in this.newsContext.CategoryTranslations on c.ParentCategoryId equals pct.CategoryId into pctg
                             from w in pctg.DefaultIfEmpty()
                             where x.Language == model.Language && (w.Language == model.Language || w.Language == null) && c.IsActive
                             select new CategoryServiceModel
                             {
                                 Id = c.Id,
                                 Name = x.Name,
                                 ParentCategoryId = c.ParentCategoryId,
                                 ParentCategoryName = w.Name,
                                 LastModifiedDate = c.LastModifiedDate,
                                 CreatedDate = c.CreatedDate
                             };
            
            if (!string.IsNullOrWhiteSpace(model.SearchTerm))
            {
                categories = categories.Where(x => x.Name.StartsWith(model.SearchTerm));
            }

            categories = categories.ApplySort(model.OrderBy);

            return categories.PagedIndex(new Pagination(categories.Count(), model.ItemsPerPage), model.PageIndex);
        }

        public async Task<CategoryServiceModel> UpdateAsync(UpdateCategoryServiceModel model)
        {
            var category = await this.newsContext.Categories.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);
            if (category is null)
            {
                throw new CustomException(this.newsLocalizer.GetString("CategoryNotFound"), (int)HttpStatusCode.NotFound);
            }

            var parentCategory = await this.newsContext.Categories.FirstOrDefaultAsync(x => x.Id == model.ParentCategoryId && x.IsActive);
            if (parentCategory is  null)
            {
                throw new CustomException(this.newsLocalizer.GetString("ParentCategoryNotFound"), (int)HttpStatusCode.NotFound);
            }

            category.ParentCategoryId = model.ParentCategoryId;
            category.LastModifiedDate = DateTime.UtcNow;

            var categoryTranslation = this.newsContext.CategoryTranslations.FirstOrDefault(x => x.CategoryId == model.Id && x.Language == model.Language && x.IsActive);
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

                this.newsContext.CategoryTranslations.Add(newCategoryTranslation.FillCommonProperties());
            }

            await this.newsContext.SaveChangesAsync();

            return await this.GetAsync(new GetCategoryServiceModel { Id = category.Id, Language = model.Language, OrganisationId = model.OrganisationId, Username = model.Username });
        }
    }
}