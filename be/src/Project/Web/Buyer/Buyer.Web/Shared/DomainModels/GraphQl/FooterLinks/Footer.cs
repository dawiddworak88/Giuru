using System.Collections.Generic;
using Buyer.Web.Shared.DomainModels.GraphQl.Shared;

namespace Buyer.Web.Shared.DomainModels.GraphQl.FooterLinks
{
    public class Footer
    {
        public string Copyright { get; set; }
        public IEnumerable<Link> Links { get; set; }
    }
}
