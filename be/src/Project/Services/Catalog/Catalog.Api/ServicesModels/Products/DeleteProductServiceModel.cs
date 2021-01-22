using Foundation.Extensions.Models;
using System;

namespace Catalog.Api.ServicesModels.Products
{
    public class DeleteProductServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
