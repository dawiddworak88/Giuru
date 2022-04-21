using Foundation.GenericRepository.Paginations;
using Inventory.Api.ServicesModels;
using Inventory.Api.ServicesModels.InventoryServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inventory.Api.Services
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
        Task SyncInventoryProducts(UpdateProductsInventoryServiceModel model);
        Task DeleteAsync(DeleteInventoryServiceModel model);
        Task UpdateInventoryBasket(Guid? ProductId, int BookedQuantity);
        Task UpdateInventoryEan(Guid? ProductId, string ean);
        Task UpdateInventoryProduct(Guid? ProductId, string ProductName, string ProductSku, Guid? OrganisationId);
        Task<PagedResults<IEnumerable<InventorySumServiceModel>>> GetAvailableProductsInventoriesAsync(GetInventoriesServiceModel serviceModel);
    }
}
