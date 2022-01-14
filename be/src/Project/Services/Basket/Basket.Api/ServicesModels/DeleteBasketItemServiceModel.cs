using Foundation.Extensions.Models;
using System;

namespace Basket.Api.ServicesModels
{
    public class DeleteBasketItemServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
