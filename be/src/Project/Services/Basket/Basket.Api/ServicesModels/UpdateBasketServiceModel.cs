using Foundation.Extensions.Models;
using System;
using System.Collections.Generic;

namespace Basket.Api.ServicesModels
{
    public class UpdateBasketServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
        public bool IsSeller { get; set; }
        public IEnumerable<UpdateBasketItemServiceModel> Items { get; set; }
    }
}
