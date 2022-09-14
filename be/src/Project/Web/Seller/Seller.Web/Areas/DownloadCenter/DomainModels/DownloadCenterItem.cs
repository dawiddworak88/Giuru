using System;

namespace Seller.Web.Areas.DownloadCenter.DomainModels
{
    public class DownloadCenterItem
    {
        public Guid Id { get; set; }
        public string CdnUrl { get; set; }
        public string Url { get; set; }
        public string Filename { get; set; }
        public string Categories { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
