using AspNetCoreHero.ToastNotification.Abstractions;
using FAMAndIMS.Common;
using FAMAndIMS.Data.Model.AssetManagementModel.AssetAllocationModel;
using FAMAndIMS.Data.Model.CommonModel;
using FAMAndIMS.Data.Model.InventoryManagementModel.ItemsModel;
using FAMAndIMS.Data.Services.CommonddlServices;
using FAMAndIMS.Data.Services.CommonServices;
using FAMAndIMS.Data.Services.InventoryManagementServices.ItemsServices;
using FAMAndIMS.Extension;
using FAMAndIMS.Filter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Net;

namespace FAMAndIMS.Controllers.InventoryManagementController.ItemsController
{
    public class ItemsController : Controller
    {
        private readonly INotyfService _notyfService;
        private readonly IItemsServices _itemsServices;
        private readonly ICommonddlService _commonddlService;
        private readonly ICommonServices _commonServices;
        public ItemsController(INotyfService notyfService, IItemsServices itemsServices, ICommonddlService commonddlService, ICommonServices commonServices)
        {
            _notyfService = notyfService;
            _itemsServices = itemsServices;
            _commonddlService=commonddlService;
            _commonServices = commonServices;
        }
        #region Index
        [MenuAccess]
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] ItemsDto itemsDto)
        {
            Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";
            ViewBag.view = HttpContext.Items["view"];
            ViewBag.create = HttpContext.Items["create"];
            ViewBag.delete = HttpContext.Items["delete"];
            ViewBag.update = HttpContext.Items["update"];
            var itemsList = await _itemsServices.GetItemsList(itemsDto);
            if (WebHelper.IsAjaxRequest(Request))
            {
                return PartialView("_ItemsIndex", itemsList);
            }
            return View(itemsList);

        }
        #endregion

        #region Save Items
        [HttpGet]
        public async Task<IActionResult> SaveItems()
        {
            var unitDdlList = await _commonddlService.GetUnitDDL();
            ViewBag.unitDdlList = new SelectList(unitDdlList, "Value", "Text");
            return PartialView();
        }
        [HttpPost]
        [LogUserActivity(UserActionTypes.SaveItems)]
        public async Task<IActionResult> SaveItems(ItemsVM itemsVM)
        {
            var unitDdlList = await _commonddlService.GetUnitDDL();
            ViewBag.unitDdlList = new SelectList(unitDdlList, "Value", "Text");
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var responseMessage = await _itemsServices.SaveItems(itemsVM);
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

        #region Update Items

        [HttpGet]
        public async Task<IActionResult> UpdateItems(int id)
        {
            var unitDdlList = await _commonddlService.GetUnitDDL();
            ViewBag.unitDdlList = new SelectList(unitDdlList, "Value", "Text");
            var data = await _itemsServices.GetItemsById(id);
            return PartialView(data);
        }

        [HttpPost]
        [LogUserActivity(UserActionTypes.UpdateItems)]
        public async Task<IActionResult> UpdateItems(ItemsVM itemsVM)
        {
            var unitDdlList = await _commonddlService.GetUnitDDL();
            ViewBag.unitDdlList = new SelectList(unitDdlList, "Value", "Text");
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var responseMessage = await _itemsServices.SaveItems(itemsVM);
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

        #region Delete Items

        [HttpGet]
        public async Task<IActionResult> DeleteItems(int Id)
        {
            var data = await _itemsServices.GetItemsById(Id);
            return PartialView(data);
        }
        [HttpPost]
        [LogUserActivity(UserActionTypes.DeleteItems)]
        public async Task<IActionResult> DeleteItems(ItemsVM itemsVM)
        {
            var responseMessage = await _itemsServices.DeleteItems(itemsVM.Id);
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
        [LogUserActivity(UserActionTypes.UpdateItemsStatus)]
        public async Task<IActionResult> UpdateStatus(int Id, bool IsActive)
        {
            var tableFlag = "tbl_Items";
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
        public async Task<IActionResult> ItemsListExportExcell(ItemsDto itemsDto)
        {
            itemsDto.IsExport = 1;
            var data = await _itemsServices.ExportItemsList(itemsDto);
            List<DataTable> dataTables = await IEnumerableExtensions.ToDataTablesAsync<ExportItemsList>(data.Items, 500000);
            var (excelFileByteArr, fileFormat, fileName) = await DataExportHelper.ToExcelAsync(dataTables, "ItemsExcelReports", true);
            return File(excelFileByteArr, fileFormat, fileName);
        }
        [HttpGet]
        public async Task<IActionResult> ItemsListExportCsv([FromQuery] ItemsDto itemsDto)
        {
            itemsDto.IsExport = 1;
            var data = await _itemsServices.ExportItemsList(itemsDto);
            var (bytes, fileformate, filename) = DataExportHelper.GenerateCsv(data.Items, new string[] { }, null, "ItemsCSVReports", true);
            return File(bytes, fileformate, filename);
        }
        [HttpGet]
        public async Task<IActionResult> ItemsListsExportPdf([FromQuery] ItemsDto itemsDto)
        {
            itemsDto.IsExport = 1;
            var data = await _itemsServices.ExportItemsList(itemsDto);
            var (bytedata, format) = await DataExportHelper.TopdfAsync<ExportItemsList>(data, "ItemsPdfReports");
            return File(bytedata, format, "ItemsPDFReports.pdf");
        }
        #endregion
    }
}
