using System;

namespace DownloadCenter.Api.v1.ResponseModel
{
    public class DownloadCenterItemResponseModel
    {
        public Guid? Id { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int? Order { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
