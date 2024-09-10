using Client.Api.ServicesModels.Notification;
using Client.Api.v1.RequestModels;
using Client.Api.Validators.NotificationsType;
using Foundation.Extensions.Definitions;
using Foundation.Extensions.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System;
using Foundation.Account.Definitions;
using Client.Api.v1.ResponseModels;
using Client.Api.Services.NotificationsTypesApprovals;
using Foundation.Extensions.Helpers;
using System.Globalization;
using System.Security.Claims;

namespace Client.Api.v1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{veriosn:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    public class ClientNotificationTypeApprovalsController : ControllerBase
    {
        private readonly IClientNotificationTypeApprovalService _clientNotificationTypeApprovalService;

        public ClientNotificationTypeApprovalsController(
            IClientNotificationTypeApprovalService clientNotificationTypeApprovalService)
        {
            _clientNotificationTypeApprovalService = clientNotificationTypeApprovalService;
        }

        /// <summary>
        /// Get list of notification types by client id.
        /// </summary>
        /// <param name="id">client id.</param>
        /// <returns>The list of notification types.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Get(Guid? id)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new GetClientNotificationTypeApprovalsServiceModel
            {
                ClientId = id,
                Language = CultureInfo.CurrentCulture.Name,
                Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var validator = new GetClientNotificationTypeApprovalsModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var notificationTypeApprovals = _clientNotificationTypeApprovalService.Get(serviceModel);

                if (notificationTypeApprovals is not null)
                {
                    return StatusCode((int)HttpStatusCode.OK, notificationTypeApprovals.Select(x => new ClientNotificationTypeApprovalResponseModel
                    {
                        Id = x.Id,
                        ClientId = x.ClientId,
                        IsApproved = x.IsApproved,
                        ApprovalDate = x.ApprovalDate,
                        NotificationTypeId = x.NotificationTypeId
                    }));
                }
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Save notification type approvals by client id. 
        /// </summary>
        /// <param name="request">The model.</param>
        [HttpPost, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Save([FromBody] ClintNotificationTypeApprovalRequestModel request)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new SaveNotificationTypeApprovalServiceModel
            {
                ClientId = request.ClientId,
                NotificationTypeIds = request.NotificationTypeIds,
                Language = CultureInfo.CurrentCulture.Name,
                Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var validator = new SaveClientNotificationTypeApprovalModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                await _clientNotificationTypeApprovalService.SaveAsync(serviceModel);

                return StatusCode((int)HttpStatusCode.OK);
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }
    }
}
