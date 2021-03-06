using Buyer.Web.Shared.ViewModels.Catalogs;
using Foundation.GenericRepository.Paginations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.Services.Products
{
    public interface IProductsService
    {
        Task<PagedResults<IEnumerable<CatalogItemViewModel>>> GetProductsAsync(Guid? categoryId, Guid? sellerId, string language, string searchTerm, int pageIndex, int itemsPerPage, string token);
        Task<IEnumerable<string>> GetProductSuggestionsAsync(string searchTerm, int size, string language, string token);
    }
}
