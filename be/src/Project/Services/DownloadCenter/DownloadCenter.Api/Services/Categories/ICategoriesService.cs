using DownloadCenter.Api.ServicesModels.Categories;
using Foundation.GenericRepository.Paginations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DownloadCenter.Api.Services.Categories
{
    public interface ICategoriesService
    {
        PagedResults<IEnumerable<CategoryServiceModel>> Get(GetCategoriesServiceModel model);
        Task<Guid> CreateAsync(CreateCategoryServiceModel model);
        Task<Guid> UpdateAsync(UpdateCategoryServiceModel model);
        Task<CategoryServiceModel> GetAsync(GetCategoryServiceModel model);
        Task DeleteAsync(DeleteCategoryServiceModel model);
    }
}
