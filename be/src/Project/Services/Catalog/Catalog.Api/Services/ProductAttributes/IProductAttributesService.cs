using Catalog.Api.ServicesModels.ProductAttributes;
using Foundation.GenericRepository.Paginations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Api.Services.ProductAttributes
{
    public interface IProductAttributesService
    {
        Task<ProductAttributeServiceModel> CreateProductAttributeAsync(CreateUpdateProductAttributeServiceModel model);
        Task<ProductAttributeItemServiceModel> CreateProductAttributeItemAsync(CreateUpdateProductAttributeItemServiceModel model);
        Task<ProductAttributeServiceModel> UpdateProductAttributeAsync(CreateUpdateProductAttributeServiceModel model);
        Task<ProductAttributeItemServiceModel> UpdateProductAttributeItemAsync(CreateUpdateProductAttributeItemServiceModel model);
        Task<PagedResults<IEnumerable<ProductAttributeServiceModel>>> GetAsync(GetProductAttributesServiceModel model);
        Task<ProductAttributeServiceModel> GetProductAttributeByIdAsync(GetProductAttributeByIdServiceModel model);
        Task<ProductAttributeItemServiceModel> GetProductAttributeItemByIdAsync(GetProductAttributeItemByIdServiceModel model);
        Task DeleteProductAttributeAsync(DeleteProductAttributeServiceModel model);
        Task DeleteProductAttributeItemAsync(DeleteProductAttributeItemServiceModel model);
        Task<PagedResults<IEnumerable<ProductAttributeItemServiceModel>>> GetProductAttributeItemsAsync(GetProductAttributeItemsServiceModel model);
    }
}
