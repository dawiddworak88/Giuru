using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Seller.Web.Areas.Orders.ApiRequestModels
{
    public class SaveBasketRequestModel
    {
        private string _discountCode;

        public Guid? Id { get; set; }
        public Guid? ClientId { get; set; }
        public IEnumerable<BasketItemRequestModel> Items { get; set; }

        public string DiscountCode
        {
            get => _discountCode;
            set
            {
                _discountCode = value;
                HasDiscountCode = true;
            }
        }

        [JsonIgnore]
        public bool HasDiscountCode { get; private set; }
    }
}
