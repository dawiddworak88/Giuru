using System;
using System.Collections.Generic;

namespace DownloadCenter.Api.v1.ResponseModel
{
    public class DownloadCenterResponseModel
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public IEnumerable<DownloadCenterCategoryResponseModel> Categories { get; set; }
        public int? Order { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
