using FAMAndIMS.Data.Model.Paging;
using System.Data;
using FAMAndIMS.Data.Model.EmployeeManagementModel;
using FAMAndIMS.Data.DBManager;
using FAMAndIMS.Data.Library;
using AutoMapper;
using Dapper;
using FAMAndIMS.Data.Model.CommonModel;
using FAMAndIMS.Data.Model.MenuManagerModel;
using FAMAndIMS.Data.Common.Utils.Crypto;
using System.Data.Common;
using System.Security.Claims;
using FAMAndIMS.Data.Model.AdminUserManagementModel;
using Microsoft.AspNetCore.Http;

namespace FAMAndIMS.Data.Services.EmployeeManagementServices
{
    public class AdminUserManagementServices : IAdminUserManagementServices
    {
        private readonly DBConnectionManager _dbConnectionManager;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClaimsPrincipal _loggedInUser;

        public AdminUserManagementServices(DBConnectionManager dBConnectionManager, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _dbConnectionManager = dBConnectionManager;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _loggedInUser = _httpContextAccessor.HttpContext.User;
        }
        public async Task<PagedResponse<AdminUserManagementListModel>> GetAllAdminUserList(AdminUserListFilterDto adminUserListFilterDto)
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                var p = adminUserListFilterDto.PrepareDynamicParameters();
                db.Open();
                var result = await db.QueryMultipleAsync("[dbo].[usp_GetAdminUserList]", param: p, commandType: CommandType.StoredProcedure);
                var AdminUserList = await result.ReadAsync<AdminUserManagementListModel>();
                var pagedInfo = await result.ReadFirstAsync<PagedInfo>();
                var mappedData = _mapper.Map<PagedResponse<AdminUserManagementListModel>>(pagedInfo);
                mappedData.Items = AdminUserList;
                db.Close();
                return mappedData;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<SpResponseMessage> SaveAdminUser(AdminUserVM adminUserVM)
        {
            try
            {
                //Hashing Password
                var passwordSalt = CryptoUtils.GenerateKeySalt(128);
                var passwordHash = CryptoUtils.HashHmacSha512(adminUserVM.Password, passwordSalt);

                var AdminUserModel = new AdminUserModel
                {
                    Id = adminUserVM.Id,
                    FirstName = adminUserVM.FirstName,
                    LastName = adminUserVM.LastName,
                    Email = adminUserVM.Email,
                    UserName = adminUserVM.UserName,
                    RoleId = adminUserVM.RoleId,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    MobileNumber = adminUserVM.MobileNumber,
                    GenderId = adminUserVM.GenderId,
                    CountryId = adminUserVM.CountryId,
                    ProvinceId = adminUserVM.ProvinceId,
                    DistrictId = adminUserVM.DistrictId,
                    MunicipalityId  = adminUserVM.MunicipalityId,
                    DOBAD = adminUserVM.DOBAD ?? new DateTime(1753, 1, 1),
                    IsActive = adminUserVM.IsActive,
                    IsDeleted = false
                };

                using var db = _dbConnectionManager.GetConnection();
                var p = AdminUserModel.PrepareDynamicParameters();

                if (adminUserVM.Id > 0)
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

                var result = await db.ExecuteAsync("[dbo].[usp_IUDAdminUser]", param: p, commandType: CommandType.StoredProcedure);
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

        public async Task<AdminUserVM> GetAdminUserById(int Id)
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                var p = new DynamicParameters();
                db.Open();
                p.Add("@AdminUserId", Id);
                var data = await db.QuerySingleAsync<AdminUserVM>(sql: "[dbo].[usp_get_adminuser_byid]", param: p, commandType: CommandType.StoredProcedure);
                db.Close();
                return data;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<SpResponseMessage> UpdateAdminUser(AdminUserUpdateVM adminUserUpdateVM)
        {
            try
            {
                var AdminUserUpdateModel = new AdminUserUpdateModel
                {
                    Id = adminUserUpdateVM.Id,
                    FirstName = adminUserUpdateVM.FirstName,
                    LastName = adminUserUpdateVM.LastName,
                    Email = adminUserUpdateVM.Email,
                    UserName = adminUserUpdateVM.UserName,
                    RoleId = adminUserUpdateVM.RoleId,
                    MobileNumber = adminUserUpdateVM.MobileNumber,
                    GenderId = adminUserUpdateVM.GenderId,
                    CountryId = adminUserUpdateVM.CountryId,
                    ProvinceId = adminUserUpdateVM.ProvinceId,
                    DistrictId = adminUserUpdateVM.DistrictId,
                    MunicipalityId  = adminUserUpdateVM.MunicipalityId,
                    DOBAD = adminUserUpdateVM.DOBAD,
                    IsActive = adminUserUpdateVM.IsActive,
                    IsDeleted = false
                };

                using var db = _dbConnectionManager.GetConnection();
                var p = AdminUserUpdateModel.PrepareDynamicParameters();

                if (adminUserUpdateVM.Id > 0)
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

                var result = await db.ExecuteAsync("[dbo].[usp_IUDAdminUser]", param: p, commandType: CommandType.StoredProcedure);
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

        public async Task<SpResponseMessage> DeleteAdminUser(int Id)
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

                var result = await db.ExecuteAsync("[dbo].[usp_IUDAdminUser]", param: p, commandType: CommandType.StoredProcedure);
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

        public async Task<SpResponseMessage> AdminUserResetPassword(AdminUserResetPasswordVm adminUserResetPasswordVm)
        {
            try
            {
                var passwordSalt = CryptoUtils.GenerateKeySalt(128);
                var passwordHash = CryptoUtils.HashHmacSha512(adminUserResetPasswordVm.Password, passwordSalt);
                using var db = _dbConnectionManager.GetConnection();
                var param = new DynamicParameters();
                db.Open();
                param.Add("@UpdatedBy", _loggedInUser.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
                param.Add("@Id", adminUserResetPasswordVm.Id);
                param.Add("@PasswordHash", passwordHash);
                param.Add("@PasswordSalt", passwordSalt);
                param.Add("@Return_Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@Msg", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);
                param.Add("@StatusCode", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@MsgType", dbType: DbType.String, size: 50, direction: ParameterDirection.Output);
                var result = await db.ExecuteAsync("[dbo].[USP_AdminUserResetPassword]", param: param, commandType: CommandType.StoredProcedure);
                var spresponsemessage = new SpResponseMessage
                {
                    ReturnId = param.Get<int>("@Return_Id"),
                    Msg = param.Get<string>("@Msg"),
                    StatusCode = param.Get<int>("@StatusCode"),
                    MsgType = param.Get<string>("@MsgType")
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
