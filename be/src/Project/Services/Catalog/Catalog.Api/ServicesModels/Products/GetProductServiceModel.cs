using Foundation.Extensions.Models;
using System;

namespace Catalog.Api.ServicesModels.Products
{
    public class GetProductServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
