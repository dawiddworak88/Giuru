using System.Collections.Generic;

namespace Buyer.Web.Areas.Content.ViewModel
{
    public class StrapiContentWidgetsViewModel
    {
        public string Title { get; set; }
        public StrapiReturnButtonViewModel ReturnButton { get; set; }
        public IEnumerable<StrapiWidgetViewModel> Widgets { get; set; }
    }
}
