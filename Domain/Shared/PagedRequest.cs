namespace Domain.Shared
{
    /// <summary>
    /// Pagination and filtering request
    /// </summary>
    public class PagedRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Search { get; set; }
        public bool? IsActive { get; set; }
    }
}
