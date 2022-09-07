using System;

namespace Buyer.Web.Areas.DownloadCenter.DomainModels
{
    public class DownloadCenterFile
    {
        public Guid Id { get; set; }
        public string Filename { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
