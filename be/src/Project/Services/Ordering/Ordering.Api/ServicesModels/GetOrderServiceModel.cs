using Foundation.Extensions.Models;
using System;

namespace Ordering.Api.ServicesModels
{
    public class GetOrderServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
        public bool IsSeller { get; set; }
    }
}
