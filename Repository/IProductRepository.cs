using Microsoft.AspNetCore.Mvc;

namespace ProductManagement;

public interface IProductRepository
{
    public Task<Product> GetProductById(int productId);
    Task<IEnumerable<Product>> GetProducts();
    Task<Product> AddProduct(Product product);

}
