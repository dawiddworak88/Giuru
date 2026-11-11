using System.Threading.Tasks;

namespace Foundation.Pricing.Services
{
    // Per-app adapter over that app's IProductColorsService.
    public interface IProductColorTranslator
    {
        Task<string> ToEnglishAsync(string color);
    }
}
