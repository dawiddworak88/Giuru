using Catalog.Api.ServicesModels.Products;
using Foundation.GenericRepository.Paginations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Api.Services.Products
{
    public interface IProductsService
    {
        Task<Guid?> CreateAsync(CreateUpdateProductModel model);
        Task<bool> IsEmptyAsync();
        Task<Guid?> UpdateAsync(CreateUpdateProductModel model);
        Task<ProductServiceModel> GetByIdAsync(GetProductByIdServiceModel getProductModel);
        Task<ProductServiceModel> GetBySkuAsync(GetProductBySkuServiceModel serviceModel);
        Task<PagedResults<IEnumerable<ProductServiceModel>>> GetAsync(GetProductsServiceModel getProductsModel);
        Task<PagedResults<IEnumerable<ProductServiceModel>>> GetByIdsAsync(GetProductsByIdsServiceModel getProductsModel);
        Task<PagedResults<IEnumerable<ProductServiceModel>>> GetBySkusAsync(GetProductsBySkusServiceModel model);
        Task DeleteAsync(DeleteProductServiceModel deleteProductModel);
        IEnumerable<string> GetProductSuggestions(GetProductSuggestionsServiceModel model);
        Task TriggerCatalogIndexRebuildAsync(RebuildCatalogIndexServiceModel model);
    }
}