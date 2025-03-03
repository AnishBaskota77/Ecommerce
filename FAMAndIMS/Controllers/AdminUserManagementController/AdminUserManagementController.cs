using AspNetCoreHero.ToastNotification.Abstractions;
using FAMAndIMS.Common;
using FAMAndIMS.Data.Model.AdminUserManagementModel;
using FAMAndIMS.Data.Model.CommonModel;
using FAMAndIMS.Data.Model.EmployeeManagementModel;
using FAMAndIMS.Data.Model.MenuManagerModel;
using FAMAndIMS.Data.Model.RoleMenuManagerModel;
using FAMAndIMS.Data.Services.CommonddlServices;
using FAMAndIMS.Data.Services.CommonServices;
using FAMAndIMS.Data.Services.EmployeeManagementServices;
using FAMAndIMS.Filter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;

namespace FAMAndIMS.Controllers.EmployeeManagementController
{
    [Authorize]
    public class AdminUserManagementController : Controller
    {
        private readonly INotyfService _notyfService;
        private readonly ICommonddlService _commonddlService;
        private readonly IAdminUserManagementServices _adminUserManagementServices;
        private readonly ICommonServices _commonServices;
        public AdminUserManagementController(INotyfService notyfService,ICommonddlService commonddlService,IAdminUserManagementServices adminUserManagementServices,ICommonServices commonServices)
        {
            _notyfService = notyfService;
            _commonddlService = commonddlService;
            _adminUserManagementServices = adminUserManagementServices;
            _commonServices = commonServices;
        }

