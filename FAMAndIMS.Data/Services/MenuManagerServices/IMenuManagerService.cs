using FAMAndIMS.Data.Model.CommonModel;
using FAMAndIMS.Data.Model.MenuManagerListModel;
using FAMAndIMS.Data.Model.MenuManagerModel;
using FAMAndIMS.Data.Model.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Services.MenuServices
{
    public interface IMenuManagerService
    {
        Task<IEnumerable<MainParentModel>> GetMainParentMenuList();
        Task<IEnumerable<MenuItemModel>> GetSideBarMenuList();
        Task<SpResponseMessage> SaveMenuManager(MenuManagerVM menuManagerVM);
        Task<MenuManagerVM> GetMenuManagerById(int Id);
        Task<PagedResponse<MenuManagerListModel>> GetMenuManagerList(MenuManagerDTO menuManagerDTO);
        Task<MenuManagerVM> GetMenuById(int Id);
        Task<SpResponseMessage> DeleteMenu(int id);

    }
}
