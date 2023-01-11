using Foundation.GenericRepository.Paginations;
using Inventory.Api.ServicesModels.OutletServiceModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inventory.Api.Services.OutletItems
{
    public interface IOutletService
    {
        PagedResults<IEnumerable<OutletServiceModel>> Get(GetOutletsServiceModel model);
        PagedResults<IEnumerable<OutletServiceModel>> GetByIds(GetOutletsByIdsServiceModel model);
        Task<OutletServiceModel> UpdateAsync(UpdateOutletServiceModel model);
        OutletServiceModel Get(GetOutletServiceModel model);
        Task<OutletServiceModel> CreateAsync(CreateOutletServiceModel serviceModel);
        Task<OutletSumServiceModel> GetOutletByProductId(GetOutletByProductIdServiceModel model);
        Task<OutletSumServiceModel> GetOutletByProductSku(GetOutletByProductSkuServiceModel model);
        Task SyncProductsOutlet(UpdateOutletProductsServiceModel model);
        Task DeleteAsync(DeleteOutletServiceModel model);
        Task UpdateOutletQuantity(Guid? productId, double bookedQuantity);
        PagedResults<IEnumerable<OutletSumServiceModel>> GetAvailableProductsOutlets(GetOutletsServiceModel model);
    }
}
