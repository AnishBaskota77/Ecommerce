using FAMAndIMS.Filter;
using FAMAndIMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FAMAndIMS.Controllers.HomeController
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [MenuAccess]
        public IActionResult Index()
        {
            return View();
        }

    }
}
