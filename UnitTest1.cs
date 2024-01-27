using ProductManagement;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductManagementUnitTest;

public class ProductControllerTest 
{

    // Test case to get product by id 
    [Fact]
    public async Task GetProductByProductId_ValidId_ReturnsOk()
    {

        var productServiceMock = new Mock<IProductService>();

        var loggerMock = new Mock<ILogger<ProductController>>();

        productServiceMock.Setup(x => x.GetProductById(1))
                          .ReturnsAsync(new Product
                          {
                              ProductID = 1,
                              ProductName = "Pen",
                              CategoryID = 11,
                              SupplierID = 21,
                              UnitPrice = 100,
                              UnitsInStock = 10,
                              Discontinued = true
                          });     

        // Create the controller with the mocked dependencies
        var controller = new ProductController(productServiceMock.Object, loggerMock.Object);

        // Act
        var result = await controller.GetProductByProductId(1);

        // Assert
        Assert.IsType<ActionResult<Product>>(result);  
        
        var okResult = Assert.IsType<OkObjectResult>(result.Result);  
        Assert.IsType<Product>(okResult.Value);

        var product = okResult.Value as Product;
        Assert.Equal(1, product.ProductID);
        Assert.Equal("Pen", product.ProductName);
        Assert.Equal(11, product.CategoryID);
        Assert.Equal(21, product.SupplierID);
        Assert.Equal(100, product.UnitPrice);
        Assert.Equal(10, product.UnitsInStock);
        Assert.True(product.Discontinued);
    }

    [Fact]
    public async void GetProductByProductId_InvalidId_ReturnsNotFound()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<ProductController>>();

        var productServiceMock = new Mock<IProductService>();

        var controller = new ProductController(productServiceMock.Object, loggerMock.Object);


        productServiceMock.Setup(x => x.GetProductById(It.IsAny<int>()))
                          .ReturnsAsync((Product)null);

        // Act
        var result = await controller.GetProductByProductId(1234);

        // Assert
        Assert.IsType<ActionResult<Product>>(result);
    }

    [Fact]
    public async void GetProductByProductId_Exception_ReturnsInternalServerError()
    {
        // Arrange
        var productServiceMock = new Mock<IProductService>();

        var loggerMock = new Mock<ILogger<ProductController>>();

        var controller = new ProductController(productServiceMock.Object, loggerMock.Object);

        // Mock the Product service to throw an exception
        productServiceMock.Setup(x => x.GetProductById(It.IsAny<int>()))
                          .ThrowsAsync(new Exception("Internal Server Error"));

        // Act
        var result = await controller.GetProductByProductId(-1);

        // Assert

        Assert.IsType<ActionResult<Product>>(result);

        var objectResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, objectResult.StatusCode);
    }

    // Test case to get the products
    [Fact]
    public async Task GetAllProducts_ReturnsOk()
    {
        // Arrange
        var productServiceMock = new Mock<IProductService>();
        
        var loggerMock = new Mock<ILogger<ProductController>>();

        var controller = new ProductController(productServiceMock.Object, loggerMock.Object);

        // Mock the ProductService to return a list of products
        var expectedProducts = new List<Product>
        {
            new Product { 
                            ProductID = 1,
                            ProductName = "Pen",
                            CategoryID = 11,
                            SupplierID = 21,
                            UnitPrice = 100,
                            UnitsInStock = 10,
                            Discontinued = true },
            new Product {   ProductID = 2,
                            ProductName = "Pencil",
                            CategoryID = 12,
                            SupplierID = 22,
                            UnitPrice = 100,
                            UnitsInStock = 10,
                            Discontinued = false },
            new Product {   ProductID = 3,
                            ProductName = "Pencil box",
                            CategoryID = 13,
                            SupplierID = 23,
                            UnitPrice = 100,
                            UnitsInStock = 10,
                            Discontinued = true }
        };

        productServiceMock.Setup(x => x.GetProducts())
                          .ReturnsAsync(expectedProducts);

        // Act
        var result = await controller.GetAllProducts();

        // Assert
        Assert.IsType<ActionResult<IEnumerable<Product>>>(result);

        var okResult = Assert.IsType<OkObjectResult>(result.Result); 
        var actualProducts = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);

        Assert.Equal(expectedProducts.Count, actualProducts.Count());

    }

    [Fact]
    public async Task GetAllProducts_Exception_ReturnsInternalServerError()
    {
        // Arrange
        var productServiceMock = new Mock<IProductService>();
        var loggerMock = new Mock<ILogger<ProductController>>();

        var controller = new ProductController(productServiceMock.Object, loggerMock.Object);

        // Mock the ProductService to throw an exception
        productServiceMock.Setup(x => x.GetProducts())
                          .ThrowsAsync(new Exception("Simulated exception"));

        // Act
        var result = await controller.GetAllProducts();

        // Assert

        Assert.IsType<ActionResult<IEnumerable<Product>>>(result);

        var objectResult = result.Result as ObjectResult;
        Assert.NotNull(objectResult);

        Assert.Equal(500, objectResult.StatusCode); 
        Assert.Equal("Internal Server Error", objectResult.Value);

    }

    // Test case to add a new product 
    [Fact]
        public async Task AddNewProduct_ReturnsCreatedAtAction()
        {
            // Arrange
            var productServiceMock = new Mock<IProductService>();
            var loggerMock = new Mock<ILogger<ProductController>>();

            var controller = new ProductController(productServiceMock.Object, loggerMock.Object);

            var productToAdd = new Product
            {
                ProductID = 3,
                ProductName = "Pencil box",
                CategoryID = 13,
                SupplierID = 23,
                UnitPrice = 100,
                UnitsInStock = 10,
                Discontinued = true
            };

            var addedProduct = new Product
            {
                ProductID = 3, 
                ProductName = "Pencil box",
                CategoryID = 13,
                SupplierID = 23,
                UnitPrice = 100,
                UnitsInStock = 10,
                Discontinued = true
            };

            productServiceMock.Setup(x => x.AddProduct(productToAdd))
                              .ReturnsAsync(addedProduct);

            // Act
            var result = await controller.AddNewProduct(productToAdd);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(nameof(controller.AddNewProduct), createdAtActionResult.ActionName);
            Assert.Equal(addedProduct.ProductID, createdAtActionResult.RouteValues["id"]);
            Assert.Equal(addedProduct.ProductID, createdAtActionResult.Value);
        }

        [Fact]
        public async Task AddNewProduct_Exception_ReturnsInternalServerError()
        {
            // Arrange
            var productServiceMock = new Mock<IProductService>();
            var loggerMock = new Mock<ILogger<ProductController>>();

            var controller = new ProductController(productServiceMock.Object, loggerMock.Object);

            var productToAdd = new Product
            {
                ProductID = 3,
                ProductName = "Pencil box",
                CategoryID = 13,
                SupplierID = 23,
                UnitPrice = 100,
                UnitsInStock = 10,
                Discontinued = true
            };

            productServiceMock.Setup(x => x.AddProduct(productToAdd))
                              .ReturnsAsync((Product)null);

            // Act
            var result = await controller.AddNewProduct(productToAdd);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Internal Server Error", objectResult.Value);
            
        }

}