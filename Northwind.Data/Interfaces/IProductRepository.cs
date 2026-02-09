using Northwind.Data.Models;

namespace Northwind.Data.Interfaces;

public interface IProductRepository
{

    IEnumerable<Product> GetAll();
    IEnumerable<Product> GetAll(int pageNumber, int pageSize, int? categoryId);
    Product? GetById(int id);
    void Add(Product product);
    void Update(Product product);
    void Delete(int id);

}
