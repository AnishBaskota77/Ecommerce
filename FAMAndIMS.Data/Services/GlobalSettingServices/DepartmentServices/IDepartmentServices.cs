using FAMAndIMS.Data.Model.CommonModel;
using FAMAndIMS.Data.Model.GlobalSettingModel.DepartmentModel;
using FAMAndIMS.Data.Model.Paging;

namespace FAMAndIMS.Data.Services.GlobalSettingServices.DepartmentServices
{
    public interface IDepartmentServices
    {
        Task<PagedResponse<DepartmentListModel>> GetDepartmentList(DepartmentDto departmentDto);
        Task<SpResponseMessage> SaveDepartment(DepartmentVM departmentVM);
        Task<DepartmentVM> GetDepartmentById(int Id);
        Task<SpResponseMessage> DeleteDepartment(int id);

    }
}
