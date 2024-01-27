using System.Data;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Threading.Tasks;

namespace ProductManagement;

public class ProductRepository : IProductRepository
{
    private readonly IDBConnectionFactory _dbConnectionFactory;

    public ProductRepository(IDBConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<Product> GetProductById(int productId)
    {
        try{
            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                if(connection != null)
                {
                    using (var command = connection.CreateCommand())
                    {
                        connection.Open();
                        command.CommandText= "SELECT * FROM product WHERE product_id = @productId";
                        command.Parameters.Add(new NpgsqlParameter("@productId", productId));

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var product = MapProductFromReader(reader);
                                return product;
                            }
                        }
                    }
                }
            }
        }
        catch(Exception ex){
            Console.WriteLine("Error while getting the product by id", ex);
        }
        return null;
    }

    public async Task<IEnumerable<Product>> GetProducts()
    {
        var products = new List<Product>();
        try{
            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = "SELECT * FROM product";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var product = MapProductFromReader(reader);
                        products.Add(product);
                    }
                }
            }
        }
        }
        catch(Exception ex){
            Console.WriteLine("error while getting the products", ex);
        }
        return products;
    }

    public async Task<Product> AddProduct(Product product)
    {
        try{
        using (var connection = _dbConnectionFactory.CreateConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO Product(product_id,product_name, category_id, supplier_id, unit_price, unit_in_stock, discontinued) " +
                                      "VALUES (@productId,@productName, @categoryID, @supplierID, @unitPrice, @unitsInStock, @discontinued) " +
                                      "RETURNING product_id";
                MapProductToCommandParameters(command, product);
                var noOfRowsAffected = command.ExecuteNonQuery();
                return product;
            }
        }
        }
        catch(Exception ex){
            Console.WriteLine("Error while adding the products",ex);
            return null;
        }

    }

    private Product MapProductFromReader(IDataReader reader)
    {
        return new Product
        {
            ProductID = reader.GetInt32(reader.GetOrdinal("product_id")),
            ProductName = reader.GetString(reader.GetOrdinal("product_name")),
            CategoryID = reader.GetInt32(reader.GetOrdinal("category_id")),
            SupplierID = reader.GetInt32(reader.GetOrdinal("supplier_id")),
            UnitPrice = reader.GetDecimal(reader.GetOrdinal("unit_price")),
            UnitsInStock = reader.GetInt32(reader.GetOrdinal("unit_in_stock")),
            Discontinued = reader.GetBoolean(reader.GetOrdinal("discontinued"))
        };
    }

    private void MapProductToCommandParameters(IDbCommand command, Product product)
    {
        command.Parameters.Add(new NpgsqlParameter("@productId",product.ProductID));
        command.Parameters.Add(new NpgsqlParameter("@productName", product.ProductName));
        command.Parameters.Add(new NpgsqlParameter("@categoryID", product.CategoryID));
        command.Parameters.Add(new NpgsqlParameter("@supplierID", product.SupplierID));
        command.Parameters.Add(new NpgsqlParameter("@unitPrice", product.UnitPrice));
        command.Parameters.Add(new NpgsqlParameter("@unitsInStock", product.UnitsInStock));
        command.Parameters.Add(new NpgsqlParameter("@discontinued", product.Discontinued));
    }

}
