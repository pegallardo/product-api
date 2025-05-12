# Product API

A RESTful Web API built with .NET 8.0 that integrates with the mock API provided at [https://restful-api.dev](https://restful-api.dev/).

## Features

- **Product Retrieval**: Get products with filtering by name and pagination
- **Product Management**: Create, update, and delete products
- **Validation**: Input validation using FluentValidation
- **Error Handling**: Comprehensive error handling and logging
- **Documentation**: Swagger/OpenAPI documentation

## Prerequisites

- .NET 8.0 SDK
- Visual Studio 2022 or Visual Studio Code

## Getting Started

### Clone the Repository

```bash
git clone https://github.com/pegallardo/product-api.git
cd product-api
```

### Build and Run

```bash
# Build the project
dotnet build

# Run the application
dotnet run
```

The API will be available at:
- HTTP: http://localhost:5000
- HTTPS: https://localhost:5001

Swagger UI will be available at:
- https://localhost:5001/swagger

## API Endpoints

### Get Products

Retrieves a paginated list of products with optional filtering.

```
GET /api/products?pageNumber=1&pageSize=10&nameFilter=laptop
```

Parameters:
- `pageNumber` (optional): Page number (default: 1)
- `pageSize` (optional): Number of items per page (default: 10, max: 50)
- `nameFilter` (optional): Filter products by name (substring match)

### Get Product by ID

Retrieves a specific product by its ID.

```
GET /api/products/{id}
```

### Create Product

Creates a new product.

```
POST /api/products
```

Request body:
```json
{
  "name": "New Laptop",
  "data": {
    "year": 2023,
    "price": 1299.99,
    "CPU_model": "Intel i7",
    "Hard_disk_size": "1TB"
  }
}
```

### Update Product

Updates an existing product.

```
PUT /api/products/{id}
```

Request body:
```json
{
  "name": "Updated Laptop",
  "data": {
    "year": 2023,
    "price": 1399.99,
    "CPU_model": "Intel i9",
    "Hard_disk_size": "2TB"
  }
}
```

### Delete Product

Deletes a product.

```
DELETE /api/products/{id}
```

## Project Structure

- **Controllers/**: Contains API controllers that handle HTTP requests and responses
- **Models/**: Contains data models and DTOs (Data Transfer Objects)
- **Services/**: Contains service interfaces and implementations for business logic
- **Validators/**: Contains validation rules for input data

## Error Handling

The API implements comprehensive error handling:
- **Validation Errors**: Return 400 Bad Request with details about the validation failures
- **Not Found Resources**: Return 404 Not Found when a requested resource doesn't exist
- **Server Errors**: Return 500 Internal Server Error for unexpected exceptions

## Dependencies

- **FluentValidation**: For model validation
- **Swashbuckle**: For Swagger/OpenAPI documentation

## Best Practices Implemented

- **Dependency Injection**: Services are registered and injected where needed
- **Repository Pattern**: Service layer abstracts data access
- **Input Validation**: All input is validated before processing
- **Error Handling**: Comprehensive error handling with appropriate status codes
- **Logging**: Detailed logging for debugging and monitoring
- **API Documentation**: Swagger documentation for API endpoints
- **Pagination**: Efficient data retrieval with pagination