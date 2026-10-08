using System;

namespace Client.Api.v1.RequestModels
{
    public class DiscountCodeRequestModel
    {
        public Guid? Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool IsDisabled { get; set; }
    }
}
