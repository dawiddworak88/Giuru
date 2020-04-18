using Foundation.Account.UserStores;
using Foundation.Database.Areas.Accounts.Entities;
using Foundation.Database.Shared.Contexts;
using Foundation.GenericRepository.Configurations;
using IdentityServer4.Events;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Feature.Account.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly IIdentityServerInteractionService interactionService;
        private readonly IEventService eventsService;
        private readonly DatabaseContextFactory databaseContextFactory;
        private readonly ConnectionStringsConfiguration connectionStringsConfiguration;
        private readonly UserStoreFactory userStoreFactory;

        public UserService(IIdentityServerInteractionService interactionService,
            IEventService eventsService,
            UserStoreFactory userStoreFactory,
            DatabaseContextFactory databaseContextFactory,
            IOptionsMonitor<ConnectionStringsConfiguration> connectionStringsConfigurationFactory)
        {
            this.userStoreFactory = userStoreFactory;
            this.interactionService = interactionService;
            this.eventsService = eventsService;
            this.databaseContextFactory = databaseContextFactory;
            this.connectionStringsConfiguration = connectionStringsConfigurationFactory.CurrentValue;
        }

        public async Task<bool> SignInAsync(HttpContext httpContext, string email, string password, string returnUrl)
        {
            var passwordHasher = new PasswordHasher<ApplicationUser>();

            var databaseContext = this.databaseContextFactory.CreateDbContext(this.connectionStringsConfiguration.DatabaseContext);

            var userStore = userStoreFactory.CreateUserStore<ApplicationUser>(databaseContext);

            var user = await userStore.FindByNameAsync(email, new System.Threading.CancellationToken());

            var passwordStore = userStore as IUserPasswordStore<ApplicationUser>;

            var hashedPassword = await passwordStore.GetPasswordHashAsync(user, new System.Threading.CancellationToken());

            if (passwordHasher.VerifyHashedPassword(user, hashedPassword, password) != PasswordVerificationResult.Failed)
            {
                var context = await this.interactionService.GetAuthorizationContextAsync(returnUrl);

                if (context != null)
                {
                    await this.eventsService.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName, clientId: context?.ClientId));

                    await httpContext.SignInAsync(user.Id, user.UserName);

                    return true;
                }
            }

            return false;
        }
    }
}
