﻿using FluentValidation;
using Foundation.Extensions.Validators;
using Client.Api.ServicesModels.Notification;

namespace Client.Api.Validators.NotificationsType
{
    public class DeleteClientNotaficationTypeModelValidator : BaseServiceModelValidator<DeleteClientNotificationTypeServiceModel>
    {
        public DeleteClientNotaficationTypeModelValidator()
        {
            RuleFor(x => x.Id).NotNull();
        }
    }
}