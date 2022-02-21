using Foundation.GenericRepository.Paginations;
using Inventory.Api.ServicesModels.OutletServiceModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inventory.Api.Services.OutletItems
{
    public interface IOutletService
    {
        Task<PagedResults<IEnumerable<OutletServiceModel>>> GetAsync(GetOutletsServiceModel model);
        Task<PagedResults<IEnumerable<OutletServiceModel>>> GetByIdsAsync(GetOutletsByIdsServiceModel model);
        Task<OutletServiceModel> UpdateAsync(UpdateOutletServiceModel model);
        Task<OutletServiceModel> GetAsync(GetOutletServiceModel model);
        Task<OutletServiceModel> CreateAsync(CreateOutletServiceModel serviceModel);
        Task<OutletSumServiceModel> GetOutletByProductId(GetOutletByProductIdServiceModel model);
        Task<OutletSumServiceModel> GetOutletByProductSku(GetOutletByProductSkuServiceModel model);
        Task SyncOutletProducts(UpdateOutletProductsServiceModel model);
        Task DeleteAsync(DeleteOutletServiceModel model);
        Task UpdateOutletBasket(Guid? productId, int bookedQuantity);
        Task UpdateOutletProduct(Guid? productId, string productName, string productSku, Guid? organisationId);
        Task<PagedResults<IEnumerable<OutletSumServiceModel>>> GetAvailableProductsOutletsAsync(GetOutletsServiceModel model);
    }
}
