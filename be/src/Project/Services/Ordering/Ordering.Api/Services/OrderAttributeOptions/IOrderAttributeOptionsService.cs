using Foundation.GenericRepository.Paginations;
using Ordering.Api.ServicesModels.OrderAttributeOptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ordering.Api.Services.OrderAttributeOptions
{
    public interface IOrderAttributeOptionsService
    {
        Task<Guid> CreateAsync(CreateOrderAttributeOptionServiceModel model);
        Task<Guid> UpdateAsync(UpdateOrderAttributeOptionServiceModel model);
        Task DeleteAsync(DeleteOrderAttributeOptionServiceModel model);
        Task<OrderAttributeOptionServiceModel> GetAsync(GetOrderAttributeOptionServiceModel model);
        PagedResults<IEnumerable<OrderAttributeOptionServiceModel>> Get(GetOrderAttributeOptionsServiceModel model);
    }
}
