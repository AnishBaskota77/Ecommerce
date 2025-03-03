using AspNetCoreHero.ToastNotification.Abstractions;
using FAMAndIMS.Common;
using FAMAndIMS.Data.Model.CommonModel;
using FAMAndIMS.Data.Model.GlobalSettingModel.SubCategoryModel;
using FAMAndIMS.Data.Services.CommonddlServices;
using FAMAndIMS.Data.Services.CommonServices;
using FAMAndIMS.Data.Services.GlobalSettingServices.SubCategoryServices;
using FAMAndIMS.Filter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;

namespace FAMAndIMS.Controllers.GlobalSettingController.SubCategoryController
{
    [Authorize]
    public class SubCategoryController : Controller
    {
        private readonly INotyfService _notyfService;
        private readonly ISubCategoryServices _subCategoryServices;
        private readonly ICommonddlService _commonddlService;
        private readonly ICommonServices _commonServices;

        public SubCategoryController(INotyfService notyfService, ISubCategoryServices subCategoryServices, ICommonddlService commonddlService, ICommonServices commonServices)
        {
            _notyfService = notyfService;
            _subCategoryServices = subCategoryServices;
            _commonddlService = commonddlService;
            _commonServices=commonServices;
        }
        #region Index
        [MenuAccess]
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] SubCategoryDto subCategoryDto)
        {
            Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";
            ViewBag.view = HttpContext.Items["view"];
            ViewBag.create = HttpContext.Items["create"];
            ViewBag.delete = HttpContext.Items["delete"];
            ViewBag.update = HttpContext.Items["update"];
            var subCategoryList = await _subCategoryServices.GetSubCategoryList(subCategoryDto);
            if (WebHelper.IsAjaxRequest(Request))
            {
                return PartialView("_SubCategoryIndex", subCategoryList);
            }
            return View(subCategoryList);

        }
        #endregion

        #region Save SubCategory
        [HttpGet]
        public async Task<IActionResult> SaveSubCategory()
        {
            var categoryDdlList = await _commonddlService.GetCategoryDDL();
            ViewBag.categoryDdlList = new SelectList(categoryDdlList, "Value", "Text");
            return PartialView();
        }
        [HttpPost]
        [LogUserActivity(UserActionTypes.SaveSubCategory)]
        public async Task<IActionResult> SaveSubCategory(SubCategoryVm subCategoryVm)
        {
            var categoryDdlList = await _commonddlService.GetCategoryDDL();
            ViewBag.categoryDdlList = new SelectList(categoryDdlList, "Value", "Text");
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var responseMessage = await _subCategoryServices.SaveSubCategory(subCategoryVm);
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

        #region Update SubCategory

        [HttpGet]
        public async Task<IActionResult> UpdateSubCategory(int id)
        {
            var categoryDdlList = await _commonddlService.GetCategoryDDL();
            ViewBag.categoryDdlList = new SelectList(categoryDdlList, "Value", "Text");
            var data = await _subCategoryServices.GetSubCategoryById(id);
            return PartialView(data);
        }

        [HttpPost]
        [LogUserActivity(UserActionTypes.UpdateSubCategory)]
        public async Task<IActionResult> UpdateSubCategory(SubCategoryVm subCategoryVm)
        {
            var categoryDdlList = await _commonddlService.GetCategoryDDL();
            ViewBag.categoryDdlList = new SelectList(categoryDdlList, "Value", "Text");
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var responseMessage = await _subCategoryServices.SaveSubCategory(subCategoryVm);
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

        #region Delete SubCategory

        [HttpGet]
        public async Task<IActionResult> DeleteSubCategory(int Id)
        {
            var data = await _subCategoryServices.GetSubCategoryById(Id);
            return PartialView(data);
        }
        [HttpPost]
        [LogUserActivity(UserActionTypes.DeleteSubCategory)]
        public async Task<IActionResult> DeleteSubCategory(SubCategoryVm subCategoryVm)
        {
            var responseMessage = await _subCategoryServices.DeleteSubCategory(subCategoryVm.Id);
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
        [LogUserActivity(UserActionTypes.UpdateSubCategoryStatus)]
        public async Task<IActionResult> UpdateStatus(int Id, bool IsActive)
        {
            var tableFlag = "tbl_SubCategory";
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
