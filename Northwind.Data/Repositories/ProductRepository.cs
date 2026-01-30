using Microsoft.Data.SqlClient;
using Northwind.Data.Interfaces;
using Northwind.Data.Models;
using System.Data;

namespace Northwind.Data.Repositories;

public class ProductRepository : IProductRepository
{

    private readonly string _connectionString;

    public ProductRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    private IDbConnection Connection => new SqlConnection(_connectionString);

    public void Add(Product product)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Product> GetAll()
    {
        throw new NotImplementedException();
    }

    public Product GetById(int id)
    {
        throw new NotImplementedException();
    }

    public void Update(Product product)
    {
        throw new NotImplementedException();
    }
}
