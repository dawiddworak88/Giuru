using Catalog.Api.ServicesModels.Products;
using Foundation.GenericRepository.Paginations;
using Foundation.Search.Paginations;
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
        void TriggerCatalogIndexRebuild(RebuildCatalogIndexServiceModel model);
        Task<PagedResults<IEnumerable<ProductFileServiceModel>>> GetProductFiles(GetProductFilesServiceModel model);
        Task<PagedResultsWithFilters<IEnumerable<ProductServiceModel>>> GetPagedResultsWithFilters(SearchProductsServiceModel model);
        Task<PagedResultsWithFilters<IEnumerable<ProductServiceModel>>> GetPagedResultsWithFiltersByIds(SearchProductsByIdsServiceModel model);
    }
}