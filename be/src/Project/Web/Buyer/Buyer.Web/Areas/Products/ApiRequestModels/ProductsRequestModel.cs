using Foundation.ApiExtensions.Models.Request;
using System;

namespace Buyer.Web.Areas.Products.ApiRequestModels
{
    public class ProductsRequestModel : RequestModelBase
    {
        public string Ids { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? SellerId { get; set; }
        public string SearchTerm { get; set; }
        public bool? HasPrimaryProduct { get; set; }
        public int PageIndex { get; set; }
        public int ItemsPerPage { get; set; }
        public string OrderBy { get; set; }
    }
}
