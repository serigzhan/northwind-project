using Dapper;
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
        const string sql = @"
            INSERT INTO Products (ProductName, SupplierID, CategoryID, QuantityPerUnit,
                                  UnitPrice, UnitsInStock, UnitsOnOrder, ReorderLevel, Discontinued)
            VALUES (@ProductName, @SupplierID, @CategoryID, @QuantityPerUnit,
                    @UnitPrice, @UnitsInStock, @UnitsOnOrder, @ReorderLevel, @Discontinued);
            SELECT CAST(SCOPE_IDENTITY() AS int);";

        using var connection = Connection;
        product.ProductID = connection.QuerySingle<int>(sql, product);
    }

    public void Delete(int id)
    {
        const string sql = "DELETE FROM Products WHERE ProductID = @Id";

        using var connection = Connection;
        connection.Execute(sql, new { Id = id });
    }

    public IEnumerable<Product> GetAll()
    {
        const string sql = @"
            SELECT ProductID, ProductName, SupplierID, CategoryID, QuantityPerUnit,
                   UnitPrice, UnitsInStock, UnitsOnOrder, ReorderLevel, Discontinued
            FROM Products";

        using var connection = Connection;
        return connection.Query<Product>(sql).ToList();
    }

    public IEnumerable<Product> GetAll(int pageNumber, int pageSize, int? categoryId)
    {
        var sql = @"
            SELECT ProductID, ProductName, SupplierID, CategoryID, QuantityPerUnit,
                   UnitPrice, UnitsInStock, UnitsOnOrder, ReorderLevel, Discontinued
            FROM Products";

        var parameters = new DynamicParameters();

        if (categoryId.HasValue)
        {
            sql += " WHERE CategoryID = @CategoryId";
            parameters.Add("CategoryId", categoryId.Value);
        }

        sql += " ORDER BY ProductID OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
        parameters.Add("Offset", (pageNumber - 1) * pageSize);
        parameters.Add("PageSize", pageSize);

        using var connection = Connection;
        return connection.Query<Product>(sql, parameters).ToList();
    }

    public Product? GetById(int id)
    {
        const string sql = @"
            SELECT ProductID, ProductName, SupplierID, CategoryID, QuantityPerUnit,
                   UnitPrice, UnitsInStock, UnitsOnOrder, ReorderLevel, Discontinued
            FROM Products
            WHERE ProductID = @Id";

        using var connection = Connection;
        return connection.QuerySingleOrDefault<Product>(sql, new { Id = id });
    }

    public void Update(Product product)
    {
        const string sql = @"
            UPDATE Products
            SET ProductName = @ProductName,
                SupplierID = @SupplierID,
                CategoryID = @CategoryID,
                QuantityPerUnit = @QuantityPerUnit,
                UnitPrice = @UnitPrice,
                UnitsInStock = @UnitsInStock,
                UnitsOnOrder = @UnitsOnOrder,
                ReorderLevel = @ReorderLevel,
                Discontinued = @Discontinued
            WHERE ProductID = @ProductID";

        using var connection = Connection;
        connection.Execute(sql, product);
    }
}
