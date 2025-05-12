using Microsoft.AspNetCore.Mvc;
using ProductAPI.Services;
using ProductAPI.Models;
using ProductAPI.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using System.Text.Json.Serialization;

// Create a new web application builder
var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Configure JSON serialization to ignore null values when writing
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// Register HttpClient for API communication with the mock API
// This creates a typed HttpClient that will be injected into the ProductService
builder.Services.AddHttpClient<IProductService, ProductService>(client =>
{
    // Set the base address for all HTTP requests
    client.BaseAddress = new Uri("https://restful-api.dev/");
});

// Add FluentValidation for model validation
// This automatically validates models in controller actions
builder.Services.AddFluentValidationAutoValidation();
// Register all validators from the assembly containing ProductValidator
builder.Services.AddValidatorsFromAssemblyContaining<ProductValidator>();

// Add Swagger/OpenAPI support for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Build the application
var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    // Enable Swagger UI in development environment
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Redirect HTTP requests to HTTPS
app.UseHttpsRedirection();

// Enable authorization middleware
app.UseAuthorization();

// Map controller routes
app.MapControllers();

// Start the application
app.Run();