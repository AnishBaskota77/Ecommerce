namespace FAMAndIMS.Models.Pagination
{
    public class PaginationFooter
    {
        public PaginationFooter(int pageNumber, int pageSize, int pageItemsCount, int filteredCount, int totalCount, int totalPages)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            PageItemsCount = pageItemsCount;
            FilteredCount = filteredCount;
            TotalCount = totalCount;
            TotalPages = totalPages;
        }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int PageItemsCount { get; set; }
        public int FilteredCount { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }
}
