using AspNetCoreHero.ToastNotification.Abstractions;
using FAMAndIMS.Common;
using FAMAndIMS.Data.Model.CommonModel;
using FAMAndIMS.Data.Model.GlobalSettingModel.EmployeeModel;
using FAMAndIMS.Data.Services.CommonddlServices;
using FAMAndIMS.Data.Services.CommonServices;
using FAMAndIMS.Data.Services.GlobalSettingServices.EmployeeServices;
using FAMAndIMS.Filter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Reflection;

namespace FAMAndIMS.Controllers.GlobalSettingController.EmployeeController
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly INotyfService _notyfService;
        private readonly IEmployeeServices _employeeServices;
        private readonly ICommonddlService _commonddlService;
        private readonly ICommonServices _commonServices;

        public EmployeeController(INotyfService notyfService, IEmployeeServices employeeServices, ICommonddlService commonddlService, ICommonServices commonServices)
        {
            _notyfService = notyfService;
            _employeeServices = employeeServices;
            _commonddlService = commonddlService;
            _commonServices=commonServices;
        }
        #region Index
        [MenuAccess]
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] EmployeeDto employeeDto)
        {
            Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";
            ViewBag.view = HttpContext.Items["view"];
            ViewBag.create = HttpContext.Items["create"];
            ViewBag.delete = HttpContext.Items["delete"];
            ViewBag.update = HttpContext.Items["update"];
            var employeeList = await _employeeServices.GetEmployeeList(employeeDto);
            if (WebHelper.IsAjaxRequest(Request))
            {
                return PartialView("_EmployeeIndex", employeeList);
            }
            return View(employeeList);

        }
        #endregion

        #region Save Employee
        [HttpGet]
        public async Task<IActionResult> SaveEmployee()
        {
            var branchDdlList = await _commonddlService.GetBranchDDL();
            ViewBag.branchDdlList = new SelectList(branchDdlList, "Value", "Text");
            var departmentDdlList = await _commonddlService.GetDepartmentDDL();
            ViewBag.departmentDdlList = new SelectList(departmentDdlList, "Value", "Text");
            return PartialView();
        }
        [HttpPost]
        [LogUserActivity(UserActionTypes.SaveEmployee)]
        public async Task<IActionResult> SaveEmployee(EmployeeVM employeeVM)
        {
            var branchDdlList = await _commonddlService.GetBranchDDL();
            ViewBag.branchDdlList = new SelectList(branchDdlList, "Value", "Text");
            var departmentDdlList = await _commonddlService.GetDepartmentDDL();
            ViewBag.departmentDdlList = new SelectList(departmentDdlList, "Value", "Text");
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var responseMessage = await _employeeServices.SaveEmployee(employeeVM);
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

        #region Update Employee

        [HttpGet]
        public async Task<IActionResult> UpdateEmployee(int id)
        {
            var branchDdlList = await _commonddlService.GetBranchDDL();
            ViewBag.branchDdlList = new SelectList(branchDdlList, "Value", "Text");
            var departmentDdlList = await _commonddlService.GetDepartmentDDL();
            ViewBag.departmentDdlList = new SelectList(departmentDdlList, "Value", "Text");
            var data = await _employeeServices.GetEmployeeById(id);
            return PartialView(data);
        }

        [HttpPost]
        [LogUserActivity(UserActionTypes.UpdateEmployee)]
        public async Task<IActionResult> UpdateEmployee(EmployeeVM employeeVM)
        {
            var branchDdlList = await _commonddlService.GetBranchDDL();
            ViewBag.categoryDdlList = new SelectList(branchDdlList, "Value", "Text");
            var departmentDdlList = await _commonddlService.GetDepartmentDDL();
            ViewBag.categoryDdlList = new SelectList(departmentDdlList, "Value", "Text");
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var responseMessage = await _employeeServices.SaveEmployee(employeeVM);
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

        #region Delete Employee

        [HttpGet]
        public async Task<IActionResult> DeleteEmployee(int Id)
        {
            var data = await _employeeServices.GetEmployeeById(Id);
            return PartialView(data);
        }
        [HttpPost]
        [LogUserActivity(UserActionTypes.DeleteEmployee)]
        public async Task<IActionResult> DeleteEmployee(EmployeeVM employeeVM)
        {
            var responseMessage = await _employeeServices.DeleteEmployee(employeeVM.Id);
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

        #region Upload Bulk Excel
        [HttpGet]
        public async Task<IActionResult> UploadBulkExcel()
        {
            return PartialView();
        }
        [HttpPost]
        public async Task<IActionResult> UploadBulkExcel([Required] IFormFile excelFile)
        {
            if (!ModelState.IsValid)
            {
                _notyfService.Error("File is Required!");
                return Ok();
            }
            else
            {
                var validateExcelFile = await ExcelFileDataUploader.EmployeeBulkExcelUploaderValidation(excelFile);
                if (validateExcelFile.StatusCode != HttpStatusCode.OK)
                {
                    _notyfService.Error("Columns Not Matched!");
                    return Ok();
                }
                List<EmployeeBulkVM> employeeData = await ExcelFileDataUploader.UploadBulkDataExcel<EmployeeBulkVM>(excelFile);

                var responseMessage = await _employeeServices.BulkExcelUpload(employeeData);
                if (responseMessage.ReturnId > 0)
                {
                    _notyfService.Success(responseMessage.Msg);
                    return Ok();
                }
                else
                {
                    _notyfService.Error(responseMessage.Msg);
                    return Ok();
                }
            }
        }
        #endregion
        #region Update Status
        [HttpPost]
        [LogUserActivity(UserActionTypes.UpdateEmployeeStatus)]
        public async Task<IActionResult> UpdateStatus(int Id, bool IsActive)
        {
            var tableFlag = "tbl_Employee";
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
