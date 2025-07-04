using System.Threading.Tasks;

namespace Foundation.Extensions.Services.Claims
{
    public interface IClaimsCacheInvalidatorService
    {
        Task InvalidateAsync(string email);
    }
}
