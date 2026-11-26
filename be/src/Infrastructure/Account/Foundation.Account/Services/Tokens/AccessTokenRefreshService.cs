using Foundation.Account.Definitions;
using Foundation.Account.ServicesModels.Tokens;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Foundation.Account.Services.Tokens
{
    public class AccessTokenRefreshService : IAccessTokenRefreshService
    {
        private readonly IOptionsMonitor<OpenIdConnectOptions> openIdConnectOptions;
        private readonly ILogger<AccessTokenRefreshService> logger;

        public AccessTokenRefreshService(
            IOptionsMonitor<OpenIdConnectOptions> openIdConnectOptions,
            ILogger<AccessTokenRefreshService> logger)
        {
            this.openIdConnectOptions = openIdConnectOptions;
            this.logger = logger;
        }

        public async Task<RefreshedTokensServiceModel> RefreshAsync(string refreshToken, CancellationToken cancellationToken)
        {
            try
            {
                var options = this.openIdConnectOptions.Get(AccountConstants.AuthenticationSchemes.OpenIdConnect);

                var configuration = await options.ConfigurationManager.GetConfigurationAsync(cancellationToken);
                var response = await options.Backchannel.RequestRefreshTokenAsync(new RefreshTokenRequest
                {
                    Address = configuration.TokenEndpoint,
                    ClientId = options.ClientId,
                    ClientSecret = options.ClientSecret,
                    RefreshToken = refreshToken
                }, cancellationToken);

                if (response.IsError is false)
                {
                    return new RefreshedTokensServiceModel
                    {
                        Succeeded = true,
                        AccessToken = response.AccessToken,
                        RefreshToken = response.RefreshToken,
                        IdentityToken = response.IdentityToken,
                        ExpiresIn = response.ExpiresIn
                    };
                }

                if (response.ErrorType == ResponseErrorType.Protocol && response.Error == OidcConstants.TokenErrors.InvalidGrant)
                {
                    this.logger.LogInformation("The refresh token was rejected, the session has ended");

                    return new RefreshedTokensServiceModel
                    {
                        IsRefreshTokenRejected = true
                    };
                }

                this.logger.LogWarning("Refreshing the access token failed: {ErrorType} {Error}", response.ErrorType, response.Error);
            }
            catch (Exception exception) when (exception is not OperationCanceledException)
            {
                this.logger.LogWarning(exception, "Refreshing the access token failed");
            }

            return new RefreshedTokensServiceModel();
        }

        public async Task RevokeAsync(string refreshToken, CancellationToken cancellationToken)
        {
            try
            {
                var options = this.openIdConnectOptions.Get(AccountConstants.AuthenticationSchemes.OpenIdConnect);

                var configuration = await options.ConfigurationManager.GetConfigurationAsync(cancellationToken);
                var response = await options.Backchannel.RevokeTokenAsync(new TokenRevocationRequest
                {
                    Address = configuration.RevocationEndpoint,
                    ClientId = options.ClientId,
                    ClientSecret = options.ClientSecret,
                    Token = refreshToken,
                    TokenTypeHint = OidcConstants.TokenTypes.RefreshToken
                }, cancellationToken);

                if (response.IsError)
                {
                    this.logger.LogWarning("Revoking the refresh token failed: {ErrorType} {Error}", response.ErrorType, response.Error);
                }
            }
            catch (Exception exception) when (exception is not OperationCanceledException)
            {
                this.logger.LogWarning(exception, "Revoking the refresh token failed");
            }
        }
    }
}
