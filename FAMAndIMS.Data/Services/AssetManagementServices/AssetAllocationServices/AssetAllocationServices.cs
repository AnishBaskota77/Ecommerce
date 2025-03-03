using AutoMapper;
using Dapper;
using FAMAndIMS.Data.DBManager;
using FAMAndIMS.Data.Library;
using FAMAndIMS.Data.Model.AssetManagementModel.AssetAllocationModel;
using FAMAndIMS.Data.Model.CommonModel;
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

namespace FAMAndIMS.Data.Services.AssetManagementServices.AssetAllocationServices
{
    public class AssetAllocationServices : IAssetAllocationServices
    {
        private readonly DBConnectionManager _dbConnectionManager;
        private readonly IHostingEnvironment _hostEnv;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClaimsPrincipal _loggedInUser;
        public AssetAllocationServices(DBConnectionManager dbConnectionManager, IHostingEnvironment hostEnv, IConfiguration config, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _dbConnectionManager = dbConnectionManager;
            _hostEnv = hostEnv;
            _config = config;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _loggedInUser = _httpContextAccessor.HttpContext.User;
        }
        public async Task<PagedResponse<AssetAllocationListModel>> GetAssetAllocationList(AssetAllocationDto assetAllocationDto)
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                var p = assetAllocationDto.PrepareDynamicParameters();
                db.Open();
                var result = await db.QueryMultipleAsync("[dbo].[usp_GetAssetAllocationList]", param: p, commandType: CommandType.StoredProcedure);
                var assetAllocationList = await result.ReadAsync<AssetAllocationListModel>();
                var pagedInfo = await result.ReadFirstAsync<PagedInfo>();
                var mappedData = _mapper.Map<PagedResponse<AssetAllocationListModel>>(pagedInfo);
                mappedData.Items = assetAllocationList;
                db.Close();
                return mappedData;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<PagedResponse<ExportAssetAllocationList>> ExportAssetAllocationList(AssetAllocationDto assetAllocationDto)
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                var p = assetAllocationDto.PrepareDynamicParameters();
                db.Open();
                var result = await db.QueryMultipleAsync("[dbo].[usp_GetAssetAllocationList]", param: p, commandType: CommandType.StoredProcedure);
                var assetAllocationList = await result.ReadAsync<ExportAssetAllocationList>();
                var pagedInfo = await result.ReadFirstAsync<PagedInfo>();
                var mappedData = _mapper.Map<PagedResponse<ExportAssetAllocationList>>(pagedInfo);
                mappedData.Items = assetAllocationList;
                db.Close();
                return mappedData;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<SpResponseMessage> SaveAssetAllocation(AssetAllocationVM assetAllocationVM)
        {
            try
            {
                if (assetAllocationVM.Id == 0)
                {
                    var responseMessage = await CheckExistingAssetsCode(assetAllocationVM.AssetCode);
                    if (responseMessage.StatusCode != 200)
                    {
                        var returnCheckExistingCodeResponseMessage = new SpResponseMessage
                        {
                            ReturnId = responseMessage.ReturnId,
                            Msg = responseMessage.Msg,
                            StatusCode = responseMessage.StatusCode,
                            MsgType = responseMessage.MsgType
                        };
                    }
                }
               
                var AssetAllocationModel = new AssetAllocationModel
                {
                    Id = assetAllocationVM.Id,
                    AssetId = assetAllocationVM.AssetId,
                    EmployeeId = assetAllocationVM.EmployeeId,
                    BranchId = assetAllocationVM.BranchId,
                    DepartmentId = assetAllocationVM.DepartmentId,
                    AllocateDate = assetAllocationVM.AllocateDate,
                    AllocateDateBS = assetAllocationVM.AllocateDateBS,
                    AssetCode = assetAllocationVM.AssetCode,
                    BarCodeImageUrl = assetAllocationVM.BarCodeImageUrl,
                    IsActive = assetAllocationVM.IsActive,
                    IsDeleted = false
                };

                using var db = _dbConnectionManager.GetConnection();
                var p = AssetAllocationModel.PrepareDynamicParameters();

                if (assetAllocationVM.Id > 0)
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
                var result = await db.ExecuteAsync("[dbo].[usp_IUDAssetAllocation]", param: p, commandType: CommandType.StoredProcedure);
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
        public async Task<AssetAllocationVM> GetAssetAllocationById(int Id)
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                var p = new DynamicParameters();
                db.Open();
                p.Add("@Id", Id);
                var data = await db.QuerySingleAsync<AssetAllocationVM>(sql: "[dbo].[usp_GetAssetAllocationById]", param: p, commandType: CommandType.StoredProcedure);
                db.Close();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<SpResponseMessage> DeleteAssetAllocation(int Id)
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

                var result = await db.ExecuteAsync("[dbo].[usp_IUDAssetAllocation]", param: p, commandType: CommandType.StoredProcedure);
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
        public async Task<SpResponseMessage> BulkExcelUpload(List<AssetAllocationBulkVM> assetAllocationBulkVM)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("AssetId", typeof(int));
            dataTable.Columns.Add("EmployeeId", typeof(int));
            dataTable.Columns.Add("BranchId", typeof(int));
            dataTable.Columns.Add("DepartmentId", typeof(int));
            dataTable.Columns.Add("AllocateDate", typeof(DateOnly));
            dataTable.Columns.Add("AllocateDateBS", typeof(string));
            dataTable.Columns.Add("IsActive", typeof(bool));
            dataTable.Columns.Add("CreatedDate", typeof(DateTime));
            dataTable.Columns.Add("UpdatedDate", typeof(DateTime));

            var dataRows = assetAllocationBulkVM.Select(item => dataTable.Rows.Add(
               item.AssetId, item.EmployeeId, item.BranchId,
               item.DepartmentId, item.AllocateDate, item.AllocateDateBS, item.IsActive, item.CreatedDate, item.UpdatedDate
           )).ToArray();
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                var p = new DynamicParameters();
                db.Open();
                p.Add("@Return_Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add("@Msg", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
                p.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add("@MsgType", dbType: DbType.String, size: 50, direction: ParameterDirection.Output);
                p.Add("@CreatedBy", _loggedInUser.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
                p.Add("@UpdatedBy", _loggedInUser.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
                p.Add("@BulkData", dataTable.AsTableValuedParameter("dbo.BulkAssetAllocationInsert"));
                var result = await db.ExecuteAsync("[dbo].[usp_BulkAssetAllocationInsert]", param: p, commandType: CommandType.StoredProcedure, commandTimeout: 600);
                var spresponsemessage = new SpResponseMessage
                {
                    ReturnId = p.Get<int>("@Return_Id"),
                    Msg = p.Get<string>("@Msg"),
                    StatusCode = p.Get<int>("@StatusCode"),
                    MsgType = p.Get<string>("@MsgType")
                };
                db.Close();
                return spresponsemessage;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<SpResponseMessage> CheckExistingAssetsCode(string AssetsCode)
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                var p = new DynamicParameters();

                db.Open();
                p.Add("@AssetsCode", AssetsCode);

                p.Add("@Return_Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add("@Msg", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
                p.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add("@MsgType", dbType: DbType.String, size: 50, direction: ParameterDirection.Output);

                var result = await db.ExecuteAsync("[dbo].[usp_CheckExistingAssetCode]", param: p, commandType: CommandType.StoredProcedure);
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
