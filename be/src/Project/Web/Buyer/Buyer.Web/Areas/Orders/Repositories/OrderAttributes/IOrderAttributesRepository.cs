using Buyer.Web.Areas.Orders.DomainModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Orders.Repositories.OrderAttributes
{
    public interface IOrderAttributesRepository
    {
        Task<IEnumerable<OrderAttribute>> GetAsync(string token, string language);
    }
}
