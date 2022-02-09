using Foundation.GenericRepository.Paginations;
using Inventory.Api.ServicesModels.OutletServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inventory.Api.Services.Outlets
{
    public interface IOutletService
    {
        Task<OutletServiceModel> SyncOutletAsync(OutletServiceModel model);
        Task<PagedResults<IEnumerable<OutletItemServiceModel>>> GetAsync(GetOutletsServiceModel model);
    }
}
