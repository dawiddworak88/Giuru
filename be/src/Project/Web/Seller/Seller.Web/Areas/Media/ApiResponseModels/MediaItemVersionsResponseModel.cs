using Foundation.ApiExtensions.Models.Response;
using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.Media.ApiResponseModels
{
    public class MediaItemVersionsResponseModel : BaseResponseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string MetaData { get; set; }
        public IEnumerable<MediaItemResponseModel> Versions { get; set; }
    }
}
