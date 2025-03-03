using AutoMapper;
using Dapper;
using FAMAndIMS.Data.DBManager;
using FAMAndIMS.Data.Library;
using FAMAndIMS.Data.Model.InventoryManagementModel.StockModel;
using FAMAndIMS.Data.Model.Paging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Security.Claims;

namespace FAMAndIMS.Data.Services.InventoryManagementServices.StockServices
{
    public class StockServices : IStockServices
    {
        private readonly DBConnectionManager _dbConnectionManager;
        private readonly IHostingEnvironment _hostEnv;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClaimsPrincipal _loggedInUser;
        public StockServices(DBConnectionManager dbConnectionManager, IHostingEnvironment hostEnv, IConfiguration config, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _dbConnectionManager = dbConnectionManager;
            _hostEnv = hostEnv;
            _config = config;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _loggedInUser = _httpContextAccessor.HttpContext.User;
        }
        public async Task<PagedResponse<StockListModel>> GetStockList(StockDto stockDto)
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                var p = stockDto.PrepareDynamicParameters();
                db.Open();
                var result = await db.QueryMultipleAsync("[dbo].[usp_GetStockList]", param: p, commandType: CommandType.StoredProcedure);
                var stockList = await result.ReadAsync<StockListModel>();
                var pagedInfo = await result.ReadFirstAsync<PagedInfo>();
                var mappedData = _mapper.Map<PagedResponse<StockListModel>>(pagedInfo);
                mappedData.Items = stockList;
                db.Close();
                return mappedData;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<PagedResponse<ExportStockList>> ExportStockList(StockDto stockDto)
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                var p = stockDto.PrepareDynamicParameters();
                db.Open();
                var result = await db.QueryMultipleAsync("[dbo].[usp_GetStockList]", param: p, commandType: CommandType.StoredProcedure);
                var stockList = await result.ReadAsync<ExportStockList>();
                var pagedInfo = await result.ReadFirstAsync<PagedInfo>();
                var mappedData = _mapper.Map<PagedResponse<ExportStockList>>(pagedInfo);
                mappedData.Items = stockList;
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
