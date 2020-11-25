using Catalog.Api.Infrastructure;
using Catalog.Api.v1.Areas.Categories.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Catalog.Api.v1.Areas.Categories.ResultModels;
using Microsoft.EntityFrameworkCore;
using Foundation.GenericRepository.Paginations;

namespace Catalog.Api.v1.Areas.Categories.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly CatalogContext context;

        public CategoryService(CatalogContext context)
        {
            this.context = context;
        }

        public async Task<PagedResults<IEnumerable<CategoryResultModel>>> GetAsync(GetCategoriesModel model)
        {
            var categories = from c in this.context.Categories
                             join t in this.context.CategoryTranslations on c.Id equals t.CategoryId into ct
                             from x in ct.DefaultIfEmpty()
                             join m in this.context.CategoryImages on c.Id equals m.CategoryId into cm
                             from y in cm.DefaultIfEmpty()
                             where x.Language == model.Language && c.IsActive
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

            if (!string.IsNullOrWhiteSpace(model.SearchTerm))
            {
                categories = categories.Where(x => x.Name.StartsWith(model.SearchTerm));
            }

            if (model.Level.HasValue)
            {
                categories = categories.Where(x => x.Level == model.Level.Value);
            }

            var pagedCategories = categories.PagedIndex(new Pagination(categories.Count(), model.ItemsPerPage), model.PageIndex);

            var categoriesList = pagedCategories.Data.ToList();

            foreach (var categoryItem in categoriesList)
            {
                if (categoryItem.ParentId.HasValue)
                {
                    var parentCategoryItemTranslation = await this.context.CategoryTranslations.FirstOrDefaultAsync(x => x.CategoryId == categoryItem.ParentId && x.Language == model.Language);

                    if (parentCategoryItemTranslation != null)
                    {
                        categoryItem.ParentCategoryName = parentCategoryItemTranslation.Name;
                    }
                }
            }

            pagedCategories.Data = categoriesList;

            return pagedCategories;
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
    }
}
