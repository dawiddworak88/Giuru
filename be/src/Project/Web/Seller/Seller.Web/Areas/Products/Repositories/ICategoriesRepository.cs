using Foundation.GenericRepository.Paginations;
using Seller.Web.Areas.Products.DomainModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Products.Repositories
{
    public interface ICategoriesRepository
    {
        Task<Guid> SaveAsync(string token, string language, Guid? id, Guid? parentCategoryId, string name, IEnumerable<Guid> files, string schema, string uiSchema);
        Task<PagedResults<IEnumerable<Category>>> GetCategoriesAsync(string token, string language, string searchTerm, int pageIndex, int itemsPerPage, string orderBy);
        Task<IEnumerable<Category>> GetAllCategoriesAsync(string token, string language, bool? leafOnly, string orderBy);
        Task DeleteAsync(string token, string language, Guid? id);
        Task<Category> GetCategoryAsync(string token, string language, Guid? id);
        Task<CategorySchema> GetCategorySchemaAsync(string token, string language, Guid? categoryId);
        Task<CategorySchemas> GetCategorySchemasAsync(string token, string language, Guid? categoryId);
        Task SaveAsync(string token, string language, Guid? id, string schema, string uiSchema);
    }
}
