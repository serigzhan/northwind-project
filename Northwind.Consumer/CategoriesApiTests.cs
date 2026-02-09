using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Northwind.Data.Models;

namespace Northwind.Consumer;

[TestClass]
public sealed class CategoriesApiTests
{
    private const string BaseUrl = "/api/categories";
    private static readonly WebApplicationFactory<Program> Factory = new();
    private readonly HttpClient _client = Factory.CreateClient();

    [TestMethod]
    public async Task GetAll_ReturnsOkWithCategories()
    {
        var response = await _client.GetAsync(BaseUrl);

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var categories = await response.Content.ReadFromJsonAsync<List<Category>>();
        Assert.IsNotNull(categories);
        Assert.IsTrue(categories.Count > 0);
    }

    [TestMethod]
    public async Task GetById_ExistingId_ReturnsOkWithCategory()
    {
        var response = await _client.GetAsync($"{BaseUrl}/1");

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var category = await response.Content.ReadFromJsonAsync<Category>();
        Assert.IsNotNull(category);
        Assert.AreEqual(1, category.CategoryID);
    }

    [TestMethod]
    public async Task Post_ValidCategory_ReturnsCreated()
    {
        var newCategory = new Category
        {
            CategoryName = "Test Category",
            Description = "Test Description"
        };

        var response = await _client.PostAsJsonAsync(BaseUrl, newCategory);

        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        var created = await response.Content.ReadFromJsonAsync<Category>();
        Assert.IsNotNull(created);
        Assert.AreEqual("Test Category", created.CategoryName);
        Assert.IsTrue(created.CategoryID > 0);
    }

    [TestMethod]
    public async Task Put_ExistingCategory_ReturnsNoContent()
    {
        var newCategory = new Category
        {
            CategoryName = "To Update",
            Description = "Original"
        };
        var createResponse = await _client.PostAsJsonAsync(BaseUrl, newCategory);
        var created = await createResponse.Content.ReadFromJsonAsync<Category>();

        var updated = new Category
        {
            CategoryName = "Updated Name",
            Description = "Updated Description"
        };
        var response = await _client.PutAsJsonAsync($"{BaseUrl}/{created!.CategoryID}", updated);

        Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);

        var getResponse = await _client.GetAsync($"{BaseUrl}/{created.CategoryID}");
        var fetched = await getResponse.Content.ReadFromJsonAsync<Category>();
        Assert.AreEqual("Updated Name", fetched!.CategoryName);
    }

    [TestMethod]
    public async Task Put_NonExistingCategory_ReturnsNotFound()
    {
        var category = new Category
        {
            CategoryName = "Ghost",
            Description = "Does not exist"
        };

        var response = await _client.PutAsJsonAsync($"{BaseUrl}/99999", category);

        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    }

    [TestMethod]
    public async Task Delete_ExistingCategory_ReturnsNoContent()
    {
        var newCategory = new Category
        {
            CategoryName = "To Delete",
            Description = "Will be removed"
        };
        var createResponse = await _client.PostAsJsonAsync(BaseUrl, newCategory);
        var created = await createResponse.Content.ReadFromJsonAsync<Category>();

        var response = await _client.DeleteAsync($"{BaseUrl}/{created!.CategoryID}");

        Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
    }
}
