using FAMAndIMS.Data.Services.MenuServices;
using FAMAndIMS.Data.Services.RoleMenuManagerServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FAMAndIMS.Filter
{
    public sealed class MenuAccessAttribute : TypeFilterAttribute
    {

        public MenuAccessAttribute() : base(typeof(MenuAccessFilter)) { }
        public class MenuAccessFilter : IAsyncResourceFilter
        {
            private readonly IMenuManagerService _menuManagerService;

            public MenuAccessFilter(IMenuManagerService menuManagerService)
            {
                _menuManagerService = menuManagerService;
            }

            public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
            {
                bool viewper = false;
                bool createper = false;
                bool deleteper = false;
                bool updateper = false;

                var requestedpath = context.HttpContext.Request.Path;
                var data = await _menuManagerService.GetSideBarMenuList();

                foreach (var url in data)
                {
                    if (url.MenuUrl == requestedpath)
                    {
                        viewper = url.ViewPer;
                        createper = url.CreatePer;
                        deleteper = url.DeletePer;
                        updateper = url.UpdatePer;
                    }
                    foreach (var submenu in url.SubMenuItems)
                    {
                        if (submenu.MenuUrl == requestedpath)
                        {
                            viewper = submenu.ViewPer;
                            createper = submenu.CreatePer;
                            deleteper = submenu.DeletePer;
                            updateper = submenu.UpdatePer;
                        }
                    }

                }

                context.HttpContext.Items["view"] = viewper;
                context.HttpContext.Items["create"] = createper;
                context.HttpContext.Items["delete"] = deleteper;
                context.HttpContext.Items["update"] = updateper;
                await next();


            }
        }
    }
}