using Buyer.Web.Areas.Products.DomainModels;
using Buyer.Web.Shared.ViewModels.Catalogs;
using Foundation.GenericRepository.Paginations;
using Foundation.Search.Models;
using Foundation.Search.Paginations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.Services.Products
{
    public interface IProductsService
    {
        Task<PagedResults<IEnumerable<CatalogItemViewModel>>> GetProductsAsync(IEnumerable<Guid> ids, Guid? categoryId, Guid? sellerId, string language, string searchTerm, bool? hasPrimaryProduct, int pageIndex, int itemsPerPage, string token, string orderBy);
        Task<PagedResultsWithFilters<IEnumerable<CatalogItemViewModel>>> GetProductsAsync(
            string token, 
            string language, 
            IEnumerable<Guid> ids,
            QueryFilters filters,
            string searchTerm, 
            int pageIndex, 
            int itemsPerPage, 
            string orderBy);
        Task<IEnumerable<string>> GetProductSuggestionsAsync(string searchTerm, int size, string language, string token);
        Task<string> GetProductAttributesAsync(IEnumerable<ProductAttribute> productAttributes);
        string GetSleepAreaSize(IEnumerable<ProductAttribute> attributes);
        string GetSize(IEnumerable<ProductAttribute> attributes);
        string GetFirstAvailableAttributeValue(IEnumerable<ProductAttribute> attributes, string possibleKeys);
    }
}
