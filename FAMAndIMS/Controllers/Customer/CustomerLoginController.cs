using Microsoft.AspNetCore.Mvc;

namespace FAMAndIMS.Controllers.Customer
{
    public class CustomerLoginController : Controller
    {
        public IActionResult CustomerMain()
        {
            return View();
        }
    }
}
