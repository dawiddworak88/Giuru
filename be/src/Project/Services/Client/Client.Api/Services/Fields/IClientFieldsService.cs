using Client.Api.ServicesModels.Fields;
using System;
using System.Threading.Tasks;

namespace Client.Api.Services.Fields
{
    public interface IClientFieldsService
    {
        Task<Guid> CreateAsync(CreateClientFieldServiceModel model);
        Task<ClientFieldServiceModel> GetAsync(GetClientFieldDefinitionServiceModel model);
    }
}
