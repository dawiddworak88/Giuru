using System.Collections.Generic;

namespace Buyer.Web.Shared.DomainModels.GraphQl
{
    public class Footer
    {
        public string Copyright { get; set; }
        public IEnumerable<Link> Links { get; set; }
    }
}
