using Feature.Order.Models;
using Feature.Order.ResultModels;
using System.Threading.Tasks;

namespace Feature.Order.Services
{
    public interface IOrderService
    {
        Task<OrderValidationResultModel> ValidateOrderAsync(OrderValidationModel validateOrderModel);
    }
}
