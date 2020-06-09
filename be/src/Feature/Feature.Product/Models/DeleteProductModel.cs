using Foundation.Extensions.Models;
using System;

namespace Feature.Product.Models
{
    public class DeleteProductModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
