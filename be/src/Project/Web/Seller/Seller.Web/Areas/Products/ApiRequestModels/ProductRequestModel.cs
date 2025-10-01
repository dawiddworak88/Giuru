using Foundation.ApiExtensions.Models.Request;
using System;

namespace Seller.Web.Areas.Products.ApiRequestModels
{
    public class ProductRequestModel : RequestModelBase
    {
        public Guid? SellerId { get; set; }
    }
}
