using Foundation.GenericRepository.Paginations;
using Inventory.Api.ServicesModels.OutletServices;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inventory.Api.Services.Outlets
{
    public interface IOutletService
    {
        Task<SyncOutletServiceModel> SyncOutletAsync(SyncOutletServiceModel model);
        Task<PagedResults<IEnumerable<SyncOutletItemServiceModel>>> GetAsync(GetOutletsServiceModel model);
        Task DeleteAsync(DeleteOutletServiceModel model);
        Task<Guid> CreateAsync(OutletServiceModel model);
        Task<OutletServiceModel> GetAsync(GetOutletServiceModel model);
    }
}
