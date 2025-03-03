using Dapper;
using FAMAndIMS.Data.DBManager;
using FAMAndIMS.Data.Model.CommondddlModel;
using FAMAndIMS.Data.Model.MenuManagerModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Services.CommonddlServices
{
    public class CommonddlService : ICommonddlService
    {
        private readonly DBConnectionManager _dbConnectionManager;

        public CommonddlService(DBConnectionManager dbConnectionManager)
        {
            _dbConnectionManager = dbConnectionManager;
        }
        public async Task<IEnumerable<CommonddlModel>> GetMainParentMenuList()
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                db.Open();
                var result = await db.QueryAsync<CommonddlModel>("[dbo].[usp_GetMainParentMenuDDL]", commandType: CommandType.StoredProcedure);
                db.Close();
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IEnumerable<CommonddlModel>> GetSubParentMenuList()
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                db.Open();
                var result = await db.QueryAsync<CommonddlModel>("[dbo].[usp_GetSubParentMenuDDL]", commandType: CommandType.StoredProcedure);
                db.Close();
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IEnumerable<CommonddlModel>> GetRoleDDL()
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                db.Open();
                var result = await db.QueryAsync<CommonddlModel>("[dbo].[usp_GetRoleddl]", commandType: CommandType.StoredProcedure);
                db.Close();
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IEnumerable<CommonddlModel>> GetGenderDDL()
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                db.Open();
                var result = await db.QueryAsync<CommonddlModel>("[dbo].[usp_GetGenderDDL]", commandType: CommandType.StoredProcedure);
                db.Close();
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IEnumerable<CommonddlModel>> GetCountryDDL()
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                db.Open();
                var result = await db.QueryAsync<CommonddlModel>("[dbo].[usp_GetCountryDDL]", commandType: CommandType.StoredProcedure);
                db.Close();
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IEnumerable<CommonddlModel>> GetProvinceDDL()
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                db.Open();
                var result = await db.QueryAsync<CommonddlModel>("[dbo].[usp_GetProvinceDDL]", commandType: CommandType.StoredProcedure);
                db.Close();
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<CommonddlModel>> GetDistrictDDL()
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                db.Open();
                var result = await db.QueryAsync<CommonddlModel>("[dbo].[usp_GetDistrictDDL]", commandType: CommandType.StoredProcedure);
                db.Close();
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IEnumerable<CommonddlModel>> GetMunicipalityDDL()
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                db.Open();
                var result = await db.QueryAsync<CommonddlModel>("[dbo].[usp_GetMunicipalityDDL]", commandType: CommandType.StoredProcedure);
                db.Close();
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<CommonddlModel>> GetCategoryDDL()
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                db.Open();
                var result = await db.QueryAsync<CommonddlModel>("[dbo].[usp_GetCategoryDDL]", commandType: CommandType.StoredProcedure);
                db.Close();
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IEnumerable<CommonddlModel>> GetBranchDDL()
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                db.Open();
                var result = await db.QueryAsync<CommonddlModel>("[dbo].[usp_GetBranchDDL]", commandType: CommandType.StoredProcedure);
                db.Close();
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<CommonddlModel>> GetDepartmentDDL()
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                db.Open();
                var result = await db.QueryAsync<CommonddlModel>("[dbo].[usp_GetDepartmentDDL]", commandType: CommandType.StoredProcedure);
                db.Close();
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<CommonddlModel>> GetAssetDDL()
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                db.Open();
                var result = await db.QueryAsync<CommonddlModel>("[dbo].[usp_GetAssetDDL]", commandType: CommandType.StoredProcedure);
                db.Close();
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<CommonddlModel>> GetInsuranceCompanyDDL()
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                db.Open();
                var result = await db.QueryAsync<CommonddlModel>("[dbo].[usp_GetInsuranceCompanyDDL]", commandType: CommandType.StoredProcedure);
                db.Close();
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IEnumerable<CommonddlModel>> GetEmployeeDDL()
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                db.Open();
                var result = await db.QueryAsync<CommonddlModel>("[dbo].[usp_GetEmployeeDDL]", commandType: CommandType.StoredProcedure);
                db.Close();
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<CommonddlModel>> GetUnitDDL()
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                db.Open();
                var result = await db.QueryAsync<CommonddlModel>("[dbo].[usp_GetUnitDDL]", commandType: CommandType.StoredProcedure);
                db.Close();
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IEnumerable<CommonddlModel>> GetSubCategoryDDL()
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                db.Open();
                var result = await db.QueryAsync<CommonddlModel>("[dbo].[usp_GetSubCategoryDDL]", commandType: CommandType.StoredProcedure);
                db.Close();
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IEnumerable<CommonddlModel>> GetDepreciationDDL()
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                db.Open();
                var result = await db.QueryAsync<CommonddlModel>("[dbo].[usp_GetDepreciationDDL]", commandType: CommandType.StoredProcedure);
                db.Close();
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<CommonddlModel>> GetConditionDDL()
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                db.Open();
                var result = await db.QueryAsync<CommonddlModel>("[dbo].[usp_GetConditionDDL]", commandType: CommandType.StoredProcedure);
                db.Close();
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<CommonddlModel>> GetVendorDDL()
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                db.Open();
                var result = await db.QueryAsync<CommonddlModel>("[dbo].[usp_GetVendorDDL]", commandType: CommandType.StoredProcedure);
                db.Close();
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<CommonddlModel>> GetItemsDDL()
        {
            try
            {
                using var db = _dbConnectionManager.GetConnection();
                db.Open();
                var result = await db.QueryAsync<CommonddlModel>("[dbo].[usp_GetItemsDDL]", commandType: CommandType.StoredProcedure);
                db.Close();
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
