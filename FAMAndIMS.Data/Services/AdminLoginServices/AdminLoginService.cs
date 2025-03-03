using Dapper;
using FAMAndIMS.Data.Common.Utils.Crypto;
using FAMAndIMS.Data.DBManager;
using FAMAndIMS.Data.Model.AdminLoginModel;
using FAMAndIMS.Data.Model.CommonModel;
using FAMAndIMS.Data.Model.MenuManagerModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Services.AdminLoginServices
{
    public class AdminLoginService : IAdminLoginService
    {
        private readonly DBConnectionManager _dbConnectionManager;

        public AdminLoginService(DBConnectionManager dbConnectionManager)
        {
            _dbConnectionManager = dbConnectionManager;
        }

        public async Task<(AdminLoginModel, SpResponseMessage)> GetAdminUserByUsername(AdminLoginVM adminUserVm)
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                var p = new DynamicParameters();
                p.Add("@Username", adminUserVm.UserName);
                p.Add("@Return_Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add("@Msg", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
                p.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add("@MsgType", dbType: DbType.String, size: 50, direction: ParameterDirection.Output);
                db.Open();
                var result = await db.QueryFirstOrDefaultAsync<AdminLoginModel>("[dbo].[usp_GetAdminUserByUsername]", param: p, commandType: CommandType.StoredProcedure);
                db.Close();
                var spResponseMessage = new SpResponseMessage
                {
                    ReturnId = p.Get<int>("@Return_Id"),
                    Msg = p.Get<string>("@Msg"),
                    StatusCode = p.Get<int>("@StatusCode"),
                    MsgType = p.Get<string>("@MsgType")
                };
                return (result, spResponseMessage);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<bool> CheckPasswordAsync(string Password, AdminLoginModel adminUserMdl)
        {
            try
            {
                if (adminUserMdl is null)
                    throw new ArgumentNullException(nameof(adminUserMdl));

                if (string.IsNullOrEmpty(Password))
                    throw new ArgumentNullException(nameof(Password));

                if (adminUserMdl.PasswordHash is null || adminUserMdl.PasswordSalt is null)
                    return await Task.FromResult(false);
                return await Task.FromResult(CryptoUtils.CheckEqualHashHmacSha512(Password, adminUserMdl.PasswordHash, adminUserMdl.PasswordSalt));

            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
