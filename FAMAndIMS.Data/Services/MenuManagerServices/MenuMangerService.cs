using AutoMapper;
using Dapper;
using FAMAndIMS.Data.Common;
using FAMAndIMS.Data.DBManager;
using FAMAndIMS.Data.Library;
using FAMAndIMS.Data.Model.CommonModel;
using FAMAndIMS.Data.Model.MenuManagerListModel;
using FAMAndIMS.Data.Model.MenuManagerModel;
using FAMAndIMS.Data.Model.Paging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FAMAndIMS.Data.Services.MenuServices
{
    public class MenuMangerService : IMenuManagerService
    {
        private readonly DBConnectionManager _dbConnectionManager;
        private readonly IHostingEnvironment _hostEnv;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClaimsPrincipal _loggedInUser;
        public MenuMangerService(DBConnectionManager dbConnectionManager, IHostingEnvironment hostEnv, IConfiguration config, IMapper mapper,IHttpContextAccessor httpContextAccessor)
        {
            _dbConnectionManager = dbConnectionManager;
            _hostEnv = hostEnv;
            _config = config;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _loggedInUser = _httpContextAccessor.HttpContext.User;
        }
        public async Task<IEnumerable<MainParentModel>> GetMainParentMenuList()
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                var p = new DynamicParameters();
                db.Open();
                var menuList = await db.QueryAsync<MainParentModel>("[dbo].[usp_GetMainParentMenu]", param: p, commandType: CommandType.StoredProcedure);
                db.Close();
                return menuList;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<MenuItemModel>> GetSideBarMenuList()
        {
            var userName = _httpContextAccessor.HttpContext.User.Identity.Name;
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                var p = new DynamicParameters();
                p.Add("@Username", userName);
                db.Open();
                var result = await db.QueryAsync<MenuItemModel>("[dbo].[usp_GetRoleMenuPermissions_ByUserName]", param: p, commandType: CommandType.StoredProcedure);
                db.Close();
                var menuItems = result.ToList();

                var menuDictionary = menuItems.ToDictionary(menu => menu.Id, menu => menu);

                foreach (var item in menuItems)
                {
                    if (item.SubParentId != 0)
                    {
                        if (menuDictionary.ContainsKey(item.SubParentId))
                        {
                            var parentMenu = menuDictionary[item.SubParentId];
                            parentMenu.SubMenuItems.Add(item);
                        }
                    }
                }

                return menuItems.Where(item => item.SubParentId == 0);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<PagedResponse<MenuManagerListModel>> GetMenuManagerList(MenuManagerDTO menuManagerDTO)
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                var p = menuManagerDTO.PrepareDynamicParameters();
                db.Open();
                var result = await db.QueryMultipleAsync("[dbo].[usp_GetMenuManagerList]", param: p, commandType: CommandType.StoredProcedure);
                var menuManagerList = await result.ReadAsync<MenuManagerListModel>();
                var pagedInfo = await result.ReadFirstAsync<PagedInfo>();
                var mappedData = _mapper.Map<PagedResponse<MenuManagerListModel>>(pagedInfo);
                mappedData.Items = menuManagerList;
                db.Close();

                return mappedData;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<SpResponseMessage> SaveMenuManager(MenuManagerVM menuManagerVM)
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                var p = new DynamicParameters();

                if (menuManagerVM.Id > 0)
                {
                    p.Add("@Event", "U");
                }

                db.Open();
                p.Add("@Id", menuManagerVM.Id);
                p.Add("@Title", menuManagerVM.Title);
                p.Add("@MainParentId", menuManagerVM.MainParentId);
                p.Add("@SubParentId", menuManagerVM.SubParentId);
                p.Add("@MenuUrl", menuManagerVM.MenuUrl);
                p.Add("@IsActive", menuManagerVM.IsActive);
                p.Add("@IsDeleted", menuManagerVM.IsDeleted);
                p.Add("@DisplayOrder", menuManagerVM.DisplayOrder);
                p.Add("@IconDataFeather", menuManagerVM.IconDataFeather);
                p.Add("@CreatedBy", _loggedInUser.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
                p.Add("@UpdatedBy", _loggedInUser.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
                p.Add("@Return_Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add("@Msg", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
                p.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add("@MsgType", dbType: DbType.String, size: 50, direction: ParameterDirection.Output);

                var result = await db.ExecuteAsync("[dbo].[usp_IUDMenuManger]", param: p, commandType: CommandType.StoredProcedure);
                var spResponseMessage = new SpResponseMessage
                {
                    ReturnId = p.Get<int>("@Return_Id"),
                    Msg = p.Get<string>("@Msg"),
                    StatusCode = p.Get<int>("@StatusCode"),
                    MsgType = p.Get<string>("@MsgType")
                };
                db.Close();
                return spResponseMessage;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<MenuManagerVM> GetMenuManagerById(int Id)
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                db.Open();
                var result = await db.QuerySingleOrDefaultAsync<MenuManagerVM>("[dbo].[usp_GetSideBarMenuList]", commandType: CommandType.StoredProcedure);
                db.Close();
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<MenuManagerVM> GetMenuById(int Id)
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                var p = new DynamicParameters();
                db.Open();
                p.Add("@MenuId", Id);
                var data = await db.QuerySingleAsync<MenuManagerVM>(sql: "[dbo].[usp_menu_get_byid]", param: p, commandType: CommandType.StoredProcedure);
                db.Close();
                return data;
               
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<SpResponseMessage> DeleteMenu(int Id)
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                var p = new DynamicParameters();

                db.Open();
                p.Add("@Event", 'D');
                p.Add("@Id",Id);
                p.Add("@UpdatedBy", _loggedInUser.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
                p.Add("@IsDeleted", true);
                p.Add("@Return_Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add("@Msg", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
                p.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add("@MsgType", dbType: DbType.String, size: 50, direction: ParameterDirection.Output);

                var result = await db.ExecuteAsync("[dbo].[usp_IUDMenuManger]", param: p, commandType: CommandType.StoredProcedure);
                var spResponseMessage = new SpResponseMessage
                {
                    ReturnId = p.Get<int>("@Return_Id"),
                    Msg = p.Get<string>("@Msg"),
                    StatusCode = p.Get<int>("@StatusCode"),
                    MsgType = p.Get<string>("@MsgType")
                };
                db.Close();
                return spResponseMessage;
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}

    


