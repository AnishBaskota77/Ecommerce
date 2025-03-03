using Dapper;
using FAMAndIMS.Data.DBManager;
using FAMAndIMS.Data.Library;
using FAMAndIMS.Data.Model.MenuManagerModel;
using FAMAndIMS.Data.Model.UserActivityLogModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Services.UserActivityLogServices
{
    public class UserActivityLogs : IUserActivityLogs
    {
        private readonly DBConnectionManager _dbConnectionManager;

        public UserActivityLogs(DBConnectionManager dbConnectionManager)
        {
            _dbConnectionManager = dbConnectionManager;
        }

        public async Task AddAsync(UserActivityLogModel userActivityLogModel)
        {
            var p = userActivityLogModel.PrepareDynamicParameters();
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                db.Open();
                var _ = await db.ExecuteAsync("[dbo].[usp_InsertUserActivityLog]", param: p, commandType: CommandType.StoredProcedure);
                db.Close();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
