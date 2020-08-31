using Catalog.Api.v1.Areas.Products.Models;
using Catalog.Api.v1.Areas.Products.ResultModels;
using System.Threading.Tasks;

namespace Catalog.Api.v1.Areas.Products.Services
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