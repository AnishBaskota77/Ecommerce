using FAMAndIMS.Data.Model.MenuManagerModel;
using FAMAndIMS.Data.Services.MenuServices;
using Microsoft.AspNetCore.Mvc;

namespace FAMAndIMS.Components
{
    [ViewComponent]
    public class SideBarNavigation : ViewComponent
    {
        private readonly IMenuManagerService _menuManagerService;
        public SideBarNavigation(IMenuManagerService menuManagerService)
        {
            _menuManagerService = menuManagerService;
        }
        public async Task<IViewComponentResult> InvokeAsync(string viewName)
        {
            var mainParentMenu = await _menuManagerService.GetMainParentMenuList();
            var parentMenu = await _menuManagerService.GetSideBarMenuList();

            var model = new SidebarMenuViewModel
            {
                MainParentMenu = mainParentMenu.ToList(),
                ParentMenu = parentMenu.ToList()
            };
            return View(viewName, model);
        }
    }
}
