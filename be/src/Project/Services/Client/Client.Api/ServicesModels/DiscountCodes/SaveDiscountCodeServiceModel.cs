using Foundation.Extensions.Models;
using System;
using System.Collections.Generic;

namespace Client.Api.ServicesModels.DiscountCodes
{
    public class SaveDiscountCodeServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool IsDisabled { get; set; }
        public IEnumerable<Guid> ClientIds { get; set; }
    }
}
