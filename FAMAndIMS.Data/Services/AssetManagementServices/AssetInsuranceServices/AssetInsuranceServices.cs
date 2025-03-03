using AutoMapper;
using Dapper;
using FAMAndIMS.Data.DBManager;
using FAMAndIMS.Data.Library;
using FAMAndIMS.Data.Model.AssetManagementModel.AssetInsuranceModel;
using FAMAndIMS.Data.Model.CommonModel;
using FAMAndIMS.Data.Model.Paging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Security.Claims;

namespace FAMAndIMS.Data.Services.AssetManagementServices.AssetInsuranceServices
{
    public class AssetInsuranceServices : IAssetInsuranceServices
    {
        private readonly DBConnectionManager _dbConnectionManager;
        private readonly IHostingEnvironment _hostEnv;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClaimsPrincipal _loggedInUser;
        public AssetInsuranceServices(DBConnectionManager dbConnectionManager, IHostingEnvironment hostEnv, IConfiguration config, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _dbConnectionManager = dbConnectionManager;
            _hostEnv = hostEnv;
            _config = config;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _loggedInUser = _httpContextAccessor.HttpContext.User;
        }
        public async Task<PagedResponse<AssetInsuranceListModel>> GetAssetInsuranceList(AssetInsuranceDto assetInsuranceDto)
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                var p = assetInsuranceDto.PrepareDynamicParameters();
                db.Open();
                var result = await db.QueryMultipleAsync("[dbo].[usp_GetAssetInsuranceList]", param: p, commandType: CommandType.StoredProcedure);
                var assetInsuranceList = await result.ReadAsync<AssetInsuranceListModel>();
                var pagedInfo = await result.ReadFirstAsync<PagedInfo>();
                var mappedData = _mapper.Map<PagedResponse<AssetInsuranceListModel>>(pagedInfo);
                mappedData.Items = assetInsuranceList;
                db.Close();
                return mappedData;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<PagedResponse<ExportAssetInsuranceList>> ExportAssetInsuranceList(AssetInsuranceDto assetInsuranceDto)
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                var p = assetInsuranceDto.PrepareDynamicParameters();
                db.Open();
                var result = await db.QueryMultipleAsync("[dbo].[usp_GetAssetInsuranceList]", param: p, commandType: CommandType.StoredProcedure);
                var assetInsuranceList = await result.ReadAsync<ExportAssetInsuranceList>();
                var pagedInfo = await result.ReadFirstAsync<PagedInfo>();
                var mappedData = _mapper.Map<PagedResponse<ExportAssetInsuranceList>>(pagedInfo);
                mappedData.Items = assetInsuranceList;
                db.Close();
                return mappedData;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<SpResponseMessage> SaveAssetInsurance(AssetInsuranceVM assetInsuranceVM)
        {
            try
            {
                var AssetInsuranceModel = new AssetInsuranceModel
                {
                    Id = assetInsuranceVM.Id,
                    AssetId = assetInsuranceVM.AssetId,
                    InsuranceCompanyId = assetInsuranceVM.InsuranceCompanyId,
                    PremiumAmount = assetInsuranceVM.PremiumAmount,
                    PremiumYear = assetInsuranceVM.PremiumYear,
                    IssueDate = assetInsuranceVM.IssueDate,
                    ExpiryDate = assetInsuranceVM.ExpiryDate,
                    InsuranceType = assetInsuranceVM.InsuranceType,
                    IsActive = assetInsuranceVM.IsActive,
                    IsDeleted = false
                };

                using var db = _dbConnectionManager.GetConnection();
                var p = AssetInsuranceModel.PrepareDynamicParameters();

                if (assetInsuranceVM.Id > 0)
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
                var result = await db.ExecuteAsync("[dbo].[usp_IUDAssetInsurance]", param: p, commandType: CommandType.StoredProcedure);
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
        public async Task<AssetInsuranceVM> GetAssetInsuranceById(int Id)
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                var p = new DynamicParameters();
                db.Open();
                p.Add("@Id", Id);
                var data = await db.QuerySingleAsync<AssetInsuranceVM>(sql: "[dbo].[usp_GetAssetInsuranceById]", param: p, commandType: CommandType.StoredProcedure);
                db.Close();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SpResponseMessage> DeleteAssetInsurance(int Id)
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

                var result = await db.ExecuteAsync("[dbo].[usp_IUDAssetInsurance]", param: p, commandType: CommandType.StoredProcedure);
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

        public async Task<SpResponseMessage> BulkExcelUpload(List<AssetInsuranceBulkVM> assetInsuranceBulkVM)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("AssetId", typeof(int));
            dataTable.Columns.Add("InsuranceCompanyId", typeof(int));
            dataTable.Columns.Add("PremiumAmount", typeof(decimal));
            dataTable.Columns.Add("PremiumYear", typeof(string));
            dataTable.Columns.Add("IssueDate", typeof(DateTime));
            dataTable.Columns.Add("ExpiryDate", typeof(DateTime));
            dataTable.Columns.Add("InsuranceType", typeof(string));
            dataTable.Columns.Add("IsActive", typeof(bool));
            dataTable.Columns.Add("CreatedDate", typeof(DateTime));
            dataTable.Columns.Add("UpdatedDate", typeof(DateTime));

            var dataRows = assetInsuranceBulkVM.Select(item => dataTable.Rows.Add(
               item.AssetId, item.InsuranceCompanyId, item.PremiumAmount,
               item.PremiumYear,item.IssueDate,item.ExpiryDate,item.InsuranceType, item.IsActive, item.CreatedDate, item.UpdatedDate
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
                p.Add("@BulkData", dataTable.AsTableValuedParameter("dbo.BulkAssetInsuranceInsert"));
                var result = await db.ExecuteAsync("[dbo].[usp_BulkAssetInsuranceInsert]", param: p, commandType: CommandType.StoredProcedure, commandTimeout: 600);
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
