using Microsoft.Data.SqlClient;
using Northwind.Data.Interfaces;
using Northwind.Data.Models;
using System.Data;

namespace Northwind.Data.Repositories;

public class CategoryRepository : ICategoryRepository
{

    private readonly string _connectionString;

    public CategoryRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    private IDbConnection Connection => new SqlConnection(_connectionString);

    public void Add(Category product)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Category> GetAll()
    {
        throw new NotImplementedException();
    }

    public Category GetById(int id)
    {
        throw new NotImplementedException();
    }

    public void Update(Category product)
    {
        throw new NotImplementedException();
    }
}
