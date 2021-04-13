using Buyer.Web.Shared.DomainModels.Brands;
using System;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.Repositories.Brands
{
    public interface IBrandRepository
    {
        Task<Brand> GetBrandAsync(Guid? sellerId, string token, string language);
    }
}
