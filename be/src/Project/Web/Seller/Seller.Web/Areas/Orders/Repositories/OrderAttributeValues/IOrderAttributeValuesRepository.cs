using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Seller.Web.Areas.Orders.DomainModels;

namespace Seller.Web.Areas.Orders.Repositories.OrderAttributeValues
{
    public interface IOrderAttributeValuesRepository
    {
        Task<IEnumerable<OrderAttributeValue>> GetAsync(string token, string language, Guid? orderId, Guid? orderItemId);
        Task BatchAsync(string token, string language, IEnumerable<ApiOrderAttributeValue> values);
    }
}