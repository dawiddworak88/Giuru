using Buyer.Web.Shared.Repositories.Clients;
using Buyer.Web.Shared.Repositories.Global;
using Foundation.ApiExtensions.Definitions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.Middlewares
{
    public class ClaimsEnrichmentMiddleware : IMiddleware
    {
        private readonly IClientsRepository _clientsRepository;
        private readonly IClientAddressesRepository _clientAddressesRepository;
        private readonly IGlobalRepository _globalRepository;
        private readonly IClientFieldValuesRepository _clientFieldValuesRepository;
        
        public ClaimsEnrichmentMiddleware(
            IClientsRepository clientsRepository,
            IClientAddressesRepository clientAddressesRepository,
            IGlobalRepository globalRepository,
            IClientFieldValuesRepository clientFieldValuesRepository)
        {
            _clientsRepository = clientsRepository;
            _clientAddressesRepository = clientAddressesRepository;
            _globalRepository = globalRepository;
            _clientFieldValuesRepository = clientFieldValuesRepository;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                var userEmail = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

                if (string.IsNullOrWhiteSpace(userEmail))
                {
                    return;
                }

                var token = await context.Request.HttpContext.GetTokenAsync(ApiExtensionsConstants.TokenName);
                var language = CultureInfo.CurrentUICulture.Name;
                var client = await _clientsRepository.GetClientByEmailAsync(token, language, userEmail);

                if (client is not null)
                {
                    var claimsIdentity = (ClaimsIdentity)context.User.Identity;

                    var countries = await _globalRepository.GetCountriesAsync(token, language, null);
                    
                    var clientAddress = await _clientAddressesRepository.GetAsync(token, language, client.DefaultDeliveryAddressId);

                    if (clientAddress is not null)
                    {
                        var country = countries.FirstOrDefault(x => x.Id == clientAddress.CountryId);

                        if (country is not null)
                        {
                            claimsIdentity.AddClaim(new Claim("ZipCode", $"{clientAddress.PostCode} ({clientAddress.City}, {country.Name})"));
                            claimsIdentity.AddClaim(new Claim("Country", $"{country.Name}"));
                        }
                    }

                    var clientFieldValues = await _clientFieldValuesRepository.GetAsync(token, language, client.Id);

                    if (clientFieldValues.Any())
                    {
                        var extraPackingField = clientFieldValues.FirstOrDefault(x => x.FieldName == "Extra Packing");

                        if (extraPackingField is not null)
                        {
                            claimsIdentity.AddClaim(new Claim("ExtraPacking", extraPackingField.FieldValue));
                        }

                        var paletteLoading = clientFieldValues.FirstOrDefault(x => x.FieldName == "Palette Loading");

                        if (paletteLoading is not null)
                        {
                            claimsIdentity.AddClaim(new Claim("PaletteLoading", paletteLoading.FieldValue));
                        }
                    }
                }
            }

            await next(context);
        }
    }
}
