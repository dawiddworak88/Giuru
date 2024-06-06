using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.Settings.ApiRequestModels
{
    public class SaveSettingsRequestModel
    {
        public Guid? SellerId { get; set; }
        public Dictionary<string, string> Settings { get; set; }
    }
}
