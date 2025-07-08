using Seller.Web.Areas.Products.DomainModels;
using System.Collections.Generic;

namespace Seller.Web.Shared.Services.Products
{
    public interface IProductsService
    {
        string GetFirstAvailableAttributeValue(IEnumerable<ReadProductAttribute> attributes, string possibleKeys);
        string GetSleepAreaSize(IEnumerable<ReadProductAttribute> attributes);
        string GetSize(IEnumerable<ReadProductAttribute> attributes);
    }
}
