using FAMAndIMS.Data.Model.CommonModel;
using FAMAndIMS.Data.Model.GlobalSettingModel.InsuranceCompanyModel;
using FAMAndIMS.Data.Model.Paging;

namespace FAMAndIMS.Data.Services.GlobalSettingServices.InsuranceCompanyServices
{
    public interface IInsuranceCompanyServices
    {
        Task<PagedResponse<InsuranceCompanyListModel>> GetInsuranceCompanyList(InsuranceCompanyDto insuranceCompanyDto);
        Task<SpResponseMessage> SaveInsuranceCompany(InsuranceCompanyVM insuranceCompanyVM);
        Task<InsuranceCompanyVM> GetInsuranceCompanyById(int Id);
        Task<SpResponseMessage> DeleteInsuranceCompany(int id);
    }
}
