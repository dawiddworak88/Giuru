﻿using Foundation.ApiExtensions.Controllers;
using Foundation.Extensions.Definitions;
using Foundation.Extensions.Exceptions;
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
        private readonly IUsersService _userService;

        public UsersController(
            IUsersService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Get information about user
        /// </summary>
        /// <param name="email">The user email</param>
        /// <returns>The user data.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [Route("{email}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var serviceModel = new GetUserByEmailServiceModel
            {
                Email = email
            };

            var validator = new GetUserByEmailModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var user = await _userService.GetByEmail(serviceModel);

                if (user is not null)
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
                        IsDisabled = user.IsDisabled
                    };

                    return StatusCode((int)HttpStatusCode.OK, response);
                }

                return StatusCode((int)HttpStatusCode.OK);
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Creates or updates use
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
            if (request.Id.HasValue is false)
            {
                var serviceModel = new CreateUserServiceModel
                {
                    Name = request.Name,
                    Email = request.Email,
                    CommunicationsLanguage = request.CommunicationLanguage,
                    ReturnUrl = request.ReturnUrl,
                    Scheme = HttpContext.Request.Scheme,
                    Host = HttpContext.Request.Host
                };

                var validator = new CreateUserModelValidator();
                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var response = await _userService.CreateAsync(serviceModel);

                    return StatusCode((int)HttpStatusCode.Created, new { Id = response.Id });
                }
                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
            else
            {
                var serviceModel = new UpdateUserServiceModel
                {
                    Id = request.Id,
                    Name = request.Name,
                    Email = request.Email,
                    CommunicationLanguage = request.CommunicationLanguage,
                    PhoneNumber = request.PhoneNumber,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    TwoFactorEnabled = request.TwoFactorEnabled,
                    AccessFailedCount = request.AccessFailedCount,
                    LockoutEnd = request.LockoutEnd,
                    IsDisabled = request.IsDisabled
                };

                var validator = new UpdateUserModelValidator();
                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var response = await _userService.UpdateAsync(serviceModel);

                    return StatusCode((int)HttpStatusCode.OK, new { Id = response.Id });
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

            if (validationResult.IsValid)
            {
                var response = await _userService.SetPasswordAsync(serviceModel);

                return this.StatusCode((int)HttpStatusCode.OK, new { response.Id });
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Resets a user's password
        /// </summary>
        /// <param name="request">The model.</param>
        [HttpPost, MapToApiVersion("1.0")]
        [Route("resetpassword")]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> ResetPassword(ResetUserPasswordRequestModel request)
        {
            var serviceModel = new ResetUserPasswordServiceModel
            {
                Email = request.Email,
                ReturnUrl = request.ReturnUrl,
                Scheme = HttpContext.Request.Scheme,
                Host = HttpContext.Request.Host
            };

            var validator = new ResetUserPasswordModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                await _userService.ResetPasswordAsync(serviceModel);

                return StatusCode((int)HttpStatusCode.OK);
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }
    }
}