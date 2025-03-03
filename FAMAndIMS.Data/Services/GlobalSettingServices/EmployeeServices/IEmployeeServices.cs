using FAMAndIMS.Data.Model.CommonModel;
using FAMAndIMS.Data.Model.GlobalSettingModel.EmployeeModel;
using FAMAndIMS.Data.Model.Paging;

namespace FAMAndIMS.Data.Services.GlobalSettingServices.EmployeeServices
{
    public interface IEmployeeServices
    {
        Task<PagedResponse<EmployeeListModel>> GetEmployeeList(EmployeeDto employeeDto);
        Task<SpResponseMessage> SaveEmployee(EmployeeVM employeeVM);
        Task<EmployeeVM> GetEmployeeById(int Id);
        Task<SpResponseMessage> DeleteEmployee(int id);
        Task<SpResponseMessage> BulkExcelUpload(List<EmployeeBulkVM> employeeBulkVM);
    }
}
