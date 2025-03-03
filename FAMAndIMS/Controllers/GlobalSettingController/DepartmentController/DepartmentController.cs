using AspNetCoreHero.ToastNotification.Abstractions;
using FAMAndIMS.Common;
using FAMAndIMS.Data.Model.CommonModel;
using FAMAndIMS.Data.Model.EmployeeManagementModel;
using FAMAndIMS.Data.Model.GlobalSettingModel.DepartmentModel;
using FAMAndIMS.Data.Services.CommonServices;
using FAMAndIMS.Data.Services.GlobalSettingServices.DepartmentServices;
using FAMAndIMS.Filter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;

namespace FAMAndIMS.Controllers.GlobalSettingController.DepartmentController
{
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly INotyfService _notyfService;
        private readonly IDepartmentServices _departmentServices;
        private readonly ICommonServices _commonServices;
        public DepartmentController(INotyfService notyfService, IDepartmentServices departmentServices, ICommonServices commonServices)
        {
            _notyfService = notyfService;
            _departmentServices = departmentServices;
            _commonServices=commonServices;
        }
        #region Index
        [MenuAccess]
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] DepartmentDto departmentDto)
        {
            Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";
            ViewBag.view = HttpContext.Items["view"];
            ViewBag.create = HttpContext.Items["create"];
            ViewBag.delete = HttpContext.Items["delete"];
            ViewBag.update = HttpContext.Items["update"];
            var departmentList = await _departmentServices.GetDepartmentList(departmentDto);
            if (WebHelper.IsAjaxRequest(Request))
            {
                return PartialView("_DepartmentIndex", departmentList);
            }
            return View(departmentList);

        }
        #endregion

        #region Save Department
        [HttpGet]
        public async Task<IActionResult> SaveDepartment()
        {
            return PartialView();
        }
        [HttpPost]
        [LogUserActivity(UserActionTypes.SaveDepartment)]
        public async Task<IActionResult> SaveDepartment(DepartmentVM departmentVM)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var responseMessage = await _departmentServices.SaveDepartment(departmentVM);
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

        #region Update Department

        [HttpGet]
        public async Task<IActionResult> UpdateDepartment(int id)
        {
            var data = await _departmentServices.GetDepartmentById(id);
            return PartialView(data);
        }

        [HttpPost]
        [LogUserActivity(UserActionTypes.UpdateDepartment)]
        public async Task<IActionResult> UpdateDepartment(DepartmentVM departmentVM)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var responseMessage = await _departmentServices.SaveDepartment(departmentVM);
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

        #region Delete Department

        [HttpGet]
        public async Task<IActionResult> DeleteDepartment(int Id)
        {
            var data = await _departmentServices.GetDepartmentById(Id);
            return PartialView(data);
        }
        [HttpPost]
        [LogUserActivity(UserActionTypes.DeleteDepartment)]
        public async Task<IActionResult> DeleteDepartment(DepartmentVM departmentVM)
        {
            var responseMessage = await _departmentServices.DeleteDepartment(departmentVM.Id);
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
        [LogUserActivity(UserActionTypes.UpdateDepartmentStatus)]
        public async Task<IActionResult> UpdateStatus(int Id, bool IsActive)
        {
            var tableFlag = "tbl_Department";
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
