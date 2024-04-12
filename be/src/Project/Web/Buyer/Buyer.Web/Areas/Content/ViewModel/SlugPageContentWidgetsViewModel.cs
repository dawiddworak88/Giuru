using System.Collections.Generic;

namespace Buyer.Web.Areas.Content.ViewModel
{
    public class SlugPageContentWidgetsViewModel
    {
        public string Title { get; set; }
        public IEnumerable<WidgetViewModel> Widgets { get; set; }
    }
}
