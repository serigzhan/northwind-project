using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Northwind.Data.Interfaces;
using Northwind.Data.Models;

namespace Northwind.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController(ICategoryRepository repository) : ControllerBase
{

    private readonly ICategoryRepository _repository = repository;

    [HttpGet]
    public IActionResult GetAll()
    {

        var categories = _repository.GetAll();
        return Ok(categories);

    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {

        var category = _repository.GetById(id);
        return Ok(category);

    }

    [HttpPost]
    public IActionResult Create([FromBody] Category category)
    {

        _repository.Add(category);
        return CreatedAtAction(nameof(GetById), new { id = category.CategoryID }, category);

    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] Category category)
    {
        var existing = _repository.GetById(id);

        if (existing == null)
        {
            return NotFound();
        }

        category.CategoryID = id;
        _repository.Update(category);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {

        _repository.Delete(id);
        return NoContent();

    }

}
