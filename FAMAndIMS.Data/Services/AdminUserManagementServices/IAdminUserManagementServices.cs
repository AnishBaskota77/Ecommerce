using FAMAndIMS.Data.Model.AdminUserManagementModel;
using FAMAndIMS.Data.Model.CommonModel;
using FAMAndIMS.Data.Model.EmployeeManagementModel;
using FAMAndIMS.Data.Model.Paging;

namespace FAMAndIMS.Data.Services.EmployeeManagementServices
{
    public interface IAdminUserManagementServices
    {
        public Task<PagedResponse<AdminUserManagementListModel>> GetAllAdminUserList(AdminUserListFilterDto adminUserListFilterDto);
        Task<SpResponseMessage> SaveAdminUser(AdminUserVM adminUserVM);
        Task<SpResponseMessage> UpdateAdminUser(AdminUserUpdateVM adminUserUpdateVM);
        Task<AdminUserVM> GetAdminUserById(int Id);
        Task<SpResponseMessage> DeleteAdminUser(int id);
        Task<SpResponseMessage> AdminUserResetPassword(AdminUserResetPasswordVm adminUserResetPasswordVm);

    }
}
