using AspNetCoreHero.ToastNotification.Abstractions;
using FAMAndIMS.Common;
using FAMAndIMS.Data.Model.CommonModel;
using FAMAndIMS.Data.Model.GlobalSettingModel.CategoryModel;
using FAMAndIMS.Data.Model.GlobalSettingModel.UnitModel;
using FAMAndIMS.Data.Services.CommonServices;
using FAMAndIMS.Data.Services.GlobalSettingServices.CategoryServices;
using FAMAndIMS.Data.Services.GlobalSettingServices.UnitServices;
using FAMAndIMS.Filter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FAMAndIMS.Controllers.GlobalSettingController.UnitController
{
    [Authorize]
    public class UnitController : Controller
    {
        private readonly INotyfService _notyfService;
        private readonly IUnitServices _unitServices;
        private readonly ICommonServices _commonServices;
        public UnitController(INotyfService notyfService, IUnitServices unitServices, ICommonServices commonServices)
        {
            _notyfService = notyfService;
            _unitServices = unitServices;
            _commonServices=commonServices;
        }
        #region Index
        [MenuAccess]
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] UnitDto unitDto)
        {
            Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";
            ViewBag.view = HttpContext.Items["view"];
            ViewBag.create = HttpContext.Items["create"];
            ViewBag.delete = HttpContext.Items["delete"];
            ViewBag.update = HttpContext.Items["update"];
            var unitList = await _unitServices.GetUnitList(unitDto);
            if (WebHelper.IsAjaxRequest(Request))
            {
                return PartialView("_UnitIndex", unitList);
            }
            return View(unitList);

        }
        #endregion

        #region Save Unit
        [HttpGet]
        public async Task<IActionResult> SaveUnit()
        {
            return PartialView();
        }
        [HttpPost]
        [LogUserActivity(UserActionTypes.SaveUnit)]
        public async Task<IActionResult> SaveUnit(UnitVM unitVM)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var responseMessage = await _unitServices.SaveUnit(unitVM);
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

        #region Update Unit

        [HttpGet]
        public async Task<IActionResult> UpdateUnit(int id)
        {
            var data = await _unitServices.GetUnitById(id);
            return PartialView(data);
        }

        [HttpPost]
        [LogUserActivity(UserActionTypes.UpdateUnit)]
        public async Task<IActionResult> UpdateUnit(UnitVM unitVM)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var responseMessage = await _unitServices.SaveUnit(unitVM);
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

        #region Delete Unit

        [HttpGet]
        public async Task<IActionResult> DeleteUnit(int Id)
        {
            var data = await _unitServices.GetUnitById(Id);
            return PartialView(data);
        }
        [HttpPost]
        [LogUserActivity(UserActionTypes.DeleteUnit)]
        public async Task<IActionResult> DeleteUnit(UnitVM unitVM)
        {
            var responseMessage = await _unitServices.DeleteUnit(unitVM.Id);
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
        [LogUserActivity(UserActionTypes.UpdateUnitStatus)]
        public async Task<IActionResult> UpdateStatus(int Id, bool IsActive)
        {
            var tableFlag = "tbl_Unit";
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
