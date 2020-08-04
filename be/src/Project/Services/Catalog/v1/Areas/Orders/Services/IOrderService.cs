using Catalog.Api.v1.Areas.Schemas.Models;
using Catalog.Api.v1.Areas.Schemas.ResultModels;
using System.Threading.Tasks;

namespace Catalog.Api.v1.Areas.Schemas.Services
{
    public interface IOrderService
    {
        Task<OrderValidationResultModel> ValidateOrderAsync(OrderValidationModel model);
    }
}
