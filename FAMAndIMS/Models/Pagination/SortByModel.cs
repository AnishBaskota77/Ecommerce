namespace FAMAndIMS.Models.Pagination
{
    public class SortByModel
    {
        public SortByModel(Dictionary<string, string> sortByNameValues, string defaultSortByValue)
        {
            NameValues = sortByNameValues ?? new();
            DefaultSortBy = defaultSortByValue ?? string.Empty;
        }

        public Dictionary<string, string> NameValues { get; set; }
        public string DefaultSortBy { get; set; }
    }
}
