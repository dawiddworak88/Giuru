using System.Collections.Generic;

namespace Buyer.Web.Shared.Files.ViewModels
{
    public class FilesViewModel
    {
        public string DownloadLabel { get; set; }
        public string FilenameLabel { get; set; }
        public string NameLabel { get; set; }
        public string DescriptionLabel { get; set; }
        public string SizeLabel { get; set; }
        public string LastModifiedDateLabel { get; set; }
        public string CreatedDateLabel { get; set; }
        public IEnumerable<FileViewModel> Files { get; set; }
    }
}
