using System;

namespace Foundation.PageContent.Components.ContentGrids.ViewModels
{
    public class ContentGridItemViewModel
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public string ImageAlt { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
    }
}
