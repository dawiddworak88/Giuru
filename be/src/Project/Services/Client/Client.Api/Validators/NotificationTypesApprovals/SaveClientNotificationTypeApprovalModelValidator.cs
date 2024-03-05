using Foundation.Extensions.Validators;
using Client.Api.ServicesModels.NotificationTypesApprovals;
using FluentValidation;
namespace Client.Api.Validators.NotificationTypesApprovals
{
    public class SaveClientNotificationTypeApprovalModelValidator : BaseServiceModelValidator<SaveNotificationTypesApprovalsServiceModel>
    {
        public SaveClientNotificationTypeApprovalModelValidator()
        {
            RuleFor(x => x.ClientId).NotNull();
        }
    }
}
