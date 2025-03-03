using FAMAndIMS.Data.Model.CommonModel;
using FAMAndIMS.Data.Model.Paging;
using FAMAndIMS.Data.Model.RoleMenuManagerModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Services.RoleMenuManagerServices
{
    public interface IRoleMenuMangerService
    {
        Task<PagedResponse<RoleMenuMangerListModel>> GetRoleMenuManagerList(RoleMenuManagerDTO roleMenuManagerDTO);
        Task<SpResponseMessage> SaveRoleMenuManager(RoleMenuManagerVM roleMenuManagerVM);
        Task<IEnumerable<MenuRoleWithChild>> GetMenuByRoleId(int RoleId);
        Task<RoleMenuManagerVM> GetRoleMenuManagerById(int RoleId);
        Task<SpResponseMessage> UpdateAssignRolePermissions(List<MenuRoleWithChild> menuRoleWithChild, int roleId);
        Task<SpResponseMessage> DeleteRoleMenuManager(RoleMenuManagerVM roleMenuManagerVM);
   
    }
}
