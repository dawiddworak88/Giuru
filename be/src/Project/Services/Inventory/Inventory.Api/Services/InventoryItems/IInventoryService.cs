using Foundation.GenericRepository.Paginations;
using Inventory.Api.ServicesModels.InventoryServiceModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inventory.Api.Services.InventoryItems
{
    public interface IInventoryService
    {
        Task<PagedResults<IEnumerable<InventoryServiceModel>>> GetAsync(GetInventoriesServiceModel getInventoriesModel);
        Task<PagedResults<IEnumerable<InventoryServiceModel>>> GetByIdsAsync(GetInventoriesByIdsServiceModel model);
        Task<InventoryServiceModel> UpdateAsync(UpdateInventoryServiceModel serviceModel);
        Task<InventoryServiceModel> GetAsync(GetInventoryServiceModel model);
        Task<InventoryServiceModel> CreateAsync(CreateInventoryServiceModel serviceModel);
        Task<InventorySumServiceModel> GetInventoryByProductId(GetInventoryByProductIdServiceModel model);
        Task<InventorySumServiceModel> GetInventoryByProductSku(GetInventoryByProductSkuServiceModel model);
        Task SyncProductsInventories(UpdateProductsInventoryServiceModel model);
        Task DeleteAsync(DeleteInventoryServiceModel model);
        Task UpdateInventoryQuantity(Guid? ProductId, double BookedQuantity);
        Task<PagedResults<IEnumerable<InventorySumServiceModel>>> GetAvailableProductsInventoriesAsync(GetInventoriesServiceModel serviceModel);
    }
}
