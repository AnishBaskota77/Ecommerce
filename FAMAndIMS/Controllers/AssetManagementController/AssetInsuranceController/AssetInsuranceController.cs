using AspNetCoreHero.ToastNotification.Abstractions;
using FAMAndIMS.Common;
using FAMAndIMS.Data.Model.AssetManagementModel.AssetInsuranceModel;
using FAMAndIMS.Data.Model.CommonModel;
using FAMAndIMS.Data.Services.AssetManagementServices.AssetInsuranceServices;
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

namespace FAMAndIMS.Controllers.AssetManagementController.AssetInsuranceController
{
    [Authorize]
    public class AssetInsuranceController : Controller
    {
        private readonly INotyfService _notyfService;
        private readonly IAssetInsuranceServices _assetInsuranceServices;
        private readonly ICommonddlService _commonddlService;
        private readonly ICommonServices _commonServices;

        public AssetInsuranceController(INotyfService notyfService, IAssetInsuranceServices assetInsuranceServices, ICommonddlService commonddlService, ICommonServices commonServices)
        {
            _notyfService = notyfService;
            _assetInsuranceServices = assetInsuranceServices;
            _commonddlService = commonddlService;
            _commonServices=commonServices;
        }
        #region Index
        [MenuAccess]
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] AssetInsuranceDto assetInsuranceDto)
        {
            Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";
            ViewBag.view = HttpContext.Items["view"];
            ViewBag.create = HttpContext.Items["create"];
            ViewBag.delete = HttpContext.Items["delete"];
            ViewBag.update = HttpContext.Items["update"];
            var assetInsuranceList = await _assetInsuranceServices.GetAssetInsuranceList(assetInsuranceDto);
            if (WebHelper.IsAjaxRequest(Request))
            {
                return PartialView("_AssetInsuranceIndex", assetInsuranceList);
            }
            return View(assetInsuranceList);

        }
        #endregion

        #region Save Asset Insurance
        [HttpGet]
        public async Task<IActionResult> SaveAssetInsurance()
        {
            var assetDdlList = await _commonddlService.GetAssetDDL();
            ViewBag.assetDdlList = new SelectList(assetDdlList, "Value", "Text");
            var insuranceDdlList = await _commonddlService.GetInsuranceCompanyDDL();
            ViewBag.insuranceDdlList = new SelectList(insuranceDdlList, "Value", "Text");
            return PartialView();
        }
        [HttpPost]
        [LogUserActivity(UserActionTypes.SaveAssetInsurance)]
        public async Task<IActionResult> SaveAssetInsurance(AssetInsuranceVM assetInsuranceVM)
        {
            var assetDdlList = await _commonddlService.GetAssetDDL();
            ViewBag.assetDdlList = new SelectList(assetDdlList, "Value", "Text");
            var insuranceDdlList = await _commonddlService.GetInsuranceCompanyDDL();
            ViewBag.insuranceDdlList = new SelectList(insuranceDdlList, "Value", "Text");
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var responseMessage = await _assetInsuranceServices.SaveAssetInsurance(assetInsuranceVM);
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

        #region Update Asset Insurance

        [HttpGet]
        public async Task<IActionResult> UpdateAssetInsurance(int id)
        {
            var assetDdlList = await _commonddlService.GetAssetDDL();
            ViewBag.assetDdlList = new SelectList(assetDdlList, "Value", "Text");
            var insuranceDdlList = await _commonddlService.GetInsuranceCompanyDDL();
            ViewBag.insuranceDdlList = new SelectList(insuranceDdlList, "Value", "Text");
            var data = await _assetInsuranceServices.GetAssetInsuranceById(id);
            return PartialView(data);
        }

        [HttpPost]
        [LogUserActivity(UserActionTypes.UpdateAssetInsurance)]
        public async Task<IActionResult> UpdateAssetInsurance(AssetInsuranceVM assetInsuranceVM)
        {
            var assetDdlList = await _commonddlService.GetAssetDDL();
            ViewBag.assetDdlList = new SelectList(assetDdlList, "Value", "Text");
            var insuranceDdlList = await _commonddlService.GetInsuranceCompanyDDL();
            ViewBag.insuranceDdlList = new SelectList(insuranceDdlList, "Value", "Text");
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var responseMessage = await _assetInsuranceServices.SaveAssetInsurance(assetInsuranceVM);
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

        #region Delete Asset Insurance

        [HttpGet]
        public async Task<IActionResult> DeleteAssetInsurance(int Id)
        {
            var data = await _assetInsuranceServices.GetAssetInsuranceById(Id);
            return PartialView(data);
        }
        [HttpPost]
        [LogUserActivity(UserActionTypes.DeleteAssetInsurance)]
        public async Task<IActionResult> DeleteAssetInsurance(AssetInsuranceVM assetInsuranceVM)
        {
            var responseMessage = await _assetInsuranceServices.DeleteAssetInsurance(assetInsuranceVM.Id);
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
        [LogUserActivity(UserActionTypes.AssetInsuranceBulkUpload)]
        public async Task<IActionResult> UploadBulkExcel([Required] IFormFile excelFile)
        {
            if (!ModelState.IsValid)
            {
                _notyfService.Error("File is Required!");
                return Ok();
            }
            else
            {
                var validateExcelFile = await ExcelFileDataUploader.AssetInsuranceBulkExcelUploaderValidation(excelFile);
                if (validateExcelFile.StatusCode != HttpStatusCode.OK)
                {
                    _notyfService.Error("Columns Not Matched!");
                    return Ok();
                }
                List<AssetInsuranceBulkVM> assetInsuranceData = await ExcelFileDataUploader.UploadBulkDataExcel<AssetInsuranceBulkVM>(excelFile);

                var responseMessage = await _assetInsuranceServices.BulkExcelUpload(assetInsuranceData);
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
        [LogUserActivity(UserActionTypes.UpdateAssetInsuranceStatus)]
        public async Task<IActionResult> UpdateStatus(int Id, bool IsActive)
        {
            var tableFlag = "tbl_AssetInsurance";
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
        public async Task<IActionResult> AssetInsuranceListExportExcell(AssetInsuranceDto assetInsuranceDto)
        {
            assetInsuranceDto.IsExport = 1;
            var data = await _assetInsuranceServices.ExportAssetInsuranceList(assetInsuranceDto);
            List<DataTable> dataTables = await IEnumerableExtensions.ToDataTablesAsync<ExportAssetInsuranceList>(data.Items, 500000);
            var (excelFileByteArr, fileFormat, fileName) = await DataExportHelper.ToExcelAsync(dataTables, "AssetInsuranceExcelReports", true);
            return File(excelFileByteArr, fileFormat, fileName);
        }
        [HttpGet]
        public async Task<IActionResult> AssetInsuranceListExportCsv([FromQuery] AssetInsuranceDto assetInsuranceDto)
        {
            assetInsuranceDto.IsExport = 1;
            var data = await _assetInsuranceServices.ExportAssetInsuranceList(assetInsuranceDto);
            var (bytes, fileformate, filename) = DataExportHelper.GenerateCsv(data.Items, new string[] { }, null, "AssetInsuranceCSVReports", true);
            return File(bytes, fileformate, filename);
        }
        [HttpGet]
        public async Task<IActionResult> AssetInsuranceListsExportPdf([FromQuery] AssetInsuranceDto assetInsuranceDto)
        {
            assetInsuranceDto.IsExport = 1;
            var data = await _assetInsuranceServices.ExportAssetInsuranceList(assetInsuranceDto);
            var (bytedata, format) = await DataExportHelper.TopdfAsync<ExportAssetInsuranceList>(data, "AssetInsurancePdfReports");
            return File(bytedata, format, "AssetInsurancePDFReports.pdf");
        }
        #endregion
    }
}
