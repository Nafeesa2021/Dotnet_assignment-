namespace ProductManagement;

public interface IProductService
{
    Task<Product> GetProductById(int productId);
    Task<IEnumerable<Product>> GetProducts();
    Task<Product> AddProduct(Product product);
}
