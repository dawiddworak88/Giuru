using Client.Api.ServicesModels.Addresses;
using Foundation.GenericRepository.Paginations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Api.Services.Addresses
{
    public interface IClientAddressesService
    {
        Task<Guid> CreateAsync(CreateClientAddressServiceModel model);
        Task<Guid> UpdateAsync(UpdateClientAddressServiceModel model);
        Task DeleteAsync(DeleteClientAddressServiceModel model);
        Task<PagedResults<IEnumerable<ClientAddressServiceModel>>> GetAsync(GetClientAddressesServiceModel model);
        Task<ClientAddressServiceModel> GetAsync(GetClientAddressServiceModel model);
    }
}
