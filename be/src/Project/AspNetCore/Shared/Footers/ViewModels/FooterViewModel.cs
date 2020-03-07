using Feature.PageContent.Shared.Links;
using System.Collections.Generic;

namespace AspNetCore.Shared.Footers.ViewModels
{
    public class FooterViewModel
    {
        public string Copyright { get; set; }
        public IEnumerable<LinkViewModel> Links { get; set; }
    }
}
