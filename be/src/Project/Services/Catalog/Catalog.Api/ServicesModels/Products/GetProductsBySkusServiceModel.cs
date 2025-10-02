using Foundation.Extensions.Models;
using System.Collections.Generic;

namespace Catalog.Api.ServicesModels.Products
{
    public class GetProductsBySkusServiceModel : BaseServiceModel
    {
        public IEnumerable<string> Skus { get; set; }
        public int? PageIndex { get; set; }
        public int? ItemsPerPage { get; set; }
        public string OrderBy { get; set; }
        public bool? IsSeller { get; set; }
    }
}
