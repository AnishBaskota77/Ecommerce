using AutoMapper;
using Dapper;
using FAMAndIMS.Data.DBManager;
using FAMAndIMS.Data.Library;
using FAMAndIMS.Data.Model.CommonModel;
using FAMAndIMS.Data.Model.Paging;
using FAMAndIMS.Data.Model.RoleMenuManagerModel;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Data;
using System.Security.Claims;

namespace FAMAndIMS.Data.Services.RoleMenuManagerServices
{
    public class RoleMenuManagerService : IRoleMenuMangerService
    {
        private readonly DBConnectionManager _dbConnectionManager;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClaimsPrincipal _loggedInUser;
        public RoleMenuManagerService(DBConnectionManager dbConnectionManager, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _dbConnectionManager = dbConnectionManager;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _loggedInUser = _httpContextAccessor.HttpContext.User;
        }
        public async Task<PagedResponse<RoleMenuMangerListModel>> GetRoleMenuManagerList(RoleMenuManagerDTO roleMenuManagerDTO)
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                var p = roleMenuManagerDTO.PrepareDynamicParameters();
                db.Open();
                var result = await db.QueryMultipleAsync("[dbo].[usp_GetRoleMenuManagerList]", param: p, commandType: CommandType.StoredProcedure);
                var roleMenuManagerList = await result.ReadAsync<RoleMenuMangerListModel>();
                var pagedInfo = await result.ReadFirstAsync<PagedInfo>();
                var mappedData = _mapper.Map<PagedResponse<RoleMenuMangerListModel>>(pagedInfo);
                mappedData.Items = roleMenuManagerList;
                db.Close();

                return mappedData;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<SpResponseMessage> SaveRoleMenuManager(RoleMenuManagerVM roleMenuManagerVM)
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                var p = new DynamicParameters();

                if (roleMenuManagerVM.Id > 0)
                {
                    p.Add("@Event", "U");
                }

                db.Open();
                p.Add("@Id", roleMenuManagerVM.Id);
                p.Add("@RoleName", roleMenuManagerVM.RoleName);
                p.Add("@IsActive", roleMenuManagerVM.IsActive);
                p.Add("@IsDeleted", roleMenuManagerVM.IsDeleted);
                p.Add("@CreatedBy", _loggedInUser.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
                p.Add("@UpdatedBy", _loggedInUser.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
                p.Add("@Return_Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add("@Msg", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
                p.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add("@MsgType", dbType: DbType.String, size: 50, direction: ParameterDirection.Output);

                var result = await db.ExecuteAsync("[dbo].[usp_IUDRoleMenuManger]", param: p, commandType: CommandType.StoredProcedure);
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
        public async Task<IEnumerable<MenuRoleWithChild>> GetMenuByRoleId(int RoleId)
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                var p = new DynamicParameters();
                db.Open();
                p.Add("@RoleId", RoleId);
                var result = await db.QueryAsync<MenuRoleWithChild>("[dbo].[usp_GetRoleMenuPermissions_ByRoleId]", param: p, commandType: CommandType.StoredProcedure);
                db.Close();
                var menuItems = result.ToList();
                return menuItems;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<RoleMenuManagerVM> GetRoleMenuManagerById(int RoleId)
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                var p = new DynamicParameters();
                p.Add("@RoleId", RoleId);
                db.Open();
                var result = await db.QuerySingleAsync<RoleMenuManagerVM>("[dbo].[usp_GetRoleMenuManagerById]", param: p, commandType: CommandType.StoredProcedure);
                db.Close();
                return result;

            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<SpResponseMessage> UpdateAssignRolePermissions(List<MenuRoleWithChild> menuRoleWithChild, int roleId)
        {
            if (roleId < 1)
                return (new SpResponseMessage { Msg = "Invalid RoleId", StatusCode = 400, ReturnId = 0, MsgType = "Error" });
            var updatedDate = DateTime.Now;

            string updatedBy = _loggedInUser.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;

            var listRoleMenuPermissionsType = menuRoleWithChild.Select(x => new RoleMenuPermissionsType
            {
                Id = x.Id,
                ViewPer = x.ViewPer,
                CreatePer = x.CreatePer,
                UpdatePer = x.UpdatePer,
                DeletePer = x.DeletePer,
                UpdatedDate = updatedDate,
                UpdatedBy = updatedBy
            });

            var dataTableRmp = GetDataTableRoleMenuPermissions();

            foreach (var rmpType in listRoleMenuPermissionsType)
            {
                var row = dataTableRmp.NewRow();
                row["Id"] = rmpType.Id;
                row["ViewPer"] = rmpType.ViewPer;
                row["CreatePer"] = rmpType.CreatePer;
                row["UpdatePer"] = rmpType.UpdatePer;
                row["DeletePer"] = rmpType.DeletePer;
                row["UpdatedDate"] = rmpType.UpdatedDate!;
                row["UpdatedBy"] = rmpType.UpdatedBy;
                dataTableRmp.Rows.Add(row);
            }
            try
            {
                using var db = _dbConnectionManager.GetConnection();

                db.Open();

                var param = new DynamicParameters();
                param.Add("@RoleId", roleId);
                param.Add("@RoleMenuPermissions", dataTableRmp.AsTableValuedParameter("[dbo].[RoleMenuPermissionsType]"));
                param.Add("@sqlActionStatus", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
                var result = await db.ExecuteAsync("[dbo].[usp_rolemenupermissions_insert_byroleid]", param: param, commandType: CommandType.StoredProcedure);
                var sqlActionStatus = param.Get<int>("@sqlActionStatus");
                if (sqlActionStatus < 0)
                    return (new SpResponseMessage { Msg = "Server Error", StatusCode = 400, ReturnId = 0, MsgType = "Error" });

                return (new SpResponseMessage { Msg = "Updated SucessFully", StatusCode = 200, ReturnId = 1, MsgType = "Sucess" });


            }
            catch (Exception)
            {

                throw;
            }
        }
        private DataTable GetDataTableRoleMenuPermissions()
        {
            var dataTableRmp = new DataTable();
            dataTableRmp.Columns.Add("Id", typeof(int));
            dataTableRmp.Columns.Add("ViewPer", typeof(bool));
            dataTableRmp.Columns.Add("CreatePer", typeof(bool));
            dataTableRmp.Columns.Add("UpdatePer", typeof(bool));
            dataTableRmp.Columns.Add("DeletePer", typeof(bool));
            dataTableRmp.Columns.Add("CreatedDate", typeof(DateTime)).AllowDBNull = true;
            dataTableRmp.Columns.Add("CreatedBy", typeof(string)).AllowDBNull = true;
            dataTableRmp.Columns.Add("UpdatedDate", typeof(DateTime)).AllowDBNull = true;
            dataTableRmp.Columns.Add("UpdatedBy", typeof(string)).AllowDBNull = true;
            dataTableRmp.Columns.Add("IsActive", typeof(bool));
            return dataTableRmp;
        }
        public async Task<SpResponseMessage> DeleteRoleMenuManager(RoleMenuManagerVM roleMenuManagerVM)
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                var p = new DynamicParameters();

                db.Open();
                p.Add("@Event", 'D');
                p.Add("@Id", roleMenuManagerVM.Id);
                p.Add("@UpdatedBy", _loggedInUser.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
                p.Add("@IsDeleted", true);
                p.Add("@Return_Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add("@Msg", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
                p.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add("@MsgType", dbType: DbType.String, size: 50, direction: ParameterDirection.Output);

                var result = await db.ExecuteAsync("[dbo].[usp_IUDRoleMenuManger]", param: p, commandType: CommandType.StoredProcedure);
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
