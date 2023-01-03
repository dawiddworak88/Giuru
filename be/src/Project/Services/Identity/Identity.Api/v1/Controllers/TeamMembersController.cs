using Foundation.Account.Definitions;
using Foundation.ApiExtensions.Controllers;
using Foundation.Extensions.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.Helpers;
using Foundation.GenericRepository.Paginations;
using Identity.Api.Services.TeamMembers;
using Identity.Api.ServicesModels.TeamMembers;
using Identity.Api.v1.RequestModels;
using Identity.Api.v1.ResponseModels;
using Identity.Api.Validators.TeamMembers;
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
    public class TeamMembersController : BaseApiController
    {
        private readonly ITeamMemberService teamMemberService;

        public TeamMembersController(
            ITeamMemberService teamMemberService)
        {
            this.teamMemberService = teamMemberService;
        }

        /// <summary>
        /// Delete team member by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>OK.</returns>
        [HttpDelete, MapToApiVersion("1.0")]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(TeamMemberResponseModel))]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Delete(Guid? id)
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new DeleteTeamMemberServiceModel
            {
                Id = id,
                Language = CultureInfo.CurrentCulture.Name,
                Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var validator = new DeleteTeamMemberModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                await this.teamMemberService.DeleteAsync(serviceModel);

                return this.StatusCode((int)HttpStatusCode.OK);
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Gets team member by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The team member.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(TeamMemberResponseModel))]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Get(Guid? id)
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new GetTeamMemberServiceModel
            {
                Id = id,
                Language = CultureInfo.CurrentCulture.Name,
                Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var validator = new GetTeamMemberModelValidator();
            var validationResult = await validator.ValidateAsync(serviceModel);

            if (validationResult.IsValid)
            {
                var teamMember = await this.teamMemberService.GetAsync(serviceModel);

                if (teamMember is not null)
                {
                    var response = new TeamMemberResponseModel
                    {
                        Id = teamMember.Id,
                       FirstName = teamMember.FirstName,
                       LastName = teamMember.LastName,
                       Email = teamMember.Email
                    };

                    return this.StatusCode((int)HttpStatusCode.OK, response);
                }
            }

            throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Gets list of team members.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <param name="orderBy">The optional order by.</param>
        /// <returns>The list of news.</returns>
        [HttpGet, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(PagedResults<IEnumerable<TeamMemberResponseModel>>))]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Get(string searchTerm, int? pageIndex, int? itemsPerPage, string orderBy)
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            var serviceModel = new GetTeamMembersServiceModel
            {
                SearchTerm = searchTerm,
                PageIndex = pageIndex,
                ItemsPerPage = itemsPerPage,
                OrderBy = orderBy,
                Language = CultureInfo.CurrentCulture.Name,
                Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
            };

            var teamMembers = await this.teamMemberService.GetAsync(serviceModel);

            if (teamMembers is not null)
            {
                var response = new PagedResults<IEnumerable<TeamMemberResponseModel>>(teamMembers.Total, teamMembers.PageSize)
                {
                    Data = teamMembers.Data.OrEmptyIfNull().Select(x => new TeamMemberResponseModel
                    {
                        Id = x.Id,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        Email = x.Email
                    })
                };

                return this.StatusCode((int)HttpStatusCode.OK, response);
            }

            return this.StatusCode((int)HttpStatusCode.UnprocessableEntity);
        }

        /// <summary>
        /// Creates a TeamMember.
        /// </summary>
        /// <param name="request">The model.</param>
        /// <returns>The organisation id.</returns>
        [HttpPost, MapToApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
        public async Task<IActionResult> Save([FromBody] TeamMemberRequestModel request)
        {
            var sellerClaim = this.User.Claims.FirstOrDefault(x => x.Type == AccountConstants.Claims.OrganisationIdClaim);

            if (request.Id.HasValue)
            {
                var serviceModel = new UpdateTeamMemberServiceModel
                {
                    Id = request.Id,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    Language = CultureInfo.CurrentCulture.Name,
                    Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
                };

                var validator = new UpdateTeamMemberModelValidator();
                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var teamMemberId = await this.teamMemberService.UpdateAsync(serviceModel);

                    return this.StatusCode((int)HttpStatusCode.OK, new { Id = teamMemberId });
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
            else
            {
                var serviceModel = new CreateTeamMemberServiceModel
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    Host = this.Request.Host,
                    Scheme = this.Request.Scheme,
                    ReturnUrl = request.ReturnUrl,
                    Language = CultureInfo.CurrentCulture.Name,
                    Username = this.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    OrganisationId = GuidHelper.ParseNullable(sellerClaim?.Value)
                };

                var validator = new CreateTeamMemberModelValidator();
                var validationResult = await validator.ValidateAsync(serviceModel);

                if (validationResult.IsValid)
                {
                    var teamMemberId = await this.teamMemberService.CreateAsync(serviceModel);

                    return this.StatusCode((int)HttpStatusCode.OK, new { Id = teamMemberId });
                }

                throw new CustomException(string.Join(ErrorConstants.ErrorMessagesSeparator, validationResult.Errors.Select(x => x.ErrorMessage)), (int)HttpStatusCode.UnprocessableEntity);
            }
        }
    }
}
