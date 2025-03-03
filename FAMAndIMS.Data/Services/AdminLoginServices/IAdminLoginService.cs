using FAMAndIMS.Data.Model.AdminLoginModel;
using FAMAndIMS.Data.Model.CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Services.AdminLoginServices
{
    public interface IAdminLoginService
    {
        Task<(AdminLoginModel, SpResponseMessage)> GetAdminUserByUsername(AdminLoginVM adminUserVm);
        Task<bool> CheckPasswordAsync(string Password, AdminLoginModel adminUserMdl);
    }
}
