using System;
using System.Threading.Tasks;
using Seller.Portal.Areas.Products.DomainModels;

namespace Seller.Portal.Areas.Products.Repositories
{
    public interface IProductRepository
    {
        Task<Product> GetProductAsync(string token, string language, Guid? id);
    }
}
