using Foundation.Pricing.Configurations;
using Foundation.Pricing.Definitions;
using Foundation.Pricing.DomainModels;
using Grula.PricingIntelligencePlatform.Sdk;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foundation.Pricing.Services
{
    public class PriceService : IPriceService
    {
        private const int MaxBatchSize = 100;

        private readonly GrulaApiClient _grulaApiClient;
        private readonly IPricingSettings _settings;
        private readonly ILogger<PriceService> _logger;

        public PriceService(
            GrulaApiClient grulaApiClient,
            IPricingSettings settings,
            ILogger<PriceService> logger)
        {
            _grulaApiClient = grulaApiClient;
            _settings = settings;
            _logger = logger;
        }

        public async Task<Price> GetPrice(
            DateTime pricingDate,
            PriceProduct product,
            PriceClient client)
        {
            if (!_settings.IsGrulaConfigured ||
                string.IsNullOrWhiteSpace(product.PrimarySku) ||
                string.IsNullOrWhiteSpace(product.FabricsGroup) ||
                !CanSeePrices(client?.Id))
            {
                return null;
            }

            var priceQuery = new GetPriceByPriceDriversQuery
            {
                EnvironmentId = Guid.Parse(_settings.GrulaEnvironmentId),
                PriceDrivers = CreatePriceDrivers(product, client),
                CurrencyThreeLetterCode = client?.CurrencyCode ?? _settings.DefaultCurrency,
                PricingDate = pricingDate
            };

            try
            {
                var grulaPrice = await _grulaApiClient.GetPriceByPriceDriversAsync(priceQuery);

                if (grulaPrice?.Amount is not null)
                {
                    return new Price
                    {
                        CurrentPrice = (decimal)grulaPrice.Amount.Amount,
                        CurrencyCode = grulaPrice.Amount.CurrencyThreeLetterCode,
                    };
                }

                return default;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching price for product {PrimarySku} for client {ClientId} ({ClientName}) from the Grula API.", product?.PrimarySku, client?.Id, client?.Name);

                return default;
            }
        }

        public async Task<IEnumerable<Price>> GetPrices(
            DateTime pricingDate,
            IEnumerable<PriceProduct> products,
            PriceClient client)
        {
            if (!_settings.IsGrulaConfigured || !CanSeePrices(client?.Id))
            {
                return Enumerable.Empty<Price>();
            }

            var productList = (products ?? Enumerable.Empty<PriceProduct>()).ToList();
            var result = new Price[productList.Count];

            var validIndexed = productList
                .Select((p, idx) => new { Product = p, Index = idx })
                .Where(x => !string.IsNullOrWhiteSpace(x.Product.PrimarySku) &&
                            !string.IsNullOrWhiteSpace(x.Product.FabricsGroup))
                .ToList();

            if (!validIndexed.Any())
            {
                return result;
            }

            var pairs = validIndexed.Select(x => new
            {
                x.Index,
                Request = new PriceRequest
                {
                    PriceDrivers = CreatePriceDrivers(x.Product, client),
                    CurrencyThreeLetterCode = client?.CurrencyCode ?? _settings.DefaultCurrency,
                    PricingDate = pricingDate
                }
            }).ToList();

            var batches = pairs.Chunk(MaxBatchSize);
            int batchIdx = 0;

            foreach (var batch in batches)
            {
                var priceQuery = new GetPricesByPriceDriversQuery
                {
                    EnvironmentId = Guid.Parse(_settings.GrulaEnvironmentId),
                    PriceRequests = batch.Select(b => b.Request).ToList(),
                };

                try
                {
                    var grulaPrices = await _grulaApiClient.GetPricesByPriceDriversAsync(priceQuery);

                    for (int i = 0; i < batch.Length && i < grulaPrices.Count; i++)
                    {
                        var grulaPrice = grulaPrices.ElementAtOrDefault(i);
                        if (grulaPrice?.Amount is not null)
                        {
                            result[batch[i].Index] = new Price
                            {
                                CurrentPrice = (decimal)grulaPrice.Amount.Amount,
                                CurrencyCode = grulaPrice.Amount.CurrencyThreeLetterCode
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while fetching prices from the Grula API for batch {BatchIndex} of size {BatchSize}.", batchIdx, batch.Length);
                }

                batchIdx++;
            }

            return result;
        }

        public async Task<IReadOnlyList<PriceLookupResult>> GetPriceResultsForBasketAsync(
            DateTime pricingDate,
            IEnumerable<PriceProduct> products,
            PriceClient client)
        {
            var productList = (products ?? Enumerable.Empty<PriceProduct>()).ToList();
            var results = new PriceLookupResult[productList.Count];

            if (!_settings.IsGrulaConfigured)
            {
                return productList.Select(_ => new PriceLookupResult { Status = PriceLookupStatus.ServiceUnavailable }).ToArray();
            }

            // Price visibility is a per-client rule, and it must be applied before anything is persisted -
            // masking a response is not enough, because the basket is read back by SSR, upload and checkout.
            if (!CanSeePrices(client?.Id))
            {
                return productList.Select(_ => new PriceLookupResult { Status = PriceLookupStatus.PricesHidden }).ToArray();
            }

            var validIndexed = productList
                .Select((product, index) => new { Product = product, Index = index })
                .Where(x =>
                {
                    var valid = x.Product is not null &&
                                !string.IsNullOrWhiteSpace(x.Product.PrimarySku) &&
                                !string.IsNullOrWhiteSpace(x.Product.FabricsGroup);
                    if (!valid)
                    {
                        _logger.LogWarning(
                            "Skipping Grula pricing for basket line {Index}: product {Sku} has incomplete price drivers (PrimarySku or price-group attribute missing).",
                            x.Index,
                            x.Product?.ProductVariantSku ?? x.Product?.PrimarySku);

                        results[x.Index] = new PriceLookupResult { Status = PriceLookupStatus.InvalidPriceDrivers };
                    }

                    return valid;
                })
                .ToList();

            foreach (var batch in validIndexed.Chunk(MaxBatchSize))
            {
                try
                {
                    var query = new GetPricesByPriceDriversQuery
                    {
                        EnvironmentId = Guid.Parse(_settings.GrulaEnvironmentId),
                        PriceRequests = batch.Select(x => new PriceRequest
                        {
                            PriceDrivers = CreatePriceDrivers(x.Product, client),
                            CurrencyThreeLetterCode = client?.CurrencyCode ?? _settings.DefaultCurrency,
                            PricingDate = pricingDate
                        }).ToList()
                    };
                    var grulaPrices = await _grulaApiClient.GetPricesByPriceDriversAsync(query);

                    for (var i = 0; i < batch.Length; i++)
                    {
                        var grulaPrice = grulaPrices?.ElementAtOrDefault(i);
                        results[batch[i].Index] = grulaPrices is null || i >= grulaPrices.Count
                            ? new PriceLookupResult { Status = PriceLookupStatus.MissingResponse }
                            : grulaPrice?.Amount is null
                                ? new PriceLookupResult { Status = PriceLookupStatus.AuthoritativeNoPrice }
                                : new PriceLookupResult
                                {
                                    Status = PriceLookupStatus.Priced,
                                    Price = new Price
                                    {
                                        CurrentPrice = (decimal)grulaPrice.Amount.Amount,
                                        CurrencyCode = grulaPrice.Amount.CurrencyThreeLetterCode
                                    }
                                };
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while fetching trusted basket prices from the Grula API for a batch of size {BatchSize}.", batch.Length);
                    foreach (var item in batch)
                    {
                        results[item.Index] = new PriceLookupResult { Status = PriceLookupStatus.ServiceUnavailable };
                    }
                }
            }

            return results;
        }

        private List<PriceDriver> CreatePriceDrivers(PriceProduct product, PriceClient client)
        {
            var priceDrivers = new List<PriceDriver>
            {
                new PriceDriver
                {
                    Name = PriceDriversConstants.ProductDriver,
                    Value = product.PrimarySku
                },
                new PriceDriver
                {
                    Name = PriceDriversConstants.FabricsGroupDriver,
                    Value = product.FabricsGroup
                }
            };

            if (!string.IsNullOrWhiteSpace(product.SleepAreaSize))
            {
                priceDrivers.Add(new PriceDriver
                {
                    Name = PriceDriversConstants.SleepAreaDriver,
                    Value = product.SleepAreaSize
                });
            }

            if (!string.IsNullOrWhiteSpace(product.ExtraPacking))
            {
                priceDrivers.Add(new PriceDriver
                {
                    Name = PriceDriversConstants.ProductExtraPackingDriver,
                    Value = product.ExtraPacking
                });
            }

            if (!string.IsNullOrWhiteSpace(product.PaletteSize))
            {
                priceDrivers.Add(new PriceDriver
                {
                    Name = PriceDriversConstants.PaletteSizeDriver,
                    Value = product.PaletteSize
                });
            }

            if (!string.IsNullOrWhiteSpace(product.IsOutlet))
            {
                priceDrivers.Add(new PriceDriver
                {
                    Name = PriceDriversConstants.OutletDriver,
                    Value = product.IsOutlet
                });
            }

            if (!string.IsNullOrWhiteSpace(product.Mirror))
            {
                priceDrivers.Add(new PriceDriver
                {
                    Name = PriceDriversConstants.MirrorDriver,
                    Value = product.Mirror
                });
            }

            if (!string.IsNullOrWhiteSpace(product.Size))
            {
                priceDrivers.Add(new PriceDriver
                {
                    Name = PriceDriversConstants.SizeDriver,
                    Value = product.Size
                });
            }

            if (!string.IsNullOrWhiteSpace(product.Shape))
            {
                priceDrivers.Add(new PriceDriver
                {
                    Name = PriceDriversConstants.ShapeDriver,
                    Value = product.Shape
                });
            }

            if (!string.IsNullOrWhiteSpace(product.PointsOfLight))
            {
                priceDrivers.Add(new PriceDriver
                {
                    Name = PriceDriversConstants.PointsOfLightDriver,
                    Value = product.PointsOfLight
                });
            }

            if (!string.IsNullOrWhiteSpace(product.LampshadeType))
            {
                priceDrivers.Add(new PriceDriver
                {
                    Name = PriceDriversConstants.LampshadeTypeDriver,
                    Value = product.LampshadeType
                });
            }

            if (!string.IsNullOrWhiteSpace(product.LampshadeSize))
            {
                priceDrivers.Add(new PriceDriver
                {
                    Name = PriceDriversConstants.LampshadeSizeDriver,
                    Value = product.LampshadeSize
                });
            }

            if (!string.IsNullOrWhiteSpace(product.LinearLight))
            {
                priceDrivers.Add(new PriceDriver
                {
                    Name = PriceDriversConstants.LinearLightDriver,
                    Value = product.LinearLight
                });
            }

            if (!string.IsNullOrWhiteSpace(product.PrimaryColor))
            {
                priceDrivers.Add(new PriceDriver
                {
                    Name = PriceDriversConstants.PrimaryColorDriver,
                    Value = product.PrimaryColor
                });
            }

            if (!string.IsNullOrWhiteSpace(product.SecondaryColor))
            {
                priceDrivers.Add(new PriceDriver
                {
                    Name = PriceDriversConstants.SecondaryColorDriver,
                    Value = product.SecondaryColor
                });
            }

            if (!string.IsNullOrWhiteSpace(product.BodyColour))
            {
                priceDrivers.Add(new PriceDriver
                {
                    Name = PriceDriversConstants.BodyColourDriver,
                    Value = product.BodyColour
                });
            }

            if (!string.IsNullOrWhiteSpace(product.ShelfType))
            {
                priceDrivers.Add(new PriceDriver
                {
                    Name = PriceDriversConstants.ShelfTypeDriver,
                    Value = product.ShelfType
                });
            }

            if (!string.IsNullOrWhiteSpace(product.NumberOfMirrors))
            {
                priceDrivers.Add(new PriceDriver
                {
                    Name = PriceDriversConstants.NumberOfMirrorsDriver,
                    Value = product.NumberOfMirrors
                });
            }

            if (!string.IsNullOrWhiteSpace(product.Led))
            {
                priceDrivers.Add(new PriceDriver
                {
                    Name = PriceDriversConstants.LedDriver,
                    Value = product.Led
                });
            }

            if (!string.IsNullOrWhiteSpace(product.ProductVariantSku))
            {
                priceDrivers.Add(new PriceDriver
                {
                    Name = PriceDriversConstants.ProductVariantSkuDriver,
                    Value = product.ProductVariantSku
                });
            }

            if (client is not null)
            {
                if (!string.IsNullOrWhiteSpace(client.DiscountCode))
                {
                    priceDrivers.Add(new PriceDriver
                    {
                        Name = PriceDriversConstants.DiscountCodeDriver,
                        Value = client.DiscountCode
                    });
                }

                if (!string.IsNullOrWhiteSpace(client.Name))
                {
                    priceDrivers.Add(new PriceDriver
                    {
                        Name = PriceDriversConstants.ClientDriver,
                        Value = client.Name
                    });
                }

                if (!string.IsNullOrWhiteSpace(client.ExtraPacking))
                {
                    priceDrivers.Add(new PriceDriver
                    {
                        Name = PriceDriversConstants.ClientExtraPackingDriver,
                        Value = client.ExtraPacking
                    });
                }

                if (!string.IsNullOrWhiteSpace(client.PaletteLoading))
                {
                    priceDrivers.Add(new PriceDriver
                    {
                        Name = PriceDriversConstants.PaletteLoadingDriver,
                        Value = client.PaletteLoading
                    });
                }

                if (!string.IsNullOrWhiteSpace(client.Country))
                {
                    priceDrivers.Add(new PriceDriver
                    {
                        Name = PriceDriversConstants.ClientCountryDriver,
                        Value = client.Country
                    });
                }

                if (!string.IsNullOrWhiteSpace(client.DeliveryZipCode))
                {
                    priceDrivers.Add(new PriceDriver
                    {
                        Name = PriceDriversConstants.DeliveryAddressDriver,
                        Value = client.DeliveryZipCode
                    });
                }

                var currencyValue = MapCurrencyDriverValue(client.CurrencyCode ?? _settings.DefaultCurrency);
                if (!string.IsNullOrWhiteSpace(currencyValue))
                {
                    priceDrivers.Add(new PriceDriver
                    {
                        Name = PriceDriversConstants.CurrencyDriver,
                        Value = currencyValue
                    });
                }
            }

            return priceDrivers;
        }

        private static string MapCurrencyDriverValue(string currencyCode) => currencyCode?.ToUpperInvariant() switch
        {
            "EUR" => "Euro",
            "PLN" => "Złoty",
            _ => null
        };

        public bool CanSeePrices(Guid? priceClientId)
        {
            if (string.IsNullOrWhiteSpace(_settings.EnablePricesForClients))
            {
                return true;
            }

            if (!priceClientId.HasValue)
            {
                return false;
            }

            var allowedClients = _settings.EnablePricesForClients.Split('&');

            return allowedClients.Contains(priceClientId.ToString());
        }
    }
}
