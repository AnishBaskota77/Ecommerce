using FAMAndIMS.Models.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace FAMAndIMS.Components.Pagination
{
    public class PaginationSummary : ViewComponent
    {
        public PaginationSummary()
        {

        }
        public IViewComponentResult Invoke(PaginationSummaryModel model)
        {
            return View(model);
        }

    }
}
