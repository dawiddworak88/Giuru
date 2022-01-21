using Foundation.Extensions.Models;
using System;

namespace Basket.Api.ServicesModels
{
    public class DeleteBasketServiceModel : BaseServiceModel 
    {
        public Guid? Id { get; set; }
    }
}
