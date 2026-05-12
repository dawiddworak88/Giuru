using Foundation.Account.Definitions;
using Foundation.ApiExtensions.Controllers;
using Foundation.Extensions.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.Helpers;
using Foundation.GenericRepository.Paginations;
using Identity.Api.Services.ClientTeamMembers;
using Identity.Api.ServicesModels.ClientTeamMembers;
using Identity.Api.v1.RequestModels;
using Identity.Api.v1.ResponseModels;
using Identity.Api.Validators.ClientTeamMembers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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
    public class ClientTeamMembersController : BaseApiController
    {
        private readonly IClientTeamMemberService _clientTeamMemberService;

        public ClientTeamMembersController(
            IClientTeamMemberService clientTeamMemberService)
        {
            _clientTeamMemberService = clientTeamMemberService;
        }

        /// <summary>
        /// Delete client team member by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>OK.</returns>
        [HttpDelete, MapToApiVersion("1.0")]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new DeleteClientTeamMemberServiceModel
            {
                Id = id,
                Language = CultureInfo.CurrentCulture.Name,
                Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value),
                IsSeller = User.IsInRole("Seller")
            };

            var validator = new DeleteClientTeamMemberModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                await _clientTeamMemberService.DeleteAsync(serviceModel);

                return StatusCode((int)HttpStatusCode.OK);
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Gets client team member by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The team member.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ClientTeamMemberResponseModel))]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Get(Guid? id)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new GetClientTeamMemberServiceModel
            {
                Id = id,
                Language = CultureInfo.CurrentCulture.Name,
                Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value),
                IsSeller = User.IsInRole("Seller")
            };

            var validator = new GetClientTeamMemberModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var teamMember = await _clientTeamMemberService.GetAsync(serviceModel);

                if (teamMember is not null)
                {
                    var response = new ClientTeamMemberResponseModel
                    {
                        Id = teamMember.Id,
                        FirstName = teamMember.FirstName,
                        LastName = teamMember.LastName,
                        Email = teamMember.Email,
                        IsDisabled = teamMember.IsDisabled
                    };

                    return StatusCode((int)HttpStatusCode.OK, response);
                }
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Gets list of client team members.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <param name="orderBy">The optional order by.</param>
        /// <param name="organisationId">The client's organisation id.</param>
        /// <returns>The list of client team members.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PagedResults<IEnumerable<ClientTeamMemberResponseModel>>))]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Get(string searchTerm, int? pageIndex, int? itemsPerPage, string orderBy, Guid? organisationId)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);
            var isSeller = User.IsInRole("Seller");

            var serviceModel = new GetClientTeamMembersServiceModel
            {
                SearchTerm = searchTerm,
                PageIndex = pageIndex,
                ItemsPerPage = itemsPerPage,
                OrderBy = orderBy,
                Language = CultureInfo.CurrentCulture.Name,
                Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = isSeller ? organisationId : GuidHelper.ParseNullable(sellerClaim?.Value),
                IsSeller = isSeller
            };

            var validator = new GetClientTeamMembersModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var teamMembers = await _clientTeamMemberService.GetAsync(serviceModel);

                if (teamMembers is not null)
                {
                    var response = new PagedResults<IEnumerable<ClientTeamMemberResponseModel>>(teamMembers.Total, teamMembers.PageSize)
                    {
                        Data = teamMembers.Data.OrEmptyIfNull().Select(x => new ClientTeamMemberResponseModel
                        {
                            Id = x.Id,
                            FirstName = x.FirstName,
                            LastName = x.LastName,
                            Email = x.Email,
                            IsDisabled = x.IsDisabled
                        })
                    };

                    return StatusCode((int)HttpStatusCode.OK, response);
                }
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Creates or updates a client team member.
        /// </summary>
        /// <param name="request">The model.</param>
        /// <returns>The team member id.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Save([FromBody] ClientTeamMemberRequestModel request)
        {
            var sellerClaim = User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);
            var isSeller = User.IsInRole("Seller");

            if (request.Id.HasValue)
            {
                var serviceModel = new UpdateClientTeamMemberServiceModel
                {
                    Id = request.Id,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    IsDisabled = request.IsDisabled,
                    Language = CultureInfo.CurrentCulture.Name,
                    Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    OrganisationId = isSeller ? request.OrganisationId : GuidHelper.ParseNullable(sellerClaim?.Value),
                    IsSeller = isSeller
                };

                var validator = new UpdateClientTeamMemberModelValidator();
                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var teamMemberId = await _clientTeamMemberService.UpdateAsync(serviceModel);

                    return StatusCode((int)HttpStatusCode.OK, new { Id = teamMemberId });
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
            else
            {
                var serviceModel = new CreateClientTeamMemberServiceModel
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    IsDisabled = request.IsDisabled,
                    Host = Request.Host,
                    Scheme = Request.Scheme,
                    ReturnUrl = request.ReturnUrl,
                    Language = CultureInfo.CurrentCulture.Name,
                    Username = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    OrganisationId = isSeller ? request.OrganisationId : GuidHelper.ParseNullable(sellerClaim?.Value),
                    IsSeller = isSeller
                };

                var validator = new CreateClientTeamMemberModelValidator();
                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var teamMemberId = await _clientTeamMemberService.CreateAsync(serviceModel);

                    return StatusCode((int)HttpStatusCode.OK, new { Id = teamMemberId });
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
        }
    }
}
