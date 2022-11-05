using Foundation.PageContent.Components.ListItems.ViewModels;
using System.Collections.Generic;

namespace Seller.Web.Areas.Media.ViewModel
{
    public class MediaFormViewModel
    {
        public string Title { get; set; }
        public string SaveMediaUrl { get; set; }
        public string MediaUrl { get; set; }
        public string BackToMediaText { get; set; }
        public string MediaItemsLabel { get; set; }
        public string GeneralErrorMessage { get; set; }
        public string DropOrSelectImagesLabel { get; set; }
        public string DropFilesLabel { get; set; }
        public string DeleteLabel { get; set; }
        public string SaveMediaChunkUrl { get; set; }
        public string SaveMediaChunkCompleteUrl { get; set; }
        public bool IsUploadInChunksEnabled { get; set; }
        public int? ChunkSize { get; set; }
        public string NoGroupsText { get; set; }
        public string GroupsLabel { get; set; }
        public string SaveText { get; set; }
        public string SaveUrl { get; set; }
        public IEnumerable<ListItemViewModel> Groups { get; set; }
    }
}
