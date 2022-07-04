using Download.Api.ServicesModels.Categories;
using Foundation.GenericRepository.Paginations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Download.Api.Services.Categories
{
    public interface ICategoriesService
    {
        Task<PagedResults<IEnumerable<CategoryServiceModel>>> GetAsync(GetCategoriesServiceModel model);
        Task<Guid> CreateAsync(CreateCategoryServiceModel model);
        Task<Guid> UpdateAsync(UpdateCategoryServiceModel model);
        Task<CategoryServiceModel> GetAsync(GetCategoryServiceModel model);
        Task DeleteAsync(DeleteCategoryServiceModel model);
    }
}
