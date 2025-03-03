using AspNetCoreHero.ToastNotification.Abstractions;
using FAMAndIMS.Common;
using FAMAndIMS.Data.Model.CommonModel;
using FAMAndIMS.Data.Model.MenuManagerModel;
using FAMAndIMS.Data.Model.RoleMenuManagerModel;
using FAMAndIMS.Data.Services.CommonServices;
using FAMAndIMS.Data.Services.RoleMenuManagerServices;
using FAMAndIMS.Filter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net;

namespace FAMAndIMS.Controllers.RoleMenuManagerController
{
    [Authorize]
    public class RoleMenuManagerController : Controller
    {
        private readonly IRoleMenuMangerService _roleMenuManagerService;
        private readonly INotyfService _notyfService;
        private readonly ICommonServices _commonServices;

        public RoleMenuManagerController(IRoleMenuMangerService roleMenuManagerService, INotyfService notyfService,ICommonServices commonServices)
        {
            _roleMenuManagerService = roleMenuManagerService;
            _notyfService = notyfService;
            _commonServices = commonServices;
        }

        #region Index
        [MenuAccess]
        [HttpGet]
        public async Task<IActionResult> Index(RoleMenuManagerDTO roleMenuManagerDTO)
        {
            Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";
            ViewBag.view = HttpContext.Items["view"];
            ViewBag.create = HttpContext.Items["create"];
            ViewBag.delete = HttpContext.Items["delete"];
            ViewBag.update = HttpContext.Items["update"];
            var roleMenuManagerLisr = await _roleMenuManagerService.GetRoleMenuManagerList(roleMenuManagerDTO);
            if (WebHelper.IsAjaxRequest(Request))
            {
                return PartialView("_RoleMenuManagerIndex", roleMenuManagerLisr);
            }
            return View(roleMenuManagerLisr);
        }
        #endregion

        #region Save Role 
        [HttpGet]
        public async Task<IActionResult> SaveRoleMenuManager()
        {
            return PartialView();
        }
        [HttpPost]
        [LogUserActivity(UserActionTypes.SaveRole)]
        public async Task<IActionResult> SaveRoleMenuManager(RoleMenuManagerVM roleMenuManagerVM)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var responseMessage = await _roleMenuManagerService.SaveRoleMenuManager(roleMenuManagerVM);
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

        #region Edit Role
        [HttpGet]
        public async Task<IActionResult> UpdateRoleMenuManager(int Id)
        {
            var data = await _roleMenuManagerService.GetRoleMenuManagerById(Id);
            return PartialView(data);
        }
        [HttpPost]
        [LogUserActivity(UserActionTypes.UpdateRole)]
        public async Task<IActionResult> UpdateRoleMenuManager(RoleMenuManagerVM roleMenuManagerVM)
        {

            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var responseMessage = await _roleMenuManagerService.SaveRoleMenuManager(roleMenuManagerVM);
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

        #region Assign Role
        [HttpGet]
        public async Task<IActionResult> AssignRole(int RoleId)
        {
            var roleDetails = await _roleMenuManagerService.GetRoleMenuManagerById(RoleId);
            var menuList = await _roleMenuManagerService.GetMenuByRoleId(RoleId);
            ViewBag.Rolename = roleDetails.RoleName;
            ViewBag.RoleId = RoleId;

            ViewBag.Viewall = menuList.All(x => x.ViewPer) == true ? "Checked" : "Unchecked";
            ViewBag.Createall = menuList.All(x => x.CreatePer) == true ? "Checked" : "Unchecked";
            ViewBag.Editall = menuList.All(x => x.UpdatePer) == true ? "Checked" : "Unchecked";
            ViewBag.Deleteall = menuList.All(x => x.DeletePer) == true ? "Checked" : "Unchecked";

            return PartialView(menuList.ToList());
        }
        [HttpPost]
        [LogUserActivity(UserActionTypes.AssignRole)]
        public async Task<IActionResult> AssignRole([FromBody] List<PermissionRole> permissions)
        {
            if (permissions is not null)
            {
                var roleDetails = await _roleMenuManagerService.GetRoleMenuManagerById(permissions.FirstOrDefault().RoleId);
                int roleId = roleDetails.Id;
                ViewBag.RoleId = roleId;
                ViewBag.Rolename = roleDetails.RoleName;
                var menuList = await _roleMenuManagerService.GetMenuByRoleId(roleId);
                List<MenuRoleWithChild> menuItems = new List<MenuRoleWithChild>();

                foreach (var item in permissions)
                {
                    MenuRoleWithChild menuItem = menuList.Where(x => x.Id == item.Id).FirstOrDefault();
                    if (menuItem.ViewPer != item.Viewper ||
                      menuItem.CreatePer != item.Createper ||
                      menuItem.UpdatePer != item.Updateper ||
                      menuItem.DeletePer != item.Deleteper)
                    {
                        menuItem.ViewPer = item.Viewper;
                        menuItem.CreatePer = item.Createper;
                        menuItem.UpdatePer = item.Updateper;
                        menuItem.DeletePer = item.Deleteper;
                        menuItems.Add(menuItem);
                    }

                }
                if (menuItems.Count > 0)
                {
                    var result = await _roleMenuManagerService.UpdateAssignRolePermissions(menuItems, Convert.ToInt32(roleDetails.Id));
                    _notyfService.Success(result.Msg);
                    return Json(true);
                }
                else
                {
                    _notyfService.Warning("No Any Permission Changes");
                    return Json(false);
                }
            }
            else
            {
                return Json(false);
            }
        }
        #endregion

        #region Update Status
        [HttpPost]
        [LogUserActivity(UserActionTypes.UpdateRoleStatus)]
        public async Task<IActionResult> UpdateStatus(int Id, bool IsActive)
        {
            var tableFlag = "tbl_Role";
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

        #region Delete Role
        [HttpGet]
        public async Task<IActionResult> DeleteRoleMenuManager(int Id)
        {
            ViewBag.Id = Id;
            return PartialView();
        }
        [HttpPost]
        [LogUserActivity(UserActionTypes.DeleteRole)]
        public async Task<IActionResult> DeleteRoleMenuManager(RoleMenuManagerVM roleMenuManagerVM)
        {
            var responseMessage = await _roleMenuManagerService.DeleteRoleMenuManager(roleMenuManagerVM);
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
