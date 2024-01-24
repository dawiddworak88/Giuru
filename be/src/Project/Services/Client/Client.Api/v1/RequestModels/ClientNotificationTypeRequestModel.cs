using Foundation.ApiExtensions.Models.Request;
using System;

namespace Client.Api.v1.RequestModels
{
    public class ClientNotificationTypeRequestModel : RequestModelBase
    {
        public string Name { get; set; }
    }
}
