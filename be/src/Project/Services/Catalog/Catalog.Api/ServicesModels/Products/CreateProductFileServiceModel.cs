using System;

namespace Catalog.Api.ServicesModels.Products
{
    public class CreateProductFileServiceModel
    {
        public Guid Id { get; set; }
        public int Order { get; set; }
    }
}
