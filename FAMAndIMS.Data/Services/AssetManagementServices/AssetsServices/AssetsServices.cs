using AutoMapper;
using Dapper;
using FAMAndIMS.Data.Common;
using FAMAndIMS.Data.DBManager;
using FAMAndIMS.Data.Library;
using FAMAndIMS.Data.Model.AssetManagementModel.AssetInsuranceModel;
using FAMAndIMS.Data.Model.AssetManagementModel.AssetsModel;
using FAMAndIMS.Data.Model.AssetManagementModel.AssetsModel.GeneratedDepreciationModel;
using FAMAndIMS.Data.Model.CommonModel;
using FAMAndIMS.Data.Model.Paging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Reflection;
using System.Security.Claims;

namespace FAMAndIMS.Data.Services.AssetManagementServices.AssetsServices
{
    public class AssetsServices : IAssetsServices
    {
        private readonly DBConnectionManager _dbConnectionManager;
        private readonly IHostingEnvironment _hostEnv;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClaimsPrincipal _loggedInUser;
        private readonly IConfiguration _configuration;

        public AssetsServices(DBConnectionManager dbConnectionManager, IHostingEnvironment hostEnv, IConfiguration config, IMapper mapper, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _dbConnectionManager = dbConnectionManager;
            _hostEnv = hostEnv;
            _config = config;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _loggedInUser = _httpContextAccessor.HttpContext.User;
            _configuration = configuration;
        }

        public async Task<PagedResponse<AssetsListModel>> GetAssetsList(AssetsDto assetsDto)
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                var p = assetsDto.PrepareDynamicParameters();
                db.Open();
                var result = await db.QueryMultipleAsync("[dbo].[usp_GetAssetsList]", param: p, commandType: CommandType.StoredProcedure);
                var assetsList = await result.ReadAsync<AssetsListModel>();
                var pagedInfo = await result.ReadFirstAsync<PagedInfo>();
                var mappedData = _mapper.Map<PagedResponse<AssetsListModel>>(pagedInfo);
                mappedData.Items = assetsList;
                db.Close();
                return mappedData;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<PagedResponse<ExportAssetsList>> ExportAssetsList(AssetsDto assetsDto)
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                var p = assetsDto.PrepareDynamicParameters();
                db.Open();
                var result = await db.QueryMultipleAsync("[dbo].[usp_GetAssetsList]", param: p, commandType: CommandType.StoredProcedure);
                var assetsList = await result.ReadAsync<ExportAssetsList>();
                var pagedInfo = await result.ReadFirstAsync<PagedInfo>();
                var mappedData = _mapper.Map<PagedResponse<ExportAssetsList>>(pagedInfo);
                mappedData.Items = assetsList;
                db.Close();
                return mappedData;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<SpResponseMessage> SaveAssets(AssetsVM assetsVM)
        {
            string imagePath = "";
            string existingImage = string.Empty;
            if (assetsVM.Id > 0)
            {
                var result = await GetAssetsById(assetsVM.Id);
                existingImage = result.ImageOfAssetUrl;
            }
            if (assetsVM.ImageOfAsset != null)
            {
                if (!string.IsNullOrEmpty(existingImage) && existingImage != " ")
                {
                    existingImage = existingImage.Substring(1);
                    string existingImages = Path.Combine("" + _hostEnv.WebRootPath, existingImage);
                    System.IO.File.Delete(existingImages);

                }

                var folderPath = _configuration["ImageFolderPath:AssetsImagePath"];
                imagePath = await FileUploadHandler.UploadFile(_hostEnv, folderPath, assetsVM.ImageOfAsset);
            }

            try
            {
                var AssetsModel = _mapper.Map<AssetsModel>(assetsVM);
                AssetsModel.IsDeleted = false;
                AssetsModel.ImageOfAssetUrl = assetsVM.ImageOfAsset != null ? imagePath : existingImage;

                using var db = _dbConnectionManager.GetConnection();
                var p = AssetsModel.PrepareDynamicParameters();

                if (assetsVM.Id > 0)
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
                var result = await db.ExecuteAsync("[dbo].[usp_IUDAssets]", param: p, commandType: CommandType.StoredProcedure);
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
        public async Task<AssetsVM> GetAssetsById(int Id)
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                var p = new DynamicParameters();
                db.Open();
                p.Add("@Id", Id);
                var data = await db.QuerySingleAsync<AssetsVM>(sql: "[dbo].[usp_GetAssetsById]", param: p, commandType: CommandType.StoredProcedure);
                db.Close();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<SpResponseMessage> DeleteAssets(int Id)
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

                var result = await db.ExecuteAsync("[dbo].[usp_IUDAssets]", param: p, commandType: CommandType.StoredProcedure);
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
        public async Task<PagedResponse<GeneratedDepreciationList>> GetDepreciationGeneratedList(GeneratedDepreciationDto generatedDepreciationDto)
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                var p = generatedDepreciationDto.PrepareDynamicParameters();
                db.Open();
                var result = await db.QueryMultipleAsync("[dbo].[usp_GetGeneratedDepreciationList]", param: p, commandType: CommandType.StoredProcedure);
                var generatedDepreciationList = await result.ReadAsync<GeneratedDepreciationList>();
                var pagedInfo = await result.ReadFirstAsync<PagedInfo>();
                var mappedData = _mapper.Map<PagedResponse<GeneratedDepreciationList>>(pagedInfo);
                mappedData.Items = generatedDepreciationList;
                db.Close();
                return mappedData;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<PagedResponse<GeneratedDepreciationList>> ExportGenerateDepreciationList(GeneratedDepreciationDto generatedDepreciationDto)
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                var p = generatedDepreciationDto.PrepareDynamicParameters();
                db.Open();
                var result = await db.QueryMultipleAsync("[dbo].[usp_GetGeneratedDepreciationList]", param: p, commandType: CommandType.StoredProcedure);
                var list = await result.ReadAsync<GeneratedDepreciationList>();
                var pagedInfo = await result.ReadFirstAsync<PagedInfo>();
                var mappedData = _mapper.Map<PagedResponse<GeneratedDepreciationList>>(pagedInfo);
                mappedData.Items = list;
                db.Close();
                return mappedData;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
