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
        private readonly IdentityContext _context;
        private readonly IMailingService _mailingService;
        private readonly IOptionsMonitor<MailingConfiguration> _mailingOptions;
        private readonly IOptionsMonitor<AppSettings> _options;
        private readonly LinkGenerator _linkGenerator;
        private readonly IStringLocalizer<TeamMembersResources> _teamMembersLocalizer;

        public TeamMemberService(
            IdentityContext context,
            IMailingService mailingService,
            IOptionsMonitor<MailingConfiguration> mailingOptions,
            IOptionsMonitor<AppSettings> options,
            IStringLocalizer<TeamMembersResources> teamMembersLocalizer,
            LinkGenerator linkGenerator)
        {
            _context = context;
            _mailingService = mailingService;
            _mailingOptions = mailingOptions;
            _options = options;
            _linkGenerator = linkGenerator;
            _teamMembersLocalizer = teamMembersLocalizer;
        }

        public async Task<Guid> CreateAsync(CreateTeamMemberServiceModel model)
        {
            var organisation = await _context.Organisations.FirstOrDefaultAsync(x => x.Id == model.OrganisationId && x.IsActive);

            if (organisation is null)
            {
                throw new NotFoundException(_teamMembersLocalizer.GetString("OrganisationNotFound"));
            }

            var user = await _context.Accounts.FirstOrDefaultAsync(x => x.Email == model.Email);

            if (user is not null)
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.OrganisationId = organisation.Id;

                await _context.SaveChangesAsync();

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
                IsDisabled = false
            };

            Thread.CurrentThread.CurrentCulture = new CultureInfo(organisation.Language);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            await _context.Accounts.AddAsync(userAccount);
            await _context.SaveChangesAsync();
            await _mailingService.SendTemplateAsync(new TemplateEmail
            {
                RecipientEmailAddress = model.Email,
                RecipientName = model.FirstName + " " + model.LastName,
                SenderEmailAddress = _mailingOptions.CurrentValue.SenderEmail,
                SenderName = _mailingOptions.CurrentValue.SenderName,
                TemplateId = _options.CurrentValue.ActionSendGridTeamMemberInvitationTemplateId,
                DynamicTemplateData = new
                {
                    lang = organisation.Language,
                    subject = _teamMembersLocalizer.GetString("tm_subject").Value,
                    lineOne = _teamMembersLocalizer.GetString("tm_lineOne").Value,
                    buttonLabel = _teamMembersLocalizer.GetString("tm_buttonLabel").Value,
                    signAccountLink = _linkGenerator.GetUriByAction("Index", "SetPassword", new { Area = "Accounts", culture = organisation.Language, Id = userAccount.ExpirationId, ReturnUrl = string.IsNullOrWhiteSpace(model.ReturnUrl) ? null : HttpUtility.UrlEncode(model.ReturnUrl) }, model.Scheme, model.Host)
                }
            });

            return Guid.Parse(userAccount.Id);
        }

        public async Task DeleteAsync(DeleteTeamMemberServiceModel model)
        {
            var user = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == model.Id.ToString() && x.OrganisationId == model.OrganisationId);

            if (user is null)
            {
                throw new NotFoundException(_teamMembersLocalizer.GetString("TeamMemberNotFound"));
            }

            _context.Accounts.Remove(user);

            await _context.SaveChangesAsync();
        }

        public async Task<PagedResults<IEnumerable<TeamMemberServiceModel>>> GetAsync(GetTeamMembersServiceModel model)
        {
            var teamMembers = from u in _context.Accounts
                              where u.OrganisationId == model.OrganisationId
                              select new TeamMemberServiceModel
                              {
                                  Id = Guid.Parse(u.Id),
                                  FirstName = u.FirstName,
                                  LastName = u.LastName,
                                  Email = u.Email,
                                  IsDisabled = u.IsDisabled
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
            var existingTeamMember = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == model.Id.ToString() && x.OrganisationId == model.OrganisationId);

            if (existingTeamMember is null)
            {
                throw new NotFoundException(_teamMembersLocalizer.GetString("TeamMemberNotFound"));
            }

            var teamMember = new TeamMemberServiceModel
            {
                Id = model.Id,
                FirstName = existingTeamMember.FirstName,
                LastName = existingTeamMember.LastName,
                Email = existingTeamMember.Email,
                IsDisabled = existingTeamMember.IsDisabled,
            };

            return teamMember;
        }

        public async Task<Guid> UpdateAsync(UpdateTeamMemberServiceModel model)
        {
            var user = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == model.Id.ToString());

            if (user is null)
            {
                throw new NotFoundException(_teamMembersLocalizer.GetString("TeamMemberNotFound"));
            }

            var existingUserWithEmail = await _context.Users.FirstOrDefaultAsync(x => x.Email == model.Email);

            if (existingUserWithEmail is not null)
            {
                existingUserWithEmail.IsDisabled = model.IsDisabled;
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.IsDisabled = model.IsDisabled;

            await _context.SaveChangesAsync();

            return Guid.Parse(user.Id);
        }
    }
}
