using Buyer.Web.Areas.Products.DomainModels;
using Buyer.Web.Shared.ViewModels.Catalogs;
using Foundation.GenericRepository.Paginations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.Services.Products
{
    public interface IProductsService
    {
        Task<PagedResults<IEnumerable<CatalogItemViewModel>>> GetProductsAsync(IEnumerable<Guid> ids, Guid? categoryId, Guid? sellerId, string language, string searchTerm, int pageIndex, int itemsPerPage, string token);
        Task<IEnumerable<string>> GetProductSuggestionsAsync(string searchTerm, int size, string language, string token);
        Task<string> GetProductAttributesAsync(IEnumerable<ProductAttribute> productAttributes);
    }
}
