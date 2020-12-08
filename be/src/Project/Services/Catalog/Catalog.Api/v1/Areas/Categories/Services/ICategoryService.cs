using Catalog.Api.v1.Areas.Categories.Models;
using Catalog.Api.v1.Areas.Categories.ResultModels;
using Foundation.GenericRepository.Paginations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Api.v1.Areas.Categories.Services
{
    public interface ICategoryService
    {
        Task<PagedResults<IEnumerable<CategoryResultModel>>> GetAsync(GetCategoriesModel model);
        Task<CategoryResultModel> GetAsync(GetCategoryModel model);
        Task DeleteAsync(DeleteCategoryModel model);
        Task<CategoryResultModel> UpdateAsync(UpdateCategoryModel serviceModel);
        Task<CategoryResultModel> CreateAsync(CreateCategoryModel serviceModel);
    }
}
