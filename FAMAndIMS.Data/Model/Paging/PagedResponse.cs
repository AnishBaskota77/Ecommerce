namespace FAMAndIMS.Data.Model.Paging
{
    public class PagedResponse<T> : PagedInfo
    {
        private IEnumerable<T> _items = new List<T>();
        public IEnumerable<T> Items
        {
            get => _items;
            set => _items = value ?? new List<T>();
        }
    }
}
