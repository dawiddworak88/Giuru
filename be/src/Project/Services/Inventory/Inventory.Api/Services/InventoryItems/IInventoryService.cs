using Foundation.GenericRepository.Paginations;
using Inventory.Api.ServicesModels.InventoryServiceModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inventory.Api.Services.InventoryItems
{
    public interface IInventoryService
    {
        PagedResults<IEnumerable<InventoryServiceModel>> Get(GetInventoriesServiceModel getInventoriesModel);
        PagedResults<IEnumerable<InventoryServiceModel>> GetByIds(GetInventoriesByIdsServiceModel model);
        Task<InventoryServiceModel> UpdateAsync(UpdateInventoryServiceModel serviceModel);
        InventoryServiceModel Get(GetInventoryServiceModel model);
        Task<InventoryServiceModel> CreateAsync(CreateInventoryServiceModel serviceModel);
        Task<InventorySumServiceModel> GetInventoryByProductId(GetInventoryByProductIdServiceModel model);
        Task<InventorySumServiceModel> GetInventoryByProductSku(GetInventoryByProductSkuServiceModel model);
        Task SyncProductsInventories(UpdateProductsInventoryServiceModel model);
        Task DeleteAsync(DeleteInventoryServiceModel model);
        Task UpdateInventoryQuantity(Guid? ProductId, double BookedQuantity);
        PagedResults<IEnumerable<InventorySumServiceModel>> GetAvailableProductsInventories(GetInventoriesServiceModel serviceModel);
    }
}
