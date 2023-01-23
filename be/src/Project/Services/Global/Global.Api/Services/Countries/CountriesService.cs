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
        private readonly GlobalContext _context;
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;

        public CountriesService(
            GlobalContext context,
            IStringLocalizer<GlobalResources> globalLocalizer)
        {
            _context = context;
            _globalLocalizer = globalLocalizer;
        }

        public async Task CreateAsync(CreateCountryServiceModel model)
        {
            var country = new Country();

            await _context.Countries.AddAsync(country.FillCommonProperties());

            var countryTranslation = new CountryTranslation
            {
                CountryId = country.Id,
                Name = model.Name,
                Language = model.Language
            };

            await _context.CountryTranslations.AddAsync(countryTranslation.FillCommonProperties());
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(DeleteCountryServiceModel model)
        {
            var country = await _context.Countries.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (country is null)
            {
                throw new CustomException(_globalLocalizer.GetString("CountryNotFound"), (int)HttpStatusCode.NoContent);
            }

            country.IsActive = false;

            await _context.SaveChangesAsync();
        }

        public PagedResults<IEnumerable<CountryServiceModel>> Get(GetCountriesServiceModel model)
        {
            var countries = from c in _context.Countries
                            select new CountryServiceModel
                            {
                                Id = c.Id,
                                Name = c.Translations.FirstOrDefault(t => t.CountryId == c.Id && t.Language == model.Language).Name,
                                LastModifiedDate = c.LastModifiedDate,
                                CreatedDate = c.CreatedDate
                            };

            if (string.IsNullOrWhiteSpace(model.SearchTerm) is false)
            {
                countries = countries.Where(x => x.Name.StartsWith(model.SearchTerm));
            }

            countries = countries.ApplySort(model.OrderBy);

            PagedResults<IEnumerable<CountryServiceModel>> pagedResults;

            if (model.PageIndex.HasValue is false || model.ItemsPerPage.HasValue is false)
            {
                countries = countries.Take(Constants.MaxItemsPerPageLimit);

                pagedResults = countries.PagedIndex(new Pagination(countries.Count(), Constants.MaxItemsPerPage), Constants.DefaultPageIndex);
            }
            else
            {
                pagedResults = countries.PagedIndex(new Pagination(countries.Count(), model.ItemsPerPage.Value), model.PageIndex.Value);
            }

            return new PagedResults<IEnumerable<CountryServiceModel>>(pagedResults.Total, pagedResults.PageSize)
            {
                Data = pagedResults.Data.OrEmptyIfNull().Select(x => new CountryServiceModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    LastModifiedDate = x.LastModifiedDate,
                    CreatedDate = x.CreatedDate
                })
            };
        }

        public async Task<CountryServiceModel> GetAsync(GetCountryServiceModel model)
        {
            var country = await _context.Countries.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (country is null)
            {
                throw new CustomException(_globalLocalizer.GetString("CountryNotFound"), (int)HttpStatusCode.NoContent);
            }

            return new CountryServiceModel
            {
                Id = country.Id,
                Name = country.Translations.FirstOrDefault(t => t.CountryId == country.Id && t.Language == model.Language && t.IsActive)?.Name ?? country.Translations.FirstOrDefault(t => t.CountryId == country.Id && t.IsActive)?.Name,
                LastModifiedDate = country.LastModifiedDate,
                CreatedDate = country.CreatedDate
            };
        }

        public async Task UpdateAsync(UpdateCountryServiceModel model)
        {
            var country = await _context.Countries.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (country is null)
            {
                throw new CustomException(_globalLocalizer.GetString("CountryNotFound"), (int)HttpStatusCode.NoContent);
            }

            var countryTranslation = await _context.CountryTranslations.FirstOrDefaultAsync(x => x.CountryId == model.Id && x.Language == model.Language && x.IsActive);

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

                await _context.CountryTranslations.AddAsync(newCountryTranslation.FillCommonProperties());
            }

            country.LastModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }
    }
}
