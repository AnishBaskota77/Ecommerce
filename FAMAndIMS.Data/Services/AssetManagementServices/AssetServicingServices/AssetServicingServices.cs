using AutoMapper;
using Dapper;
using FAMAndIMS.Data.Common;
using FAMAndIMS.Data.DBManager;
using FAMAndIMS.Data.Library;
using FAMAndIMS.Data.Model.AssetManagementModel.AssetMaintenanceModel;
using FAMAndIMS.Data.Model.AssetManagementModel.AssetServicingModel;
using FAMAndIMS.Data.Model.AssetManagementModel.AssetsModel;
using FAMAndIMS.Data.Model.CommonModel;
using FAMAndIMS.Data.Model.Paging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Security.Claims;

namespace FAMAndIMS.Data.Services.AssetManagementServices.AssetServicingServices
{
    public class AssetServicingServices : IAssetServicingServices
    {
        private readonly DBConnectionManager _dbConnectionManager;
        private readonly IHostingEnvironment _hostEnv;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClaimsPrincipal _loggedInUser;
        public AssetServicingServices(DBConnectionManager dbConnectionManager, IHostingEnvironment hostEnv, IConfiguration config, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _dbConnectionManager = dbConnectionManager;
            _hostEnv = hostEnv;
            _config = config;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _loggedInUser = _httpContextAccessor.HttpContext.User;
        }
        public async Task<PagedResponse<AssetServicingListModel>> GetAssetServicingList(AssetServicingDto assetServicingDto)
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                var p = assetServicingDto.PrepareDynamicParameters();
                db.Open();
                var result = await db.QueryMultipleAsync("[dbo].[usp_GetAssetServicingList]", param: p, commandType: CommandType.StoredProcedure);
                var assetServicingList = await result.ReadAsync<AssetServicingListModel>();
                var pagedInfo = await result.ReadFirstAsync<PagedInfo>();
                var mappedData = _mapper.Map<PagedResponse<AssetServicingListModel>>(pagedInfo);
                mappedData.Items = assetServicingList;
                db.Close();
                return mappedData;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<SpResponseMessage> SaveAssetServicing(AssetServicingVM assetServicingVM)
        {
            string imagePath = "";
            string existingImage = string.Empty;
            if (assetServicingVM.Id > 0)
            {
                var result = await GetAssetServicingById(assetServicingVM.Id);
                existingImage = result.BillImageURL;
            }
            if (assetServicingVM.BillImage != null)
            {
                if (!string.IsNullOrEmpty(existingImage) && existingImage != " ")
                {
                    existingImage = existingImage.Substring(1);
                    string existingImages = Path.Combine("" + _hostEnv.WebRootPath, existingImage);
                    System.IO.File.Delete(existingImages);
                }

                var folderPath = _config["ImageFolderPath:AssetServicingImagePath"];
                imagePath = await FileUploadHandler.UploadFile(_hostEnv, folderPath, assetServicingVM.BillImage);
            }
            try
            {
                var AssetServicingModel = new AssetServicingModel
                {
                    Id = assetServicingVM.Id,
                    AssetId = assetServicingVM.AssetId,
                    ServicingDate = assetServicingVM.ServicingDate,
                    ServicingDateBS = assetServicingVM.ServicingDateBS,
                    NextServicingDate = assetServicingVM.NextServicingDate,
                    NextServicingDateBS = assetServicingVM.NextServicingDateBS,
                    CurrentDistanceRun = assetServicingVM.CurrentDistanceRun,
                    ServicingCharge = assetServicingVM.ServicingCharge,
                    Remarks = assetServicingVM.Remarks,
                    IsDeleted = false,
                    BillImageURL = assetServicingVM.BillImage != null ? imagePath : existingImage
                };

                using var db = _dbConnectionManager.GetConnection();
                var p = AssetServicingModel.PrepareDynamicParameters();

                if (assetServicingVM.Id > 0)
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
                var result = await db.ExecuteAsync("[dbo].[usp_IUDAssetServicing]", param: p, commandType: CommandType.StoredProcedure);
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

        public async Task<AssetServicingVM> GetAssetServicingById(int Id)
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                var p = new DynamicParameters();
                db.Open();
                p.Add("@Id", Id);
                var data = await db.QuerySingleAsync<AssetServicingVM>(sql: "[dbo].[usp_GetAssetServicingById]", param: p, commandType: CommandType.StoredProcedure);
                db.Close();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SpResponseMessage> DeleteAssetServicing(int Id)
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

                var result = await db.ExecuteAsync("[dbo].[usp_IUDAssetServicing]", param: p, commandType: CommandType.StoredProcedure);
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
