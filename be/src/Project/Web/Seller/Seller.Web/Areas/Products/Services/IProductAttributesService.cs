using System;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Products.Services
{
    public interface IProductAttributesService
    {
        Task DeleteAsync(string token, string language, Guid? id);
    }
}
