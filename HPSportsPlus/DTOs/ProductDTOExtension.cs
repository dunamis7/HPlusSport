using HPSportsPlus.Models;

namespace HPSportsPlus.DTOs
{
    public static class ProductDTOExtension
    {
        public static ProductDTO GetProductDTO(this Product product)
        {
            return new ProductDTO
            { 
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                IsAvailable = product.IsAvailable,
                Sku = product.Sku,
                Price = product.Price
                
            };
                   
        }
    }
}
