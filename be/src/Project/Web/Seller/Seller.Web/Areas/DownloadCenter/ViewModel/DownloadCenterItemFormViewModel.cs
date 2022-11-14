using Foundation.PageContent.Components.ListItems.ViewModels;
using Seller.Web.Shared.ViewModels;
using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.DownloadCenter.ViewModel
{
    public class DownloadCenterItemFormViewModel
    {
        public Guid? Id { get; set; }
        public Guid? CategoryId { get; set; }
        public string Title { get; set; }
        public string SelectCategoryLabel { get; set; }
        public string SaveText { get; set; }
        public string NavigateToDownloadCenterLabel { get; set; }
        public string DownloadCenterUrl { get; set; }
        public string CategoriesLabel { get; set; }
        public string IdLabel { get; set; }
        public string OrderLabel { get; set; }
        public string SaveUrl { get; set; }
        public string GeneralErrorMessage { get; set; }
        public string FieldRequiredErrorMessage { get; set; }
        public string FilesLabel { get; set; }
        public bool IsUploadInChunksEnabled { get; set; }
        public int? ChunkSize { get; set; }
        public string SaveMediaUrl { get; set; }
        public string SaveMediaChunkUrl { get; set; }
        public string SaveMediaChunkCompleteUrl { get; set; }
        public string DeleteLabel { get; set; }
        public string DropFilesLabel { get; set; }
        public string DropOrSelectFilesLabel { get; set; }
        public string NoGroupsText { get; set; }
        public string GroupsLabel { get; set; }
        public IEnumerable<Guid> CategoriesIds { get; set; }
        public IEnumerable<Guid> GroupIds { get; set; }
        public IEnumerable<FileViewModel> Files { get; set; }
        public IEnumerable<ListItemViewModel> Categories { get; set; }
        public IEnumerable<ListItemViewModel> Groups { get; set; }
    }
}
