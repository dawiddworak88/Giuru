using Feature.Account;
using Foundation.Extensions.Exceptions;
using Foundation.GenericRepository.Extensions;
using Foundation.Mailing.Configurations;
using Foundation.Mailing.Models;
using Foundation.Mailing.Services;
using Identity.Api.Areas.Accounts.Services.UserServices;
using Identity.Api.Infrastructure;
using Identity.Api.Infrastructure.Accounts.Entities;
using Identity.Api.Infrastructure.Organisations.Entities;
using Identity.Api.ServicesModels.Users;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Identity.Api.Services.Users
{
    public class UsersService : IUsersService
    {
        private readonly IdentityContext identityContext;
        private readonly IOptionsMonitor<MailingConfiguration> mailingOptions;
        private readonly IMailingService mailingService;
        private readonly IStringLocalizer accountLocalizer;
        private readonly IUserService userService;
        private readonly LinkGenerator linkGenerator;

        public UsersService(
            IdentityContext identityContext,
            IOptionsMonitor<MailingConfiguration> mailingOptions,
            IMailingService mailingService,
            IStringLocalizer<AccountResources> accountLocalizer,
            IUserService userService,
            LinkGenerator linkGenerator)
        {
            this.identityContext = identityContext;
            this.mailingOptions = mailingOptions;
            this.mailingService = mailingService;
            this.accountLocalizer = accountLocalizer;
            this.userService = userService;
            this.linkGenerator = linkGenerator;
        }

        public async Task<UserServiceModel> CreateAsync(CreateUserServiceModel serviceModel)
        {
            var timeNow = DateTime.Now;
            var timeExpiration = timeNow.AddHours(5);

            var existingOrganisation = await this.identityContext.Organisations.FirstOrDefaultAsync(x => x.ContactEmail == serviceModel.Email && x.IsActive);
            if (existingOrganisation == null)
            {
                var organisation = new Organisation
                {
                    Name = serviceModel.Name,
                    ContactEmail = serviceModel.Email,
                    IsSeller = false,
                    Domain = new MailAddress(serviceModel.Email).Host,
                    Key = new MailAddress(serviceModel.Email).Host.Split('.')[0],
                    Language = serviceModel.CommunicationsLanguage
                };

                existingOrganisation = organisation;
                this.identityContext.Organisations.Add(organisation.FillCommonProperties());
            }

            var user = await this.identityContext.Accounts.FirstOrDefaultAsync(x => x.Email == serviceModel.Email);
            if (user != null)
            {
                user.EmailConfirmed = false;
                user.VerifyExpirationDate = timeExpiration;
                user.ExpirationId = Guid.NewGuid();
                await this.mailingService.SendTemplateAsync(new TemplateEmail
                {
                    RecipientEmailAddress = user.Email,
                    RecipientName = user.UserName,
                    SenderEmailAddress = this.mailingOptions.CurrentValue.SenderEmail,
                    SenderName = this.mailingOptions.CurrentValue.SenderName,
                    TemplateId = this.mailingOptions.CurrentValue.ActionSendGridResetTemplateId,
                    DynamicTemplateData = new Dictionary<string, string>
                    {
                        {"resetAccountLink", this.linkGenerator.GetPathByAction("Index", "SetPassword", new { Area = "Accounts", culture = CultureInfo.CurrentUICulture.Name, Id = user.ExpirationId }) }
                    }
                });

                await this.identityContext.SaveChangesAsync();
                return await this.GetAsync(new GetUserServiceModel { Id = user.Id, Language = serviceModel.Language,  Username = serviceModel.Username, OrganisationId = serviceModel.OrganisationId});
            }

            var userAccount = new ApplicationUser
            {
                UserName = serviceModel.Email,
                NormalizedUserName = serviceModel.Email,
                Email = serviceModel.Email,
                NormalizedEmail = serviceModel.Email,
                OrganisationId = existingOrganisation.Id,
                SecurityStamp = Guid.NewGuid().ToString(),
                VerifyExpirationDate = timeExpiration,
                ExpirationId = Guid.NewGuid(),
                AccessFailedCount = 0,
                EmailConfirmed = false,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
            };

            this.identityContext.Accounts.Add(userAccount);
            await this.identityContext.SaveChangesAsync();
            await this.mailingService.SendTemplateAsync(new TemplateEmail
            {
                RecipientEmailAddress = userAccount.Email,
                RecipientName = userAccount.UserName,
                SenderEmailAddress = this.mailingOptions.CurrentValue.SenderEmail,
                SenderName = this.mailingOptions.CurrentValue.SenderName,
                TemplateId = this.mailingOptions.CurrentValue.ActionSendGridCreateTemplateId,
                DynamicTemplateData = new Dictionary<string, string>
                {
                    {"signAccountLink", this.linkGenerator.GetPathByAction("Index", "SetPassword", new { Area = "Accounts", culture = CultureInfo.CurrentUICulture.Name, Id = userAccount.ExpirationId }) }
                }

            });

            return await this.GetAsync(new GetUserServiceModel { Id = userAccount.Id, Language = serviceModel.Language, Username = serviceModel.Username, OrganisationId = serviceModel.OrganisationId });
        }

        public async Task<UserServiceModel> GetAsync(GetUserServiceModel serviceModel)
        {
            var user = await this.identityContext.Accounts.FirstOrDefaultAsync(x => x.Id == serviceModel.Id || x.ExpirationId == Guid.Parse(serviceModel.Id));
            return new UserServiceModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                OrganisationId = user.OrganisationId,
                TwoFactorEnabled = user.TwoFactorEnabled,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                PhoneNumber = user.PhoneNumber
            };
        }

        public async Task<UserServiceModel> SetPasswordAsync(SetUserPasswordServiceModel serviceModel)
        {
            var existingUser = await this.identityContext.Accounts.FirstOrDefaultAsync(x => x.ExpirationId == serviceModel.ExpirationId.Value);
            if (existingUser is null)
            {
                throw new CustomException(this.accountLocalizer.GetString("UserNotFound"), (int)HttpStatusCode.NotFound);
            }

            if (!existingUser.EmailConfirmed)
            {
                var timeNow = DateTime.Now;
                if (existingUser.VerifyExpirationDate >= timeNow)
                {
                    existingUser.EmailConfirmed = true;
                    existingUser.PasswordHash = userService.GeneratePasswordHash(existingUser, serviceModel.Password);

                    await this.identityContext.SaveChangesAsync();
                    return await this.GetAsync(new GetUserServiceModel { Id = existingUser.Id, Language = serviceModel.Language, Username = serviceModel.Username, OrganisationId = serviceModel.OrganisationId });
                }
                else
                {
                    throw new CustomException(this.accountLocalizer.GetString("VerifyDateExpired"), (int)HttpStatusCode.NotFound);
                }
            }

            return default;
        }

        public async Task<UserServiceModel> UpdateAsync(UpdateUserServiceModel serviceModel)
        {
            var existingUser = await this.identityContext.Accounts.FirstOrDefaultAsync(x => x.Id == serviceModel.Id);
            if (existingUser == null)
            {
                throw new CustomException(this.accountLocalizer.GetString("UserNotFound"), (int)HttpStatusCode.NotFound);
            }

            existingUser.FirstName = serviceModel.FirstName;
            existingUser.LastName = serviceModel.LastName;
            existingUser.TwoFactorEnabled = serviceModel.TwoFactorEnabled.Value;
            existingUser.PhoneNumber = serviceModel.PhoneNumber;
            existingUser.LockoutEnd = serviceModel.LockoutEnd;

            await this.identityContext.SaveChangesAsync();

            return await this.GetAsync(new GetUserServiceModel { Id = serviceModel.Id, Language = serviceModel.Language, Username = serviceModel.Username, OrganisationId = serviceModel.OrganisationId });
        }
    }
}
