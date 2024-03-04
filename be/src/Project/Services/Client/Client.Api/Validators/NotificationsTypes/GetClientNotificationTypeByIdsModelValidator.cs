using Client.Api.ServicesModels.Notification;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators.NotificationsType
{
    public class GetClientNotificationTypeByIdsModelValidator : BaseServiceModelValidator<GetClientNotificationTypeByIdsServiceModel>
    {
        public GetClientNotificationTypeByIdsModelValidator() 
        { 
            RuleFor(x => x.Ids).NotNull().NotEmpty();
        }
    }
}
