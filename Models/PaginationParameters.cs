namespace ProductAPI.Models
{
    /// <summary>
    /// Parameters for pagination and filtering of API results
    /// </summary>
    public class PaginationParameters
    {
        /// <summary>
        /// Maximum allowed page size to prevent performance issues
        /// </summary>
        private const int MaxPageSize = 50;
        
        /// <summary>
        /// Default page size if not specified
        /// </summary>
        private int _pageSize = 10;

        /// <summary>
        /// Current page number (1-based)
        /// </summary>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// Number of items per page
        /// Enforces a maximum page size to prevent performance issues
        /// </summary>
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        /// <summary>
        /// Optional filter to search for products by name (substring match)
        /// </summary>
        public string? NameFilter { get; set; }
    }
}