using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Northwind.Data.Models;

namespace Northwind.Consumer;

[TestClass]
public sealed class ProductsApiTests
{
    private const string BaseUrl = "/api/products";
    private static readonly WebApplicationFactory<Program> Factory = new();
    private readonly HttpClient _client = Factory.CreateClient();

    [TestMethod]
    public async Task GetAll_ReturnsOkWithProducts()
    {
        var response = await _client.GetAsync(BaseUrl);

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var products = await response.Content.ReadFromJsonAsync<List<Product>>();
        Assert.IsNotNull(products);
        Assert.IsTrue(products.Count > 0);
    }

    [TestMethod]
    public async Task GetAll_WithPagination_ReturnsPagedResults()
    {
        var response = await _client.GetAsync($"{BaseUrl}?pageNumber=1&pageSize=5");

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var products = await response.Content.ReadFromJsonAsync<List<Product>>();
        Assert.IsNotNull(products);
        Assert.IsTrue(products.Count <= 5);
    }

    [TestMethod]
    public async Task GetAll_WithCategoryFilter_ReturnsFilteredResults()
    {
        var response = await _client.GetAsync($"{BaseUrl}?categoryId=1");

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var products = await response.Content.ReadFromJsonAsync<List<Product>>();
        Assert.IsNotNull(products);
        Assert.IsTrue(products.All(p => p.CategoryID == 1));
    }

    [TestMethod]
    public async Task GetById_ExistingId_ReturnsOkWithProduct()
    {
        var response = await _client.GetAsync($"{BaseUrl}/1");

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var product = await response.Content.ReadFromJsonAsync<Product>();
        Assert.IsNotNull(product);
        Assert.AreEqual(1, product.ProductID);
    }

    [TestMethod]
    public async Task Post_ValidProduct_ReturnsCreated()
    {
        var newProduct = new Product
        {
            ProductName = "Test Product",
            UnitPrice = 9.99m,
            Discontinued = false
        };

        var response = await _client.PostAsJsonAsync(BaseUrl, newProduct);

        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        var created = await response.Content.ReadFromJsonAsync<Product>();
        Assert.IsNotNull(created);
        Assert.AreEqual("Test Product", created.ProductName);
        Assert.IsTrue(created.ProductID > 0);
    }

    [TestMethod]
    public async Task Put_ExistingProduct_ReturnsNoContent()
    {
        var newProduct = new Product
        {
            ProductName = "To Update",
            UnitPrice = 5.00m,
            Discontinued = false
        };
        var createResponse = await _client.PostAsJsonAsync(BaseUrl, newProduct);
        var created = await createResponse.Content.ReadFromJsonAsync<Product>();

        var updated = new Product
        {
            ProductName = "Updated Product",
            UnitPrice = 15.00m,
            Discontinued = true
        };
        var response = await _client.PutAsJsonAsync($"{BaseUrl}/{created!.ProductID}", updated);

        Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);

        var getResponse = await _client.GetAsync($"{BaseUrl}/{created.ProductID}");
        var fetched = await getResponse.Content.ReadFromJsonAsync<Product>();
        Assert.AreEqual("Updated Product", fetched!.ProductName);
    }

    [TestMethod]
    public async Task Put_NonExistingProduct_ReturnsNotFound()
    {
        var product = new Product
        {
            ProductName = "Ghost",
            Discontinued = false
        };

        var response = await _client.PutAsJsonAsync($"{BaseUrl}/99999", product);

        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    }

    [TestMethod]
    public async Task Delete_ExistingProduct_ReturnsNoContent()
    {
        var newProduct = new Product
        {
            ProductName = "To Delete",
            UnitPrice = 1.00m,
            Discontinued = false
        };
        var createResponse = await _client.PostAsJsonAsync(BaseUrl, newProduct);
        var created = await createResponse.Content.ReadFromJsonAsync<Product>();

        var response = await _client.DeleteAsync($"{BaseUrl}/{created!.ProductID}");

        Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
    }
}
