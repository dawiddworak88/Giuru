using System;

namespace Client.Api.ServicesModels.DiscountCodes
{
    public class DiscountCodeServiceModel
    {
        public Guid? Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool IsDisabled { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
