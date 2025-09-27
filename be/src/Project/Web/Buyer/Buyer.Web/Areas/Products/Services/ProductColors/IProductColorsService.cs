using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.Services.ProductColors
{
    public interface IProductColorsService
    {
        Task<string> ToEnglishAsync(string color);
    }
}
