using Foundation.GenericRepository.Paginations;
using Ordering.Api.ServicesModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ordering.Api.Services.OrderAttributes
{
    public interface IOrderAttributesService
    {
        Task<Guid> CreateAsync(CreateOrderAttributeServiceModel model);
        Task<Guid> UpdateAsync(UpdateOrderAttributeServiceModel model);
        Task DeleteAsync(DeleteOrderAttributeServiceModel model);
        Task<OrderAttributeServiceModel> GetAsync(GetOrderAttributeServiceModel model);
        PagedResults<IEnumerable<OrderAttributeServiceModel>> Get(GetOrderAttributesServiceModel model);
    }
}
