using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Definitions;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Foundation.Mailing.Configurations;
using Foundation.Mailing.Models;
using Foundation.Mailing.Services;
using Identity.Api.Configurations;
using Identity.Api.Definitions;
using Identity.Api.Infrastructure;
using Identity.Api.Infrastructure.Accounts.Entities;
using Identity.Api.ServicesModels.TeamMembers;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Identity.Api.Services.TeamMembers
{
    public class TeamMemberService : ITeamMemberService
    {
        private readonly IdentityContext context;
        private readonly IMailingService mailingService;
        private readonly IOptionsMonitor<MailingConfiguration> mailingOptions;
        private readonly IOptionsMonitor<AppSettings> options;
        private readonly LinkGenerator linkGenerator;
        private readonly IStringLocalizer<TeamMembersResources> teamMembersLocalizer;

        public TeamMemberService(
            IdentityContext context,
            IMailingService mailingService,
            IOptionsMonitor<MailingConfiguration> mailingOptions,
            IOptionsMonitor<AppSettings> options,
            IStringLocalizer<TeamMembersResources> teamMembersLocalizer,
            LinkGenerator linkGenerator)
        {
            this.context = context;
            this.mailingService = mailingService;
            this.mailingOptions = mailingOptions;
            this.options = options;
            this.linkGenerator = linkGenerator;
            this.teamMembersLocalizer = teamMembersLocalizer;
        }

        public async Task<Guid> CreateAsync(CreateTeamMemberServiceModel model)
        {
            var organisation = await this.context.Organisations.FirstOrDefaultAsync(x => x.Id == model.OrganisationId && x.IsActive);

            if (organisation is null)
            {
                throw new CustomException(this.teamMembersLocalizer.GetString("OrganisationNotFound"), (int)HttpStatusCode.NoContent);
            }

            var user = await this.context.Accounts.FirstOrDefaultAsync(x => x.Email == model.Email);

            if (user is not null)
            {
                user.OrganisationId = organisation.Id;

                await this.context.SaveChangesAsync();

                return Guid.Parse(user.Id);
            }

            var timeExpiration = DateTime.UtcNow.AddHours(IdentityConstants.VerifyTimeExpiration);

            var userAccount = new ApplicationUser
            {
                UserName = model.Email,
                NormalizedUserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                NormalizedEmail = model.Email,
                OrganisationId = organisation.Id,
                SecurityStamp = Guid.NewGuid().ToString(),
                VerifyExpirationDate = timeExpiration,
                ExpirationId = Guid.NewGuid(),
                AccessFailedCount = 0,
                EmailConfirmed = false,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
            };

            Thread.CurrentThread.CurrentCulture = new CultureInfo(organisation.Language);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            await this.context.Accounts.AddAsync(userAccount);
            await this.context.SaveChangesAsync();
            await this.mailingService.SendTemplateAsync(new TemplateEmail
            {
                RecipientEmailAddress = model.Email,
                RecipientName = model.FirstName + " " + model.LastName,
                SenderEmailAddress = this.mailingOptions.CurrentValue.SenderEmail,
                SenderName = this.mailingOptions.CurrentValue.SenderName,
                TemplateId = this.options.CurrentValue.ActionSendGridTeamMemberInvitationTemplateId,
                DynamicTemplateData = new
                {
                    lang = organisation.Language,
                    subject = this.teamMembersLocalizer.GetString("tm_subject").Value,
                    lineOne = this.teamMembersLocalizer.GetString("tm_lineOne").Value,
                    buttonLabel = this.teamMembersLocalizer.GetString("tm_buttonLabel").Value,
                    signAccountLink = this.linkGenerator.GetUriByAction("Index", "SetPassword", new { Area = "Accounts", culture = organisation.Language, Id = userAccount.ExpirationId, ReturnUrl = string.IsNullOrWhiteSpace(model.ReturnUrl) ? null : HttpUtility.UrlEncode(model.ReturnUrl) }, model.Scheme, model.Host)
                }
            });

            return Guid.Parse(userAccount.Id);
        }

        public async Task DeleteAsync(DeleteTeamMemberServiceModel model)
        {
            var user = await this.context.Accounts.FirstOrDefaultAsync(x => x.Id == model.Id.ToString() && x.OrganisationId == model.OrganisationId);

            if (user is null)
            {
                throw new CustomException(this.teamMembersLocalizer.GetString("TeamMemberNotFound"), (int)HttpStatusCode.NoContent);
            }

            this.context.Accounts.Remove(user);

            await this.context.SaveChangesAsync();
        }

        public async Task<PagedResults<IEnumerable<TeamMemberServiceModel>>> GetAsync(GetTeamMembersServiceModel model)
        {
            var teamMembers = from u in this.context.Accounts
                              where u.OrganisationId == model.OrganisationId
                              select new TeamMemberServiceModel
                              {
                                  Id = Guid.Parse(u.Id),
                                  FirstName = u.FirstName,
                                  LastName = u.LastName,
                                  Email = u.Email
                              };

            if (string.IsNullOrWhiteSpace(model.SearchTerm) is false)
            {
                teamMembers = teamMembers.Where(x => x.FirstName.StartsWith(model.SearchTerm) || x.LastName.StartsWith(model.SearchTerm) || x.Id.ToString() == model.SearchTerm);
            }

            teamMembers = teamMembers.ApplySort(model.OrderBy);

            if (model.PageIndex.HasValue is false || model.ItemsPerPage.HasValue is false)
            {
                teamMembers = teamMembers.Take(Constants.MaxItemsPerPageLimit);

                return teamMembers.PagedIndex(new Pagination(teamMembers.Count(), Constants.MaxItemsPerPageLimit), Constants.DefaultPageIndex);
            }

            return teamMembers.PagedIndex(new Pagination(teamMembers.Count(), model.ItemsPerPage.Value), model.PageIndex.Value);
        }

        public async Task<TeamMemberServiceModel> GetAsync(GetTeamMemberServiceModel model)
        {
            var existingTeamMember = await this.context.Accounts.FirstOrDefaultAsync(x => x.Id == model.Id.ToString() && x.OrganisationId == model.OrganisationId);

            if (existingTeamMember is null)
            {
                throw new CustomException(this.teamMembersLocalizer.GetString("TeamMemberNotFound"), (int)HttpStatusCode.NoContent);
            }

            var teamMember = new TeamMemberServiceModel
            {
                Id = model.Id,
                FirstName = existingTeamMember.FirstName,
                LastName = existingTeamMember.LastName,
                Email = existingTeamMember.Email
            };

            return teamMember;
        }

        public async Task<Guid> UpdateAsync(UpdateTeamMemberServiceModel model)
        {
            var user = await this.context.Accounts.FirstOrDefaultAsync(x => x.Id == model.Id.ToString());

            if (user is null)
            {
                throw new CustomException(this.teamMembersLocalizer.GetString("TeamMemberNotFound"), (int)HttpStatusCode.NoContent);
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;

            await this.context.SaveChangesAsync();

            return Guid.Parse(user.Id);
        }
    }
}
