using Foundation.ApiExtensions.Models.Response;
using System;

namespace News.Api.v1.News.ResponseModels
{
    public class NewsItemFileResponseModel : BaseResponseModel
    {
        public DateTime? LastModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
