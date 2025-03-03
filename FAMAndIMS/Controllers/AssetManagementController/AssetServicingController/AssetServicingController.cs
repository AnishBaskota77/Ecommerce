using AspNetCoreHero.ToastNotification.Abstractions;
using FAMAndIMS.Common;
using FAMAndIMS.Data.Model.AssetManagementModel.AssetMaintenanceModel;
using FAMAndIMS.Data.Model.AssetManagementModel.AssetServicingModel;
using FAMAndIMS.Data.Model.AssetManagementModel.AssetsModel;
using FAMAndIMS.Data.Model.CommonModel;
using FAMAndIMS.Data.Services.AssetManagementServices.AssetServicingServices;
using FAMAndIMS.Data.Services.AssetManagementServices.AssetsServices;
using FAMAndIMS.Data.Services.CommonddlServices;
using FAMAndIMS.Data.Services.CommonServices;
using FAMAndIMS.Filter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;

namespace FAMAndIMS.Controllers.AssetManagementController.AssetServicingController
{
    public class AssetServicingController : Controller
    {
        private readonly INotyfService _notyfService;
        private readonly IAssetServicingServices _assetServicingServices;
        private readonly ICommonddlService _commonddlService;
        private readonly ICommonServices _commonServices;

        public AssetServicingController(INotyfService notyfService, IAssetServicingServices assetServicingServices, ICommonddlService commonddlService, ICommonServices commonServices)
        {
            _notyfService = notyfService;
            _assetServicingServices = assetServicingServices;
            _commonddlService = commonddlService;
            _commonServices = commonServices;
        }
        #region Index
        [MenuAccess]
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] AssetServicingDto assetServicingDto)
        {
            Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";
            ViewBag.view = HttpContext.Items["view"];
            ViewBag.create = HttpContext.Items["create"];
            ViewBag.delete = HttpContext.Items["delete"];
            ViewBag.update = HttpContext.Items["update"];
            var assetServicingList = await _assetServicingServices.GetAssetServicingList(assetServicingDto);
            if (WebHelper.IsAjaxRequest(Request))
            {
                return PartialView("_AssetServicingIndex", assetServicingList);
            }
            return View(assetServicingList);

        }
        #endregion

        #region Save Asset Servicing
        [HttpGet]
        public async Task<IActionResult> SaveAssetServicing()
        {
            var assetDdlList = await _commonddlService.GetAssetDDL();
            ViewBag.assetDdlList = new SelectList(assetDdlList, "Value", "Text");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SaveAssetServicing(AssetServicingVM assetServicingVM)
        {
            var assetDdlList = await _commonddlService.GetAssetDDL();
            ViewBag.assetDdlList = new SelectList(assetDdlList, "Value", "Text");
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return View();
            }
            else
            {
                var responseMessage = await _assetServicingServices.SaveAssetServicing(assetServicingVM);
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

        #region Update Asset Servicing

        [HttpGet]
        public async Task<IActionResult> UpdateAssetServicing(int id)
        {
            var assetDdlList = await _commonddlService.GetAssetDDL();
            ViewBag.assetDdlList = new SelectList(assetDdlList, "Value", "Text");
            var data = await _assetServicingServices.GetAssetServicingById(id);
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAssetServicing(AssetServicingVM assetServicingVM)
        {
            var assetDdlList = await _commonddlService.GetAssetDDL();
            ViewBag.assetDdlList = new SelectList(assetDdlList, "Value", "Text");
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return View();
            }
            else
            {
                var responseMessage = await _assetServicingServices.SaveAssetServicing(assetServicingVM);
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

        #region Delete Asset Servicing

        [HttpGet]
        public async Task<IActionResult> DeleteAssetServicing(int Id)
        {
            var data = await _assetServicingServices.GetAssetServicingById(Id);
            return PartialView(data);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteAssetServicing(AssetServicingVM assetServicingVM)
        {
            var responseMessage = await _assetServicingServices.DeleteAssetServicing(assetServicingVM.Id);
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
