using HPSportsPlus.Models;
using HPSportsPlus.Models.AdvancedRetreival;
using HPSportsPlus.Models.Advanced_Retrieval_Classes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using HPSportsPlus.Services;
using HPSportsPlus.DTOs;

namespace HPSportsPlus.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    //[Route("api/[controller]")]

    //[Route("v{v:apiVersion}/products")] url versioning
    [Route("products")]
    public class ProductsV1Controller : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsV1Controller(IProductService service)
        {
          _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts([FromQuery]ProductQueryParameters queryParameters) // instantiating query parameters
        {
           // IQueryable<Product> products = _service.Products; // returns all items as an IQueryable so we can query it

            var products =  _service.GetAll();
            if (!string.IsNullOrEmpty(queryParameters.Name)
                || !string.IsNullOrEmpty(queryParameters.Sku) 
                || queryParameters.Price != null || queryParameters.MinPrice!= null 
                || queryParameters.MaxPrice !=null)
            {
             
                products = SearchQuery.CustomSearch(products, queryParameters);
            }


           // Condition to do sorting
            if (!string.IsNullOrEmpty(queryParameters.Sortby))
            {
                if (typeof(Product).GetProperty(queryParameters.Sortby) != null)// checks if the keyword to sort by is a property or column for the table
                {
                    products = products.OrderByCustom(
                        queryParameters.Sortby,
                        queryParameters.SortOrder
                        );
                }
            }
            //Pagination
            products = products
                .Skip(queryParameters.Size * (queryParameters.Page - 1))
                .Take(queryParameters.Size);

            return Ok(products.Select(ProductDTOExtension.GetProductDTO));
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _service.GetAProductAsync(id);

            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> PostProduct([FromBody]Product product)
        {
            await _service.AddProductAsync(product);

            return CreatedAtAction(
                "GetProduct",
                new { id = product.Id },
                product);
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            if (id != product.Id) return BadRequest();
            await _service.UpdateProductAsync(id, product);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task <ActionResult<Product>> DeleteProduct(int id)
        {
            var product = await _service.GetAProductAsync(id);
            if(product == null) return NotFound();

             await _service.DeleteProductAsync(id);

            return Ok(product);

        }

        //[HttpPost]
        //[Route("Delete")]
        //public async Task <IActionResult> DeleteMultiple([FromQuery] int[] ids)
        //{
        //    var products = new List<Product>();

        //    foreach (var id in ids)
        //    {
        //        var product = await _context.Products.FindAsync(id);

        //        if (product == null) return NotFound();

        //        products.Add(product);

        //    }

        //    _context.Products.RemoveRange(products);
        //    await _context.SaveChangesAsync();

        //    return Ok(products);
        //}


    }




 

}
