using Catalog.Api.v1.Areas.Orders.Models;
using Catalog.Api.v1.Areas.Orders.ResultModels;
using System.Threading.Tasks;

namespace Catalog.Api.v1.Areas.Orders.Services
{
    public interface IOrderService
    {
        Task<OrderValidationResultModel> ValidateOrderAsync(OrderValidationModel model);
    }
}
