using AspNetCoreHero.ToastNotification.Abstractions;
using FAMAndIMS.Common;
using FAMAndIMS.Data.Model.CommonModel;
using FAMAndIMS.Data.Model.GlobalSettingModel.DepreciationModel;
using FAMAndIMS.Data.Services.CommonServices;
using FAMAndIMS.Data.Services.GlobalSettingServices.DepreciationServices;
using FAMAndIMS.Filter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FAMAndIMS.Controllers.GlobalSettingController.DepreciationTypeController
{
    [Authorize]
    public class DepreciationTypeController : Controller
    {
        private readonly INotyfService _notyfService;
        private readonly IDepreciationTypeServices _depreciationTypeServices;
        private readonly ICommonServices _commonServices;
        public DepreciationTypeController(INotyfService notyfService, IDepreciationTypeServices depreciationTypeServices, ICommonServices commonServices)
        {
            _notyfService = notyfService;
            _depreciationTypeServices = depreciationTypeServices;
            _commonServices=commonServices;
        }
        #region Index
        [MenuAccess]
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] DepreciationTypeDto depreciationTypeDto)
        {
            Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";
            ViewBag.view = HttpContext.Items["view"];
            ViewBag.create = HttpContext.Items["create"];
            ViewBag.delete = HttpContext.Items["delete"];
            ViewBag.update = HttpContext.Items["update"];
            var depreciationTypeList = await _depreciationTypeServices.GetDepreciationTypeList(depreciationTypeDto);
            if (WebHelper.IsAjaxRequest(Request))
            {
                return PartialView("_DepreciationTypeIndex", depreciationTypeList);
            }
            return View(depreciationTypeList);

        }
        #endregion

        #region Save DepreciationType
        [HttpGet]
        public async Task<IActionResult> SaveDepreciationType()
        {
            return PartialView();
        }
        [HttpPost]
        [LogUserActivity(UserActionTypes.SaveDepreciationType)]
        public async Task<IActionResult> SaveDepreciationType(DepreciationTypeVM depreciationTypeVM)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var responseMessage = await _depreciationTypeServices.SaveDepreciationType(depreciationTypeVM);
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

        #region Update DepreciationType

        [HttpGet]
        public async Task<IActionResult> UpdateDepreciationType(int id)
        {
            var data = await _depreciationTypeServices.GetDepreciationTypeById(id);
            return PartialView(data);
        }

        [HttpPost]
        [LogUserActivity(UserActionTypes.UpdateDepreciationType)]
        public async Task<IActionResult> UpdateDepreciationType(DepreciationTypeVM depreciationTypeVM)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var responseMessage = await _depreciationTypeServices.SaveDepreciationType(depreciationTypeVM);
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

        #region Delete DepreciationType

        [HttpGet]
        public async Task<IActionResult> DeleteDepreciationType(int Id)
        {
            var data = await _depreciationTypeServices.GetDepreciationTypeById(Id);
            return PartialView(data);
        }
        [HttpPost]
        [LogUserActivity(UserActionTypes.DeleteDepreciationType)]
        public async Task<IActionResult> DeleteDepreciationType(DepreciationTypeVM depreciationTypeVM)
        {
            var responseMessage = await _depreciationTypeServices.DeleteDepreciationType(depreciationTypeVM.Id);
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
        [LogUserActivity(UserActionTypes.UpdateDepreciationTypeStatus)]
        public async Task<IActionResult> UpdateStatus(int Id, bool IsActive)
        {
            var tableFlag = "tbl_DepreciationType";
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
