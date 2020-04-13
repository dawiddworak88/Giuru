using Feature.Client.Models;
using Feature.Client.Validators;
using Foundation.Account.Definitions;
using Foundation.Account.Services;
using Foundation.Account.UserStores;
using Foundation.Database.Areas.Tenants.Entities;
using Foundation.Database.Shared.Repositories;
using Foundation.TenantDatabase.Areas.Accounts.Entities;
using Foundation.TenantDatabase.Shared.Contexts;
using Foundation.TenantDatabase.Shared.Repositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace Feature.Client.Services
{
    public class ClientService : IClientService
    {
        private readonly IGenericRepository<Tenant> tenantRepository;
        private readonly TenantGenericRepositoryFactory genericRepositoryFactory;
        private readonly UserStoreFactory userStoreFactory;
        private readonly TenantDatabaseContextFactory tenantDatabaseContextFactory;
        private readonly IPasswordHasher<ApplicationUser> passwordHasher;
        private readonly IPasswordGenerationService passwordGenerationService;

        public ClientService(
            IGenericRepository<Tenant> tenantRepository,
            TenantGenericRepositoryFactory genericRepositoryFactory,
            UserStoreFactory userStoreFactory,
            TenantDatabaseContextFactory tenantDatabaseContextFactory,
            IPasswordHasher<ApplicationUser> passwordHasher,
            IPasswordGenerationService passwordGenerationService)
        {
            this.tenantRepository = tenantRepository;
            this.genericRepositoryFactory = genericRepositoryFactory;
            this.userStoreFactory = userStoreFactory;
            this.tenantDatabaseContextFactory = tenantDatabaseContextFactory;
            this.passwordHasher = passwordHasher;
            this.passwordGenerationService = passwordGenerationService;
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
                        Host = new MailAddress(model.Email).Host,
                        IsActive = true,
                        LastModifiedBy = model.Username,
                        LastModifiedDate = DateTime.UtcNow,
                        CreatedBy = model.Username,
                        CreatedDate = DateTime.UtcNow
                    };

                    var clientRepository = await this.genericRepositoryFactory.CreateTenantGenericRepository<Foundation.TenantDatabase.Areas.Clients.Entities.Client>(tenant.DatabaseConnectionString);

                    await clientRepository.CreateAsync(client);

                    clientRepository.SaveChanges();

                    var context = await this.tenantDatabaseContextFactory.CreateDbContextAsync(tenant.DatabaseConnectionString);

                    var userStore = this.userStoreFactory.CreateUserStore<ApplicationUser>(context);

                    var password = this.passwordGenerationService.GeneratePassword(PasswordConstants.DefaultMinLength);

                    var user = new ApplicationUser
                    {
                        Client = client,
                        UserName = model.Email,
                        Email = model.Email,
                        NormalizedEmail = model.Email,
                        NormalizedUserName = model.Email,
                        EmailConfirmed = true
                    };

                    user.PasswordHash = this.passwordHasher.HashPassword(user, password);

                    await userStore.CreateAsync(user, new CancellationToken());

                    createClientResultModel.Client = client;
                }
            }

            return createClientResultModel;
        }
    }
}
