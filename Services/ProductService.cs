using Microsoft.AspNetCore.Mvc;

namespace ProductManagement;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Product> GetProductById(int productId)
        {
            try{
                return await _productRepository.GetProductById(productId);
            }
            catch(Exception ex){
                Console.WriteLine("error while getting the product using id",ex);
                return null;
            }
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            try{
                return await _productRepository.GetProducts();
            }
            catch(Exception ex){
                Console.WriteLine("Error while getting the products", ex);
                return Enumerable.Empty<Product>();
            }
        }

        public async Task<Product> AddProduct(Product product)
        {
            try{
                return await _productRepository.AddProduct(product);
            }
            catch(Exception ex){
                Console.WriteLine("Error while adding new product",ex);
                return null;
            }
            
        }

}
