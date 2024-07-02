using Foundation.Extensions.Models;
using System;
using System.Collections.Generic;

namespace Global.Api.ServicesModels.Settings
{
    public class UpdateSettingServiceModel : BaseServiceModel
    {
        public Guid SellerId { get; set; }
        public Dictionary<string, string> Settings { get; set; }
    }
}
