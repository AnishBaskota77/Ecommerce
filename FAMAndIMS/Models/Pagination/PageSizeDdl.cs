namespace FAMAndIMS.Models.Pagination
{
    public class PageSizeDdl : ActionSpecs
    {
        public int[] PageSizeOptions { get; set; }
        public int PageSize { get; set; }

        public PageSizeDdl(int pageSize, string controller, string action)
        {
            PageSizeOptions = new int[] { 10, 20, 30, 50, 100 };
            PageSize = pageSize;

            Controller = controller;
            Action = action;
        }

        public PageSizeDdl(int[] pageSizeOptions, int pageSize, string controller, string action)
        {
            PageSizeOptions = pageSizeOptions;
            PageSize = pageSize;

            Controller = controller;
            Action = action;
        }
    }
}
