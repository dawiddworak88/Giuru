using Foundation.ApiExtensions.Controllers;
using Foundation.Extensions.Definitions;
using Foundation.Extensions.Exceptions;
using Identity.Api.Services.Organisations;
using Identity.Api.Services.Users;
using Identity.Api.ServicesModels.Users;
using Identity.Api.v1.RequestModels;
using Identity.Api.v1.ResponseModels;
using Identity.Api.Validators.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Identity.Api.v1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [Authorize(AuthenticationSchemes = "IsToken")]
    [ApiController]
    public class UsersController : BaseApiController
    {
        private readonly IOrganisationService organisationService;
        private readonly IUsersService userService;

        public UsersController(
            IOrganisationService organisationService,
            IUsersService userService)
        {
            this.organisationService = organisationService;
            this.userService = userService;
        }

        /// <summary>
        /// Get information about user
        /// </summary>
        /// <param name="id">The user id</param>
        /// <returns>The user data.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [Route("{id}")]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Get(Guid? id)
        {
            var serviceModel = new GetUserServiceModel
            {
                Id = id
            };

            var validator = new GetUserModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);
            if (validationResult != null)
            {
                var user = await this.userService.GetById(serviceModel);
                if (user != null)
                {
                    var response = new UserResponseModel
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        PhoneNumber = user.PhoneNumber,
                        TwoFactorEnabled = user.TwoFactorEnabled,
                        EmailConfirmed = user.EmailConfirmed,
                        PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                        OrganisationId = user.OrganisationId.Value,
                    };

                    return this.StatusCode((int)HttpStatusCode.OK, response);
                }
                else
                {
                    return this.StatusCode((int)HttpStatusCode.NotFound);
                }
            }
            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Creates or updates user
        /// </summary>
        /// <param name="request">The model.</param>
        /// <returns>The organisation id.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Save(UserRequestModel request)
        {
            var url = this.Request.Scheme + "://" + this.Request.Host.ToString();
            if (request.Id == null)
            {
                var serviceModel = new CreateUserServiceModel
                {
                    Name = request.Name,
                    Email = request.Email,
                    CommunicationsLanguage = request.CommunicationLanguage,
                    Url = url
                };

                var validator = new CreateUserModelValidator();
                var validationResult = await validator.ValidateAsync(serviceModel);
                if (validationResult != null)
                {
                    var response = await this.userService.CreateAsync(serviceModel);

                    return this.StatusCode((int)HttpStatusCode.Created, new { response.Id });
                }
                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
            else
            {
                var serviceModel = new UpdateUserServiceModel
                {
                    Id = request.Id,
                    PhoneNumber = request.PhoneNumber,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    TwoFactorEnabled = request.TwoFactorEnabled,
                    AccessFailedCount = request.AccessFailedCount,
                    LockoutEnd = request.LockoutEnd,
                    Url = url
                };

                var validator = new UpdateUserModelValidator();
                var validationResult = await validator.ValidateAsync(serviceModel);
                if (validationResult != null)
                {
                    var response = await this.userService.UpdateAsync(serviceModel);

                    return this.StatusCode((int)HttpStatusCode.OK, new { response.Id });
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
        }

        /// <summary>
        /// Sets a user password
        /// </summary>
        /// <param name="request">The model.</param>
        /// <returns>The user id.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [Route("password")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> SetPassword(SetUserPasswordRequestModel request)
        {
            var serviceModel = new SetUserPasswordServiceModel
            {
                ExpirationId = request.ExpirationId.Value,
                Password = request.Password,
            };

            var validator = new SetUserPasswordModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);
            if (validationResult != null)
            {
                var response = await this.userService.SetPasswordAsync(serviceModel);

                return this.StatusCode((int)HttpStatusCode.OK, new { response.Id });
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }
    }
}
