namespace FAMAndIMS.Models.Pagination
{
    public class PaginationSummaryModel
    {
        public PaginationSummaryModel(int pageIndex, int pageSize, int pageItemsCount, int matchedRecords, int totalRecords)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            PageItemsCount = pageItemsCount;
            MatchedRecords = matchedRecords;
            TotalRecords = totalRecords;
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int PageItemsCount { get; set; }
        public int MatchedRecords { get; set; }
        public int TotalRecords { get; set; }
    }
}
