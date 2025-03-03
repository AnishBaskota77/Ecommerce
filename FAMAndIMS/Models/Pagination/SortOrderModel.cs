namespace FAMAndIMS.Models.Pagination
{
    public class SortOrderModel
    {
        public SortOrderModel(string[] sortOrderOptions, string sortOrder)
        {
            Options = sortOrderOptions ?? Array.Empty<string>();
            SortOrder = string.IsNullOrWhiteSpace(sortOrder) ? "ASC" : sortOrder;
        }

        public string[] Options { get; set; }
        public string SortOrder { get; set; }
    }
}
