using Catalog.Api.v1.Areas.Categories.Models;
using Catalog.Api.v1.Areas.Categories.ResultModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Api.v1.Areas.Categories.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryResultModel>> GetAsync(GetCategoriesModel model);
        Task<CategoryResultModel> GetAsync(GetCategoryModel model);
    }
}
