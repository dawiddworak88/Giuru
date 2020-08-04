using Foundation.GenericRepository.Paginations;
using Foundation.Database.Areas.Products.Entities;
using System.Collections.Generic;

namespace Catalog.Api.v1.Areas.Products.ResponseModels
{
    public class ProductsResponseModel
    {
        public PagedResults<IEnumerable<ProductResponseModel>> PagedProducts { get; set; }

        public ProductsResponseModel(PagedResults<IEnumerable<Product>> pagedProducts)
        {
            var productsList = new List<ProductResponseModel>();

            foreach (var product in pagedProducts.Data)
            {
                productsList.Add(new ProductResponseModel(product));
            }

            this.PagedProducts = new PagedResults<IEnumerable<ProductResponseModel>>
            { 
                Data = productsList,
                PageCount = pagedProducts.PageCount,
                Total = pagedProducts.Total
            };
        }
    }
}
