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
        Task<ClientFieldServiceModel> GetAsync(GetClientFieldDefinitionServiceModel model);
        PagedResults<IEnumerable<ClientFieldServiceModel>> Get(GetClientFieldsServiceModel model);
    }
}
