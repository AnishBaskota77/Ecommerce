using AspNetCoreHero.ToastNotification.Abstractions;
using FAMAndIMS.Common;
using FAMAndIMS.Data.Model.AssetManagementModel.AssetLeasedModel;
using FAMAndIMS.Data.Services.AssetManagementServices.AssetLeasedServices;
using FAMAndIMS.Data.Services.CommonddlServices;
using FAMAndIMS.Data.Services.CommonServices;
using FAMAndIMS.Filter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;

namespace FAMAndIMS.Controllers.AssetManagementController.AssetLeasedController
{
    public class AssetLeasedController : Controller
    {
        private readonly INotyfService _notyfService;
        private readonly IAssetLeasedServices _assetLeasedServices;
        private readonly ICommonddlService _commonddlService;
        private readonly ICommonServices _commonServices;

        public AssetLeasedController(INotyfService notyfService, IAssetLeasedServices assetLeasedServices, ICommonddlService commonddlService, ICommonServices commonServices)
        {
            _notyfService = notyfService;
            _assetLeasedServices = assetLeasedServices;
            _commonddlService = commonddlService;
            _commonServices = commonServices;
        }
        #region Index
        [MenuAccess]
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] AssetLeasedDto assetLeasedDto)
        {
            Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";
            ViewBag.view = HttpContext.Items["view"];
            ViewBag.create = HttpContext.Items["create"];
            ViewBag.delete = HttpContext.Items["delete"];
            ViewBag.update = HttpContext.Items["update"];
            var assetLeasedList = await _assetLeasedServices.GetAssetLeasedList(assetLeasedDto);
            if (WebHelper.IsAjaxRequest(Request))
            {
                return PartialView("_AssetLeasedIndex", assetLeasedList);
            }
            return View(assetLeasedList);

        }
        #endregion

        #region Save Asset Leased
        [HttpGet]
        public async Task<IActionResult> SaveAssetLeased()
        {
            var assetDdlList = await _commonddlService.GetAssetDDL();
            ViewBag.assetDdlList = new SelectList(assetDdlList, "Value", "Text");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SaveAssetLeased(AssetLeasedVM assetLeasedVM)
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
                var responseMessage = await _assetLeasedServices.SaveAssetLeased(assetLeasedVM);
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

        #region Update Asset Leased

        [HttpGet]
        public async Task<IActionResult> UpdateAssetLeased(int id)
        {
            var assetDdlList = await _commonddlService.GetAssetDDL();
            ViewBag.assetDdlList = new SelectList(assetDdlList, "Value", "Text");
            var data = await _assetLeasedServices.GetAssetLeasedById(id);
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAssetLeased(AssetLeasedVM assetLeasedVM)
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
                var responseMessage = await _assetLeasedServices.SaveAssetLeased(assetLeasedVM);
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

        #region Delete Asset Leased

        [HttpGet]
        public async Task<IActionResult> DeleteAssetLeased(int Id)
        {
            var data = await _assetLeasedServices.GetAssetLeasedById(Id);
            return PartialView(data);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteAssetLeased(AssetLeasedVM assetLeasedVM)
        {
            var responseMessage = await _assetLeasedServices.DeleteAssetLeased(assetLeasedVM.Id);
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
