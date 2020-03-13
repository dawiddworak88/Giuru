using System;

namespace Feature.PageContent.Shared.Links.ViewModels
{
    public class LinkViewModel
    {
        public Guid UniqueId { get; set; }
        public string Url { get; set; }
        public string Text { get; set; }
    }
}
