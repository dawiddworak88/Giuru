using System;

namespace Buyer.Web.Shared.DomainModels.Global
{
    public class Country
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
