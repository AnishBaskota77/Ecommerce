using AspNetCoreHero.ToastNotification.Abstractions;
using FAMAndIMS.Common;
using FAMAndIMS.Data.Model.CommonModel;
using FAMAndIMS.Data.Model.GlobalSettingModel.BranchModel;
using FAMAndIMS.Data.Model.GlobalSettingModel.DepartmentModel;
using FAMAndIMS.Data.Services.CommonServices;
using FAMAndIMS.Data.Services.GlobalSettingServices.BranchServices;
using FAMAndIMS.Filter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FAMAndIMS.Controllers.GlobalSettingController.BranchController
{
    [Authorize]
    public class BranchController : Controller
    {
        private readonly INotyfService _notyfService;
        private readonly IBranchServices _branchServices;
        private readonly ICommonServices _commonServices;
        public BranchController(INotyfService notyfService, IBranchServices branchServices, ICommonServices commonServices)
        {
            _notyfService = notyfService;
            _branchServices = branchServices;
            _commonServices=commonServices;
        }
        #region Index
        [MenuAccess]
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] BranchDto branchDto)
        {
            Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";
            ViewBag.view = HttpContext.Items["view"];
            ViewBag.create = HttpContext.Items["create"];
            ViewBag.delete = HttpContext.Items["delete"];
            ViewBag.update = HttpContext.Items["update"];
            var branchList = await _branchServices.GetBranchList(branchDto);
            if (WebHelper.IsAjaxRequest(Request))
            {
                return PartialView("_BranchIndex", branchList);
            }
            return View(branchList);

        }
        #endregion

        #region Save Branch
        [HttpGet]
        public async Task<IActionResult> SaveBranch()
        {
            return PartialView();
        }
        [HttpPost]
        [LogUserActivity(UserActionTypes.SaveBranch)]
        public async Task<IActionResult> SaveBranch(BranchVM branchVM)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var responseMessage = await _branchServices.SaveBranch(branchVM);
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

        #region Update Branch

        [HttpGet]
        public async Task<IActionResult> UpdateBranch(int id)
        {
            var data = await _branchServices.GetBranchById(id);
            return PartialView(data);
        }

        [HttpPost]
        [LogUserActivity(UserActionTypes.UpdateBranch)]
        public async Task<IActionResult> UpdateBranch(BranchVM branchVM)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var responseMessage = await _branchServices.SaveBranch(branchVM);
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

        #region Delete Branch

        [HttpGet]
        public async Task<IActionResult> DeleteBranch(int Id)
        {
            var data = await _branchServices.GetBranchById(Id);
            return PartialView(data);
        }
        [HttpPost]
        [LogUserActivity(UserActionTypes.DeleteBranch)]
        public async Task<IActionResult> DeleteBranch(BranchVM branchVm)
        {
            var responseMessage = await _branchServices.DeleteBranch(branchVm.Id);
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
        [LogUserActivity(UserActionTypes.UpdateBranchStatus)]
        public async Task<IActionResult> UpdateStatus(int Id, bool IsActive)
        {
            var tableFlag = "tbl_Branch";
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
