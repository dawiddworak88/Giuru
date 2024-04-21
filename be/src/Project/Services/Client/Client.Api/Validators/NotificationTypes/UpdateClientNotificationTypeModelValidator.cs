using Foundation.Extensions.Validators;
using FluentValidation;
using Client.Api.ServicesModels.NotificationTypes;

namespace Client.Api.Validators.NotificationType
{
    public class UpdateClientNotificationTypeModelValidator : BaseServiceModelValidator<UpdateClientNotificationTypeServiceModel>
    {
        public UpdateClientNotificationTypeModelValidator()
        { 
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.Name).NotNull().NotEmpty();
        }
    }
}
