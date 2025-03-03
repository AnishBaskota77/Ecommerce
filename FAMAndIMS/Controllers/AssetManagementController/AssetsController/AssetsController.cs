using AspNetCoreHero.ToastNotification.Abstractions;
using FAMAndIMS.Common;
using FAMAndIMS.Data.Model.AssetManagementModel.AssetsModel;
using FAMAndIMS.Data.Model.AssetManagementModel.AssetsModel.GeneratedDepreciationModel;
using FAMAndIMS.Data.Model.CommonModel;
using FAMAndIMS.Data.Services.AssetManagementServices.AssetsServices;
using FAMAndIMS.Data.Services.CommonddlServices;
using FAMAndIMS.Data.Services.CommonServices;
using FAMAndIMS.Extension;
using FAMAndIMS.Filter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Net;

namespace FAMAndIMS.Controllers.AssetManagementController.AssetsController
{
    [Authorize]
    public class AssetsController : Controller
    {
        private readonly INotyfService _notyfService;
        private readonly IAssetsServices _assetsServices;
        private readonly ICommonddlService _commonddlService;
        private readonly ICommonServices _commonServices;

        public AssetsController(INotyfService notyfService, IAssetsServices assetsServices, ICommonddlService commonddlService, ICommonServices commonServices)
        {
            _notyfService = notyfService;
            _assetsServices = assetsServices;
            _commonddlService = commonddlService;
            _commonServices = commonServices;
        }
        #region Index
        [MenuAccess]
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] AssetsDto assetsDto)
        {
            Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";
            ViewBag.view = HttpContext.Items["view"];
            ViewBag.create = HttpContext.Items["create"];
            ViewBag.delete = HttpContext.Items["delete"];
            ViewBag.update = HttpContext.Items["update"];
            var assetsList = await _assetsServices.GetAssetsList(assetsDto);
            if (WebHelper.IsAjaxRequest(Request))
            {
                return PartialView("_AssetsIndex", assetsList);
            }
            return View(assetsList);

        }
        #endregion

        #region Save Asset 
        [HttpGet]
        public async Task<IActionResult> SaveAssets()
        {
            var unitDdlList = await _commonddlService.GetUnitDDL();
            ViewBag.unitDdlList = new SelectList(unitDdlList, "Value", "Text");
            var categoryDdlList = await _commonddlService.GetCategoryDDL();
            ViewBag.categoryDdlList = new SelectList(categoryDdlList, "Value", "Text");
            var subCategoryDdlList = await _commonddlService.GetSubCategoryDDL();
            ViewBag.subCategoryDdlList = new SelectList(subCategoryDdlList, "Value", "Text");
            var vendorDdlList = await _commonddlService.GetVendorDDL();
            ViewBag.vendorDdlList = new SelectList(vendorDdlList, "Value", "Text");
            var employeeDdlList = await _commonddlService.GetEmployeeDDL();
            ViewBag.employeeDdlList = new SelectList(employeeDdlList, "Value", "Text");
            var depreciationTypeDdlList = await _commonddlService.GetDepreciationDDL();
            ViewBag.depreciationTypeDdlList = new SelectList(depreciationTypeDdlList, "Value", "Text");
            var conditionDdlList = await _commonddlService.GetConditionDDL();
            ViewBag.conditionDdlList = new SelectList(conditionDdlList, "Value", "Text");
            var branchDdlList = await _commonddlService.GetBranchDDL();
            ViewBag.branchDdlList = new SelectList(branchDdlList, "Value", "Text");
            var departmentDdlList = await _commonddlService.GetDepartmentDDL();
            ViewBag.departmentDdlList = new SelectList(departmentDdlList, "Value", "Text");
            return View();
        }
        [HttpPost]
        [LogUserActivity(UserActionTypes.SaveAssets)]
        public async Task<IActionResult> SaveAssets(AssetsVM assetsVM)
        {
            var unitDdlList = await _commonddlService.GetUnitDDL();
            ViewBag.unitDdlList = new SelectList(unitDdlList, "Value", "Text");
            var categoryDdlList = await _commonddlService.GetCategoryDDL();
            ViewBag.categoryDdlList = new SelectList(categoryDdlList, "Value", "Text");
            var subCategoryDdlList = await _commonddlService.GetSubCategoryDDL();
            ViewBag.subCategoryDdlList = new SelectList(subCategoryDdlList, "Value", "Text");
            var vendorDdlList = await _commonddlService.GetVendorDDL();
            ViewBag.vendorDdlList = new SelectList(vendorDdlList, "Value", "Text");
            var employeeDdlList = await _commonddlService.GetEmployeeDDL();
            ViewBag.employeeDdlList = new SelectList(employeeDdlList, "Value", "Text");
            var depreciationTypeDdlList = await _commonddlService.GetDepreciationDDL();
            ViewBag.depreciationTypeDdlList = new SelectList(depreciationTypeDdlList, "Value", "Text");
            var conditionDdlList = await _commonddlService.GetConditionDDL();
            ViewBag.conditionDdlList = new SelectList(conditionDdlList, "Value", "Text");
            var branchDdlList = await _commonddlService.GetBranchDDL();
            ViewBag.branchDdlList = new SelectList(branchDdlList, "Value", "Text");
            var departmentDdlList = await _commonddlService.GetDepartmentDDL();
            ViewBag.departmentDdlList = new SelectList(departmentDdlList, "Value", "Text");
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return View();
            }
            else
            {
                var responseMessage = await _assetsServices.SaveAssets(assetsVM);
                if (responseMessage.ReturnId > 0)
                {
                    _notyfService.Success(responseMessage.Msg);
                    return RedirectToAction("Index");
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    var errors = new List<string> { responseMessage.Msg };
                    ViewBag.Error = errors;
                    return View();
                }
            }
        }
        #endregion

        #region Update Assets

        [HttpGet]
        public async Task<IActionResult> UpdateAssets(int id)
        {
            var unitDdlList = await _commonddlService.GetUnitDDL();
            ViewBag.unitDdlList = new SelectList(unitDdlList, "Value", "Text");
            var categoryDdlList = await _commonddlService.GetCategoryDDL();
            ViewBag.categoryDdlList = new SelectList(categoryDdlList, "Value", "Text");
            var subCategoryDdlList = await _commonddlService.GetSubCategoryDDL();
            ViewBag.subCategoryDdlList = new SelectList(subCategoryDdlList, "Value", "Text");
            var vendorDdlList = await _commonddlService.GetVendorDDL();
            ViewBag.vendorDdlList = new SelectList(vendorDdlList, "Value", "Text");
            var employeeDdlList = await _commonddlService.GetEmployeeDDL();
            ViewBag.employeeDdlList = new SelectList(employeeDdlList, "Value", "Text");
            var depreciationTypeDdlList = await _commonddlService.GetDepreciationDDL();
            ViewBag.depreciationTypeDdlList = new SelectList(depreciationTypeDdlList, "Value", "Text");
            var conditionDdlList = await _commonddlService.GetConditionDDL();
            ViewBag.conditionDdlList = new SelectList(conditionDdlList, "Value", "Text");
            var branchDdlList = await _commonddlService.GetBranchDDL();
            ViewBag.branchDdlList = new SelectList(branchDdlList, "Value", "Text");
            var departmentDdlList = await _commonddlService.GetDepartmentDDL();
            ViewBag.departmentDdlList = new SelectList(departmentDdlList, "Value", "Text");
            var data = await _assetsServices.GetAssetsById(id);
            return View(data);
        }

        [HttpPost]
        [LogUserActivity(UserActionTypes.UpdateAssets)]
        public async Task<IActionResult> UpdateAssets(AssetsVM assetsVM)
        {
            var unitDdlList = await _commonddlService.GetUnitDDL();
            ViewBag.unitDdlList = new SelectList(unitDdlList, "Value", "Text");
            var categoryDdlList = await _commonddlService.GetCategoryDDL();
            ViewBag.categoryDdlList = new SelectList(categoryDdlList, "Value", "Text");
            var subCategoryDdlList = await _commonddlService.GetSubCategoryDDL();
            ViewBag.subCategoryDdlList = new SelectList(subCategoryDdlList, "Value", "Text");
            var vendorDdlList = await _commonddlService.GetVendorDDL();
            ViewBag.vendorDdlList = new SelectList(vendorDdlList, "Value", "Text");
            var employeeDdlList = await _commonddlService.GetEmployeeDDL();
            ViewBag.employeeDdlList = new SelectList(employeeDdlList, "Value", "Text");
            var depreciationTypeDdlList = await _commonddlService.GetDepreciationDDL();
            ViewBag.depreciationTypeDdlList = new SelectList(depreciationTypeDdlList, "Value", "Text");
            var conditionDdlList = await _commonddlService.GetConditionDDL();
            ViewBag.conditionDdlList = new SelectList(conditionDdlList, "Value", "Text");
            var branchDdlList = await _commonddlService.GetBranchDDL();
            ViewBag.branchDdlList = new SelectList(branchDdlList, "Value", "Text");
            var departmentDdlList = await _commonddlService.GetDepartmentDDL();
            ViewBag.departmentDdlList = new SelectList(departmentDdlList, "Value", "Text");
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return View();
            }
            else
            {
                var responseMessage = await _assetsServices.SaveAssets(assetsVM);
                if (responseMessage.ReturnId > 0)
                {
                    _notyfService.Success(responseMessage.Msg);
                    return RedirectToAction("Index");
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    var errors = new List<string> { responseMessage.Msg };
                    ViewBag.Error = errors;
                    return View();
                }
            }

        }
        #endregion

        #region Delete Asset 

        [HttpGet]
        public async Task<IActionResult> DeleteAssets(int Id)
        {
            var data = await _assetsServices.GetAssetsById(Id);
            return PartialView(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [LogUserActivity(UserActionTypes.DeleteAssets)]
        public async Task<IActionResult> DeleteAssets(AssetsVM assetsVM)
        {
            var responseMessage = await _assetsServices.DeleteAssets(assetsVM.Id);
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
        [LogUserActivity(UserActionTypes.UpdateAssetsStatus)]
        public async Task<IActionResult> UpdateStatus(int Id, bool IsActive)
        {
            var tableFlag = "tbl_Assets";
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
        public async Task<IActionResult> AssetsListExportExcell(AssetsDto assetsDto)
        {
            assetsDto.IsExport = 1;
            var data = await _assetsServices.ExportAssetsList(assetsDto);
            List<DataTable> dataTables = await IEnumerableExtensions.ToDataTablesAsync<ExportAssetsList>(data.Items, 500000);
            var (excelFileByteArr, fileFormat, fileName) = await DataExportHelper.ToExcelAsync(dataTables, "AssetsExcelReports", true);
            return File(excelFileByteArr, fileFormat, fileName);
        }
        [HttpGet]
        public async Task<IActionResult> AssetsListExportCsv([FromQuery] AssetsDto assetsDto)
        {
            assetsDto.IsExport = 1;
            var data = await _assetsServices.ExportAssetsList(assetsDto);
            var (bytes, fileformate, filename) = DataExportHelper.GenerateCsv(data.Items, new string[] { }, null, "AssetsCSVReports", true);
            return File(bytes, fileformate, filename);
        }
        [HttpGet]
        public async Task<IActionResult> AssetsListsExportPdf([FromQuery] AssetsDto assetsDto)
        {
            assetsDto.IsExport = 1;
            var data = await _assetsServices.ExportAssetsList(assetsDto);
            var (bytedata, format) = await DataExportHelper.TopdfAsync<ExportAssetsList>(data, "AssetsPdfReports");
            return File(bytedata, format, "AssetsPDFReports.pdf");
        }
        #endregion

        #region Generate Depreciation
        [HttpGet]
        public async Task<IActionResult> GenerateDepreciation(GeneratedDepreciationDto generatedDepreciationDto)
        {
            Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";

            ViewBag.view = HttpContext.Items["view"];
            ViewBag.create = HttpContext.Items["create"];
            ViewBag.delete = HttpContext.Items["delete"];
            ViewBag.update = HttpContext.Items["update"];

            var categoryDDLList = await _commonddlService.GetCategoryDDL();
            ViewBag.CategoryDDLList = new SelectList(categoryDDLList, "Value", "Text");

            var depreciationReports = await _assetsServices.GetDepreciationGeneratedList(generatedDepreciationDto);
            if (WebHelper.IsAjaxRequest(Request))
            {
                return PartialView("_GenerateDepreciation", depreciationReports);
            }
            return View(depreciationReports);
        }
        #endregion

        #region Export Generate Depreciation
        [HttpGet]
        public async Task<IActionResult> GenerateDepreciationListExportExcel(GeneratedDepreciationDto generatedDepreciationDto)
        {
            generatedDepreciationDto.IsExport = 1;
            var data = await _assetsServices.GetDepreciationGeneratedList(generatedDepreciationDto);
            List<DataTable> dataTables = await IEnumerableExtensions.ToDataTablesAsync<GeneratedDepreciationList>(data.Items, 500000);
            var (excelFileByteArr, fileFormat, fileName) = await DataExportHelper.ToExcelAsync(dataTables, "GeneratedDepreciationExcelReports", true);
            return File(excelFileByteArr, fileFormat, fileName);
        }
        [HttpGet]
        public async Task<IActionResult> GenerateDepreciationListExportCsv([FromQuery] GeneratedDepreciationDto assetsDto)
        {
            assetsDto.IsExport = 1;
            var data = await _assetsServices.GetDepreciationGeneratedList(assetsDto);
            var (bytes, fileformate, filename) = DataExportHelper.GenerateCsv(data.Items, new string[] { }, null, "GeneratedDepreciationCSVReports", true);
            return File(bytes, fileformate, filename);
        }
        [HttpGet]
        public async Task<IActionResult> GenerateDepreciationListsExportPdf([FromQuery] GeneratedDepreciationDto assetsDto)
        {
            assetsDto.IsExport = 1;
            var data = await _assetsServices.GetDepreciationGeneratedList(assetsDto);
            var (bytedata, format) = await DataExportHelper.TopdfAsync<GeneratedDepreciationList>(data, "GeneratedDepreciationPdfReports");
            return File(bytedata, format, "AssetsPDFReports.pdf");
        }
        #endregion
    }
}
