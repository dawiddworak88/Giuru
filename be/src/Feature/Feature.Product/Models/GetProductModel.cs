using Foundation.Extensions.Models;
using System;

namespace Feature.Product.Models
{
    public class GetProductModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
