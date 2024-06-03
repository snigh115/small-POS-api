using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using POS.Data;
using POS.Model;
using POS.Model.DTO;

namespace POS.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        // private readonly List<Product> _products = new List<Product>(); 

        private readonly MyDbContext _context;

        public ProductsController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetProducts(int page = 1, int pageSize = 10)
        {

            var totalCount = _context.Products.Count();
            var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);

            var products = _context.Products
                .Skip((page - 1) * pageSize)
                .Take(pageSize)    
                .Where(p => !p.IsDelete)                
                .ToList();       
                                                
            return Ok(products);
        }

        [HttpPost]
        public ActionResult AddProduct(ProductDto productDto)
        {
            if (productDto == null)
            {
                return BadRequest();
            }

            var product = new Product 
            {
                ProductName = productDto.ProductName,
                Description = productDto.Description,
                Price = productDto.Price,
                QuantityAvailable = productDto.QuantityAvailable,
                CategoryId = productDto.CategoryId,
                IsDelete = false,
            };

            var category = _context.Categories.Find(productDto.CategoryId);
            if (category == null)
            {
                return BadRequest("Category does not exist");
            }

            product.Category = category;


            _context.Products.Add(product);         
            _context.SaveChanges();
            
            return CreatedAtAction(nameof(GetProducts), new {id = product.ProductId }, product);
        }

        [HttpPut("update/{id}")]
        public ActionResult UpdateProduct(int id, ProductDto productDto)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound("Product does not exist");
            }

            if (productDto == null)
            {
                // return BadRequest("Product data is null");
                var productJson = JsonSerializer.Serialize(product);
                var jsonResult = new ContentResult
                {
                    Content = productJson,
                    ContentType = "application/json",
                    StatusCode = 200
                };

                return jsonResult;
            }

            var category = _context.Categories.Find(productDto.CategoryId);
            if(category == null)    
            {
                return BadRequest("Category does not exist");
            }

            product.ProductName = productDto.ProductName;
            product.Description = productDto.Description;
            product.Price = productDto.Price;
            product.QuantityAvailable = productDto.QuantityAvailable;
            product.CategoryId = productDto.CategoryId;
            product.Category = category;
            product.IsDelete = productDto.IsDelete;

            _context.Products.Update(product);
            _context.SaveChanges();

            return Ok(product);
            
        }

        [HttpDelete("delete/{id}")]
        public ActionResult DeleteProduct(int id)
        {
            var product = _context.Products.Find(id);

            if (product == null)
            {
                return NotFound("Product not found with that id!");
            }

            if ( !product.IsDelete )
            {
                return BadRequest("Product is not deleted yet!");
            }

            _context.Products.Remove(product);
            _context.SaveChanges();
            return NoContent();
        }    

        [HttpDelete("{id}")]
        public ActionResult SoftDelete (int id)
        {
            var product = _context.Products.Find(id);

            if ( product == null )
            {
                return NotFound("Product not found with that id!");
            }

            product.IsDelete = true;

            _context.SaveChanges();
            return Ok("Product is soft deleted");
        }

        [HttpGet("softDelete")]
        public ActionResult<IEnumerable<Product>> GetSoftDeletedProducts(int page = 1, int pageSize = 3)
        {
            var totalCount = _context.Products.Where(p => p.IsDelete).Count();
            var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);

            var products = _context.Products
                .Where(p => p.IsDelete)
                .Skip((page - 1) * pageSize )
                .Take(pageSize)
                .ToList();

            return Ok(products);
        }

        [HttpPut("{id}/restore")]
        public ActionResult RestoreProduct(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.IsDelete && p.ProductId == id);

            if (product == null )
            {
                return NotFound("Product not found with that id !");
            }

            product.IsDelete = false ;
            _context.SaveChanges();
            return Ok("Product is restored");
        }

        [HttpGet("{id}")]
        public ActionResult GetProductById(int id)
        {
            var product = _context.Products
                .Where(p => !p.IsDelete)
                .FirstOrDefault( p => p.ProductId == id);

            if (product == null)
            {
                return NotFound("Product not found with that id !");
            }

            return Ok(product);
        }

        [HttpGet("search/{name}")]
        public ActionResult<IEnumerable<Product>> GetProductByName(string name)
        {
            var products = _context.Products.Where(p => p.ProductName.Contains(name)).ToList();
            return Ok(products);
        }

        [HttpGet("search/category/categoryName/{categoryName}")]
        public ActionResult<IEnumerable<Product>> GetProductByCategory(string categoryName)
        {
            var products = _context.Products
                .Where(p => p.Category.Name.Contains(categoryName))
                .ToList();    
            return Ok(products);
        }

        [HttpGet("search/category/id/{categoryId}")]
        public ActionResult<IEnumerable<Product>> GetProductByCategoryId(int categoryId)
        {
            var products = _context.Products.Where(p => p.CategoryId == categoryId).ToList();
            return Ok(products);
        }
        
    }
}