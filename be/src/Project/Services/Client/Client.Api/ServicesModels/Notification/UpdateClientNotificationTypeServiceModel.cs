﻿using Foundation.Extensions.Models;
using System;

namespace Client.Api.ServicesModels.Notification
{
    public class UpdateClientNotificationTypeServiceModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
    }
}