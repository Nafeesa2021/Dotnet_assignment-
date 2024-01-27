using System.Data;
using Npgsql;

namespace ProductManagement;

public class NpgsqlDBConnection : IDBConnectionFactory
{
    private readonly IConfiguration _configuration;

    public NpgsqlDBConnection(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IDbConnection CreateConnection()
    {
        string connectionString = _configuration.GetConnectionString("DefaultConnect");
        return new NpgsqlConnection(connectionString);
    }
}
