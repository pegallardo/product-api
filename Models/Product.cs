using System.Text.Json.Serialization;

namespace ProductAPI.Models
{
    /// <summary>
    /// Represents a product entity that matches the structure of the mock API
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Unique identifier for the product
        /// This is nullable because it's assigned by the API when creating a new product
        /// </summary>
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        /// <summary>
        /// Name of the product
        /// This is a required field
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Additional product data including specifications and pricing
        /// </summary>
        [JsonPropertyName("data")]
        public ProductData Data { get; set; } = new ProductData();
    }

    /// <summary>
    /// Contains detailed information about a product
    /// </summary>
    public class ProductData
    {
        /// <summary>
        /// Year the product was manufactured or released
        /// </summary>
        [JsonPropertyName("year")]
        public int Year { get; set; }

        /// <summary>
        /// Price of the product in the default currency
        /// </summary>
        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        /// <summary>
        /// CPU model information for electronic devices
        /// </summary>
        [JsonPropertyName("CPU_model")]
        public string? CpuModel { get; set; }

        /// <summary>
        /// Hard disk size information for electronic devices
        /// </summary>
        [JsonPropertyName("Hard_disk_size")]
        public string? HardDiskSize { get; set; }
    }
}