using Microsoft.AspNetCore.Mvc;
using Northwind.Data.Interfaces;
using Northwind.Data.Models;

namespace Northwind.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController(IProductRepository repository) : ControllerBase
{

    private readonly IProductRepository _repository = repository;

    [HttpGet]
    public IActionResult GetAll()
    {

        var products = _repository.GetAll();
        return Ok(products);

    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {

        var product = _repository.GetById(id);
        return Ok(product);

    }

    [HttpPost]
    public IActionResult Create(Product product)
    {

        _repository.Add(product);
        return CreatedAtAction(nameof(GetById), new { id = product.ProductID }, product);

    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] Product product)
    {
        var existing = _repository.GetById(id);

        if (existing == null)
        {
            return NotFound();
        }

        product.ProductID = id;
        _repository.Update(product);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {

        _repository.Delete(id);
        return NoContent();

    }

}
