using System;

namespace DownloadCenter.Api.v1.ResponseModel
{
    public class DownloadCenterCategoryFileResponseModel
    {
        public Guid? Id { get; set; }
        public string Filename { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
