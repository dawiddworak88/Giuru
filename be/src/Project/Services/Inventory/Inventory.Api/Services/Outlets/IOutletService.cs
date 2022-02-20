using Foundation.GenericRepository.Paginations;
using Inventory.Api.ServicesModels.OutletServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inventory.Api.Services.Outlets
{
    public interface IOutletService
    {
        Task SyncOutletAsync(SyncOutletServiceModel model);
        Task<PagedResults<IEnumerable<SyncOutletItemServiceModel>>> GetAsync(GetOutletsServiceModel model);
        Task DeleteAsync(DeleteOutletServiceModel model);
        Task<Guid> CreateAsync(CreateOutletServiceModel model);
        Task<Guid> UpdateAsync(UpdateOutletServiceModel model);
        Task<OutletServiceModel> GetAsync(GetOutletServiceModel model);
        Task UpdateProductOutlet(Guid? ProductId, string ProductName, string ProductSku);
    }
}
