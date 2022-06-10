using Identity.Api.ServicesModels.Applications;
using System.Threading.Tasks;

namespace Identity.Api.Services.Applications
{
    public interface IClientsApplicationsService
    {
        Task CreateAsync(CreateClientApplicationServiceModel model);
    }
}
