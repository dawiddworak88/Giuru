﻿using Foundation.Catalog.Infrastructure;
using Foundation.Catalog.Repositories.ProductIndexingRepositories;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.BackgroundTasks.Services.Products
{
    public class ProductsService : IProductsService
    {
        private readonly CatalogContext _context;
        private readonly IProductIndexingRepository _productIndexingRepository;
        private readonly IBulkProductIndexingRepository _bulkProductIndexingRepository;
        private readonly ILogger<ProductsService> _logger;

        public ProductsService(
            CatalogContext context,
            IProductIndexingRepository productIndexingRepository,
            IBulkProductIndexingRepository bulkProductIndexingRepository,
            ILogger<ProductsService> logger)
        {
            _context = context;
            _productIndexingRepository = productIndexingRepository;
            _bulkProductIndexingRepository = bulkProductIndexingRepository;
            _logger = logger;
        }

        public async Task IndexAllAsync(Guid? sellerId)
        {
            if (sellerId.HasValue)
            {
                foreach (var productId in _context.Products.Where(x => x.Brand.SellerId == sellerId).Select(x => x.Id).ToList())
                {
                    try
                    {
                        await _bulkProductIndexingRepository.IndexAsync(productId);
                    }
                    catch (Exception exception)
                    {
                        _logger.LogError(exception, $"Couldn't index product: {productId}");
                    }
                }
            }
        }

        public async Task IndexCategoryProducts(Guid? categoryId, Guid? sellerId)
        {
            if (sellerId.HasValue)
            {
                int i = 1;
                foreach (var productId in _context.Products.Where(x => x.Brand.SellerId == sellerId && x.Category.Id == categoryId).Select(x => x.Id).ToList())
                {
                    _logger.LogError($"Indexing product {i} - {productId}");

                    try
                    {
                        await _productIndexingRepository.IndexAsync(productId);
                    }
                    catch (Exception exception)
                    {
                        _logger.LogError(exception, $"Couldn't index product: {productId}");
                    }

                    i++;
                }
            }
        }
    }
}
