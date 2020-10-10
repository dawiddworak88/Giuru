using Buyer.Web.Shared.Catalogs.ViewModels;
using Foundation.GenericRepository.Paginations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.Services.Products
{
    public interface IProductsService
    {
        Task<PagedResults<IEnumerable<CatalogItemViewModel>>> GetProductsAsync(Guid? categoryId, string language, string searchTerm, int pageIndex, int itemsPerPage, string token);
    }
}
