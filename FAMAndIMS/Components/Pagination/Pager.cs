using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using FAMAndIMS.Models.Pagination;

namespace FAMAndIMS.Components.Pagination
{
    public class Pager : ViewComponent
    {
        public Pager()
        {

        }

        public IViewComponentResult Invoke(PagerModel model)
        {
            return View(model);
        }
    }
}
