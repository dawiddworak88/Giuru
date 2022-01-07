using Foundation.GenericRepository.Paginations;
using Inventory.Api.ServicesModels;
using Inventory.Api.ServicesModels.WarehouseServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inventory.Api.Services.Warehouses
{
    public interface IWarehouseService
    {
        Task<PagedResults<IEnumerable<WarehouseServiceModel>>> GetAsync(GetWarehousesServiceModel model);
        Task<WarehouseServiceModel> GetAsync(GetWarehouseServiceModel model);
        Task<WarehouseServiceModel> GetAsync(GetWarehouseByNameServiceModel model);
        Task<PagedResults<IEnumerable<WarehouseServiceModel>>> GetByIdsAsync(GetWarehousesByIdsServiceModel model);
        Task<WarehouseServiceModel> UpdateAsync(UpdateWarehouseServiceModel serviceModel);
        Task<WarehouseServiceModel> CreateAsync(CreateWarehouseServiceModel serviceModel);
        Task DeleteAsync(DeleteWarehouseServiceModel model);
    }
}
