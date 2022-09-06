using System;

namespace Buyer.Web.Shared.ViewModels.Files
{
    public class DownloadCenterFilesViewModel : FilesViewModel
    {
        public Guid? Id { get; set; }
        public string DownloadSelectedLabel { get; set; }
        public string DownloadEverythingLabel { get; set; }
        public string SelectFileLabel { get; set; }
        public string SearchLabel { get; set; }
        public string SearchApiUrl { get; set; }
    }
}
