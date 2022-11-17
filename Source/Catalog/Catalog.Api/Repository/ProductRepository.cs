using Catalog.Api.Data;
using Catalog.Api.Entitites;
using MongoDB.Driver;

namespace Catalog.Api.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _context;
        public ProductRepository(ICatalogContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task CreateProduct(Product product)
        {
           await _context.Products.InsertOneAsync(product);

        }

        public async Task<bool> DeleteProduct(string id)
        {
            FilterDefinition<Product> filterDefinition = Builders<Product>.Filter.Eq(p => p.Id, id);
            DeleteResult deletedResult = await _context.Products.DeleteOneAsync(filterDefinition);

            return deletedResult.IsAcknowledged && deletedResult.DeletedCount > 0;

        }

        public async Task<Product> GetProduct(string id)
        {
            return (Product)await _context.Products.FindAsync(x=>x.Id == id);
        }

        public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
        {
            FilterDefinition<Product> filterDefinition = Builders<Product>.Filter.Eq(p => p.Category, categoryName);
            return await _context.Products.Find(filterDefinition).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByName(string productName)
        {
            FilterDefinition<Product> filterDefinition = Builders<Product>.Filter.Eq(p => p.Name, productName);
            return await _context.Products.Find(filterDefinition).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context.Products.Find(p=>true).ToListAsync();
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var updateResult = await _context.Products.ReplaceOneAsync(filter: i => i.Id == product.Id, replacement: product);
            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }
    }
}
