using Microsoft.EntityFrameworkCore;
using ProductManagement;

internal class Program
{
    private static void Main(string[] args)
    {
        // Nafeesa@16052002
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddScoped<IDBConnectionFactory,NpgsqlDBConnection>();
        builder.Services.AddScoped<IProductService,ProductService>();
        builder.Services.AddScoped<IProductRepository,ProductRepository>();
        // builder.Services.AddScoped<ILogger<ProductController>>();
        builder.Services.AddLogging();
        // Add services to the container.
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}