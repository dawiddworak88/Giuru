using IdentityServer4.Models;
using IdentityServer4.Services;
using System.Threading.Tasks;

namespace Feature.Account.Services
{
    public class ProfileService : IProfileService
    {
        #pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        #pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            // Gets profile data
        }

        #pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task IsActiveAsync(IsActiveContext context)
        #pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            context.IsActive = true;
        }
    }
}
