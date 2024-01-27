using System.Data;
using System.Threading.Tasks;
using Npgsql;

namespace ProductManagement;

public interface IDBConnectionFactory
{
    IDbConnection CreateConnection();
}
