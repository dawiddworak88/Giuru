using Microsoft.AspNetCore.Mvc;
using System.Net;
using System;
using System.Threading.Tasks;
using Identity.Api.v1.RequestModels;
using Identity.Api.v1.ResponseModels;
using System.Collections.Generic;
using Identity.Api.Services.UserApprovals;
using Identity.Api.ServicesModels.UserApprovals;
using Identity.Api.Validators.UserApprovals;
using Foundation.Extensions.Definitions;
using Foundation.Extensions.Exceptions;
using System.Linq;

namespace Identity.Api.v1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class UserApprovalsController : ControllerBase
    {
        private readonly IUserApprovalsService _userApprovalsService;

        public UserApprovalsController(
            IUserApprovalsService userApprovalsService)
        {
            _userApprovalsService = userApprovalsService;
        }

        /// <summary>
        /// Creates user approvals.
        /// </summary>
        /// <param name="model">The model.</param>
        [HttpPost, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SaveAsync(UserApprovalRequestModel model)
        {
            var serviceModel = new SaveUserApprovalsServiceModel
            {
                UserId = model.UserId,
                ApprvoalIds = model.ApprovalIds
            };

            var modelValidator = new SaveUserApprovalsModelValidator();
            var validationResult = await modelValidator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                await _userApprovalsService.SaveAsync(serviceModel);

                return StatusCode((int)HttpStatusCode.OK);
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// Get list of user approvals by id.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>The list of user approvals.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [Route("{userId}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<UserApprovalResponseModel>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Get(Guid userId)
        {
            var serviceModel = new GetUserApprovalsServiceModel
            {
                UserId = userId
            };

            var modelValidator = new GetUserApprovalsModelValidator();
            var validationResult = await modelValidator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var userApprovals = _userApprovalsService.Get(serviceModel);

                if (userApprovals.Any())
                {
                    var response = userApprovals.Select(x => new UserApprovalResponseModel
                    {
                        ApprovalId = x.ApprovalId,
                        UserId = x.UserId,
                        CreatedDate = x.CreatedDate,
                    });

                    return StatusCode((int)HttpStatusCode.OK, response);
                }
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.BadRequest);
        }
    }
}
