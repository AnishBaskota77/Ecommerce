using FAMAndIMS.Data.Model.AdminLoginModel;
using FAMAndIMS.Data.Services.AdminLoginServices;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using FAMAndIMS.Data.Services.MenuServices;

namespace FAMAndIMS.Controllers.AdminLoginController
{
    public class AdminLoginController : Controller
    {
        private readonly IAdminLoginService _adminLoginService;
        private readonly IMenuManagerService _menuManagerService;

        public AdminLoginController(IAdminLoginService adminLoginService, IMenuManagerService menuManagerService)
        {
            _adminLoginService = adminLoginService;
            _menuManagerService = menuManagerService;
        }

        [HttpGet]
        public async Task<IActionResult> AdminLoginPage(string returnUrl = "/")
        {
            if (User.Identity.IsAuthenticated)
            {
                var menuIEnumerable = await _menuManagerService.GetSideBarMenuList();
                var menuList = menuIEnumerable.ToList();

                if (menuList[0].SubMenuItems.Count > 0)
                {
                    var redirectUrl = menuList[0].SubMenuItems[0].MenuUrl;
                    return LocalRedirect(redirectUrl);
                }
                else
                {
                    var redirectUrl = menuList[0].MenuUrl;
                    return LocalRedirect(redirectUrl);
                }
            }
            return View(new AdminLoginVM { ReturnUrl = returnUrl });
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AdminLoginPage(AdminLoginVM adminLoginVM, string returnUrl)
        {
            var (result, responseMessage) = await _adminLoginService.GetAdminUserByUsername(adminLoginVM);
            if (responseMessage.StatusCode != 200)
            {
                ViewBag.Error = responseMessage.Msg;
                return View();
            }
            var checkPassword = await _adminLoginService.CheckPasswordAsync(adminLoginVM.Password, result);

            if (checkPassword == false)
            {
                ViewBag.Error = "Invalid Credentials";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,result.Id.ToString()),
                new Claim(ClaimTypes.Email, result.Email),
                new Claim(ClaimTypes.Name, result.Username),
                new Claim(ClaimTypes.Role, result.RoleId.ToString()),
                new Claim("RoleName", result.RoleName),
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var principle = new ClaimsPrincipal(identity);


            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                principle,
                new AuthenticationProperties { RedirectUri = returnUrl });

            //var menuIEnumerable = await _menuManagerService.GetSideBarMenuList();
            //var menuList = menuIEnumerable.ToList();

            //if (menuList.Count > 0 && menuList[0].SubMenuItems != null && menuList[0].SubMenuItems.Count > 0)
            //{

            //}


            if (returnUrl != "/")
            {
                return LocalRedirect(returnUrl);
            }
            return LocalRedirect("~/Home/Index");
        }

        [AllowAnonymous]
        public async Task<IActionResult> LogOut()
        {
            // Create a new empty ClaimsIdentity
            if (User.Identity.IsAuthenticated)
            {
                // Clear all claims
                var identity = (ClaimsIdentity)User.Identity;
                identity.Claims.ToList().ForEach(c => identity.RemoveClaim(c));

                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            return Redirect("/");
        }
    }
}
