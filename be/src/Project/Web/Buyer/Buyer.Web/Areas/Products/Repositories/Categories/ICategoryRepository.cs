using Buyer.Web.Shared.Brands.DomainModels;
using System;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.Repositories.Categories
{
    public interface ICategoryRepository
    {
        Task<Category> GetCategoryAsync(Guid? categoryId, string token, string language);
    }
}
