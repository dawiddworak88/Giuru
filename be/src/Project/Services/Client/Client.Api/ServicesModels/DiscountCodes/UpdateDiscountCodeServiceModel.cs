using Foundation.Extensions.Models;
using System;
using System.Collections.Generic;

namespace Client.Api.ServicesModels.DiscountCodes
{
    public class UpdateDiscountCodeServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
        public string Version { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool IsDisabled { get; set; }
        public IEnumerable<Guid> ClientIds { get; set; }
    }
}
