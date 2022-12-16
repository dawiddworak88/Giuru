using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Definitions;
using Foundation.GenericRepository.Extensions;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Global.Api.Infrastructure;
using Global.Api.Infrastructure.Entities.Countries;
using Global.Api.ServicesModels.Countries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Threading.Tasks;

namespace Global.Api.Services.Countries
{
    public class CountriesService : ICountriesService
    {
        private readonly GlobalContext context;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;

        public CountriesService(
            GlobalContext context,
            IStringLocalizer<GlobalResources> globalLocalizer)
        {
            this.context = context;
            this.globalLocalizer = globalLocalizer;
        }

        public async Task CreateAsync(CreateCountryServiceModel model)
        {
            var country = new Country();

            await this.context.Countries.AddAsync(country.FillCommonProperties());

            var countryTranslation = new CountryTranslation
            {
                CountryId = country.Id,
                Name = model.Name,
                Language = model.Language
            };

            await this.context.CountryTranslations.AddAsync(countryTranslation.FillCommonProperties());
            await this.context.SaveChangesAsync();
        }

        public async Task DeleteAsync(DeleteCountryServiceModel model)
        {
            var country = await this.context.Countries.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (country is null)
            {
                throw new CustomException(this.globalLocalizer.GetString("CountryNotFound"), (int)HttpStatusCode.NoContent);
            }

            country.IsActive = false;

            await this.context.SaveChangesAsync();
        }

        public async Task<PagedResults<IEnumerable<CountryServiceModel>>> GetAsync(GetCountriesServiceModel model)
        {
            var countries = this.context.Countries.Where(x => x.IsActive);

            if (string.IsNullOrWhiteSpace(model.SearchTerm) is false)
            {
                countries = countries.Where(x => x.Translations.Any(x => x.Name.StartsWith(model.SearchTerm)));
            }

            countries = countries.ApplySort(model.OrderBy);

            PagedResults<IEnumerable<Country>> pagedResults;

            if (model.PageIndex.HasValue is false || model.ItemsPerPage.HasValue is false)
            {
                countries = countries.Take(Constants.MaxItemsPerPageLimit);

                pagedResults = countries.PagedIndex(new Pagination(countries.Count(), Constants.MaxItemsPerPage), Constants.DefaultPageIndex);
            }
            else
            {
                pagedResults = countries.PagedIndex(new Pagination(countries.Count(), model.ItemsPerPage.Value), model.PageIndex.Value);
            }

            var pagedCountriesServiceModel = new PagedResults<IEnumerable<CountryServiceModel>>(pagedResults.Total, pagedResults.PageSize);

            var countriesItems = new List<CountryServiceModel>();

            foreach (var countryItem in pagedResults.Data.OrEmptyIfNull().ToList())
            {
                var country = new CountryServiceModel
                {
                    Id = countryItem.Id,
                    LastModifiedDate = countryItem.LastModifiedDate,
                    CreatedDate = countryItem.CreatedDate
                };

                var countryTranslations = this.context.CountryTranslations.FirstOrDefault(x => x.Language == model.Language && x.CountryId == country.Id && x.IsActive);

                if (countryTranslations is null)
                {
                    countryTranslations = this.context.CountryTranslations.FirstOrDefault(x => x.IsActive);
                }

                country.Name = countryTranslations?.Name;

                countriesItems.Add(country);
            };

            pagedCountriesServiceModel.Data = countriesItems;

            return pagedCountriesServiceModel;
        }

        public async Task<CountryServiceModel> GetAsync(GetCountryServiceModel model)
        {
            var existingCountry = await this.context.Countries.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (existingCountry is null)
            {
                throw new CustomException(this.globalLocalizer.GetString("CountryNotFound"), (int)HttpStatusCode.NoContent);
            }

            var country = new CountryServiceModel
            {
                Id = existingCountry.Id,
                LastModifiedDate = existingCountry.LastModifiedDate,
                CreatedDate = existingCountry.CreatedDate
            };

            var countryTranslation = await this.context.CountryTranslations.FirstOrDefaultAsync(x => x.CountryId == existingCountry.Id && x.Language == model.Language && x.IsActive);

            if (countryTranslation is null)
            {
                countryTranslation = await this.context.CountryTranslations.FirstOrDefaultAsync(x => x.CountryId == existingCountry.Id && x.IsActive);
            }

            country.Name = countryTranslation?.Name;

            return country;
        }

        public async Task UpdateAsync(UpdateCountryServiceModel model)
        {
            var country = await this.context.Countries.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (country is null)
            {
                throw new CustomException(this.globalLocalizer.GetString("CountryNotFound"), (int)HttpStatusCode.NoContent);
            }

            var countryTranslation = await this.context.CountryTranslations.FirstOrDefaultAsync(x => x.CountryId == model.Id && x.Language == model.Language && x.IsActive);

            if (countryTranslation is not null)
            {
                countryTranslation.Name = model.Name;
                countryTranslation.LastModifiedDate = DateTime.UtcNow;
            }
            else
            {
                var newCountryTranslation = new CountryTranslation
                {
                    CountryId = country.Id,
                    Name = model.Name,
                    Language = model.Language
                };

                await this.context.CountryTranslations.AddAsync(newCountryTranslation.FillCommonProperties());
            }

            country.LastModifiedDate = DateTime.UtcNow;

            await this.context.SaveChangesAsync();
        }
    }
}
