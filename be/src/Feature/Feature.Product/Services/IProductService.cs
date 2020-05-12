using Feature.Product.Models;
using Feature.Product.ResultModels;
using System.Threading.Tasks;

namespace Feature.Product.Services
{
    public interface IProductService
    {
        Task<ProductResultModel> CreateAsync(CreateProductModel model);
        Task<ProductResultModel> GetByIdAsync(GetProductModel getProductModel);
    }
}