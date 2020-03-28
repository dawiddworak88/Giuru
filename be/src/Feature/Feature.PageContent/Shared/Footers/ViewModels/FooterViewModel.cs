using Feature.PageContent.Shared.Links.ViewModels;
using System.Collections.Generic;

namespace Feature.PageContent.Shared.Footers.ViewModels
{
    public class FooterViewModel
    {
        public string Copyright { get; set; }
        public IEnumerable<LinkViewModel> Links { get; set; }
    }
}
