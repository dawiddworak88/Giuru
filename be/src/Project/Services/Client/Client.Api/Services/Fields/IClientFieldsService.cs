using Client.Api.ServicesModels.FieldOptions;
using Client.Api.ServicesModels.Fields;
using Foundation.GenericRepository.Paginations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Api.Services.Fields
{
    public interface IClientFieldsService
    {
        Task<Guid> CreateAsync(CreateClientFieldServiceModel model);
        Task<ClientFieldOptionServiceModel> GetAsync(GetClientFieldDefinitionServiceModel model);
        PagedResults<IEnumerable<ClientFieldOptionServiceModel>> Get(GetClientFieldsServiceModel model);
        Task DeleteAsync(DeleteClientFieldServiceModel model);
        Task<Guid> UpdateAsync(UpdateClientFieldServiceModel model);
    }
}
