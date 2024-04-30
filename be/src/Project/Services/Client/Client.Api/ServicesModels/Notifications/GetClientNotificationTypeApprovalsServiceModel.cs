using Foundation.Extensions.Models;
using System;

namespace Client.Api.ServicesModels.Notification
{
    public class GetClientNotificationTypeApprovalsServiceModel : BaseServiceModel
    {
        public Guid? ClientId { get; set; }
    }
}
