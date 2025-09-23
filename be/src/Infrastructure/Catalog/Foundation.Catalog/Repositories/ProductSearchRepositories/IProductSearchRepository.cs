﻿using Foundation.Catalog.SearchModels.Products;
using Foundation.Search.Models;
using Foundation.GenericRepository.Paginations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Foundation.Search.Paginations;

namespace Foundation.Catalog.Repositories.ProductSearchRepositories
{
    public interface IProductSearchRepository
    {
        Task<PagedResults<IEnumerable<ProductSearchModel>>> GetAsync(
            string language, 
            Guid? categoryId, 
            Guid? sellerId, 
            bool? hasPrimaryProduct,
            bool? isNew,
            string searchTerm,
            int? pageIndex, 
            int? itemsPerPage,
            string orderBy);
        Task<PagedResults<IEnumerable<ProductSearchModel>>> GetAsync(string language, Guid? organisationId, IEnumerable<Guid> ids, string orderBy);
        Task<PagedResults<IEnumerable<ProductSearchModel>>> GetAsync(string language, Guid? organisationId, IEnumerable<string> skus, string orderBy);
        Task<PagedResults<IEnumerable<ProductSearchModel>>> GetProductVariantsAsync(Guid id, string language, Guid? organisationId);
        Task<PagedResultsWithFilters<IEnumerable<ProductSearchModel>>> GetPagedResultsWithFilters(
            string langauge,
            Guid? organisationId,
            int? pageIndex,
            int? itemsPerPage,
            string orderBy,
            QueryFilters filters);
        Task<PagedResultsWithFilters<IEnumerable<ProductSearchModel>>> GetPagedResultsWithFilters(
            string langauge,
            IEnumerable<Guid> ids,
            Guid? organisationId,
            string orderBy,
            QueryFilters filters);
        Task<ProductSearchModel> GetByIdAsync(Guid id, string language, Guid? organisationId);
        Task<ProductSearchModel> GetBySkuAsync(string sku, string language, Guid? organisationId);
        Task<int?> CountAllAsync();
        IEnumerable<string> GetProductSuggestions(string searchTerm, int size, string language, Guid? organisationId);
    }
}
