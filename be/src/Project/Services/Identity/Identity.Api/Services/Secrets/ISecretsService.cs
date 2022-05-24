using Identity.Api.ServicesModels.Secrets;
using System;
using System.Threading.Tasks;

namespace Identity.Api.Services.Secrets
{
    public interface ISecretsService
    {
        Task<SecretServiceModel> CreateAsync(CreateSecretServiceModel model);
        Task<Guid> GetAsync(GetSecretServiceModel model);
    }
}
