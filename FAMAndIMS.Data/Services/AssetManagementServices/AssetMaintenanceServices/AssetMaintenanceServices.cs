using AutoMapper;
using Dapper;
using FAMAndIMS.Data.DBManager;
using FAMAndIMS.Data.Library;
using FAMAndIMS.Data.Model.AssetManagementModel.AssetMaintenanceModel;
using FAMAndIMS.Data.Model.CommonModel;
using FAMAndIMS.Data.Model.Paging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Security.Claims;

namespace FAMAndIMS.Data.Services.AssetManagementServices.AssetMaintenanceServices
{
    public class AssetMaintenanceServices :IAssetMaintenanceServices
    {
        private readonly DBConnectionManager _dbConnectionManager;
        private readonly IHostingEnvironment _hostEnv;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClaimsPrincipal _loggedInUser;
        public AssetMaintenanceServices(DBConnectionManager dbConnectionManager, IHostingEnvironment hostEnv, IConfiguration config, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _dbConnectionManager = dbConnectionManager;
            _hostEnv = hostEnv;
            _config = config;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _loggedInUser = _httpContextAccessor.HttpContext.User;
        }
        public async Task<PagedResponse<AssetMaintenanceListModel>> GetAssetMaintenanceList(AssetMaintenanceDto assetMaintenanceDto)
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                var p = assetMaintenanceDto.PrepareDynamicParameters();
                db.Open();
                var result = await db.QueryMultipleAsync("[dbo].[usp_GetAssetMaintenanceList]", param: p, commandType: CommandType.StoredProcedure);
                var assetMaintenanceList = await result.ReadAsync<AssetMaintenanceListModel>();
                var pagedInfo = await result.ReadFirstAsync<PagedInfo>();
                var mappedData = _mapper.Map<PagedResponse<AssetMaintenanceListModel>>(pagedInfo);
                mappedData.Items = assetMaintenanceList;
                db.Close();
                return mappedData;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<PagedResponse<ExportAssetMaintenanceList>> ExportAssetMaintenanceList(AssetMaintenanceDto assetMaintenanceDto)
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                var p = assetMaintenanceDto.PrepareDynamicParameters();
                db.Open();
                var result = await db.QueryMultipleAsync("[dbo].[usp_GetAssetMaintenanceList]", param: p, commandType: CommandType.StoredProcedure);
                var assetMaintenanceList = await result.ReadAsync<ExportAssetMaintenanceList>();
                var pagedInfo = await result.ReadFirstAsync<PagedInfo>();
                var mappedData = _mapper.Map<PagedResponse<ExportAssetMaintenanceList>>(pagedInfo);
                mappedData.Items = assetMaintenanceList;
                db.Close();
                return mappedData;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<SpResponseMessage> SaveAssetMaintenance(AssetMaintenanceVM assetMaintenanceVM)
        {
            try
            {
                var AssetMaintenanceModel = new AssetMaintenanceModel
                {
                    Id = assetMaintenanceVM.Id,
                    AssetId = assetMaintenanceVM.AssetId,
                    AssetCode = assetMaintenanceVM.AssetCode,
                    MaintenanceDate = assetMaintenanceVM.MaintenanceDate,
                    MaintenanceCostAmount = assetMaintenanceVM.MaintenanceCostAmount,
                    IsActive = assetMaintenanceVM.IsActive,
                    IsDeleted = false
                };

                using var db = _dbConnectionManager.GetConnection();
                var p = AssetMaintenanceModel.PrepareDynamicParameters();

                if (assetMaintenanceVM.Id > 0)
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
                var result = await db.ExecuteAsync("[dbo].[usp_IUDAssetMaintenance]", param: p, commandType: CommandType.StoredProcedure);
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
        public async Task<AssetMaintenanceVM> GetAssetMaintenanceById(int Id)
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                var p = new DynamicParameters();
                db.Open();
                p.Add("@Id", Id);
                var data = await db.QuerySingleAsync<AssetMaintenanceVM>(sql: "[dbo].[usp_GetAssetMaintenanceById]", param: p, commandType: CommandType.StoredProcedure);
                db.Close();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> GetAssetCode(int assetId)
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                var p = new DynamicParameters();
                db.Open();
                p.Add("@AssetId", assetId);
                var data = await db.QuerySingleAsync<string>(sql: "[dbo].[usp_GetAssetCodeByAssetId]", param: p, commandType: CommandType.StoredProcedure);
                db.Close();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SpResponseMessage> DeleteAssetMaintenance(int Id)
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

                var result = await db.ExecuteAsync("[dbo].[usp_IUDAssetMaintenance]", param: p, commandType: CommandType.StoredProcedure);
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
        public async Task<SpResponseMessage> BulkExcelUpload(List<AssetMaintenanceBulkVM> assetMaintenanceBulkVM)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("AssetId", typeof(int));
            dataTable.Columns.Add("AssetCode", typeof(string));
            dataTable.Columns.Add("MaintenanceDate", typeof(DateTime));
            dataTable.Columns.Add("MaintenanceCostAmount", typeof(decimal));
            dataTable.Columns.Add("IsActive", typeof(bool));
            dataTable.Columns.Add("CreatedDate", typeof(DateTime));
            dataTable.Columns.Add("UpdatedDate", typeof(DateTime));

            var dataRows = assetMaintenanceBulkVM.Select(item => dataTable.Rows.Add(
               item.AssetId, item.AssetCode,
               item.MaintenanceDate,item.MaintenanceCostAmount, item.IsActive, item.CreatedDate, item.UpdatedDate
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
                p.Add("@BulkData", dataTable.AsTableValuedParameter("dbo.BulkAssetMaintenanceInsert"));
                var result = await db.ExecuteAsync("[dbo].[usp_BulkAssetMaintenanceInsert]", param: p, commandType: CommandType.StoredProcedure, commandTimeout: 600);
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
    }
}
