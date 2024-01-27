using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;


namespace ProductManagement;
[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductController> _logger;

    public ProductController(IProductService productService, ILogger<ProductController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    [HttpGet("{product_Id}")]
        public async Task<ActionResult<Product>> GetProductByProductId(int product_Id)
        {
            try{
                var product = await _productService.GetProductById(product_Id);
                if (product != null)
                {
                    return Ok(product);
                }

                return NotFound();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting product with ID {product_Id}");
                return StatusCode(500, "Internal Server Error");
            }
        }

    [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            try{
                var products = await _productService.GetProducts();
                return Ok(products);
            }
            catch(Exception ex){
                _logger.LogError(ex, $"An error occurred while getting all the products");
                return StatusCode(500, "Internal Server Error");
            }
        }

    [HttpPost]
        public async Task<ActionResult<int>> AddNewProduct(Product product)
        {
            try{
            var newProduct = await _productService.AddProduct(product);
            return CreatedAtAction(nameof(AddNewProduct), new { id = newProduct.ProductID }, newProduct.ProductID);
            }
            catch(Exception ex){
                _logger.LogError(ex, "An error occurred while adding a new product");
                return StatusCode(500, "Internal Server Error");
            }
        }

}
