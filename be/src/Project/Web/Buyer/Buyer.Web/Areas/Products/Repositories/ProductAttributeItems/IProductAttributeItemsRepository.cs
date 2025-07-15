using Buyer.Web.Areas.Products.DomainModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.Repositories.ProductAttributeItems
{
    public interface IProductAttributeItemsRepository
    {
        Task<IEnumerable<ProductAttributeItem>> GetAsync(string language, Guid? productAttributeId);
    }
}
