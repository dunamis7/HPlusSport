using HPSportsPlus.DTOs;
using HPSportsPlus.Models;
using Microsoft.EntityFrameworkCore;

namespace HPSportsPlus.Services
{
    public class ProductService : IProductService
    {
        private readonly ShopDbContext _context;

        public ProductService(ShopDbContext context)
        {
            _context = context;
        }

        public async Task AddProductAsync(Product product)
        {
        await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            
        }

        public async Task<Product> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
             _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public  IQueryable<Product> GetAll()
        {
            IQueryable<Product> products = _context.Products;
            return products;
        }
       

        public async Task<ProductDTO> GetAProductAsync(int id)
        {
            var product = await _context.Products.Select(p =>
               new ProductDTO()
               {
                   Id = p.Id,
                   Name =p.Name,
                   Sku=p.Sku,
                   Description = p.Description,
                   IsAvailable = p.IsAvailable,
                   Price =p.Price
               } ).SingleOrDefaultAsync(x => x.Id == id);

            return product;
        }

        public async Task<Product> UpdateProductAsync(int id, Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Products.Any(p => p.Id == id))
                {
                    return product;
                }
                else
                {
                    throw;
                }
            }

            return product;
        }
    }
}

