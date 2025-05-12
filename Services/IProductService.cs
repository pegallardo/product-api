using ProductAPI.Models;

namespace ProductAPI.Services
{
    /// <summary>
    /// Interface for product-related operations
    /// Defines the contract for interacting with the external product API
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// Retrieves a paginated list of products with optional filtering
        /// </summary>
        /// <param name="parameters">Pagination and filtering parameters</param>
        /// <returns>A paginated response containing products</returns>
        Task<PagedResponse<Product>> GetProductsAsync(PaginationParameters parameters);
        
        /// <summary>
        /// Retrieves a specific product by its ID
        /// </summary>
        /// <param name="id">The unique identifier of the product</param>
        /// <returns>The product if found, otherwise null</returns>
        Task<Product?> GetProductByIdAsync(string id);
        
        /// <summary>
        /// Creates a new product
        /// </summary>
        /// <param name="product">The product to create</param>
        /// <returns>The created product with assigned ID if successful, otherwise null</returns>
        Task<Product?> CreateProductAsync(Product product);
        
        /// <summary>
        /// Updates an existing product
        /// </summary>
        /// <param name="id">The unique identifier of the product to update</param>
        /// <param name="product">The updated product data</param>
        /// <returns>The updated product if successful, otherwise null</returns>
        Task<Product?> UpdateProductAsync(string id, Product product);
        
        /// <summary>
        /// Deletes a product
        /// </summary>
        /// <param name="id">The unique identifier of the product to delete</param>
        /// <returns>True if deletion was successful, otherwise false</returns>
        Task<bool> DeleteProductAsync(string id);
    }
}