using Foundation.GenericRepository.Paginations;
using Ordering.Api.ServicesModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ordering.Api.Services.OrderAttributeValues
{
    public interface IOrderAttributeValuesService
    {
        Task BatchAsync(CreateBatchOrderAttributeValuesServiceModel model);
        PagedResults<IEnumerable<OrderAttributeValueServiceModel>> Get(GetOrderAttributeValuesServiceModel model);
    }
}
