using IdentityServer4.Validation;
using System.Threading.Tasks;

namespace Feature.Account.Services
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            context.Result = new GrantValidationResult("1", "custom");
        }
    }
}
