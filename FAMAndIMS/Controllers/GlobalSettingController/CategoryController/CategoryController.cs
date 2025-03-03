using AspNetCoreHero.ToastNotification.Abstractions;
using FAMAndIMS.Common;
using FAMAndIMS.Data.Model.CommonModel;
using FAMAndIMS.Data.Model.GlobalSettingModel.CategoryModel;
using FAMAndIMS.Data.Model.GlobalSettingModel.DepartmentModel;
using FAMAndIMS.Data.Services.CommonServices;
using FAMAndIMS.Data.Services.GlobalSettingServices.CategoryServices;
using FAMAndIMS.Data.Services.GlobalSettingServices.DepartmentServices;
using FAMAndIMS.Filter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FAMAndIMS.Controllers.GlobalSettingController.CategoryController
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly INotyfService _notyfService;
        private readonly ICategoryServices _categoryServices;
        private readonly ICommonServices _commonServices;
        public CategoryController(INotyfService notyfService, ICategoryServices categoryServices, ICommonServices commonServices)
        {
            _notyfService = notyfService;
            _categoryServices = categoryServices;
            _commonServices=commonServices;
        }
        #region Index
        [MenuAccess]
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] CategoryDto categoryDto)
        {
            Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";
            ViewBag.view = HttpContext.Items["view"];
            ViewBag.create = HttpContext.Items["create"];
            ViewBag.delete = HttpContext.Items["delete"];
            ViewBag.update = HttpContext.Items["update"];
            var categoryList = await _categoryServices.GetCategoryList(categoryDto);
            if (WebHelper.IsAjaxRequest(Request))
            {
                return PartialView("_CategoryIndex", categoryList);
            }
            return View(categoryList);

        }
        #endregion

        #region Save Category
        [HttpGet]
        public async Task<IActionResult> SaveCategory()
        {
            return PartialView();
        }
        [HttpPost]
        [LogUserActivity(UserActionTypes.SaveCategory)]
        public async Task<IActionResult> SaveCategory(CategoryVm categoryVm)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var responseMessage = await _categoryServices.SaveCategory(categoryVm);
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

        #region Update Category

        [HttpGet]
        public async Task<IActionResult> UpdateCategory(int id)
        {
            var data = await _categoryServices.GetCategoryById(id);
            return PartialView(data);
        }

        [HttpPost]
        [LogUserActivity(UserActionTypes.UpdateCategory)]
        public async Task<IActionResult> UpdateCategory(CategoryVm categoryVm)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var responseMessage = await _categoryServices.SaveCategory(categoryVm);
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

        #region Delete Category

        [HttpGet]
        public async Task<IActionResult> DeleteCategory(int Id)
        {
            var data = await _categoryServices.GetCategoryById(Id);
            return PartialView(data);
        }
        [HttpPost]
        [LogUserActivity(UserActionTypes.DeleteCategory)]
        public async Task<IActionResult> DeleteCategory(CategoryVm categoryVm)
        {
            var responseMessage = await _categoryServices.DeleteCategory(categoryVm.Id);
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
        [LogUserActivity(UserActionTypes.UpdateCategoryStatus)]
        public async Task<IActionResult> UpdateStatus(int Id, bool IsActive)
        {
            var tableFlag = "tbl_Category";
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
