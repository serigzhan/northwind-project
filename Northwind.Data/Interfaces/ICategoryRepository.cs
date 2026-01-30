using Northwind.Data.Models;

namespace Northwind.Data.Interfaces;

public interface ICategoryRepository
{

    IEnumerable<Category> GetAll();
    Category GetById(int id);
    void Add(Category product);
    void Update(Category product);
    void Delete(int id);

}
