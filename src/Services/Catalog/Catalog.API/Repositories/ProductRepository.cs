using Catalog.API.Repositories.Interfaces;
using Catalog.API.Entities;
using Catalog.API.Data.Interfaces;
using MongoDB.Driver;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Catalog.API.Models;
using System;
using Nest;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Catalog.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _context;
        private readonly IElasticClient _elasticClient;
        public ProductRepository(ICatalogContext context, IElasticClient elasticClient)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _elasticClient = elasticClient ?? throw new ArgumentNullException(nameof(elasticClient));

        }

        public async Task<ProductResponse<Product>> GetProducts()
        {
            var pageSize = 6f;

            var filter = Builders<Product>.Filter.Ne(x => x.ItemsInStock, 0);

            var result = await _context.Products
                            .Find(filter)
                            .SortByDescending(p => p.CreatedAt)
                            //.Limit((int) pageSize)
                            .ToListAsync();

            return new ProductResponse<Product>
            {
                CurrentPage = 1,
                Results = result,
                TotalPages = (int) Math.Ceiling(result.Count() / pageSize)
            };
        }

        public async Task<Product> GetProduct(string id)
        {
            return await _context
                           .Products
                           .Find(p => p.Id == id)
                           .FirstOrDefaultAsync();
        }


        private ProductResponse<Product> Ok(List<Product> products)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Name, name);

            return await _context
                            .Products
                            .Find(filter)
                            .ToListAsync();
        }
        public async Task<ProductResponse<Product>> SearchProducts(string keyword, int Page)
        {
            var pageSize = 6f;
            FilterDefinition<Product> filter = Builders<Product>.Filter.Or(
         Builders<Product>.Filter.Regex(p => p.Name, new BsonRegularExpression(keyword, "i")),
         Builders<Product>.Filter.Regex(p => p.Description, new BsonRegularExpression(keyword, "i")),
         Builders<Product>.Filter.Regex(p => p.Category, new BsonRegularExpression(keyword, "i")),
         Builders<Product>.Filter.Regex(p => p.Price, keyword)
    );
            var TotalPages = _context.Products.Count(filter);

            var result = await _context.Products
                            .Find(filter)
                            .Skip((Page - 1) * (int)pageSize)
                            .Limit((int)pageSize)
                            .ToListAsync();

            return new ProductResponse<Product>
            {
                CurrentPage = Page,
                Results = result,
                TotalPages = (int)Math.Ceiling(TotalPages / pageSize)
            };
        }
        public async Task<ProductResponse<Product>> GetProductByCategory(string categoryName, int Page)
        {
            var pageSize = 6f;

            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Category, categoryName);

            var TotalPages = _context.Products.Count(filter);

            var result = await _context.Products
                            .Find(filter)
                            .Skip((Page - 1) * (int)pageSize)
                            .Limit((int)pageSize)
                            .ToListAsync();

            return new ProductResponse<Product>
            {
                CurrentPage = Page,
                Results = result,
                TotalPages = (int)Math.Ceiling(TotalPages / pageSize)
            };
        }
        public async Task<IEnumerable<Product>> GetProductByShopOwner(string ownerId)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.UserId, ownerId.Trim());

            return await _context
                            .Products
                            .Find(filter)
                            .ToListAsync();
        }
        public async Task CreateProduct(Product product)
        {
            await _context.Products.InsertOneAsync(product);
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var updateResult = await _context
                                        .Products
                                        .ReplaceOneAsync(filter: g => g.Id == product.Id, replacement: product);

            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> DeleteProduct(string id)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);

            DeleteResult deleteResult = await _context
                                                .Products
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;
        }

    }
}
