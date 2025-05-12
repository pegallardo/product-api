using Microsoft.AspNetCore.Mvc;
using ProductAPI.Models;
using ProductAPI.Services;

namespace ProductAPI.Controllers
{
    /// <summary>
    /// Controller for managing product resources
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;

        /// <summary>
        /// Constructor with dependency injection
        /// </summary>
        /// <param name="productService">Service for product operations</param>
        /// <param name="logger">Logger for error and information logging</param>
        public ProductsController(IProductService productService, ILogger<ProductsController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a paginated list of products with optional filtering
        /// </summary>
        /// <param name="parameters">Pagination and filtering parameters</param>
        /// <returns>A paginated list of products</returns>
        /// <response code="200">Returns the paginated list of products</response>
        /// <response code="500">If an error occurs during processing</response>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<Product>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProducts([FromQuery] PaginationParameters parameters)
        {
            try
            {
                _logger.LogInformation("Retrieving products with parameters: Page {PageNumber}, Size {PageSize}, Filter {NameFilter}",
                    parameters.PageNumber, parameters.PageSize, parameters.NameFilter ?? "none");
                
                // Get products from the service with pagination and filtering
                var products = await _productService.GetProductsAsync(parameters);
                
                _logger.LogInformation("Retrieved {Count} products out of {Total}", 
                    products.Items.Count(), products.TotalCount);
                
                return Ok(products);
            }
            catch (Exception ex)
            {
                // Log the error and return a generic error message to avoid exposing sensitive details
                _logger.LogError(ex, "Error occurred while fetching products");
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Retrieves a specific product by its ID
        /// </summary>
        /// <param name="id">The unique identifier of the product</param>
        /// <returns>The requested product</returns>
        /// <response code="200">Returns the requested product</response>
        /// <response code="404">If the product is not found</response>
        /// <response code="500">If an error occurs during processing</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProduct(string id)
        {
            try
            {
                _logger.LogInformation("Retrieving product with ID {Id}", id);
                
                // Get the product from the service
                var product = await _productService.GetProductByIdAsync(id);
                
                // Return 404 if the product is not found
                if (product == null)
                {
                    _logger.LogWarning("Product with ID {Id} not found", id);
                    return NotFound($"Product with ID {id} not found");
                }
                
                _logger.LogInformation("Retrieved product with ID {Id}", id);
                return Ok(product);
            }
            catch (Exception ex)
            {
                // Log the error and return a generic error message
                _logger.LogError(ex, "Error occurred while fetching product with ID {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Creates a new product
        /// </summary>
        /// <param name="product">The product to create</param>
        /// <returns>The created product</returns>
        /// <response code="201">Returns the newly created product</response>
        /// <response code="400">If the product data is invalid</response>
        /// <response code="500">If an error occurs during processing</response>
        [HttpPost]
        [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            try
            {
                // Check if the model is valid based on validation rules
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid product model state: {Errors}", 
                        string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                    return BadRequest(ModelState);
                }

                _logger.LogInformation("Creating new product with name: {Name}", product.Name);
                
                // Create the product using the service
                var createdProduct = await _productService.CreateProductAsync(product);
                
                // Return 400 if the product creation failed
                if (createdProduct == null)
                {
                    _logger.LogWarning("Failed to create product");
                    return BadRequest("Failed to create product");
                }
                
                _logger.LogInformation("Product created successfully with ID: {Id}", createdProduct.Id);
                
                // Return 201 Created with the location header pointing to the new resource
                return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProduct);
            }
            catch (Exception ex)
            {
                // Log the error and return a generic error message
                _logger.LogError(ex, "Error occurred while creating product");
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Updates an existing product
        /// </summary>
        /// <param name="id">The unique identifier of the product to update</param>
        /// <param name="product">The updated product data</param>
        /// <returns>The updated product</returns>
        /// <response code="200">Returns the updated product</response>
        /// <response code="400">If the product data is invalid</response>
        /// <response code="404">If the product is not found</response>
        /// <response code="500">If an error occurs during processing</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateProduct(string id, [FromBody] Product product)
        {
            try
            {
                // Check if the model is valid based on validation rules
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid product model state for update: {Errors}", 
                        string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                    return BadRequest(ModelState);
                }

                _logger.LogInformation("Checking if product with ID {Id} exists", id);
                
                // Check if the product exists
                var existingProduct = await _productService.GetProductByIdAsync(id);
                
                // Return 404 if the product is not found
                if (existingProduct == null)
                {
                    _logger.LogWarning("Product with ID {Id} not found for update", id);
                    return NotFound($"Product with ID {id} not found");
                }

                _logger.LogInformation("Updating product with ID {Id}", id);
                
                // Ensure the ID in the route matches the ID in the product
                product.Id = id;
                
                // Update the product using the service
                var updatedProduct = await _productService.UpdateProductAsync(id, product);
                
                // Return 400 if the product update failed
                if (updatedProduct == null)
                {
                    _logger.LogWarning("Failed to update product with ID {Id}", id);
                    return BadRequest("Failed to update product");
                }
                
                _logger.LogInformation("Product with ID {Id} updated successfully", id);
                return Ok(updatedProduct);
            }
            catch (Exception ex)
            {
                // Log the error and return a generic error message
                _logger.LogError(ex, "Error occurred while updating product with ID {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "An error occurred while processing your request");
            }
        }

        /// <summary>
        /// Deletes a product
        /// </summary>
        /// <param name="id">The unique identifier of the product to delete</param>
        /// <returns>No content if successful</returns>
        /// <response code="204">If the product was successfully deleted</response>
        /// <response code="404">If the product is not found</response>
        /// <response code="500">If an error occurs during processing</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            try
            {
                _logger.LogInformation("Checking if product with ID {Id} exists for deletion", id);
                
                // Check if the product exists
                var product = await _productService.GetProductByIdAsync(id);
                
                // Return 404 if the product is not found
                if (product == null)
                {
                    _logger.LogWarning("Product with ID {Id} not found for deletion", id);
                    return NotFound($"Product with ID {id} not found");
                }

                _logger.LogInformation("Deleting product with ID {Id}", id);
                
                // Delete the product using the service
                var result = await _productService.DeleteProductAsync(id);
                
                // Return 400 if the product deletion failed
                if (!result)
                {
                    _logger.LogWarning("Failed to delete product with ID {Id}", id);
                    return BadRequest("Failed to delete product");
                }
                
                _logger.LogInformation("Product with ID {Id} deleted successfully", id);
                
                // Return 204 No Content for successful deletion
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the error and return a generic error message
                _logger.LogError(ex, "Error occurred while deleting product with ID {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "An error occurred while processing your request");
            }
        }
    }
}