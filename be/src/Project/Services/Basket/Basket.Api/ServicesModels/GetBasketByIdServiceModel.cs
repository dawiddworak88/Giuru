using Foundation.Extensions.Models;
using System;

namespace Basket.Api.ServicesModels
{
    public class GetBasketByIdServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
