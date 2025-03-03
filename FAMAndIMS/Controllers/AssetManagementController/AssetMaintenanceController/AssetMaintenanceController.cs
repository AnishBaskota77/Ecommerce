using AspNetCoreHero.ToastNotification.Abstractions;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using FAMAndIMS.Common;
using FAMAndIMS.Data.Model.AssetManagementModel.AssetInsuranceModel;
using FAMAndIMS.Data.Model.AssetManagementModel.AssetMaintenanceModel;
using FAMAndIMS.Data.Model.CommonModel;
using FAMAndIMS.Data.Services.AssetManagementServices.AssetMaintenanceServices;
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

namespace FAMAndIMS.Controllers.AssetManagementController.AssetMaintenanceController
{
    [Authorize]
    public class AssetMaintenanceController : Controller
    {
        private readonly INotyfService _notyfService;
        private readonly IAssetMaintenanceServices _assetMaintenanceServices;
        private readonly ICommonddlService _commonddlService;
        private readonly ICommonServices _commonServices;

        public AssetMaintenanceController(INotyfService notyfService, IAssetMaintenanceServices assetMaintenanceServices, ICommonddlService commonddlService, ICommonServices commonServices)
        {
            _notyfService = notyfService;
            _assetMaintenanceServices = assetMaintenanceServices;
            _commonddlService = commonddlService;
            _commonServices=commonServices;
        }
        #region Index
        [MenuAccess]
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] AssetMaintenanceDto assetMaintenanceDto)
        {
            Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";
            ViewBag.view = HttpContext.Items["view"];
            ViewBag.create = HttpContext.Items["create"];
            ViewBag.delete = HttpContext.Items["delete"];
            ViewBag.update = HttpContext.Items["update"];
            var assetMaintenanceList = await _assetMaintenanceServices.GetAssetMaintenanceList(assetMaintenanceDto);
            if (WebHelper.IsAjaxRequest(Request))
            {
                return PartialView("_AssetMaintenanceIndex", assetMaintenanceList);
            }
            return View(assetMaintenanceList);

        }
        #endregion

        #region Save Asset Maintenance
        [HttpGet]
        public async Task<IActionResult> SaveAssetMaintenance()
        {
            var assetDdlList = await _commonddlService.GetAssetDDL();
            ViewBag.assetDdlList = new SelectList(assetDdlList, "Value", "Text");
            return PartialView();
        }
        [HttpPost]
        [LogUserActivity(UserActionTypes.SaveAssetMaintenance)]
        public async Task<IActionResult> SaveAssetMaintenance(AssetMaintenanceVM assetMaintenanceVM)
        {
            var assetDdlList = await _commonddlService.GetAssetDDL();
            ViewBag.assetDdlList = new SelectList(assetDdlList, "Value", "Text");
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var responseMessage = await _assetMaintenanceServices.SaveAssetMaintenance(assetMaintenanceVM);
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

        #region Update Asset Maintenance

        [HttpGet]
        public async Task<IActionResult> UpdateAssetMaintenance(int id)
        {
            var assetDdlList = await _commonddlService.GetAssetDDL();
            ViewBag.assetDdlList = new SelectList(assetDdlList, "Value", "Text");
            var data = await _assetMaintenanceServices.GetAssetMaintenanceById(id);
            return PartialView(data);
        }

        [HttpPost]
        [LogUserActivity(UserActionTypes.UpdateAssetMaintenance)]
        public async Task<IActionResult> UpdateAssetMaintenance(AssetMaintenanceVM assetMaintenanceVM)
        {
            var assetDdlList = await _commonddlService.GetAssetDDL();
            ViewBag.assetDdlList = new SelectList(assetDdlList, "Value", "Text");
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var responseMessage = await _assetMaintenanceServices.SaveAssetMaintenance(assetMaintenanceVM);
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

        #region Delete Asset Maintenance

        [HttpGet]
        public async Task<IActionResult> DeleteAssetMaintenance(int Id)
        {
            var data = await _assetMaintenanceServices.GetAssetMaintenanceById(Id);
            return PartialView(data);
        }
        [HttpPost]
        [LogUserActivity(UserActionTypes.DeleteAssetMaintenance)]
        public async Task<IActionResult> DeleteAssetMaintenance(AssetMaintenanceVM assetMaintenanceVM)
        {
            var responseMessage = await _assetMaintenanceServices.DeleteAssetMaintenance(assetMaintenanceVM.Id);
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
                var validateExcelFile = await ExcelFileDataUploader.AssetMaintenanceBulkExcelUploaderValidation(excelFile);
                if (validateExcelFile.StatusCode != HttpStatusCode.OK)
                {
                    _notyfService.Error("Columns Not Matched!");
                    return Ok();
                }
                List<AssetMaintenanceBulkVM> assetMaintenanceData = await ExcelFileDataUploader.UploadBulkDataExcel<AssetMaintenanceBulkVM>(excelFile);

                var responseMessage = await _assetMaintenanceServices.BulkExcelUpload(assetMaintenanceData);
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
        [LogUserActivity(UserActionTypes.UpdateBranchStatus)]
        public async Task<IActionResult> UpdateStatus(int Id, bool IsActive)
        {
            var tableFlag = "tbl_AssetMaintenance";
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

        #region Get AssetCode By AssetId
        [HttpGet]
        public async Task<IActionResult> GetAssetCodeByAssetId(int assetId)
        {
            var assetCode = await _assetMaintenanceServices.GetAssetCode(assetId);
            return Json(new { success = true, assetCode = assetCode ?? "" }); 
        }

        #endregion

        #region Export-Options
        [HttpGet]
        public async Task<IActionResult> AssetMaintenanceListExportExcell(AssetMaintenanceDto assetMaintenanceDto)
        {
            assetMaintenanceDto.IsExport = 1;
            var data = await _assetMaintenanceServices.ExportAssetMaintenanceList(assetMaintenanceDto);
            List<DataTable> dataTables = await IEnumerableExtensions.ToDataTablesAsync<ExportAssetMaintenanceList>(data.Items, 500000);
            var (excelFileByteArr, fileFormat, fileName) = await DataExportHelper.ToExcelAsync(dataTables, "AssetMaintenanceExcelReports", true);
            return File(excelFileByteArr, fileFormat, fileName);
        }
        [HttpGet]
        public async Task<IActionResult> AssetMaintenanceListExportCsv([FromQuery] AssetMaintenanceDto assetMaintenanceDto)
        {
            assetMaintenanceDto.IsExport = 1;
            var data = await _assetMaintenanceServices.ExportAssetMaintenanceList(assetMaintenanceDto);
            var (bytes, fileformate, filename) = DataExportHelper.GenerateCsv(data.Items, new string[] { }, null, "AssetMaintenanceCSVReports", true);
            return File(bytes, fileformate, filename);
        }
        [HttpGet]
        public async Task<IActionResult> AssetMaintenanceListsExportPdf([FromQuery] AssetMaintenanceDto assetMaintenanceDto)
        {
            assetMaintenanceDto.IsExport = 1;
            var data = await _assetMaintenanceServices.ExportAssetMaintenanceList(assetMaintenanceDto);
            var (bytedata, format) = await DataExportHelper.TopdfAsync<ExportAssetMaintenanceList>(data, "AssetMaintenancePdfReports");
            return File(bytedata, format, "AssetMaintenancePDFReports.pdf");
        }
        #endregion
    }
}
