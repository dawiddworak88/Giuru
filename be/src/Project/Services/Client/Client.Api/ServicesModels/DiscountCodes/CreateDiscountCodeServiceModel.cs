using Foundation.Extensions.Models;

namespace Client.Api.ServicesModels.DiscountCodes
{
    public class CreateDiscountCodeServiceModel : BaseServiceModel
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public bool IsDisabled { get; set; }
    }
}
