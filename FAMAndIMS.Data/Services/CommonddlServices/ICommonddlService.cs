using FAMAndIMS.Data.Model.CommondddlModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Services.CommonddlServices
{
    public interface ICommonddlService
    {
        Task<IEnumerable<CommonddlModel>> GetMainParentMenuList();
        Task<IEnumerable<CommonddlModel>> GetSubParentMenuList();
        Task<IEnumerable<CommonddlModel>> GetRoleDDL();
        Task<IEnumerable<CommonddlModel>> GetGenderDDL();
        Task<IEnumerable<CommonddlModel>> GetCountryDDL();
        Task<IEnumerable<CommonddlModel>> GetProvinceDDL();
        Task<IEnumerable<CommonddlModel>> GetDistrictDDL();
        Task<IEnumerable<CommonddlModel>> GetMunicipalityDDL();
        Task<IEnumerable<CommonddlModel>> GetCategoryDDL();
        Task<IEnumerable<CommonddlModel>> GetBranchDDL();
        Task<IEnumerable<CommonddlModel>> GetDepartmentDDL();
        Task<IEnumerable<CommonddlModel>> GetAssetDDL();
        Task<IEnumerable<CommonddlModel>> GetInsuranceCompanyDDL();
        Task<IEnumerable<CommonddlModel>> GetEmployeeDDL();
        Task<IEnumerable<CommonddlModel>> GetUnitDDL();
        Task<IEnumerable<CommonddlModel>> GetSubCategoryDDL();
        Task<IEnumerable<CommonddlModel>> GetDepreciationDDL();
        Task<IEnumerable<CommonddlModel>> GetConditionDDL();
        Task<IEnumerable<CommonddlModel>> GetVendorDDL();
        Task<IEnumerable<CommonddlModel>> GetItemsDDL();



    }
}
