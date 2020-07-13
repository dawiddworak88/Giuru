using Feature.Client.Definitions;
using Feature.Client.Models;
using Feature.Client.ResultModels;
using Feature.Client.Validators;
using Foundation.Account.Definitions;
using Foundation.Account.Services;
using Foundation.Account.UserStores;
using Foundation.Database.Areas.Accounts.Entities;
using Foundation.Database.Areas.Sellers.Entities;
using Foundation.Database.Shared.Contexts;
using Foundation.Database.Shared.Repositories;
using Foundation.GenericRepository.Paginations;
using Foundation.GenericRepository.Services;
using Foundation.Localization;
using Foundation.Localization.Definitions;
using Foundation.Localization.Services;
using Foundation.Mailing.Configurations;
using Foundation.Mailing.Models;
using Foundation.Mailing.Services;
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
        private readonly DatabaseContext context;
        private readonly IGenericRepository<Seller> sellerRepository;
        private readonly UserStoreFactory userStoreFactory;
        private readonly IPasswordHasher<ApplicationUser> passwordHasher;
        private readonly IPasswordGenerationService passwordGenerationService;
        private readonly IMailingService mailingService;
        private readonly MailingConfiguration mailingConfiguration;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly ICultureService cultureService;
        private readonly IEntityService entityService;

        public ClientService(
            DatabaseContext context,
            IGenericRepository<Seller> sellerRepository,
            UserStoreFactory userStoreFactory,
            IPasswordHasher<ApplicationUser> passwordHasher,
            IPasswordGenerationService passwordGenerationService,
            IMailingService mailingService, 
            IOptionsMonitor<MailingConfiguration> mailingConfiguration,
            IStringLocalizer<GlobalResources> globalLocalizer,
            ICultureService cultureService,
            IEntityService entityService)
        {
            this.context = context;
            this.sellerRepository = sellerRepository;
            this.userStoreFactory = userStoreFactory;
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

            var seller = this.sellerRepository.GetById(model.SellerId.Value);

            if (seller == null)
            {
                createClientResultModel.Errors.Add(Foundation.Extensions.Definitions.ErrorConstants.NoSeller);
                return createClientResultModel;
            }

            var host = new MailAddress(model.Email).Host;

            var client = new Foundation.Database.Areas.Clients.Entities.Client
            {
                ClientSecret = Guid.NewGuid(),
                Language = model.ClientPreferredLanguage,
                Name = model.Name,
                Host = host
            };

            this.cultureService.SetCulture(model.ClientPreferredLanguage.ToLowerInvariant());

            var existingClient = context.Clients.FirstOrDefault(x => x.Host == host);

            if (existingClient == null)
            {
                await context.Clients.AddAsync(this.entityService.EnrichEntity(client, model.Username));
                await context.SaveChangesAsync();
            }

            var userStore = this.userStoreFactory.CreateUserStore<ApplicationUser>(context);

            if ((await userStore.FindByNameAsync(model.Email, CancellationToken.None)) != null)
            {
                createClientResultModel.Errors.Add(ErrorConstants.DuplicateUser);
                return createClientResultModel;
            }

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

        public async Task<ClientsResultModel> GetAsync(GetClientsModel getClientsModel)
        {
            var validator = new GetClientsModelValidator();

            var validationResult = await validator.ValidateAsync(getClientsModel);

            var getClientsResultModel = new ClientsResultModel();

            if (!validationResult.IsValid)
            {
                getClientsResultModel.Errors.AddRange(validationResult.Errors.Select(x => x.ErrorMessage));
                return getClientsResultModel;
            }

            var seller = this.sellerRepository.GetById(getClientsModel.SellerId.Value);

            if (seller == null)
            {
                getClientsResultModel.Errors.Add(Foundation.Extensions.Definitions.ErrorConstants.NoSeller);
                return getClientsResultModel;
            }

            var entities = context.Clients.Where(x => x.IsActive);

            if (!string.IsNullOrWhiteSpace(getClientsModel.SearchTerm))
            {
                entities = entities.Where(x => (!string.IsNullOrWhiteSpace(x.Name) && x.Name.Contains(getClientsModel.SearchTerm)));
            }

            entities = entities.OrderByDescending(x => x.CreatedDate);

            var pagination = new Pagination(entities.Count(), getClientsModel.ItemsPerPage);

            getClientsResultModel.Clients = entities.PagedIndex(pagination, getClientsModel.PageIndex);

            return getClientsResultModel;
        }
    }
}
