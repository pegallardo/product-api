namespace ProductAPI.Models
{
    /// <summary>
    /// Generic response model for paginated data
    /// </summary>
    /// <typeparam name="T">Type of items in the collection</typeparam>
    public class PagedResponse<T>
    {
        /// <summary>
        /// Collection of items for the current page
        /// </summary>
        public IEnumerable<T> Items { get; set; } = new List<T>();
        
        /// <summary>
        /// Current page number (1-based)
        /// </summary>
        public int PageNumber { get; set; }
        
        /// <summary>
        /// Number of items per page
        /// </summary>
        public int PageSize { get; set; }
        
        /// <summary>
        /// Total number of items across all pages
        /// </summary>
        public int TotalCount { get; set; }
        
        /// <summary>
        /// Total number of pages based on page size and total count
        /// </summary>
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
        
        /// <summary>
        /// Indicates if there is a previous page available
        /// </summary>
        public bool HasPrevious => PageNumber > 1;
        
        /// <summary>
        /// Indicates if there is a next page available
        /// </summary>
        public bool HasNext => PageNumber < TotalPages;
    }
}