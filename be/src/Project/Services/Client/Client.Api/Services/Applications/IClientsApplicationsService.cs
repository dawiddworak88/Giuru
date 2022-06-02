using Client.Api.ServicesModels.Applications;
using Foundation.GenericRepository.Paginations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Api.Services.Applications
{
    public interface IClientsApplicationsService
    {
        Task<Guid> CreateAsync(CreateClientApplicationServiceModel model);
        Task<PagedResults<IEnumerable<ClientApplicationServiceModel>>> GetAsync(GetClientsApplicationsServiceModel model);
        Task<PagedResults<IEnumerable<ClientApplicationServiceModel>>> GetByIds(GetClientsApplicationsByIdsServiceModel model);
        Task<ClientApplicationServiceModel> GetAsync(GetClientApplicationServiceModel model);
    }
}
