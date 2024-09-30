using Foundation.Extensions.Validators;
using Client.Api.ServicesModels.Notification;
using FluentValidation;
namespace Client.Api.Validators.NotificationsType
{
    public class SaveClientNotificationTypeApprovalModelValidator : BaseServiceModelValidator<SaveNotificationTypeApprovalServiceModel>
    {
        public SaveClientNotificationTypeApprovalModelValidator() 
        {
            RuleFor(x => x.ClientId).NotNull();
        }
    }
}
