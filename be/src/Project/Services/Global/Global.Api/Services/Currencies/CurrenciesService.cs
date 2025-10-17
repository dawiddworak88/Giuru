using Foundation.Extensions.Exceptions;
using Foundation.GenericRepository.Definitions;
using Foundation.GenericRepository.Extensions;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Global.Api.Infrastructure;
using Global.Api.Infrastructure.Entities.Currencies;
using Global.Api.ServicesModels.Currencies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Linq;
using System.Threading.Tasks;
using Foundation.Extensions.ExtensionMethods;

namespace Global.Api.Services.Currencies
{
    public class CurrenciesService : ICurrenciesService
    {
        private readonly GlobalContext _context;
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;

        public CurrenciesService(
            GlobalContext context,
            IStringLocalizer<GlobalResources> globalLocalizer) 
        { 
            _context = context;
            _globalLocalizer = globalLocalizer;
        }

        public async Task CreateAsync(CreateCurrencyServiceModel model)
        {
            var currency = new Currency 
            { 
                CurrencyCode = model.CurrencyCode,
                Symbol = model.Symbol,
            };

            await _context.Currencies.AddAsync(currency.FillCommonProperties());

            var currencyTranslation = new CurrencyTranslation
            {
                CurrencyId = currency.Id,
                Name = model.Name,
                Language = model.Language,
            };

            await _context.CurrenciesTranslations.AddAsync(currencyTranslation.FillCommonProperties());
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(DeleteCurrencyServiceModel model)
        {
            var currency = await _context.Currencies.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (currency is null)
            {
                throw new NotFoundException(_globalLocalizer.GetString("CurrencyNotFound"));
            }

            currency.IsActive = false;

            await _context.SaveChangesAsync();
        }

        public PagedResults<IEnumerable<CurrencyServiceModel>> Get(GetCurrenciesServiceModel model)
        {
            var currencies = _context.Currencies.Where(x => x.IsActive)
                .Include(x => x.Translations)
                .AsSingleQuery()
                .Select(x => new CurrencyServiceModel
                { 
                    Id = x.Id,
                    CurrencyCode = x.CurrencyCode,
                    Symbol = x.Symbol,
                    Name = x.Translations.FirstOrDefault(t => t.CurrencyId == x.Id && t.Language == model.Language) != null ? x.Translations.FirstOrDefault(t => t.CurrencyId == x.Id && t.Language == model.Language).Name : x.Translations.FirstOrDefault(t => t.CurrencyId == x.Id).Name,
                    LastModifiedDate = x.LastModifiedDate,
                    CreatedDate = x.CreatedDate,
                });

            if (string.IsNullOrWhiteSpace(model.SearchTerm) is false)
            {
                currencies = currencies.Where(x => x.Name.StartsWith(model.SearchTerm) || x.CurrencyCode.StartsWith(model.SearchTerm));
            }

            currencies = currencies.ApplySort(model.OrderBy);

            PagedResults<IEnumerable<CurrencyServiceModel>> pagedResult;

            if (model.PageIndex.HasValue is false || model.ItemsPerPage.HasValue is false)
            {
                currencies = currencies.Take(Constants.MaxItemsPerPageLimit);

                pagedResult = currencies.PagedIndex(new Pagination(currencies.Count(), Constants.MaxItemsPerPage), Constants.DefaultPageIndex);
            }
            else
            {
                pagedResult = currencies.PagedIndex(new Pagination(currencies.Count(), model.ItemsPerPage.Value), model.PageIndex.Value);
            }

            return pagedResult;
        }

        public async Task<CurrencyServiceModel> GetAsync(GetCurrencyServiceModel model)
        {
            var currency = await _context.Currencies
                .Include(x => x.Translations)
                .FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (currency is null)
            {
                throw new NotFoundException(_globalLocalizer.GetString("CurrencyNotFound"));
            }

            return new CurrencyServiceModel
            {
                Id = currency.Id,
                CurrencyCode = currency.CurrencyCode,
                Symbol = currency.Symbol,
                Name = currency.Translations.FirstOrDefault(t => t.CurrencyId == currency.Id && t.Language == model.Language) != null ? currency.Translations.FirstOrDefault(t => t.CurrencyId == currency.Id && t.Language == model.Language).Name : currency.Translations.FirstOrDefault(t => t.CurrencyId == currency.Id).Name,
                LastModifiedDate = currency.LastModifiedDate,
                CreatedDate = currency.CreatedDate
            };
        }

        public async Task UpdateAsync(UpdateCurrencyServiceModel model)
        {
            var currency = await _context.Currencies.FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (currency is null)
            {
                throw new NotFoundException(_globalLocalizer.GetString("CurrencyNotFound"));
            }

            var currencyTranslation = await _context.CurrenciesTranslations.FirstOrDefaultAsync(x => x.CurrencyId == model.Id && x.IsActive);

            if (currencyTranslation is not null)
            {
                currencyTranslation.Name = model.Name;
                currencyTranslation.LastModifiedDate = DateTime.UtcNow;
            }
            else
            {
                var newCurrencyTranslation = new CurrencyTranslation
                {
                    CurrencyId = currency.Id,
                    Name = model.Name,
                    Language = model.Language,
                };

                await _context.CurrenciesTranslations.AddAsync(newCurrencyTranslation);
            }

            currency.LastModifiedDate = DateTime.UtcNow;
            currency.CurrencyCode = model.CurrencyCode;
            currency.Symbol = model.Symbol;

            await _context.SaveChangesAsync();
        }
    }
}
