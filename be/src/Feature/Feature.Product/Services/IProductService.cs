using Feature.Product.Models;
using Feature.Product.ResultModels;
using System.Threading.Tasks;

namespace Feature.Product.Services
{
    public interface IProductService
    {
        Task<ProductResultModel> CreateAsync(CreateUpdateProductModel model);
        Task<ProductResultModel> UpdateAsync(CreateUpdateProductModel model);
        Task<ProductResultModel> GetByIdAsync(GetProductModel getProductModel);
        Task<ProductsResultModel> GetAsync(GetProductsModel getProductsModel);
        Task<DeleteProductResultModel> DeleteAsync(DeleteProductModel deleteProductModel);
    }
}