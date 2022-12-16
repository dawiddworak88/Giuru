using Foundation.GenericRepository.Extensions;
using Global.Api.Definitions;
using Global.Api.Infrastructure.Entities.Countries;
using System;
using System.Linq;

namespace Global.Api.Infrastructure.Entities.Seeds
{
    public static class CountriesSeed
    {
        public static void SeedCountries(GlobalContext context)
        {
            SeedCountry(context, CountriesConstants.AustriaCountry, "Austria", "Austria", "Österreich");
            SeedCountry(context, CountriesConstants.BelgiumCountry, "Belgium", "Belgia", "Belgien");
            SeedCountry(context, CountriesConstants.BulgariaCountry, "Bulgaria", "Bułgaria", "Bulgarien");
            SeedCountry(context, CountriesConstants.CroatiaCountry, "Croatia", "Chrowacja", "Kroatien");
            SeedCountry(context, CountriesConstants.CzechCountry, "Czech Republic", "Czechy", "Tschechische Republik");
            SeedCountry(context, CountriesConstants.DenmarkCountry, "Denmark", "Dania", "Dänemark");
            SeedCountry(context, CountriesConstants.EstoniaCountry, "Estonia", "Estonia", "Estland");
            SeedCountry(context, CountriesConstants.FinlandCountry, "Finland", "Finlandia", "Finnland");
            SeedCountry(context, CountriesConstants.FranceCountry, "France", "Francja", "Frankreich");
            SeedCountry(context, CountriesConstants.GermanyCountry, "Germany", "Niemcy", "Deutschland");
            SeedCountry(context, CountriesConstants.GreatBritainCountry, "Great Britain", "Wielka Brytania", "Großbritannien");
            SeedCountry(context, CountriesConstants.GreeceCountry, "Greece", "Grecja", "Griechenland");
            SeedCountry(context, CountriesConstants.HollandCountry, "Holland", "Holandia", "Holland");
            SeedCountry(context, CountriesConstants.HungaryCountry, "Hungary", "Węgry", "Ungarn");
            SeedCountry(context, CountriesConstants.IsraelCountry, "Israel", "Izrael", "Israel");
            SeedCountry(context, CountriesConstants.ItalyCountry, "Italy", "Włochy", "Italien");
            SeedCountry(context, CountriesConstants.LatviaCountry, "Latvia", "Łotwa", "Lettland");
            SeedCountry(context, CountriesConstants.LithuaniaCountry, "Lithuania", "Litwa", "Litauen");
            SeedCountry(context, CountriesConstants.MoldovaCountry, "Moldova", "Mołdawia", "Moldau");
            SeedCountry(context, CountriesConstants.MongoliaCountry, "Mongolia", "Mongolia", "Mongolei");
            SeedCountry(context, CountriesConstants.NorwayCountry, "Norway", "Norwegia", "Norwegen");
            SeedCountry(context, CountriesConstants.PolandCountry, "Poland", "Polska", "Polen");
            SeedCountry(context, CountriesConstants.RomaniaCountry, "Romania", "Rumunia", "Rumänien");
            SeedCountry(context, CountriesConstants.RussiaCountry, "Russia", "Rosja", "Russland");
            SeedCountry(context, CountriesConstants.SlovakiaCountry, "Slovakia", "Słowacja", "Slowakei");
            SeedCountry(context, CountriesConstants.SloveniaCountry, "Slovenia", "Słowenia", "Slowenien");
            SeedCountry(context, CountriesConstants.SpainCountry, "Spain", "Hiszpania", "Spanien");
            SeedCountry(context, CountriesConstants.SwedenCountry, "Sweden", "Szwecja", "Schweden");
            SeedCountry(context, CountriesConstants.SwitzerlandCountry, "Switzerland", "Szwajcaria", "Schweiz");
            SeedCountry(context, CountriesConstants.USACountry, "United States", "Stany Zjednoczone", "Vereinigte Staaten");
        }

        private static void SeedCountry(GlobalContext context, Guid id, string englishName, string polishName, string germanName)
        {
            if (!context.Countries.Any(x => x.Id == id)) 
            {
                var country = new Country
                {
                    Id = id
                };

                var enCountryTranslation = new CountryTranslation
                {
                    CountryId = country.Id,
                    Name = englishName,
                    Language = "en"
                };

                var deCountryTranslation = new CountryTranslation
                {
                    CountryId = country.Id,
                    Name = germanName,
                    Language = "de"
                };

                var plCountryTranslation = new CountryTranslation
                {
                    CountryId = country.Id,
                    Name = polishName,
                    Language = "pl"
                };

                context.Add(country.FillCommonProperties());
                context.Add(enCountryTranslation.FillCommonProperties());
                context.Add(deCountryTranslation.FillCommonProperties());
                context.Add(plCountryTranslation.FillCommonProperties());

                context.SaveChanges();
            }
        }
    }
}
