using Buyer.Web.Areas.Products.DomainModels;
using System;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.Repositories.Brands
{
    public interface IBrandRepository
    {
        Task<Brand> GetBrandAsync(Guid? sellerId, string token);
    }
}
