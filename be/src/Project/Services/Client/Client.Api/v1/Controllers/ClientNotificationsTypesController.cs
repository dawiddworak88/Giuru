using Client.Api.Services.NotificationsType;
using Client.Api.ServicesModels.Notification;
using Client.Api.v1.RequestModels;
using Client.Api.v1.ResponseModels;
using Client.Api.Validators.NotificationsType;
using Foundation.Account.Definitions;
using Foundation.Extensions.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.Helpers;
using Foundation.GenericRepository.Paginations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Nest;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Client.Api.v1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{veriosn:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    public class ClientNotificationsTypesController : ControllerBase
    {
        private readonly IClientNotificationTypesService _clientNotificationTypesService;
        private readonly IClientNotificationTypeApprovalService _clientNotificationTypeApprovalService;

        public ClientNotificationsTypesController(
            IClientNotificationTypesService clientNotificationTypesService,
            IClientNotificationTypeApprovalService clientNotificationTypeApprovalService)
        {
            _clientNotificationTypesService = clientNotificationTypesService;
            _clientNotificationTypeApprovalService = clientNotificationTypeApprovalService;
        }

        /// <summary>
        /// Gets list of notifications types.
        /// </summary>
        /// <param name="ids">The notificationType ids.</param>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <param name="orderBy">The optional order by.</param>
        /// <returns>The list of notifications types.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Get(string ids, string searchTerm, int? pageIndex, int? itemsPerPage, string orderBy)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var notificationTypeIds = ids.ToEnumerableGuidIds();

            if (notificationTypeIds is not null)
            {
                var serviceModel = new GetClientNotificationTypeByIdsServiceModel
                {
                    Ids = notificationTypeIds,
                    PageIndex = pageIndex,
                    ItemsPerPage = itemsPerPage,
                    OrderBy = orderBy,
                    Language = CultureInfo.CurrentCulture.Name,
                    Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
                };

                var modelValidator = new GetClientNotificationTypeByIdsModelValidator();
                var validationResult = await modelValidator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var notificationTypes = _clientNotificationTypesService.GetByIds(serviceModel);

                    if (notificationTypes is not null)
                    {
                        var response = new PagedResults<IEnumerable<ClientNotificationTypeResponseModel>>(notificationTypes.Total, notificationTypes.PageSize)
                        {
                            Data = notificationTypes.Data.OrEmptyIfNull().Select(x => new ClientNotificationTypeResponseModel
                            {
                                Id = x.Id,
                                Name = x.Name
                            })
                        };

                        return StatusCode((int)HttpStatusCode.OK, response);
                    }
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
            else
            {
                var serviceModel = new GetClientNotificationTypesServiceModel
                {
                    Language = CultureInfo.CurrentCulture.Name,
                    SearchTerm = searchTerm,
                    PageIndex = pageIndex,
                    ItemsPerPage = itemsPerPage,
                    OrderBy = orderBy,
                    Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                };

                var clientNotificationTypes = _clientNotificationTypesService.Get(serviceModel);

                if (clientNotificationTypes is not null)
                {
                    var response = new PagedResults<IEnumerable<ClientNotificationTypeResponseModel>>(clientNotificationTypes.Total, clientNotificationTypes.PageSize)
                    {
                        Data = clientNotificationTypes.Data.OrEmptyIfNull().Select(x => new ClientNotificationTypeResponseModel
                        {
                            Id = x.Id,
                            Name = x.Name,
                            CreatedDate = x.CreatedDate,
                            LastModifiedDate = x.LastModifiedDate
                        })
                    };

                    return StatusCode((int)HttpStatusCode.OK, response);
                }

                return StatusCode((int)HttpStatusCode.UnprocessableEntity);
            }
        }

        /// <summary>
        /// Get notification type.
        /// </summary>
        /// <param name="id">The notification id.</param>
        /// <returns>The notification type.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> GetAsync(Guid? id)
        {
            var serviceModel = new GetClientNotificationTypeServiceModel
            {
                Id = id
            };

            var validator = new GetClientNotificationTypeModelValidator();
            var validatorResult = await validator.ValidateAsync(serviceModel);

            if (validatorResult.IsValid)
            {
                var clientNotificationType = await _clientNotificationTypesService.GetAsync(serviceModel);

                if (clientNotificationType is not null)
                {
                    var response = new ClientNotificationTypeResponseModel
                    {
                        Id = clientNotificationType.Id,
                        Name = clientNotificationType.Name,
                        CreatedDate = clientNotificationType.CreatedDate,
                        LastModifiedDate = clientNotificationType.LastModifiedDate
                    };

                    return StatusCode((int)HttpStatusCode.OK, response);
                }
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validatorResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Updates or creates notification type.
        /// </summary>
        /// <param name="request">The model.</param>
        /// <returns>The notification type id.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Save(ClientNotificationTypeRequestModel request)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            if (request.Id.HasValue)
            {
                var serviceModel = new UpdateClientNotificationTypeServiceModel()
                {
                    Id = request.Id,
                    Name = request.Name,
                    Language = CultureInfo.CurrentCulture.Name,
                    Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    OrganisationId = request.Id,
                };

                var validator = new UpdateClientNotificationTypeModelValidator();
                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var clientNotificationType = await _clientNotificationTypesService.UpdateAsync(serviceModel);

                    return StatusCode((int)HttpStatusCode.OK, new { Id = clientNotificationType.Id });
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
            else
            {
                var serviceModel = new CreateClientNotificationTypeServiceModel
                {
                    Name = request.Name,
                    Language = CultureInfo.CurrentCulture.Name,
                    Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    OrganisationId = request.Id,
                };

                var validator = new CreateClientNotificationTypeModelValidator();
                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var clientNotificationType = await _clientNotificationTypesService.CreateAsync(serviceModel);

                    return StatusCode((int)HttpStatusCode.OK, new { Id = clientNotificationType.Id });
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
        }

        /// <summary>
        /// Deletes notification type by id.
        /// </summary>
        /// <param name="id">The notification type id.</param>
        /// <returns>Ok.</returns>
        [HttpDelete, MapToApiVersion("1.0")]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new DeleteClientNotificationTypeServiceModel
            {
                Id = id
            };

            var validator = new DeleteClientNotaficationTypeModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                await _clientNotificationTypesService.DeleteAsync(serviceModel);

                return StatusCode((int)HttpStatusCode.OK);
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        [HttpGet, MapToApiVersion("1.0")]
        [Route("Approvals/{clietnId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Get(Guid? clietnId)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new GetClientNotificationTypeApprovalsServiceModel
            {
                ClientId = clietnId
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

        [HttpPost, MapToApiVersion("1.0")]
        [Route("Approvals")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Save([FromBody] ClintNotificationTypeApprovalRequestModel request)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new SaveNotificationTypeApprovalServiceModel
            {
                ClientId = request.ClientId,
                NotificationTypeIds = request.NotificationTypeIds,
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