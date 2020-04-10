using IdentityServer4.Validation;
using System.Threading.Tasks;

namespace Feature.Account.Services
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        #pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        #pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            context.Result = new GrantValidationResult("1", "custom");
        }
    }
}
