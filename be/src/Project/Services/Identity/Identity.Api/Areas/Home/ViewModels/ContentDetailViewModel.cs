using Identity.Api.Areas.Home.DomainModels;
using System.Collections.Generic;

namespace Identity.Api.Areas.Home.ViewModels
{
    public class ContentDetailViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IEnumerable<AccordionItem> AccordionItems { get; set; }
    }
}
