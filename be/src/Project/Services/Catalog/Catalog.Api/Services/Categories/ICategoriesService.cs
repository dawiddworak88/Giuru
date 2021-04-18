using Catalog.Api.ServicesModels.Categories;
using Foundation.GenericRepository.Paginations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Api.Services.Categories
{
    public interface ICategoriesService
    {
        Task<PagedResults<IEnumerable<CategoryServiceModel>>> GetAsync(GetCategoriesServiceModel model);
        Task<CategoryServiceModel> GetAsync(GetCategoryServiceModel model);
        Task DeleteAsync(DeleteCategoryServiceModel model);
        Task<CategoryServiceModel> UpdateAsync(UpdateCategoryServiceModel model);
        Task<CategoryServiceModel> CreateAsync(CreateCategoryServiceModel model);
        Task<CategorySchemaServiceModel> UpdateCategorySchemaAsync(UpdateCategorySchemaServiceModel model);
        Task<CategorySchemaServiceModel> GetCategorySchemaAsync(GetCategorySchemaServiceModel model);
    }
}
