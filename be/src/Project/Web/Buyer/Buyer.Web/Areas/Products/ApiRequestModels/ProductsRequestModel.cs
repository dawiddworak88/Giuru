using Foundation.ApiExtensions.Models.Request;
using System;
using System.Collections.Generic;

namespace Buyer.Web.Areas.Products.ApiRequestModels
{
    public class ProductsRequestModel : RequestModelBase
    {
        public IEnumerable<Guid> Ids { get; set; }
        public Guid? CategoryId { get; set; }
        public string SearchTerm { get; set; }
        public int PageIndex { get; set; }
        public int ItemsPerPage { get; set; }
    }
}
