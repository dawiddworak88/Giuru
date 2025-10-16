using Feature.Account;
using Foundation.Extensions.Exceptions;
using Foundation.Mailing.Configurations;
using Foundation.Mailing.Models;
using Foundation.Mailing.Services;
using Identity.Api.Areas.Accounts.Services.UserServices;
using Identity.Api.Configurations;
using Identity.Api.Definitions;
using Identity.Api.Infrastructure;
using Identity.Api.Infrastructure.Accounts.Entities;
using Identity.Api.ServicesModels.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Globalization;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Identity.Api.Services.Users
{
    public class UsersService : IUsersService
    {
        private readonly IdentityContext _identityContext;
        private readonly IOptionsMonitor<MailingConfiguration> _mailingOptions;
        private readonly IOptionsMonitor<AppSettings> _identityOptions;
        private readonly IMailingService _mailingService;
        private readonly IStringLocalizer<AccountResources> _accountLocalizer;
        private readonly IUserService _userService;
        private readonly LinkGenerator _linkGenerator;

        public UsersService(
            IdentityContext identityContext,
            IOptionsMonitor<MailingConfiguration> mailingOptions,
            IOptionsMonitor<AppSettings> identityOptions,
            IMailingService mailingService,
            IStringLocalizer<AccountResources> accountLocalizer,
            IUserService userService,
            LinkGenerator linkGenerator)
        {
            _identityContext = identityContext;
            _mailingOptions = mailingOptions;
            _identityOptions = identityOptions;
            _mailingService = mailingService;
            _accountLocalizer = accountLocalizer;
            _userService = userService;
            _linkGenerator = linkGenerator;
        }

        public async Task<UserServiceModel> CreateAsync(CreateUserServiceModel serviceModel)
        {
            var timeExpiration = DateTime.UtcNow.AddHours(IdentityConstants.VerifyTimeExpiration);

            var existingOrganisation = await _identityContext.Organisations.FirstOrDefaultAsync(x => x.ContactEmail == serviceModel.Email && x.IsActive);

            if (existingOrganisation == null)
            {
                throw new NotFoundException(_accountLocalizer.GetString("OrganisationNotFound"));
            }

            Thread.CurrentThread.CurrentCulture = new CultureInfo(existingOrganisation.Language);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            var user = await _identityContext.Accounts.FirstOrDefaultAsync(x => x.Email == serviceModel.Email);

            if (user is not null)
            {
                user.EmailConfirmed = false;
                user.VerifyExpirationDate = timeExpiration;
                user.ExpirationId = Guid.NewGuid();

                await _identityContext.SaveChangesAsync();
                await _mailingService.SendTemplateAsync(new TemplateEmail
                {
                    RecipientEmailAddress = user.Email,
                    RecipientName = user.UserName,
                    SenderEmailAddress = _mailingOptions.CurrentValue.SenderEmail,
                    SenderName = _mailingOptions.CurrentValue.SenderName,
                    TemplateId = _identityOptions.CurrentValue.ActionSendGridResetTemplateId,
                    DynamicTemplateData = new
                    {
                        lang = existingOrganisation.Language,
                        ap_subject = _accountLocalizer.GetString("ap_subject").Value,
                        ap_preHeader = _accountLocalizer.GetString("ap_preHeader").Value,
                        ap_buttonLabel = _accountLocalizer.GetString("ap_buttonLabel").Value,
                        ap_headOne = _accountLocalizer.GetString("ap_headOne").Value,
                        ap_headTwo = _accountLocalizer.GetString("ap_headTwo").Value,
                        ap_lineOne = _accountLocalizer.GetString("ap_lineOne").Value,
                        resetAccountLink = _linkGenerator.GetUriByAction("Index", "SetPassword", new { Area = "Accounts", culture = existingOrganisation.Language, Id = user.ExpirationId, ReturnUrl = string.IsNullOrWhiteSpace(serviceModel.ReturnUrl) ? null : HttpUtility.UrlEncode(serviceModel.ReturnUrl) }, serviceModel.Scheme, serviceModel.Host)
                    }
                });

                return await GetById(new GetUserServiceModel { Id = Guid.Parse(user.Id), Language = serviceModel.Language, Username = serviceModel.Username, OrganisationId = serviceModel.OrganisationId });
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
                IsDisabled = false
            };

            _identityContext.Accounts.Add(userAccount);
            await _identityContext.SaveChangesAsync();
            await _mailingService.SendTemplateAsync(new TemplateEmail
            {
                RecipientEmailAddress = userAccount.Email,
                RecipientName = userAccount.UserName,
                SenderEmailAddress = _mailingOptions.CurrentValue.SenderEmail,
                SenderName = _mailingOptions.CurrentValue.SenderName,
                TemplateId = _identityOptions.CurrentValue.ActionSendGridCreateTemplateId,
                DynamicTemplateData = new
                {
                    lang = existingOrganisation.Language,
                    nc_subject = _accountLocalizer.GetString("nc_subject").Value,
                    nc_preHeader = _accountLocalizer.GetString("nc_preHeader").Value,
                    nc_buttonLabel = _accountLocalizer.GetString("nc_buttonLabel").Value,
                    nc_headOne = _accountLocalizer.GetString("nc_headOne").Value,
                    nc_headTwo = _accountLocalizer.GetString("nc_headTwo").Value,
                    nc_lineOne = _accountLocalizer.GetString("nc_lineOne").Value,
                    nc_lineTwo = _accountLocalizer.GetString("nc_lineTwo").Value,
                    signAccountLink = _linkGenerator.GetUriByAction("Index", "SetPassword", new { Area = "Accounts", culture = existingOrganisation.Language, Id = userAccount.ExpirationId, ReturnUrl = string.IsNullOrWhiteSpace(serviceModel.ReturnUrl) ? null : HttpUtility.UrlEncode(serviceModel.ReturnUrl) }, serviceModel.Scheme, serviceModel.Host)
                }
            });

            return await GetById(new GetUserServiceModel { Id = Guid.Parse(userAccount.Id), Language = serviceModel.Language, Username = serviceModel.Username, OrganisationId = serviceModel.OrganisationId });
        }

        public async Task<UserServiceModel> GetById(GetUserServiceModel serviceModel)
        {
            var user = await _identityContext.Accounts.FirstOrDefaultAsync(x => x.Id == serviceModel.Id.ToString());

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

        public async Task<UserServiceModel> GetByExpirationId(GetUserServiceModel serviceModel)
        {
            var user = await _identityContext.Accounts.FirstOrDefaultAsync(x => x.ExpirationId == serviceModel.Id);

            if (user is not null)
            {
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

            return default;
        }

        public async Task<UserServiceModel> SetPasswordAsync(SetUserPasswordServiceModel serviceModel)
        { 
            var existingUser = await _identityContext.Accounts.FirstOrDefaultAsync(x => x.ExpirationId == serviceModel.ExpirationId.Value);

            if (existingUser is null)
            {
                throw new NotFoundException(_accountLocalizer.GetString("UserNotFound"));
            }

            if (existingUser.EmailConfirmed)
            {
                throw new ConflictException(_accountLocalizer.GetString("EmailIsConfirmedText"));
            }

            if ((existingUser.VerifyExpirationDate >= DateTime.UtcNow) is false)
            {
                throw new BadRequestException(_accountLocalizer.GetString("VerifyDateExpired"));
            }

            existingUser.EmailConfirmed = true;
            existingUser.PasswordHash = _userService.GeneratePasswordHash(existingUser, serviceModel.Password);

            await _identityContext.SaveChangesAsync();

            return await GetById(new GetUserServiceModel { Id = Guid.Parse(existingUser.Id), Language = serviceModel.Language, Username = serviceModel.Username, OrganisationId = serviceModel.OrganisationId });
        }

        public async Task<UserServiceModel> UpdateAsync(UpdateUserServiceModel serviceModel)
        {
            var existingUser = await _identityContext.Accounts.FirstOrDefaultAsync(x => x.Email == serviceModel.Email);

            if (existingUser is null)
            {
                throw new NotFoundException(_accountLocalizer.GetString("UserNotFound"));
            }

            existingUser.FirstName = serviceModel.FirstName;
            existingUser.LastName = serviceModel.LastName;
            existingUser.TwoFactorEnabled = serviceModel.TwoFactorEnabled;
            existingUser.PhoneNumber = serviceModel.PhoneNumber;
            existingUser.LockoutEnd = serviceModel.LockoutEnd;
            existingUser.IsDisabled = serviceModel.IsDisabled;

            var organisation = await _identityContext.Organisations.FirstOrDefaultAsync(x => x.Id == existingUser.OrganisationId && x.IsActive);

            if (organisation is null)
            {
                throw new NotFoundException(_accountLocalizer.GetString("OrganisationNotFound"));
            }

            organisation.Name = serviceModel.Name;
            organisation.Language = serviceModel.CommunicationLanguage;

            await _identityContext.SaveChangesAsync();

            return await GetById(new GetUserServiceModel { Id = Guid.Parse(existingUser.Id), Language = serviceModel.Language, Username = serviceModel.Username, OrganisationId = serviceModel.OrganisationId });
        }

        public async Task ResetPasswordAsync(ResetUserPasswordServiceModel serviceModel)
        {
            var user = await _identityContext.Accounts.FirstOrDefaultAsync(x => x.Email == serviceModel.Email);

            if (user is not null)
            {
                var timeExpiration = DateTime.UtcNow.AddHours(IdentityConstants.VerifyTimeExpiration);

                user.EmailConfirmed = false;
                user.VerifyExpirationDate = timeExpiration;
                user.ExpirationId = Guid.NewGuid();

                var userOrganisation = await _identityContext.Organisations.FirstOrDefaultAsync(x => x.Id == user.OrganisationId && x.IsActive);

                if (userOrganisation is null)
                {
                    throw new NotFoundException(_accountLocalizer.GetString("OrganisationNotFound"));
                }

                Thread.CurrentThread.CurrentCulture = new CultureInfo(userOrganisation.Language);
                Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

                await _identityContext.SaveChangesAsync();
                await _mailingService.SendTemplateAsync(new TemplateEmail
                {
                    RecipientEmailAddress = user.Email,
                    RecipientName = user.UserName,
                    SenderEmailAddress = _mailingOptions.CurrentValue.SenderEmail,
                    SenderName = _mailingOptions.CurrentValue.SenderName,
                    TemplateId = _identityOptions.CurrentValue.ActionSendGridResetTemplateId,
                    DynamicTemplateData = new
                    {
                        lang = userOrganisation.Language,
                        ap_subject = _accountLocalizer.GetString("ap_subject").Value,
                        ap_preHeader = _accountLocalizer.GetString("ap_preHeader").Value,
                        ap_buttonLabel = _accountLocalizer.GetString("ap_buttonLabel").Value,
                        ap_headOne = _accountLocalizer.GetString("ap_headOne").Value,
                        ap_headTwo = _accountLocalizer.GetString("ap_headTwo").Value,
                        ap_lineOne = _accountLocalizer.GetString("ap_lineOne").Value,
                        resetAccountLink = _linkGenerator.GetUriByAction("Index", "SetPassword", new { Area = "Accounts", culture = userOrganisation.Language, Id = user.ExpirationId, ReturnUrl = string.IsNullOrWhiteSpace(serviceModel.ReturnUrl) ? null : HttpUtility.UrlEncode(serviceModel.ReturnUrl) }, serviceModel.Scheme, serviceModel.Host)
                    }
                });
            }
        }

        public async Task<UserServiceModel> GetByEmail(GetUserByEmailServiceModel serviceModel)
        {
            var user = await _identityContext.Accounts.FirstOrDefaultAsync(x => x.Email == serviceModel.Email);

            if (user is not null)
            {
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
                    PhoneNumber = user.PhoneNumber,
                    IsDisabled = user.IsDisabled
                };
            }

            return default;
        }
    }
}
