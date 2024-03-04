using System.Collections.Generic;

namespace Identity.Api.Areas.Home.DomainModels
{
    public class Content
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IEnumerable<AccordionItem> Accordions { get; set; }
    }
}
