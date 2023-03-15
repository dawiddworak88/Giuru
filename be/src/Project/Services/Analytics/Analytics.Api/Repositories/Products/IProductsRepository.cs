using Analytics.Api.DomainModels;
using System;
using System.Threading.Tasks;

namespace Analytics.Api.Repositories.Products
{
    public interface IProductsRepository
    {
        Task<Product> GetAsync(string token, string language, Guid? id);
    }
}
