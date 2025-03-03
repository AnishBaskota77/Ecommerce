using AspNetCoreHero.ToastNotification.Abstractions;
using FAMAndIMS.Common;
using FAMAndIMS.Data.Model.CommonModel;
using FAMAndIMS.Data.Model.InventoryManagementModel.ItemsModel;
using FAMAndIMS.Data.Model.InventoryManagementModel.PurchaseItemModel;
using FAMAndIMS.Data.Services.CommonddlServices;
using FAMAndIMS.Data.Services.CommonServices;
using FAMAndIMS.Data.Services.InventoryManagementServices.ItemsServices;
using FAMAndIMS.Data.Services.InventoryManagementServices.PurchaseItemServices;
using FAMAndIMS.Extension;
using FAMAndIMS.Filter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Net;

namespace FAMAndIMS.Controllers.InventoryManagementController.PurchaseItemController
{
    public class PurchaseItemController : Controller
    {
        private readonly INotyfService _notyfService;
        private readonly IPurchaseItemServices _purchaseItemServices;
        private readonly ICommonddlService _commonddlService;
        private readonly ICommonServices _commonServices;
        public PurchaseItemController(INotyfService notyfService, IPurchaseItemServices purchaseItemServices, ICommonddlService commonddlService, ICommonServices commonServices)
        {
            _notyfService = notyfService;
            _purchaseItemServices = purchaseItemServices;
            _commonddlService=commonddlService;
            _commonServices = commonServices;
        }
        #region Index
        [MenuAccess]
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] PurchaseItemDto purchaseItemDto)
        {
            Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";
            ViewBag.view = HttpContext.Items["view"];
            ViewBag.create = HttpContext.Items["create"];
            ViewBag.delete = HttpContext.Items["delete"];
            ViewBag.update = HttpContext.Items["update"];
            var purchaseItemList = await _purchaseItemServices.GetPurchaseItemList(purchaseItemDto);
            if (WebHelper.IsAjaxRequest(Request))
            {
                return PartialView("_PurchaseItemIndex", purchaseItemList);
            }
            return View(purchaseItemList);

        }
        #endregion

        #region Save Purchase Item
        [HttpGet]
        public async Task<IActionResult> SavePurchaseItem()
        {
            var itemDdlList = await _commonddlService.GetItemsDDL();
            ViewBag.itemDdlList = new SelectList(itemDdlList, "Value", "Text");
            var unitDdlList = await _commonddlService.GetUnitDDL();
            ViewBag.unitDdlList = new SelectList(unitDdlList, "Value", "Text");
            var vendorDdlList = await _commonddlService.GetVendorDDL();
            ViewBag.vendorDdlList = new SelectList(vendorDdlList, "Value", "Text");
            return PartialView();
        }
        [HttpPost]
        [LogUserActivity(UserActionTypes.SavePurchaseItem)]
        public async Task<IActionResult> SavePurchaseItem(PurchaseItemVM purchaseItemVM)
        {
            var itemDdlList = await _commonddlService.GetItemsDDL();
            ViewBag.itemDdlList = new SelectList(itemDdlList, "Value", "Text");
            var unitDdlList = await _commonddlService.GetUnitDDL();
            ViewBag.unitDdlList = new SelectList(unitDdlList, "Value", "Text");
            var vendorDdlList = await _commonddlService.GetVendorDDL();
            ViewBag.vendorDdlList = new SelectList(vendorDdlList, "Value", "Text");
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var responseMessage = await _purchaseItemServices.SavePurchaseItem(purchaseItemVM);
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

        #region Update Purchase Item

        [HttpGet]
        public async Task<IActionResult> UpdatePurchaseItem(int id)
        {
            var itemDdlList = await _commonddlService.GetItemsDDL();
            ViewBag.itemDdlList = new SelectList(itemDdlList, "Value", "Text");
            var unitDdlList = await _commonddlService.GetUnitDDL();
            ViewBag.unitDdlList = new SelectList(unitDdlList, "Value", "Text");
            var vendorDdlList = await _commonddlService.GetVendorDDL();
            ViewBag.vendorDdlList = new SelectList(vendorDdlList, "Value", "Text");
            var data = await _purchaseItemServices.GetPurchaseItemById(id);
            return PartialView(data);
        }

        [HttpPost]
        [LogUserActivity(UserActionTypes.UpdatePurchaseItem)]
        public async Task<IActionResult> UpdatePurchaseItem(PurchaseItemVM purchaseItemVM)
        {
            var itemDdlList = await _commonddlService.GetItemsDDL();
            ViewBag.itemDdlList = new SelectList(itemDdlList, "Value", "Text");
            var unitDdlList = await _commonddlService.GetUnitDDL();
            ViewBag.unitDdlList = new SelectList(unitDdlList, "Value", "Text");
            var vendorDdlList = await _commonddlService.GetVendorDDL();
            ViewBag.vendorDdlList = new SelectList(vendorDdlList, "Value", "Text");
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var responseMessage = await _purchaseItemServices.SavePurchaseItem(purchaseItemVM);
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

        #region Delete Purchase Item

        [HttpGet]
        public async Task<IActionResult> DeletePurchaseItem(int Id)
        {
            var data = await _purchaseItemServices.GetPurchaseItemById(Id);
            return PartialView(data);
        }
        [HttpPost]
        [LogUserActivity(UserActionTypes.DeletePurchaseItem)]
        public async Task<IActionResult> DeletePurchaseItem(PurchaseItemVM purchaseItemVM)
        {
            var responseMessage = await _purchaseItemServices.DeletePurchaseItem(purchaseItemVM.Id);
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
        [LogUserActivity(UserActionTypes.UpdatePurchaseItemStatus)]
        public async Task<IActionResult> UpdateStatus(int Id, bool IsActive)
        {
            var tableFlag = "tbl_PurchaseItem";
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
        public async Task<IActionResult> PurchaseItemListExportExcell(PurchaseItemDto purchaseItemDto)
        {
            purchaseItemDto.IsExport = 1;
            var data = await _purchaseItemServices.ExportPurchaseItemList(purchaseItemDto);
            List<DataTable> dataTables = await IEnumerableExtensions.ToDataTablesAsync<ExportPurchaseItemList>(data.Items, 500000);
            var (excelFileByteArr, fileFormat, fileName) = await DataExportHelper.ToExcelAsync(dataTables, "PurchaseItemExcelReports", true);
            return File(excelFileByteArr, fileFormat, fileName);
        }
        [HttpGet]
        public async Task<IActionResult> PurchaseItemListExportCsv([FromQuery] PurchaseItemDto purchaseItemDto)
        {
            purchaseItemDto.IsExport = 1;
            var data = await _purchaseItemServices.ExportPurchaseItemList(purchaseItemDto);
            var (bytes, fileformate, filename) = DataExportHelper.GenerateCsv(data.Items, new string[] { }, null, "PurchaseItemCSVReports", true);
            return File(bytes, fileformate, filename);
        }
        [HttpGet]
        public async Task<IActionResult> PurchaseItemListsExportPdf([FromQuery] PurchaseItemDto purchaseItemDto)
        {
            purchaseItemDto.IsExport = 1;
            var data = await _purchaseItemServices.ExportPurchaseItemList(purchaseItemDto);
            var (bytedata, format) = await DataExportHelper.TopdfAsync<ExportPurchaseItemList>(data, "PurchaseItemPdfReports");
            return File(bytedata, format, "PurchaseItemPDFReports.pdf");
        }
        #endregion

        #region Bilk Save Purchase Item
        [HttpGet]
        public async Task<IActionResult> BulkSavePurchaseItem()
        {
            var itemDdlList = await _commonddlService.GetItemsDDL();
            ViewBag.itemDdlList = new SelectList(itemDdlList, "Value", "Text");
            var unitDdlList = await _commonddlService.GetUnitDDL();
            ViewBag.unitDdlList = new SelectList(unitDdlList, "Value", "Text");
            var vendorDdlList = await _commonddlService.GetVendorDDL();
            ViewBag.vendorDdlList = new SelectList(vendorDdlList, "Value", "Text");
            return View();
        }
        [HttpPost]
        [LogUserActivity(UserActionTypes.SavePurchaseItem)]
        public async Task<IActionResult> BulkSavePurchaseItem(List<PurchaseItemVM> purchaseItemVM)
        {
            var itemDdlList = await _commonddlService.GetItemsDDL();
            ViewBag.itemDdlList = new SelectList(itemDdlList, "Value", "Text");
            var unitDdlList = await _commonddlService.GetUnitDDL();
            ViewBag.unitDdlList = new SelectList(unitDdlList, "Value", "Text");
            var vendorDdlList = await _commonddlService.GetVendorDDL();
            ViewBag.vendorDdlList = new SelectList(vendorDdlList, "Value", "Text");
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return View();
            }
            else
            {
                var responseMessage = await _purchaseItemServices.BulkSavePurchaseItems(purchaseItemVM);
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
                    return View();
                }
            }
        }
        #endregion
    }
}
