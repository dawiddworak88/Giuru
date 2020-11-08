using Buyer.Web.Shared.Brands.DomainModels;
using System;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.Brands.Repositories
{
    public interface IBrandRepository
    {
        Task<Brand> GetBrandAsync(Guid? sellerId, string token);
    }
}
