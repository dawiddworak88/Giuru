using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.Clients.DomainModels
{
    public class ClientField
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsRequired { get; set; }
        public IEnumerable<ClientFieldOptionItem> Options {  get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
