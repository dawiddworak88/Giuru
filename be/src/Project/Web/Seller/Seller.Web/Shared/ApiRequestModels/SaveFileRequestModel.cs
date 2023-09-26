using System;

namespace Seller.Web.Shared.ApiRequestModels
{
    public class SaveFileRequestModel
    {
        public Guid Id { get; set; }
        public string MimeType { get; set; }
    }
}
