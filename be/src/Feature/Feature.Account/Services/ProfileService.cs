using IdentityServer4.Models;
using IdentityServer4.Services;
using System.Threading.Tasks;

namespace Feature.Account.Services
{
    public class ProfileService : IProfileService
    {
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
        }
    }
}
