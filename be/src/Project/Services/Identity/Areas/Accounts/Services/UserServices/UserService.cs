using Foundation.Account.UserStores;
using Foundation.Database.Areas.Accounts.Entities;
using Foundation.Database.Shared.Contexts;
using Foundation.GenericRepository.Configurations;
using IdentityServer4.Events;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.Api.Areas.Accounts.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly DatabaseContext context;
        private readonly IIdentityServerInteractionService interactionService;
        private readonly IEventService eventsService;
        private readonly ConnectionStringsConfiguration connectionStringsConfiguration;
        private readonly UserStoreFactory userStoreFactory;

        public UserService(
            DatabaseContext context,
            IIdentityServerInteractionService interactionService,
            IEventService eventsService,
            UserStoreFactory userStoreFactory,
            IOptionsMonitor<ConnectionStringsConfiguration> connectionStringsConfigurationFactory)
        {
            this.context = context;
            this.userStoreFactory = userStoreFactory;
            this.interactionService = interactionService;
            this.eventsService = eventsService;
            this.connectionStringsConfiguration = connectionStringsConfigurationFactory.CurrentValue;
        }

        public async Task<bool> SignInAsync(HttpContext httpContext, string email, string password, string returnUrl)
        {
            var passwordHasher = new PasswordHasher<ApplicationUser>();

            var userStore = userStoreFactory.CreateUserStore<ApplicationUser>(this.context);

            var user = await userStore.FindByNameAsync(email, CancellationToken.None);

            var passwordStore = userStore as IUserPasswordStore<ApplicationUser>;

            var hashedPassword = await passwordStore.GetPasswordHashAsync(user, CancellationToken.None);

            if (passwordHasher.VerifyHashedPassword(user, hashedPassword, password) != PasswordVerificationResult.Failed)
            {
                var authorizationContext = await this.interactionService.GetAuthorizationContextAsync(returnUrl);

                if (authorizationContext != null)
                {
                    await this.eventsService.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName, clientId: authorizationContext?.ClientId));

                    await httpContext.SignInAsync(user.Id, user.UserName);

                    return true;
                }
            }

            return false;
        }

        public async Task<ApplicationUser> FindByIdAsync(string userId)
        {
            var userStore = userStoreFactory.CreateUserStore<ApplicationUser>(this.context);

            return await userStore.FindByIdAsync(userId, CancellationToken.None);
        }

        public async Task<ApplicationUser> ValidateAsync(string email, string password)
        {
            var passwordHasher = new PasswordHasher<ApplicationUser>();

            var userStore = userStoreFactory.CreateUserStore<ApplicationUser>(this.context);

            var user = await userStore.FindByNameAsync(email, CancellationToken.None);

            var passwordStore = userStore as IUserPasswordStore<ApplicationUser>;

            var hashedPassword = await passwordStore.GetPasswordHashAsync(user, CancellationToken.None);

            if (passwordHasher.VerifyHashedPassword(user, hashedPassword, password) != PasswordVerificationResult.Failed)
            {
                return user;
            }

            return default;
        }
    }
}
