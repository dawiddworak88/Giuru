using Feature.Client.Definitions;
using Feature.Client.Models;
using Feature.Client.ResultModels;
using Feature.Client.Validators;
using Foundation.Account.Definitions;
using Foundation.Account.Services;
using Foundation.Account.UserStores;
using Foundation.Database.Areas.Tenants.Entities;
using Foundation.Database.Shared.Repositories;
using Foundation.GenericRepository.Services;
using Foundation.Localization;
using Foundation.Localization.Definitions;
using Foundation.Localization.Services;
using Foundation.Mailing.Configurations;
using Foundation.Mailing.Models;
using Foundation.Mailing.Services;
using Foundation.TenantDatabase.Areas.Accounts.Entities;
using Foundation.TenantDatabase.Shared.Contexts;
using Foundation.TenantDatabase.Shared.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
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
        private readonly IMailingService mailingService;
        private readonly MailingConfiguration mailingConfiguration;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly ICultureService cultureService;
        private readonly IEntityService entityService;

        public ClientService(
            IGenericRepository<Tenant> tenantRepository,
            TenantGenericRepositoryFactory genericRepositoryFactory,
            UserStoreFactory userStoreFactory,
            TenantDatabaseContextFactory tenantDatabaseContextFactory,
            IPasswordHasher<ApplicationUser> passwordHasher,
            IPasswordGenerationService passwordGenerationService,
            IMailingService mailingService, 
            IOptionsMonitor<MailingConfiguration> mailingConfiguration,
            IStringLocalizer<GlobalResources> globalLocalizer,
            ICultureService cultureService,
            IEntityService entityService)
        {
            this.tenantRepository = tenantRepository;
            this.genericRepositoryFactory = genericRepositoryFactory;
            this.userStoreFactory = userStoreFactory;
            this.tenantDatabaseContextFactory = tenantDatabaseContextFactory;
            this.passwordHasher = passwordHasher;
            this.passwordGenerationService = passwordGenerationService;
            this.mailingService = mailingService;
            this.mailingConfiguration = mailingConfiguration.CurrentValue;
            this.globalLocalizer = globalLocalizer;
            this.cultureService = cultureService;
            this.entityService = entityService;
        }

        public async Task<CreateClientResultModel> CreateAsync(CreateClientModel model)
        {
            var validator = new CreateClientModelValidator();

            var validationResult = await validator.ValidateAsync(model);

            var createClientResultModel = new CreateClientResultModel();

            if (!validationResult.IsValid)
            {
                createClientResultModel.Errors.AddRange(validationResult.Errors.Select(x => x.ErrorMessage));
                return createClientResultModel;
            }

            var tenant = this.tenantRepository.GetById(model.TenantId.Value);

            if (tenant == null)
            {
                createClientResultModel.Errors.Add(Foundation.Extensions.Definitions.ErrorConstants.NoTenant);
                return createClientResultModel;
            }

            var host = new MailAddress(model.Email).Host;

            var client = new Foundation.TenantDatabase.Areas.Clients.Entities.Client
            {
                Language = model.ClientPreferredLanguage,
                Name = model.Name,
                Host = host
            };

            var clientRepository = await this.genericRepositoryFactory.CreateTenantGenericRepository<Foundation.TenantDatabase.Areas.Clients.Entities.Client>(tenant.DatabaseConnectionString);

            this.cultureService.SetCulture(model.ClientPreferredLanguage.ToLowerInvariant());

            var existingClients = clientRepository.Get(x => x.Host == host);

            if (!existingClients.Any())
            {
                await clientRepository.CreateAsync(this.entityService.EnrichEntity(client, model.Username));
                await clientRepository.SaveChangesAsync();
            }
            else
            {
                client = existingClients.FirstOrDefault();
            }

            var context = await this.tenantDatabaseContextFactory.CreateDbContextAsync(tenant.DatabaseConnectionString);

            var userStore = this.userStoreFactory.CreateUserStore<ApplicationUser>(context);

            if ((await userStore.FindByNameAsync(model.Email, CancellationToken.None)) != null)
            {
                createClientResultModel.Errors.Add(ErrorConstants.DuplicateUser);
                return createClientResultModel;
            }

            var password = this.passwordGenerationService.GeneratePassword(PasswordConstants.DefaultMinLength);

            var user = new ApplicationUser
            {
                ClientId = client.Id,
                UserName = model.Email,
                Email = model.Email,
                NormalizedEmail = model.Email,
                NormalizedUserName = model.Email,
                EmailConfirmed = true
            };

            user.PasswordHash = this.passwordHasher.HashPassword(user, password);

            await userStore.CreateAsync(user, CancellationToken.None);

            await this.mailingService.SendTemplateAsync(new TemplateEmail
            {
                SenderEmailAddress = this.mailingConfiguration.NoReplyFromEmail,
                RecipientEmailAddress = model.Email,
                TemplateId = this.mailingConfiguration.ActionSendGridTemplateId,
                DynamicTemplateData = new
                {
                    Subject = this.globalLocalizer["Welcome"].Value,
                    Header = this.globalLocalizer["Welcome"].Value,
                    Text = this.globalLocalizer["WelcomeText"].Value + password,
                    ButtonText = this.globalLocalizer["Open"].Value,
                    ButtonLink = "#",
                    Footer = this.globalLocalizer["Copyright"].Value.Replace(LocalizationConstants.YearToken, DateTime.UtcNow.Year.ToString())
                }
            });

            createClientResultModel.Client = client;

            return createClientResultModel;
        }
    }
}
