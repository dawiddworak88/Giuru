using Client.Api.ServicesModels.Notification;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators.NotificationsType
{
    public class GetClientNotificationTypeApprovalsModelValidator : BaseServiceModelValidator<GetClientNotificationTypeApprovalsServiceModel>
    {
        public GetClientNotificationTypeApprovalsModelValidator()
        {
            RuleFor(x => x.ClientId).NotNull();
        }
    }
}