        #region Index
        [MenuAccess]
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] AdminUserListFilterDto adminUserListFilterDto)
        {
            Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";
            ViewBag.view = HttpContext.Items["view"];
            ViewBag.create = HttpContext.Items["create"];
            ViewBag.delete = HttpContext.Items["delete"];
            ViewBag.update = HttpContext.Items["update"];
            var adminUserList = await _adminUserManagementServices.GetAllAdminUserList(adminUserListFilterDto);
            if (WebHelper.IsAjaxRequest(Request))
            {
                return PartialView("_AdminUserManagementIndex", adminUserList);
            }
            return View(adminUserList);

        }
        #endregion

        #region Save AdminUser
        [HttpGet]
        public async Task<IActionResult> SaveAdminUser()
        {
            var roleDdlList = await _commonddlService.GetRoleDDL();
            ViewBag.roleDdlList = new SelectList(roleDdlList, "Value", "Text");

            var genderDDLList = await _commonddlService.GetGenderDDL();
            ViewBag.genderDDLList = new SelectList(genderDDLList, "Value", "Text");

            var countryDDLList = await _commonddlService.GetCountryDDL();
            ViewBag.countryDDLList = new SelectList(countryDDLList, "Value", "Text");

            var provinceDDLList = await _commonddlService.GetProvinceDDL();
            ViewBag.provinceDDLList = new SelectList(provinceDDLList, "Value", "Text");

            var districtDDLList = await _commonddlService.GetDistrictDDL();
            ViewBag.districtDDLList = new SelectList(districtDDLList, "Value", "Text");

            var municipalityDDLList = await _commonddlService.GetMunicipalityDDL();
            ViewBag.municipalityDDLList = new SelectList(municipalityDDLList, "Value", "Text");

            return PartialView();
        }
        [HttpPost]
        [LogUserActivity(UserActionTypes.SaveAdminUser)]
        public async Task<IActionResult> SaveAdminUser(AdminUserVM adminUserVM)
        {
            var roleDdlList = await _commonddlService.GetRoleDDL();
            ViewBag.roleDdlList = new SelectList(roleDdlList, "Value", "Text");

            var genderDDLList = await _commonddlService.GetGenderDDL();
            ViewBag.genderDDLList = new SelectList(genderDDLList, "Value", "Text");

            var countryDDLList = await _commonddlService.GetCountryDDL();
            ViewBag.countryDDLList = new SelectList(countryDDLList, "Value", "Text");

            var provinceDDLList = await _commonddlService.GetProvinceDDL();
            ViewBag.provinceDDLList = new SelectList(provinceDDLList, "Value", "Text");

            var districtDDLList = await _commonddlService.GetDistrictDDL();
            ViewBag.districtDDLList = new SelectList(districtDDLList, "Value", "Text");

            var municipalityDDLList = await _commonddlService.GetMunicipalityDDL();
            ViewBag.municipalityDDLList = new SelectList(municipalityDDLList, "Value", "Text");

            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var responseMessage = await _adminUserManagementServices.SaveAdminUser(adminUserVM);
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

        #region Update AdminUser

        [HttpGet]
        public async Task<IActionResult> AdminUserUpdate(int id)
        {
            var roleDdlList = await _commonddlService.GetRoleDDL();
            ViewBag.roleDdlList = new SelectList(roleDdlList, "Value", "Text");

            var genderDDLList = await _commonddlService.GetGenderDDL();
            ViewBag.genderDDLList = new SelectList(genderDDLList, "Value", "Text");

            var countryDDLList = await _commonddlService.GetCountryDDL();
            ViewBag.countryDDLList = new SelectList(countryDDLList, "Value", "Text");

            var provinceDDLList = await _commonddlService.GetProvinceDDL();
            ViewBag.provinceDDLList = new SelectList(provinceDDLList, "Value", "Text");

            var districtDDLList = await _commonddlService.GetDistrictDDL();
            ViewBag.districtDDLList = new SelectList(districtDDLList, "Value", "Text");

            var municipalityDDLList = await _commonddlService.GetMunicipalityDDL();
            ViewBag.municipalityDDLList = new SelectList(municipalityDDLList, "Value", "Text");
            var menudata = await _adminUserManagementServices.GetAdminUserById(id);
            return PartialView(menudata);
        }

        [HttpPost]
        [LogUserActivity(UserActionTypes.UpdateAdminUser)]
        public async Task<IActionResult> AdminUserUpdate(AdminUserUpdateVM adminUserUpdateVM)
        {
            var roleDdlList = await _commonddlService.GetRoleDDL();
            ViewBag.roleDdlList = new SelectList(roleDdlList, "Value", "Text");

            var genderDDLList = await _commonddlService.GetGenderDDL();
            ViewBag.genderDDLList = new SelectList(genderDDLList, "Value", "Text");

            var countryDDLList = await _commonddlService.GetCountryDDL();
            ViewBag.countryDDLList = new SelectList(countryDDLList, "Value", "Text");

            var provinceDDLList = await _commonddlService.GetProvinceDDL();
            ViewBag.provinceDDLList = new SelectList(provinceDDLList, "Value", "Text");

            var districtDDLList = await _commonddlService.GetDistrictDDL();
            ViewBag.districtDDLList = new SelectList(districtDDLList, "Value", "Text");

            var municipalityDDLList = await _commonddlService.GetMunicipalityDDL();
            ViewBag.municipalityDDLList = new SelectList(municipalityDDLList, "Value", "Text");
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var responseMessage = await _adminUserManagementServices.UpdateAdminUser(adminUserUpdateVM);
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

        #region Update Status
        [HttpPost]
        [LogUserActivity(UserActionTypes.UpdateAdminUserStatus)]
        public async Task<IActionResult> UpdateStatus(int Id, bool IsActive)
        {
            var tableFlag = "tbl_AdminUser";
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

        #region Delete Admin User

        [HttpGet]
        public async Task<IActionResult> DeleteAdminUser(int Id)
        {
            var menudata = await _adminUserManagementServices.GetAdminUserById(Id);
            return PartialView(menudata);
        }
        [HttpPost]
        [LogUserActivity(UserActionTypes.DeleteAdminUser)]
        public async Task<IActionResult> DeleteAdminUser(AdminUserVM adminUserVM)
        {
            var responseMessage = await _adminUserManagementServices.DeleteAdminUser(adminUserVM.Id);
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

        #region Reset Password
        [HttpGet]

        public async Task<IActionResult> AdminUserResetPassword(int Id)
        {
            ViewBag.Id = Id;
            return PartialView();
        }

        [HttpPost]
        [LogUserActivity(UserActionTypes.ResetAdminUserPassword)]
        public async Task<IActionResult> AdminUserResetPassword(AdminUserResetPasswordVm resetPasswordVm)
        {
            ViewBag.Id = resetPasswordVm.Id;
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var responseMessage = await _adminUserManagementServices.AdminUserResetPassword(resetPasswordVm);
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
    }
}
