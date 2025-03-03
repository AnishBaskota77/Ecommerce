using AspNetCoreHero.ToastNotification.Abstractions;
using FAMAndIMS.Common;
using FAMAndIMS.Data.Model.InventoryManagementModel.ItemsModel;
using FAMAndIMS.Data.Model.InventoryManagementModel.StockModel;
using FAMAndIMS.Data.Services.InventoryManagementServices.StockServices;
using FAMAndIMS.Extension;
using FAMAndIMS.Filter;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace FAMAndIMS.Controllers.InventoryManagementController.StockController
{
    public class StockController : Controller
    {
        private readonly INotyfService _notyfService;
        private readonly IStockServices _stockServices;
        public StockController(INotyfService notyfService, IStockServices stockServices)
        {
            _notyfService = notyfService;
            _stockServices = stockServices;
        }
        #region Index
        [MenuAccess]
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] StockDto stockDto)
        {
            Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";
            ViewBag.view = HttpContext.Items["view"];
            ViewBag.create = HttpContext.Items["create"];
            ViewBag.delete = HttpContext.Items["delete"];
            ViewBag.update = HttpContext.Items["update"];
            var stockList = await _stockServices.GetStockList(stockDto);
            if (WebHelper.IsAjaxRequest(Request))
            {
                return PartialView("_StockIndex", stockList);
            }
            return View(stockList);

        }
        #endregion

        #region Export-Options
        [HttpGet]
        public async Task<IActionResult> StockListExportExcell(StockDto stockDto)
        {
            stockDto.IsExport = 1;
            var data = await _stockServices.ExportStockList(stockDto);
            List<DataTable> dataTables = await IEnumerableExtensions.ToDataTablesAsync<ExportStockList>(data.Items, 500000);
            var (excelFileByteArr, fileFormat, fileName) = await DataExportHelper.ToExcelAsync(dataTables, "StockExcelReports", true);
            return File(excelFileByteArr, fileFormat, fileName);
        }
        [HttpGet]
        public async Task<IActionResult> StockListExportCsv([FromQuery] StockDto stockDto)
        {
            stockDto.IsExport = 1;
            var data = await _stockServices.ExportStockList(stockDto);
            var (bytes, fileformate, filename) = DataExportHelper.GenerateCsv(data.Items, new string[] { }, null, "StockCSVReports", true);
            return File(bytes, fileformate, filename);
        }
        [HttpGet]
        public async Task<IActionResult> StockListsExportPdf([FromQuery] StockDto stockDto)
        {
            stockDto.IsExport = 1;
            var data = await _stockServices.ExportStockList(stockDto);
            var (bytedata, format) = await DataExportHelper.TopdfAsync<ExportStockList>(data, "StockPdfReports");
            return File(bytedata, format, "StockPDFReports.pdf");
        }
        #endregion
    }
}
