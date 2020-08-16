using Catalog.Api.Infrastructure;
using Catalog.Api.Infrastructure.Categories.Entities;
using Catalog.Api.v1.Areas.Categories.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Catalog.Api.v1.Areas.Categories.ResultModels;

namespace Catalog.Api.v1.Areas.Categories.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly CatalogContext context;

        public CategoryService(CatalogContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<CategoryResultModel>> GetAsync(GetCategoriesModel model)
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
                                Parentid = c.Parentid,
                                SchemaId = c.SchemaId,
                                Name = x.Name,
                                ThumbnailMediaId = y.MediaId
                             };


            return categories;
        }
    }
}
