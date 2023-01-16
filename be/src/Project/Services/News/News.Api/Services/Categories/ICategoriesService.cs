using Foundation.GenericRepository.Paginations;
using News.Api.ServicesModels.Categories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace News.Api.Services.Categories
{
    public interface ICategoriesService
    {
        Task<CategoryServiceModel> CreateAsync(CreateCategoryServiceModel model);
        Task<CategoryServiceModel> UpdateAsync(UpdateCategoryServiceModel model);
        Task<CategoryServiceModel> GetAsync(GetCategoryServiceModel model);
        PagedResults<IEnumerable<CategoryServiceModel>> Get(GetCategoriesServiceModel model);
        Task DeleteAsync(DeleteCategoryServiceModel model);
    }
}
