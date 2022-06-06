using Client.Api.ServicesModels.Roles;
using Foundation.GenericRepository.Paginations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Api.Services.Roles
{
    public interface IClientRolesService
    {
        Task<Guid> CreateAsync(CreateClientRoleServiceModel model);
        Task<Guid> UpdateAsync(UpdateClientRoleServiceModel model);
        Task<PagedResults<IEnumerable<ClientRoleServiceModel>>> GetByIdsAsync(GetClientRolesByIdsServiceModel model);
        Task<PagedResults<IEnumerable<ClientRoleServiceModel>>> GetAsync(GetClientRolesServiceModel model);
    }
}
