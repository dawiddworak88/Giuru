using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Paginations;
using Foundation.Mailing.Configurations;
using Foundation.Mailing.Models;
using Foundation.Mailing.Services;
using Identity.Api.Configurations;
using Identity.Api.Definitions;
using Identity.Api.Infrastructure;
using Identity.Api.Infrastructure.Accounts.Entities;
using Identity.Api.ServicesModels.TeamMembers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Identity.Api.Services.TeamMembers
{
    public class TeamMemberService : ITeamMemberService
    {
        private readonly IdentityContext context;
        private readonly IMailingService mailingService;
        private readonly IOptionsMonitor<MailingConfiguration> mailingOptions;
        private readonly IOptionsMonitor<AppSettings> options;

        public TeamMemberService(
            IdentityContext context,
            IMailingService mailingService,
            IOptionsMonitor<MailingConfiguration> mailingOptions,
            IOptionsMonitor<AppSettings> options)
        {
            this.context = context;
            this.mailingService = mailingService;
            this.mailingOptions = mailingOptions;
            this.options = options;
        }

        public async Task CreateAsync(CreateTeamMemberServiceModel model)
        {
            var organisation = await this.context.Organisations.FirstOrDefaultAsync(x => x.Id == model.OrganisationId && x.IsActive);

            if (organisation is null)
            {
                throw new CustomException("OrganisationNotFound", (int)HttpStatusCode.NotFound);
            }

            var user = await this.context.Accounts.FirstOrDefaultAsync(x => x.Email == model.Email);

            if (user is null)
            {
                var timeExpiration = DateTime.UtcNow.AddHours(IdentityConstants.VerifyTimeExpiration);

                var userAccount = new ApplicationUser
                {
                    UserName = model.Email,
                    NormalizedUserName = model.Email,
                    Email = model.Email,
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

                await this.context.Accounts.AddAsync(userAccount);
                /*await this.mailingService.SendTemplateAsync(new TemplateEmail
                {
                    RecipientEmailAddress = model.Email,
                    RecipientName = model.FirstName + " " + model.LastName,
                    SenderEmailAddress = this.mailingOptions.CurrentValue.SenderEmail,
                    SenderName = this.mailingOptions.CurrentValue.SenderName,
                    TemplateId = this.options.CurrentValue.ActionSendGridCreateTemplateId,
                    DynamicTemplateData = new
                    {
                        lang = organisation.Language,
                        nc_subject = this.accountLocalizer.GetString("nc_subject").Value,
                        nc_preHeader = this.accountLocalizer.GetString("nc_preHeader").Value,
                        nc_buttonLabel = this.accountLocalizer.GetString("nc_buttonLabel").Value,
                        nc_headOne = this.accountLocalizer.GetString("nc_headOne").Value,
                        nc_headTwo = this.accountLocalizer.GetString("nc_headTwo").Value,
                        nc_lineOne = this.accountLocalizer.GetString("nc_lineOne").Value,
                        nc_lineTwo = this.accountLocalizer.GetString("nc_lineTwo").Value,
                        signAccountLink = this.linkGenerator.GetUriByAction("Index", "SetPassword", new { Area = "Accounts", culture = organisation.Language, Id = userAccount.ExpirationId, ReturnUrl = string.IsNullOrWhiteSpace(serviceModel.ReturnUrl) ? null : HttpUtility.UrlEncode(serviceModel.ReturnUrl) }, serviceModel.Scheme, serviceModel.Host)
                    }
                });*/
            }
            else
            {
                user.OrganisationId = organisation.Id;
            }

            await this.context.SaveChangesAsync();
            
        }

        public async Task DeleteAsync(DeleteTeamMemberServiceModel model)
        {
            var user = await this.context.Accounts.FirstOrDefaultAsync(x => x.Id == model.Id.ToString() && x.OrganisationId == model.OrganisationId);

            if (user is null)
            {
                throw new CustomException("", (int)HttpStatusCode.NotFound);
            }

   /*         var organisation = await this.context.Organisations.FirstOrDefaultAsync(x => x.ContactEmail == model.Email);

            if (organisation is not null)
            {
                throw new CustomException("", (int)HttpStatusCode.NotFound);
            }

            this.context.Organisations.Remove(organisation);*/
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

            return teamMembers.PagedIndex(new Pagination(teamMembers.Count(), model.ItemsPerPage), model.PageIndex);
        }

        public async Task<TeamMemberServiceModel> GetAsync(GetTeamMemberServiceModel model)
        {
            var existingTeamMember = await this.context.Accounts.FirstOrDefaultAsync(x => x.Id == model.Id.ToString() && x.OrganisationId == model.OrganisationId);

            if (existingTeamMember is null)
            {
                throw new CustomException("", (int)HttpStatusCode.NotFound);
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

        public async Task UpdateAsync(UpdateTeamMemberServiceModel model)
        {
            var user = await this.context.Accounts.FirstOrDefaultAsync(x => x.Email == model.Email);

            if (user is null)
            {
                throw new CustomException("", (int)HttpStatusCode.NotFound);
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;

            await this.context.SaveChangesAsync();
        }
    }
}
