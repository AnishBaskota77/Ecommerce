using AutoMapper;
using Dapper;
using FAMAndIMS.Data.DBManager;
using FAMAndIMS.Data.Library;
using FAMAndIMS.Data.Model.AssetManagementModel.AssetInsuranceModel;
using FAMAndIMS.Data.Model.CommonModel;
using FAMAndIMS.Data.Model.InventoryManagementModel.PurchaseItemModel;
using FAMAndIMS.Data.Model.Paging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Security.Claims;

namespace FAMAndIMS.Data.Services.InventoryManagementServices.PurchaseItemServices
{
    public class PurchaseItemServices : IPurchaseItemServices
    {
        private readonly DBConnectionManager _dbConnectionManager;
        private readonly IHostingEnvironment _hostEnv;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClaimsPrincipal _loggedInUser;
        public PurchaseItemServices(DBConnectionManager dbConnectionManager, IHostingEnvironment hostEnv, IConfiguration config, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _dbConnectionManager = dbConnectionManager;
            _hostEnv = hostEnv;
            _config = config;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _loggedInUser = _httpContextAccessor.HttpContext.User;
        }
        public async Task<PagedResponse<PurchaseItemListModel>> GetPurchaseItemList(PurchaseItemDto purchaseItemDto)
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                var p = purchaseItemDto.PrepareDynamicParameters();
                db.Open();
                var result = await db.QueryMultipleAsync("[dbo].[usp_GetPurchaseItemList]", param: p, commandType: CommandType.StoredProcedure);
                var purchaseItemList = await result.ReadAsync<PurchaseItemListModel>();
                var pagedInfo = await result.ReadFirstAsync<PagedInfo>();
                var mappedData = _mapper.Map<PagedResponse<PurchaseItemListModel>>(pagedInfo);
                mappedData.Items = purchaseItemList;
                db.Close();
                return mappedData;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<PagedResponse<ExportPurchaseItemList>> ExportPurchaseItemList(PurchaseItemDto purchaseItemDto)
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                var p = purchaseItemDto.PrepareDynamicParameters();
                db.Open();
                var result = await db.QueryMultipleAsync("[dbo].[usp_GetPurchaseItemList]", param: p, commandType: CommandType.StoredProcedure);
                var purchaseItemList = await result.ReadAsync<ExportPurchaseItemList>();
                var pagedInfo = await result.ReadFirstAsync<PagedInfo>();
                var mappedData = _mapper.Map<PagedResponse<ExportPurchaseItemList>>(pagedInfo);
                mappedData.Items = purchaseItemList;
                db.Close();
                return mappedData;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<SpResponseMessage> SavePurchaseItem(PurchaseItemVM purchaseItemVM)
        {
            try
            {
                var PurchaseItemModel = new PurchaseItemModel
                {
                    Id = purchaseItemVM.Id,
                    ItemId = purchaseItemVM.ItemId,
                    Quantity = purchaseItemVM.Quantity,
                    Rate = purchaseItemVM.Rate,
                    PurchaseAmount = purchaseItemVM.PurchaseAmount,
                    VatAmount = purchaseItemVM.VatAmount,
                    VendorId = purchaseItemVM.VendorId,
                    PurchaseDate = purchaseItemVM.PurchaseDate,
                    PurchaseDateBS = purchaseItemVM.PurchaseDateBS,
                    IsActive = purchaseItemVM.IsActive,
                    IsDeleted = false
                };

                using var db = _dbConnectionManager.GetConnection();
                var p = PurchaseItemModel.PrepareDynamicParameters();

                if (PurchaseItemModel.Id > 0)
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
                var result = await db.ExecuteAsync("[dbo].[usp_IUDPurchaseItem]", param: p, commandType: CommandType.StoredProcedure);
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
        public async Task<PurchaseItemVM> GetPurchaseItemById(int Id)
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                var p = new DynamicParameters();
                db.Open();
                p.Add("@Id", Id);
                var data = await db.QuerySingleAsync<PurchaseItemVM>(sql: "[dbo].[usp_GetPurchaseItemById]", param: p, commandType: CommandType.StoredProcedure);
                db.Close();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SpResponseMessage> DeletePurchaseItem(int Id)
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
                p.Add("@UpdatedDate", DateTime.Now);
                p.Add("@Return_Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add("@Msg", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
                p.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add("@MsgType", dbType: DbType.String, size: 50, direction: ParameterDirection.Output);

                var result = await db.ExecuteAsync("[dbo].[usp_IUDPurchaseItem]", param: p, commandType: CommandType.StoredProcedure);
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

        public async Task<SpResponseMessage> BulkSavePurchaseItems(List<PurchaseItemVM> purchaseItemVM)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("ItemId", typeof(int));
            dataTable.Columns.Add("UnitId", typeof(int));
            dataTable.Columns.Add("Quantity", typeof(decimal));
            dataTable.Columns.Add("Rate", typeof(decimal));
            dataTable.Columns.Add("PurchaseAmount", typeof(decimal));
            dataTable.Columns.Add("VatAmount", typeof(decimal));
            dataTable.Columns.Add("VendorId", typeof(int));
            dataTable.Columns.Add("PurchaseDate", typeof(DateTime));
            dataTable.Columns.Add("PurchaseDateBS", typeof(string));
            dataTable.Columns.Add("IsActive", typeof(bool));

            var dataRows = purchaseItemVM.Select(item => dataTable.Rows.Add(
               item.ItemId, item.UnitId, item.Quantity,
               item.Rate, item.PurchaseAmount, item.VatAmount, item.VendorId, item.PurchaseDate, item.PurchaseDateBS, item.IsActive
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
                p.Add("@BulkData", dataTable.AsTableValuedParameter("dbo.BulkSavePurchaseItems"));
                var result = await db.ExecuteAsync("[dbo].[usp_BulkSavePurchaseItems]", param: p, commandType: CommandType.StoredProcedure, commandTimeout: 600);
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
