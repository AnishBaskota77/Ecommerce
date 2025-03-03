using FAMAndIMS.Models.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace FAMAndIMS.Components.Pagination
{
    public class SortByViewComponent : ViewComponent
    {
        public SortByViewComponent()
        {

        }

        public IViewComponentResult Invoke(SortByModel model)
        {
            return View(model);
        }
    }
}
