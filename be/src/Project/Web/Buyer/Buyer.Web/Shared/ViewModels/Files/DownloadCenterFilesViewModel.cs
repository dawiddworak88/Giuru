﻿using System;

namespace Buyer.Web.Shared.ViewModels.Files
{
    public class DownloadCenterFilesViewModel : FilesViewModel
    {
        public string DownloadSelectedLabel { get; set; }
        public string DownloadEverythingLabel { get; set; }
        public string NoFilesDownloadedMessage { get; set; }
        public string SomeFilesNotDownloadedMessage { get; set; }
        public string SelectFileLabel { get; set; }
        public string SearchLabel { get; set; }
    }
}
