using Northwind.Data.Interfaces;
using Northwind.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("NorthwindConnection") ?? "Server=(localdb)\\MSSQLLocalDB;Database=NorthwindDB;Trusted_Connection=True;";

builder.Services.AddScoped<ICategoryRepository>(sp => new CategoryRepository(connectionString));
builder.Services.AddScoped<IProductRepository>(sp => new ProductRepository(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapControllers();

app.Run();

public partial class Program { }
