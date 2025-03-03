using FAMAndIMS.Models.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace FAMAndIMS.Components.Pagination
{
    public class SortOrderViewComponent : ViewComponent
    {
        public SortOrderViewComponent()
        {

        }

        public IViewComponentResult Invoke(SortOrderModel sortOrderModel)
        {
            return View(sortOrderModel);
        }
    }
}
