using Foundation.Extensions.Models;
using System;

namespace Catalog.Api.ServicesModels.Products
{
    public class GetProductByIdServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
        public bool? IsSeller { get; set; }
    }
}
