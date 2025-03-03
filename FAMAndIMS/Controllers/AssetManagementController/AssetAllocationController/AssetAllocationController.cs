using AspNetCoreHero.ToastNotification.Abstractions;
using FAMAndIMS.Common;
using FAMAndIMS.Data.Common;
using FAMAndIMS.Data.Model.AssetManagementModel.AssetAllocationModel;
using FAMAndIMS.Data.Model.AssetManagementModel.AssetMaintenanceModel;
using FAMAndIMS.Data.Model.CommonModel;
using FAMAndIMS.Data.Services.AssetManagementServices.AssetAllocationServices;
using FAMAndIMS.Data.Services.CommonddlServices;
using FAMAndIMS.Data.Services.CommonServices;
using FAMAndIMS.Extension;
using FAMAndIMS.Filter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Net;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace FAMAndIMS.Controllers.AssetManagementController.AssetAllocationController
{
    [Authorize]
    public class AssetAllocationController : Controller
    {
        private readonly INotyfService _notyfService;
        private readonly IAssetAllocationServices _assetAllocationServices;
        private readonly ICommonddlService _commonddlService;
        private readonly IHostingEnvironment _env;
        private readonly IConfiguration _config;
        private readonly ICommonServices _commonServices;

        public AssetAllocationController(INotyfService notyfService, IAssetAllocationServices assetAllocationServices, ICommonddlService commonddlService, IHostingEnvironment env, IConfiguration config, ICommonServices commonServices)
        {
            _notyfService = notyfService;
            _assetAllocationServices = assetAllocationServices;
            _commonddlService = commonddlService;
            _env = env;
            _config = config;
            _commonServices=commonServices;
        }
        #region Index
        [MenuAccess]
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] AssetAllocationDto assetAllocationDto)
        {
            Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";
            ViewBag.view = HttpContext.Items["view"];
            ViewBag.create = HttpContext.Items["create"];
            ViewBag.delete = HttpContext.Items["delete"];
            ViewBag.update = HttpContext.Items["update"];
            var assetAllocationList = await _assetAllocationServices.GetAssetAllocationList(assetAllocationDto);
            if (WebHelper.IsAjaxRequest(Request))
            {
                return PartialView("_AssetAllocationIndex", assetAllocationList);
            }
            return View(assetAllocationList);

        }
        #endregion

        #region Save Asset Allocation
        [HttpGet]
        public async Task<IActionResult> SaveAssetAllocation()
        {
            var assetDdlList = await _commonddlService.GetAssetDDL();
            ViewBag.assetDdlList = new SelectList(assetDdlList, "Value", "Text");
            var branchDdlList = await _commonddlService.GetBranchDDL();
            ViewBag.branchDdlList = new SelectList(branchDdlList, "Value", "Text");
            var departmentDdlList = await _commonddlService.GetDepartmentDDL();
            ViewBag.departmentDdlList = new SelectList(departmentDdlList, "Value", "Text");
            var employeeDdlList = await _commonddlService.GetEmployeeDDL();
            ViewBag.employeeDdlList = new SelectList(employeeDdlList, "Value", "Text");
            return PartialView();
        }
        [HttpPost]
        [LogUserActivity(UserActionTypes.SaveAssetAllocation)]
        public async Task<IActionResult> SaveAssetAllocation(AssetAllocationVM assetAllocationVM)
        {
            var assetDdlList = await _commonddlService.GetAssetDDL();
            ViewBag.assetDdlList = new SelectList(assetDdlList, "Value", "Text");
            var branchDdlList = await _commonddlService.GetBranchDDL();
            ViewBag.branchDdlList = new SelectList(branchDdlList, "Value", "Text");
            var departmentDdlList = await _commonddlService.GetDepartmentDDL();
            ViewBag.departmentDdlList = new SelectList(departmentDdlList, "Value", "Text");
            var employeeDdlList = await _commonddlService.GetEmployeeDDL();
            ViewBag.employeeDdlList = new SelectList(employeeDdlList, "Value", "Text");
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var responseMessage = await _assetAllocationServices.SaveAssetAllocation(assetAllocationVM);
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

        #region Update Asset Allocation

        [HttpGet]
        public async Task<IActionResult> UpdateAssetAllocation(int id)
        {
            var assetDdlList = await _commonddlService.GetAssetDDL();
            ViewBag.assetDdlList = new SelectList(assetDdlList, "Value", "Text");
            var branchDdlList = await _commonddlService.GetBranchDDL();
            ViewBag.branchDdlList = new SelectList(branchDdlList, "Value", "Text");
            var departmentDdlList = await _commonddlService.GetDepartmentDDL();
            ViewBag.departmentDdlList = new SelectList(departmentDdlList, "Value", "Text");
            var employeeDdlList = await _commonddlService.GetEmployeeDDL();
            ViewBag.employeeDdlList = new SelectList(employeeDdlList, "Value", "Text"); ;
            var data = await _assetAllocationServices.GetAssetAllocationById(id);
            return PartialView(data);
        }

        [HttpPost]
        [LogUserActivity(UserActionTypes.UpdateAssetAllocation)]
        public async Task<IActionResult> UpdateAssetAllocation(AssetAllocationVM assetAllocationVM)
        {
            var assetDdlList = await _commonddlService.GetAssetDDL();
            ViewBag.assetDdlList = new SelectList(assetDdlList, "Value", "Text");
            var branchDdlList = await _commonddlService.GetBranchDDL();
            ViewBag.branchDdlList = new SelectList(branchDdlList, "Value", "Text");
            var departmentDdlList = await _commonddlService.GetDepartmentDDL();
            ViewBag.departmentDdlList = new SelectList(departmentDdlList, "Value", "Text");
            var employeeDdlList = await _commonddlService.GetEmployeeDDL();
            ViewBag.employeeDdlList = new SelectList(employeeDdlList, "Value", "Text");
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var responseMessage = await _assetAllocationServices.SaveAssetAllocation(assetAllocationVM);
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

        #region Delete Asset Allocation

        [HttpGet]
        public async Task<IActionResult> DeleteAssetAllocation(int Id)
        {
            var data = await _assetAllocationServices.GetAssetAllocationById(Id);
            return PartialView(data);
        }
        [HttpPost]
        [LogUserActivity(UserActionTypes.DeleteAssetAllocation)]
        public async Task<IActionResult> DeleteAssetAllocation(AssetAllocationVM assetAllocationVM)
        {
            var responseMessage = await _assetAllocationServices.DeleteAssetAllocation(assetAllocationVM.Id);
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
        [LogUserActivity(UserActionTypes.AssetAllocationBulkUpload)]
        public async Task<IActionResult> UploadBulkExcel([Required] IFormFile excelFile)
        {
            if (!ModelState.IsValid)
            {
                _notyfService.Error("File is Required!");
                return Ok();
            }
            else
            {
                var validateExcelFile = await ExcelFileDataUploader.AssetAllocationBulkExcelUploaderValidation(excelFile);
                if (validateExcelFile.StatusCode != HttpStatusCode.OK)
                {
                    _notyfService.Error("Columns Not Matched!");
                    return Ok();
                }
                List<AssetAllocationBulkVM> assetAllocationData = await ExcelFileDataUploader.UploadBulkDataExcel<AssetAllocationBulkVM>(excelFile);

                var responseMessage = await _assetAllocationServices.BulkExcelUpload(assetAllocationData);
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

        #region Generate Bar Code
        public async Task<IActionResult> GenerateBarCodeString(string assetName, string employeeName, string branchName, string departmentName,string allocateDate, string allocateDateBs, string assetCode)
        {
            string formattedString = $"{assetCode}-{employeeName}-{departmentName}-{allocateDate}";

            var folderPath = _config["ImageFolderPath:GeneratedBarCodeImagePath"];
            var imageUrlString = GenerateBarCode.GenerateBarCodeString(_env, folderPath, formattedString);
            return Json(new { imageUrl = imageUrlString });
        }
        #endregion
        #region Update Status
        [HttpPost]
        [LogUserActivity(UserActionTypes.UpdateAssetAllocationStatus)]
        public async Task<IActionResult> UpdateStatus(int Id, bool IsActive)
        {
            var tableFlag = "tbl_AssetAllocation";
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

        #region Export-Options
        [HttpGet]
        public async Task<IActionResult> AssetAllocationListExportExcell(AssetAllocationDto assetAllocationDto)
        {
            assetAllocationDto.IsExport = 1;
            var data = await _assetAllocationServices.ExportAssetAllocationList(assetAllocationDto);
            List<DataTable> dataTables = await IEnumerableExtensions.ToDataTablesAsync<ExportAssetAllocationList>(data.Items, 500000);
            var (excelFileByteArr, fileFormat, fileName) = await DataExportHelper.ToExcelAsync(dataTables, "AssetAllocationExcelReports", true);
            return File(excelFileByteArr, fileFormat, fileName);
        }
        [HttpGet]
        public async Task<IActionResult> AssetAllocationListExportCsv([FromQuery] AssetAllocationDto assetAllocationDto)
        {
            assetAllocationDto.IsExport = 1;
            var data = await _assetAllocationServices.ExportAssetAllocationList(assetAllocationDto);
            var (bytes, fileformate, filename) = DataExportHelper.GenerateCsv(data.Items, new string[] { }, null, "AssetAllocationCSVReports", true);
            return File(bytes, fileformate, filename);
        }
        [HttpGet]
        public async Task<IActionResult> AssetAllocationListsExportPdf([FromQuery] AssetAllocationDto assetAllocationDto)
        {
            assetAllocationDto.IsExport = 1;
            var data = await _assetAllocationServices.ExportAssetAllocationList(assetAllocationDto);
            var (bytedata, format) = await DataExportHelper.TopdfAsync<ExportAssetAllocationList>(data, "AssetAllocationPdfReports");
            return File(bytedata, format, "AssetAllocationPDFReports.pdf");
        }
        #endregion
    }
}
