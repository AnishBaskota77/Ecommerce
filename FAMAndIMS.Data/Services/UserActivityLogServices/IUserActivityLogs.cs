using FAMAndIMS.Data.Model.UserActivityLogModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Services.UserActivityLogServices
{
    public interface IUserActivityLogs
    {
       Task AddAsync(UserActivityLogModel userActivityLogModel);
    }
}
