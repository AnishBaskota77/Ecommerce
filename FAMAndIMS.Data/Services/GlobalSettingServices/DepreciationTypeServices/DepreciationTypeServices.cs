﻿using AutoMapper;
using Dapper;
using FAMAndIMS.Data.DBManager;
using FAMAndIMS.Data.Library;
using FAMAndIMS.Data.Model.CommonModel;
using FAMAndIMS.Data.Model.GlobalSettingModel.DepreciationModel;
using FAMAndIMS.Data.Model.GlobalSettingModel.EmployeeModel;
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

namespace FAMAndIMS.Data.Services.GlobalSettingServices.DepreciationServices
{
    public class DepreciationTypeServices : IDepreciationTypeServices
    {
        private readonly DBConnectionManager _dbConnectionManager;
        private readonly IHostingEnvironment _hostEnv;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClaimsPrincipal _loggedInUser;
        public DepreciationTypeServices(DBConnectionManager dbConnectionManager, IHostingEnvironment hostEnv, IConfiguration config, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _dbConnectionManager = dbConnectionManager;
            _hostEnv = hostEnv;
            _config = config;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _loggedInUser = _httpContextAccessor.HttpContext.User;
        }
        public async Task<PagedResponse<DepreciationTypeListModel>> GetDepreciationTypeList(DepreciationTypeDto depreciationTypeDto)
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                var p = depreciationTypeDto.PrepareDynamicParameters();
                db.Open();
                var result = await db.QueryMultipleAsync("[dbo].[usp_GetDepreciationTypeList]", param: p, commandType: CommandType.StoredProcedure);
                var depreciationTypeList = await result.ReadAsync<DepreciationTypeListModel>();
                var pagedInfo = await result.ReadFirstAsync<PagedInfo>();
                var mappedData = _mapper.Map<PagedResponse<DepreciationTypeListModel>>(pagedInfo);
                mappedData.Items = depreciationTypeList;
                db.Close();
                return mappedData;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<SpResponseMessage> SaveDepreciationType(DepreciationTypeVM depreciationTypeVM)
        {
            try
            {
                var DepreciationTypeModel = new DepreciationTypeModel
                {
                    Id = depreciationTypeVM.Id,
                    DepreciationTypeName = depreciationTypeVM.DepreciationTypeName,
                    DepreciationTypeCode = depreciationTypeVM.DepreciationTypeCode,
                    IsActive = depreciationTypeVM.IsActive,
                    IsDeleted = false
                };

                using var db = _dbConnectionManager.GetConnection();
                var p = DepreciationTypeModel.PrepareDynamicParameters();

                if (depreciationTypeVM.Id > 0)
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
                var result = await db.ExecuteAsync("[dbo].[usp_IUDDepreciationType]", param: p, commandType: CommandType.StoredProcedure);
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
        public async Task<DepreciationTypeVM> GetDepreciationTypeById(int Id)
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                var p = new DynamicParameters();
                db.Open();
                p.Add("@Id", Id);
                var data = await db.QuerySingleAsync<DepreciationTypeVM>(sql: "[dbo].[usp_GetDepreciationTypeById]", param: p, commandType: CommandType.StoredProcedure);
                db.Close();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SpResponseMessage> DeleteDepreciationType(int Id)
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

                var result = await db.ExecuteAsync("[dbo].[usp_IUDDepreciationType]", param: p, commandType: CommandType.StoredProcedure);
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
