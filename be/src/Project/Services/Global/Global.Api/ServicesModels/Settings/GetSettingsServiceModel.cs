using Foundation.Extensions.Models;
using System;

namespace Global.Api.ServicesModels.Settings
{
    public class GetSettingsServiceModel : BaseServiceModel
    {
        public Guid SellerId { get; set; }
    }
}
