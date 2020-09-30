using Foundation.Extensions.Models;
using System;

namespace Catalog.Api.v1.Areas.Products.Models
{
    public class GetProductsModel : BaseServiceModel
    {
        public Guid? CategoryId { get; set; }
        public int PageIndex { get; set; }
        public int ItemsPerPage { get; set; }
        public string SearchTerm { get; set; }
        public bool PrimaryProductsOnly { get; set; }
        public bool ProductVariantsOnly { get; set; }
    }
}
