using Seller.Web.Shared.DomainModels.Products;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seller.Web.Shared.Repositories.ProductAttributeItems
{
    public interface IProductAttributeItemsRepository
    {
        Task<IEnumerable<ProductAttributeItem>> GetAsync(string language, Guid? productAttributeId);
    }
}
