using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Buyer.Web.Areas.Orders.DomainModels;

namespace Buyer.Web.Areas.Orders.Repositories.OrderAttributeValues
{
    public interface IOrderAttributeValuesRepository
    {
        Task<IEnumerable<OrderAttributeValue>> GetAsync(string token, string language, Guid? orderId, Guid? orderItemId);
    }
}
