using Buyer.Web.Areas.Products.DomainModels;
using Buyer.Web.Shared.DomainModels.Clients;
using System;
using System.Collections.Generic;

namespace Buyer.Web.Areas.Products.ApiRequestModels
{
    public class CompletionDateRequestModel
    {
        public IEnumerable<Product> Products { get; set; }
        public List<ClientFieldValue> ClientFields { get; set; }
        public DateTime CurrentDate { get; set; }
        public string Language { get; set; }
    }
}
