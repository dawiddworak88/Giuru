using System;
using System.Threading.Tasks;
using Seller.Web.Areas.Products.DomainModels;

namespace Seller.Web.Areas.Products.Repositories
{
    public interface IProductRepository
    {
        Task<Product> GetProductAsync(string token, string language, Guid? id);
    }
}
