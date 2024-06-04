using System;
using System.Collections.Generic;

namespace Global.Api.v1.RequestModels
{
    public class SettingRequestModel
    {
        public Guid SellerId { get; set; }
        public Dictionary<string, string> Settings { get; set; }
    }
}
