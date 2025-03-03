using FAMAndIMS.Models.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace FAMAndIMS.Components.Pagination
{
    public class PageSizer : ViewComponent
    {
        public PageSizer()
        {
        }

        public IViewComponentResult Invoke(PageSizeDdl model)
        {
            return View(model);
        }

    }
}
