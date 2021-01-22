using Catalog.Api.ServicesModels.Products;
using Foundation.GenericRepository.Paginations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Api.Services.Products
{
    public interface IProductsService
    {
        Task<ProductServiceModel> CreateAsync(CreateUpdateProductModel model);
        Task IndexAllAsync();
        Task<bool> IsEmptyAsync();
        Task<ProductServiceModel> UpdateAsync(CreateUpdateProductModel model);
        Task<ProductServiceModel> GetByIdAsync(GetProductServiceModel getProductModel);
        Task<PagedResults<IEnumerable<ProductServiceModel>>> GetAsync(GetProductsServiceModel getProductsModel);
        Task<PagedResults<IEnumerable<ProductServiceModel>>> GetByIdsAsync(GetProductsByIdsServiceModel getProductsModel);
        Task DeleteAsync(DeleteProductServiceModel deleteProductModel);
        IEnumerable<string> GetProductSuggestions(GetProductSuggestionsServiceModel model);
    }
}