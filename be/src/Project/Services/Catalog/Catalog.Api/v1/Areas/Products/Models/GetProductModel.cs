using Foundation.Extensions.Models;
using System;

namespace Catalog.Api.v1.Areas.Products.Models
{
    public class GetProductModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
