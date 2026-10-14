using System.Threading.Tasks;

namespace Foundation.Extensions.Services.Claims
{
    public interface IClaimsCacheInvalidatorService
    {
        /// <summary>
        /// Makes the cached client claims of every user of the organisation stale at once - the primary contact's and
        /// every team member's - so the next request of each of them looks the client up again. Call it after the
        /// change is saved, or a request in between could cache the old values under the new version.
        /// </summary>
        Task InvalidateAsync(System.Guid? organisationId);
    }
}
