using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using ProductAPI.Models;

namespace ProductAPI.Services
{
    /// <summary>
    /// Implementation of the IProductService interface
    /// Handles communication with the external mock API
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ProductService> _logger;

        /// <summary>
        /// Constructor with dependency injection
        /// </summary>
        /// <param name="httpClient">Pre-configured HttpClient with base address</param>
        /// <param name="logger">Logger for error and information logging</param>
        public ProductService(HttpClient httpClient, ILogger<ProductService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a paginated list of products with optional name filtering
        /// </summary>
        /// <param name="parameters">Pagination and filtering parameters</param>
        /// <returns>A paginated response containing filtered products</returns>
        public async Task<PagedResponse<Product>> GetProductsAsync(PaginationParameters parameters)
        {
            try
            {
                // Fetch all products from the external API
                var response = await _httpClient.GetFromJsonAsync<List<Product>>("api/objects");
                
                if (response == null)
                {
                    _logger.LogWarning("No products returned from the API");
                    return new PagedResponse<Product>();
                }

                // Apply filtering if name filter is provided
                var filteredItems = string.IsNullOrWhiteSpace(parameters.NameFilter)
                    ? response
                    : response.Where(p => p.Name.Contains(parameters.NameFilter, StringComparison.OrdinalIgnoreCase)).ToList();

                // Calculate total count for pagination metadata
                var totalCount = filteredItems.Count;
                
                // Apply pagination to get only the items for the requested page
                var items = filteredItems
                    .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                    .Take(parameters.PageSize)
                    .ToList();

                // Create and return the paged response with metadata
                return new PagedResponse<Product>
                {
                    Items = items,
                    PageNumber = parameters.PageNumber,
                    PageSize = parameters.PageSize,
                    TotalCount = totalCount
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching products");
                throw; // Re-throw to allow controller to handle the exception
            }
        }

        /// <summary>
        /// Retrieves a specific product by its ID
        /// </summary>
        /// <param name="id">The unique identifier of the product</param>
        /// <returns>The product if found, otherwise null</returns>
        public async Task<Product?> GetProductByIdAsync(string id)
        {
            try
            {
                // Request a specific product by ID from the external API
                return await _httpClient.GetFromJsonAsync<Product>($"api/objects/{id}");
            }
            catch (HttpRequestException ex)
            {
                // Log different error levels based on the status code
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _logger.LogInformation("Product with ID {Id} not found", id);
                }
                else
                {
                    _logger.LogError(ex, "HTTP error occurred while fetching product with ID {Id}", id);
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching product with ID {Id}", id);
                return null;
            }
        }

        /// <summary>
        /// Creates a new product
        /// </summary>
        /// <param name="product">The product to create</param>
        /// <returns>The created product with assigned ID if successful, otherwise null</returns>
        public async Task<Product?> CreateProductAsync(Product product)
        {
            try
            {
                // Serialize the product to JSON
                var content = new StringContent(
                    JsonSerializer.Serialize(product),
                    Encoding.UTF8,
                    "application/json");

                // Send POST request to create the product
                var response = await _httpClient.PostAsync("api/objects", content);
                
                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Deserialize and return the created product
                    var createdProduct = await response.Content.ReadFromJsonAsync<Product>();
                    _logger.LogInformation("Product created successfully with ID {Id}", createdProduct?.Id);
                    return createdProduct;
                }
                
                // Log warning if creation failed
                _logger.LogWarning("Failed to create product. Status: {StatusCode}", response.StatusCode);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating product");
                throw; // Re-throw to allow controller to handle the exception
            }
        }

        /// <summary>
        /// Updates an existing product
        /// </summary>
        /// <param name="id">The unique identifier of the product to update</param>
        /// <param name="product">The updated product data</param>
        /// <returns>The updated product if successful, otherwise null</returns>
        public async Task<Product?> UpdateProductAsync(string id, Product product)
        {
            try
            {
                // Serialize the product to JSON
                var content = new StringContent(
                    JsonSerializer.Serialize(product),
                    Encoding.UTF8,
                    "application/json");

                // Send PUT request to update the product
                var response = await _httpClient.PutAsync($"api/objects/{id}", content);
                
                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Deserialize and return the updated product
                    var updatedProduct = await response.Content.ReadFromJsonAsync<Product>();
                    _logger.LogInformation("Product with ID {Id} updated successfully", id);
                    return updatedProduct;
                }
                
                // Log warning if update failed
                _logger.LogWarning("Failed to update product with ID {Id}. Status: {StatusCode}", id, response.StatusCode);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating product with ID {Id}", id);
                throw; // Re-throw to allow controller to handle the exception
            }
        }

        /// <summary>
        /// Deletes a product
        /// </summary>
        /// <param name="id">The unique identifier of the product to delete</param>
        /// <returns>True if deletion was successful, otherwise false</returns>
        public async Task<bool> DeleteProductAsync(string id)
        {
            try
            {
                // Send DELETE request to remove the product
                var response = await _httpClient.DeleteAsync($"api/objects/{id}");
                
                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Product with ID {Id} deleted successfully", id);
                    return true;
                }
                
                // Log warning if deletion failed
                _logger.LogWarning("Failed to delete product with ID {Id}. Status: {StatusCode}", id, response.StatusCode);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting product with ID {Id}", id);
                return false;
            }
        }
    }
}