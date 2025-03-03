using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Model.MenuManagerModel
{
    public class MainParentModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int DisplayOrder { get; set; }
    }
    public class MenuItemModel
    {
        public int Id { get; set; }
        public int MainParentId { get; set; }
        public int SubParentId { get; set; }
        public string Title { get; set; }
        public string MenuUrl { get; set; }
        public List<MenuItemModel> SubMenuItems { get; set; } = new List<MenuItemModel>();       
        public string IconDataFeather { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public bool ViewPer { get; set; }
        public bool CreatePer { get; set; }
        public bool UpdatePer { get; set; }
        public bool DeletePer { get; set; }
    }
    public class SidebarMenuViewModel
    {
        public List<MainParentModel> MainParentMenu { get; set; }
        public List<MenuItemModel> ParentMenu { get; set; }
    }
}
