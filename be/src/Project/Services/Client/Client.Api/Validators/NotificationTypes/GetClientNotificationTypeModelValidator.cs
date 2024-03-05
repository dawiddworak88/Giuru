using Foundation.Extensions.Validators;
using FluentValidation;
using Client.Api.ServicesModels.NotificationTypes;

namespace Client.Api.Validators.NotificationType
{
    public class GetClientNotificationTypeModelValidator : BaseServiceModelValidator<GetClientNotificationTypeServiceModel>
    {
        public GetClientNotificationTypeModelValidator()
        {
            RuleFor(x => x.Id).NotNull();
        }
    }
}
