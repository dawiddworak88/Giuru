using System.Threading.Tasks;

namespace Seller.Web.Shared.Services.ProductColors
{
    public interface IProductColorsService
    {
        Task<string> ToEnglishAsync(string color);
        Task InvalidateAsync();
    }
}
