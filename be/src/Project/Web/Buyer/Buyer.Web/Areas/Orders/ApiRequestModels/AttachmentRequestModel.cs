using System;

namespace Buyer.Web.Areas.Orders.ApiRequestModels
{
    public class AttachmentRequestModel
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public string MimeType { get; set; }
        public string Filename { get; set; }
        public string Extension { get; set; }
    }
}
