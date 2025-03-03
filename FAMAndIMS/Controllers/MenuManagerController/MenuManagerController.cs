using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using FAMAndIMS.Common;
using FAMAndIMS.Data.Model.CommonModel;
using FAMAndIMS.Data.Model.MenuManagerModel;
using FAMAndIMS.Data.Services.CommonddlServices;
using FAMAndIMS.Data.Services.CommonServices;
using FAMAndIMS.Data.Services.MenuServices;
using FAMAndIMS.Filter;
using FAMAndIMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.Design;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FAMAndIMS.Controllers.MenuManagerController
{
    [Authorize]
    public class MenuManagerController : Controller
    {
        private readonly INotyfService _notyfService;
        private readonly ICommonddlService _commonddlService;
        private readonly IMenuManagerService _menuManagerService;
        private readonly ICommonServices _commonServices;

        public MenuManagerController(INotyfService notyfSrvice, ICommonddlService commonddlService, IMenuManagerService menuManagerService, ICommonServices commonServices)
        {
            _notyfService = notyfSrvice;
            _commonddlService = commonddlService;
            _menuManagerService = menuManagerService;
            _commonServices=commonServices;
        }
        #region Index
        [MenuAccess]
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] MenuManagerDTO menuManagerDTO)
        {
            Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";
            ViewBag.view = HttpContext.Items["view"];
            ViewBag.create = HttpContext.Items["create"];
            ViewBag.delete = HttpContext.Items["delete"];
            ViewBag.update = HttpContext.Items["update"];
            var menuManagerList = await _menuManagerService.GetMenuManagerList(menuManagerDTO);
            if(WebHelper.IsAjaxRequest(Request))
            {
                return PartialView("_MenuManagerIndex", menuManagerList);
            }
            return View(menuManagerList);
           
        }
        #endregion

        #region Save Menu Manager
        [HttpGet]
        public async Task<IActionResult> SaveMenuManager()
        {
            var mainParentMenuList = await _commonddlService.GetMainParentMenuList();
            ViewBag.MainParentMenuList = new SelectList(mainParentMenuList, "Value", "Text");

            var subParentMenuList = await _commonddlService.GetSubParentMenuList();
            ViewBag.SubParentMenuList = new SelectList(subParentMenuList, "Value", "Text");


            return PartialView();
        }
        [HttpPost]
        [LogUserActivity(UserActionTypes.SaveMenu)]
        public async Task<IActionResult> SaveMenuManager(MenuManagerVM menuManagerVM)
        {
            var mainParentMenuList = await _commonddlService.GetMainParentMenuList();
            ViewBag.MainParentMenuList = new SelectList(mainParentMenuList, "Value", "Text");

            var subParentMenuList = await _commonddlService.GetSubParentMenuList();
            ViewBag.SubParentMenuList = new SelectList(subParentMenuList, "Value", "Text");

            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var responseMessage = await _menuManagerService.SaveMenuManager(menuManagerVM);
                if (responseMessage.ReturnId > 0)
                {
                    _notyfService.Success(responseMessage.Msg);
                    return Ok();
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    var errors = new List<string> { responseMessage.Msg };
                    ViewBag.Error = errors;
                    return PartialView();
                }
            }
        }
        #endregion

        #region Menu Update

        [HttpGet]
        public async Task<IActionResult> MenuUpdate(int id)
        {
            var mainParentMenuList = await _commonddlService.GetMainParentMenuList();
            ViewBag.MainParentMenuList = new SelectList(mainParentMenuList, "Value", "Text");

            var subParentMenuList = await _commonddlService.GetSubParentMenuList();
            ViewBag.SubParentMenuList = new SelectList(subParentMenuList, "Value", "Text");
            var menudata = await _menuManagerService.GetMenuById(id);
            return PartialView(menudata);
        }

        [HttpPost]
        [LogUserActivity(UserActionTypes.UpdateMenu)]
        public async Task<IActionResult> MenuUpdate(MenuManagerVM menuManager)
        {

            var mainParentMenuList = await _commonddlService.GetMainParentMenuList();
            ViewBag.MainParentMenuList = new SelectList(mainParentMenuList, "Value", "Text");

            var subParentMenuList = await _commonddlService.GetSubParentMenuList();
            ViewBag.SubParentMenuList = new SelectList(subParentMenuList, "Value", "Text");
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var responseMessage = await _menuManagerService.SaveMenuManager(menuManager);
                if (responseMessage.ReturnId > 0)
                {
                    _notyfService.Success(responseMessage.Msg);
                    return Ok();
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    var errors = new List<string> { responseMessage.Msg };
                    ViewBag.Error = errors;
                    return PartialView();
                }
            }

        }
        #endregion

        #region Delete Menu

        [HttpGet]
        public async Task<IActionResult> DeleteMenu(int Id)
        {
            var menudata = await _menuManagerService.GetMenuById(Id);
            return PartialView(menudata);
        }
        [HttpPost]
        [LogUserActivity(UserActionTypes.DeleteMenu)]
        public async Task<IActionResult> DeleteMenu(MenuManagerVM menu)
        {
            var responseMessage = await _menuManagerService.DeleteMenu(menu.Id);
            if (responseMessage.ReturnId > 0)
            {
                _notyfService.Success(responseMessage.Msg);
                return Ok();
            }
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            var errors = new List<string> { responseMessage.Msg };
            ViewBag.Error = errors;
            return PartialView();
        }
        #endregion

        #region Update Status
        [HttpPost]
        [LogUserActivity(UserActionTypes.UpdateMenuStatus)]
        public async Task<IActionResult> UpdateStatus(int Id, bool IsActive)
        {
            var tableFlag = "tbl_Menu";
            var responseMessage = await _commonServices.UpdateStatus(Id, tableFlag, IsActive);
            if (responseMessage.ReturnId > 0)
            {
                _notyfService.Success(responseMessage.Msg);
                return Ok();
            }
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            var errors = new List<string> { responseMessage.Msg };
            ViewBag.Error = errors;
            return PartialView();
        }
        #endregion
    }
}
