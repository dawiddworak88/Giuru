using Foundation.PageContent.Components.Links.ViewModels;
using System.Collections.Generic;

namespace Foundation.PageContent.Components.Footers.ViewModels
{
    public class FooterViewModel
    {
        public string Copyright { get; set; }
        public IEnumerable<LinkViewModel> Links { get; set; }
    }
}
