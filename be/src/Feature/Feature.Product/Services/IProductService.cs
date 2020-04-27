using Feature.Product.Models;
using System.Threading.Tasks;

namespace Feature.Product.Services
{
    public interface IProductService
    {
        Task<CreateProductResultModel> CreateAsync(CreateProductModel model);
    }
}
