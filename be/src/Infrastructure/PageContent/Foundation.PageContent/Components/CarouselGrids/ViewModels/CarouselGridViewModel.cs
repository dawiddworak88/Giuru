using System.Collections.Generic;

namespace Foundation.PageContent.Components.CarouselGrids.ViewModels
{
    public class CarouselGridViewModel
    {
        public string Title { get; set; }
        public IEnumerable<CarouselGridItemViewModel> Items { get; set; }
    }
}
