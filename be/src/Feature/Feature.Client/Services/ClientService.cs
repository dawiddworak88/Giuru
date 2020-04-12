using Feature.Client.Models;
using Feature.Client.Validators;
using Foundation.Database.Areas.Tenants.Entities;
using Foundation.Database.Shared.Repositories;
using Foundation.TenantDatabase.Shared.Repositories;
using System;
using System.Threading.Tasks;

namespace Feature.Client.Services
{
    public class ClientService : IClientService
    {
        private readonly IGenericRepository<Tenant> tenantRepository;
        private readonly TenantGenericRepositoryFactory genericRepositoryFactory;

        public ClientService(
            IGenericRepository<Tenant> tenantRepository,
            TenantGenericRepositoryFactory genericRepositoryFactory)
        {
            this.tenantRepository = tenantRepository;
            this.genericRepositoryFactory = genericRepositoryFactory;
        }

        public async Task<CreateClientResultModel> CreateAsync(CreateClientModel model)
        {
            var validator = new CreateClientModelValidator();

            var validationResult = await validator.ValidateAsync(model);

            var createClientResultModel = new CreateClientResultModel
            {
                ValidationResult = validationResult
            };

            if (validationResult.IsValid)
            {
                var tenant = this.tenantRepository.GetById(model.TenantId);

                if (tenant != null)
                {
                    var client = new Foundation.TenantDatabase.Areas.Clients.Entities.Client
                    {
                        Language = model.Language,
                        Name = model.Name,
                        IsActive = true,
                        LastModifiedBy = model.Username,
                        LastModifiedDate = DateTime.UtcNow,
                        CreatedBy = model.Username,
                        CreatedDate = DateTime.UtcNow
                    };

                    var clientRepository = await this.genericRepositoryFactory.CreateTenantGenericRepository<Foundation.TenantDatabase.Areas.Clients.Entities.Client>(tenant.DatabaseConnectionString);

                    await clientRepository.CreateAsync(client);

                    clientRepository.SaveChanges();

                    createClientResultModel.Client = clientRepository.GetById(client.Id);
                }
            }

            return createClientResultModel;
        }
    }
}
