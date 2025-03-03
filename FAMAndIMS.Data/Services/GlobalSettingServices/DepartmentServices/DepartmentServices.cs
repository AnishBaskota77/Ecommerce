using AutoMapper;
using Dapper;
using FAMAndIMS.Data.Common.Utils.Crypto;
using FAMAndIMS.Data.DBManager;
using FAMAndIMS.Data.Library;
using FAMAndIMS.Data.Model.CommonModel;
using FAMAndIMS.Data.Model.EmployeeManagementModel;
using FAMAndIMS.Data.Model.GlobalSettingModel.DepartmentModel;
using FAMAndIMS.Data.Model.MenuManagerListModel;
using FAMAndIMS.Data.Model.MenuManagerModel;
using FAMAndIMS.Data.Model.Paging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Services.GlobalSettingServices.DepartmentServices
{
    public class DepartmentServices : IDepartmentServices
    {
        private readonly DBConnectionManager _dbConnectionManager;
        private readonly IHostingEnvironment _hostEnv;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClaimsPrincipal _loggedInUser;
        public DepartmentServices(DBConnectionManager dbConnectionManager, IHostingEnvironment hostEnv, IConfiguration config, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _dbConnectionManager = dbConnectionManager;
            _hostEnv = hostEnv;
            _config = config;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _loggedInUser = _httpContextAccessor.HttpContext.User;
        }
        public async Task<PagedResponse<DepartmentListModel>> GetDepartmentList(DepartmentDto departmentDto)
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                var p = departmentDto.PrepareDynamicParameters();
                db.Open();
                var result = await db.QueryMultipleAsync("[dbo].[usp_GetDepartmentList]", param: p, commandType: CommandType.StoredProcedure);
                var departmentList = await result.ReadAsync<DepartmentListModel>();
                var pagedInfo = await result.ReadFirstAsync<PagedInfo>();
                var mappedData = _mapper.Map<PagedResponse<DepartmentListModel>>(pagedInfo);
                mappedData.Items = departmentList;
                db.Close();
                return mappedData;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<SpResponseMessage> SaveDepartment(DepartmentVM departmentVM)
        {
            try
            {
                var DepartmentModel = new DepartmentModel
                {
                   Id = departmentVM.Id,
                   DepartmentName = departmentVM.DepartmentName,
                   DepartmentCode = departmentVM.DepartmentCode,
                   IsActive = departmentVM.IsActive, 
                   IsDeleted = false
                };

                using var db = _dbConnectionManager.GetConnection();
                var p = DepartmentModel.PrepareDynamicParameters();

                if (departmentVM.Id > 0)
                {
                    p.Add("@Event", "U");
                }
                db.Open();
                p.Add("@CreatedBy", _loggedInUser.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
                p.Add("@UpdatedBy", _loggedInUser.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
                p.Add("@Return_Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add("@Msg", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
                p.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add("@MsgType", dbType: DbType.String, size: 50, direction: ParameterDirection.Output);
                var result = await db.ExecuteAsync("[dbo].[usp_IUDDepartment]", param: p, commandType: CommandType.StoredProcedure);
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
        public async Task<DepartmentVM> GetDepartmentById(int Id)
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                var p = new DynamicParameters();
                db.Open();
                p.Add("@Id", Id);
                var data = await db.QuerySingleAsync<DepartmentVM>(sql: "[dbo].[usp_GetDepartmentById]", param: p, commandType: CommandType.StoredProcedure);
                db.Close();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SpResponseMessage> DeleteDepartment(int Id)
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                var p = new DynamicParameters();

                db.Open();
                p.Add("@Event", 'D');
                p.Add("@Id", Id);
                p.Add("@UpdatedBy", _loggedInUser.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
                p.Add("@IsDeleted", true);
                p.Add("@Return_Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add("@Msg", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
                p.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add("@MsgType", dbType: DbType.String, size: 50, direction: ParameterDirection.Output);

                var result = await db.ExecuteAsync("[dbo].[usp_IUDDepartment]", param: p, commandType: CommandType.StoredProcedure);
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
