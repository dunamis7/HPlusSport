using HPSportsPlus.DTOs;
using HPSportsPlus.Models;

namespace HPSportsPlus.Services
{
    public interface IProductService
    {
        Task<ProductDTO> GetAProductAsync(int id);

        IQueryable<Product> GetAll();

        Task AddProductAsync(Product product);  

        Task <Product> DeleteProductAsync(int id); 
        
        Task<Product> UpdateProductAsync(int id, Product product);
        
    }
}
