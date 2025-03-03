using AspNetCoreHero.ToastNotification.Abstractions;
using FAMAndIMS.Common;
using FAMAndIMS.Data.Model.CommonModel;
using FAMAndIMS.Data.Model.GlobalSettingModel.InsuranceCompanyModel;
using FAMAndIMS.Data.Services.CommonServices;
using FAMAndIMS.Data.Services.GlobalSettingServices.InsuranceCompanyServices;
using FAMAndIMS.Filter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FAMAndIMS.Controllers.GlobalSettingController.InsuranceCompanyController
{
    [Authorize]
    public class InsuranceCompanyController : Controller
    {
        private readonly INotyfService _notyfService;
        private readonly IInsuranceCompanyServices _insuranceCompanyServices;
        private readonly ICommonServices _commonServices;
        public InsuranceCompanyController(INotyfService notyfService, IInsuranceCompanyServices insuranceCompanyServices, ICommonServices commonServices)
        {
            _notyfService = notyfService;
            _insuranceCompanyServices = insuranceCompanyServices;
            _commonServices=commonServices;
        }

        #region Index
        [MenuAccess]
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] InsuranceCompanyDto insuranceCompanyDto)
        {
            Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";
            ViewBag.view = HttpContext.Items["view"];
            ViewBag.create = HttpContext.Items["create"];
            ViewBag.delete = HttpContext.Items["delete"];
            ViewBag.update = HttpContext.Items["update"];
            var insuranceCompanyList = await _insuranceCompanyServices.GetInsuranceCompanyList(insuranceCompanyDto);
            if (WebHelper.IsAjaxRequest(Request))
            {
                return PartialView("_InsuranceCompanyIndex", insuranceCompanyList);
            }
            return View(insuranceCompanyList);

        }
        #endregion

        #region Save Insurance Company
        [HttpGet]
        public async Task<IActionResult> SaveInsuranceCompany()
        {
            return PartialView();
        }
        [HttpPost]
        [LogUserActivity(UserActionTypes.SaveInsuranceCompany)]
        public async Task<IActionResult> SaveInsuranceCompany(InsuranceCompanyVM insuranceCompanyVM)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var responseMessage = await _insuranceCompanyServices.SaveInsuranceCompany(insuranceCompanyVM);
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

        #region Update Insurance Company

        [HttpGet]
        public async Task<IActionResult> UpdateInsuranceCompany(int id)
        {
            var data = await _insuranceCompanyServices.GetInsuranceCompanyById(id);
            return PartialView(data);
        }

        [HttpPost]
        [LogUserActivity(UserActionTypes.UpdateInsuranceCompany)]
        public async Task<IActionResult> UpdateInsuranceCompany(InsuranceCompanyVM insuranceCompanyVM)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView();
            }
            else
            {
                var responseMessage = await _insuranceCompanyServices.SaveInsuranceCompany(insuranceCompanyVM);
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

        #region Delete Insurance Company

        [HttpGet]
        public async Task<IActionResult> DeleteInsuranceCompany(int Id)
        {
            var data = await _insuranceCompanyServices.GetInsuranceCompanyById(Id);
            return PartialView(data);
        }
        [HttpPost]
        [LogUserActivity(UserActionTypes.DeleteInsuranceCompany)]
        public async Task<IActionResult> DeleteInsuranceCompany(InsuranceCompanyVM insuranceCompanyVM)
        {
            var responseMessage = await _insuranceCompanyServices.DeleteInsuranceCompany(insuranceCompanyVM.Id);
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
        [LogUserActivity(UserActionTypes.UpdateInsuranceCompanyStatus)]
        public async Task<IActionResult> UpdateStatus(int Id, bool IsActive)
        {
            var tableFlag = "tbl_InsuranceCompany";
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
