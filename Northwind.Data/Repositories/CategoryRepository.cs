using Dapper;
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

    public void Add(Category category)
    {
        const string sql = @"
            INSERT INTO Categories (CategoryName, Description)
            VALUES (@CategoryName, @Description);
            SELECT CAST(SCOPE_IDENTITY() AS int);";

        using var connection = Connection;
        category.CategoryID = connection.QuerySingle<int>(sql, category);
    }

    public void Delete(int id)
    {
        const string sql = "DELETE FROM Categories WHERE CategoryID = @Id";

        using var connection = Connection;
        connection.Execute(sql, new { Id = id });
    }

    public IEnumerable<Category> GetAll()
    {
        const string sql = "SELECT CategoryID, CategoryName, Description FROM Categories";

        using var connection = Connection;
        return connection.Query<Category>(sql).ToList();
    }

    public Category? GetById(int id)
    {
        const string sql = "SELECT CategoryID, CategoryName, Description FROM Categories WHERE CategoryID = @Id";

        using var connection = Connection;
        return connection.QuerySingleOrDefault<Category>(sql, new { Id = id });
    }

    public void Update(Category category)
    {
        const string sql = @"
            UPDATE Categories
            SET CategoryName = @CategoryName,
                Description = @Description
            WHERE CategoryID = @CategoryID";

        using var connection = Connection;
        connection.Execute(sql, category);
    }
}
