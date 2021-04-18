using System;
using System.Collections.Generic;

namespace Buyer.Web.Shared.DomainModels.Brands
{
    public class Brand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<Guid> Files { get; set; }
    }
}
