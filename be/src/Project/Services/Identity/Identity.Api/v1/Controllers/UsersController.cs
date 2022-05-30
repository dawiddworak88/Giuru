using Foundation.Account.Definitions;
using Foundation.ApiExtensions.Controllers;
using Foundation.Extensions.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.Helpers;
using Foundation.GenericRepository.Paginations;
using Identity.Api.Services.Users;
using Identity.Api.ServicesModels.Users;
using Identity.Api.v1.RequestModels;
using Identity.Api.v1.ResponseModels;
using Identity.Api.Validators.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Claims;
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
        private readonly IUsersService userService;

        public UsersController(
            IUsersService userService)
        {
            this.userService = userService;
        }

        /// <summary>
        /// Get information about user
        /// </summary>
        /// <param name="email">The user email</param>
        /// <returns>The user data.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [Route("{email}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
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
                var user = await this.userService.GetByEmail(serviceModel);

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
                    };

                    return this.StatusCode((int)HttpStatusCode.OK, response);
                }

                return this.StatusCode((int)HttpStatusCode.OK);
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
            if (request.Id == null)
            {
                var serviceModel = new CreateUserServiceModel
                {
                    Name = request.Name,
                    Email = request.Email,
                    CommunicationsLanguage = request.CommunicationLanguage,
                    ReturnUrl = request.ReturnUrl,
                    Scheme = this.HttpContext.Request.Scheme,
                    Host = this.HttpContext.Request.Host
                };

                var validator = new CreateUserModelValidator();
                var validationResult = await validator.ValidateAsync(serviceModel);
                if (validationResult != null)
                {
                    var response = await this.userService.CreateAsync(serviceModel);

                    return this.StatusCode((int)HttpStatusCode.Created, new { Id = response.Id });
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
                    LockoutEnd = request.LockoutEnd
                };

                var validator = new UpdateUserModelValidator();
                var validationResult = await validator.ValidateAsync(serviceModel);
                if (validationResult != null)
                {
                    var response = await this.userService.UpdateAsync(serviceModel);

                    return this.StatusCode((int)HttpStatusCode.OK, new { Id = response.Id });
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
                Scheme = this.HttpContext.Request.Scheme,
                Host = this.HttpContext.Request.Host
            };

            var validator = new ResetUserPasswordModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);
            if (validationResult != null)
            {
                await this.userService.ResetPasswordAsync(serviceModel);

                return this.StatusCode((int)HttpStatusCode.OK);
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Apply to be client.
        /// </summary>
        /// <param name="request">The model.</param>
        /// <returns>OK.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [Route("register")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Register(RegisterRequestModel request)
        {
            var serviceModel = new RegisterServiceModel
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                ContactJobTitle = request.ContactJobTitle,
                PhoneNumber = request.PhoneNumber,
                CompanyName = request.CompanyName,
                CompanyAddress = request.CompanyAddress,
                CompanyCountry = request.CompanyCountry,
                CompanyCity = request.CompanyCity,
                CompanyRegion = request.CompanyRegion,
                CompanyPostalCode = request.CompanyPostalCode
            };

            var validator = new RegisterModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);
            if (validationResult.IsValid)
            {
                await this.userService.RegisterAsync(serviceModel);

                return this.StatusCode((int)HttpStatusCode.OK);
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Gets list of orders.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <param name="orderBy">The optional order by.</param>
        /// <returns>The list of register applications.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [Route("register")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Registers(string searchTerm, int pageIndex, int itemsPerPage, string orderBy)
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new GetRegisterApplicationsServiceModel
            {
                SearchTerm = searchTerm,
                PageIndex = pageIndex,
                ItemsPerPage = itemsPerPage,
                OrderBy = orderBy,
                Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value),
                Language = CultureInfo.CurrentCulture.Name
            };

            var validator = new GetRegisterApplicationsModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var registerApplications = await this.userService.GetRegisterApplicationsAsync(serviceModel);

                if (registerApplications is not null)
                {
                    var response = new PagedResults<IEnumerable<RegisterApplicationsResponseModel>>(registerApplications.Total, registerApplications.PageSize)
                    {
                        Data = registerApplications.Data.Select(x => new RegisterApplicationsResponseModel
                        {
                            Id = x.Id,
                            FirstName = x.FirstName,
                            LastName = x.LastName,
                            ContactJobTitle = x.ContactJobTitle,
                            Email = x.Email,
                            PhoneNumber = x.PhoneNumber,
                            CompanyName = x.CompanyName,
                            CompanyAddress = x.CompanyAddress,
                            CompanyCity = x.CompanyCity,
                            CompanyCountry = x.CompanyCountry,
                            CompanyPostalCode = x.CompanyPostalCode,
                            CompanyRegion = x.CompanyRegion,
                            LastModifiedDate = x.LastModifiedDate,
                            CreatedDate = x.CreatedDate
                        })
                    };

                    return this.StatusCode((int)HttpStatusCode.OK, response);
                }
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }
    }
}