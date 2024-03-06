using Ordering.Api.ServicesModels;
using System;
using System.Threading.Tasks;

namespace Ordering.Api.Services.OrderAttributeOptions
{
    public interface IOrderAttributeOptionsService
    {
        Task<Guid> CreateAsync(CreateOrderAttributeOptionServiceModel model);
        Task<Guid> UpdateAsync(UpdateOrderAttributeOptionServiceModel model);
        Task DeleteAsync(DeleteOrderAttributeOptionServiceModel model);
        Task<OrderAttributeOptionServiceModel> GetAsync(GetOrderAttributeOptionServiceModel model);
    }
}
