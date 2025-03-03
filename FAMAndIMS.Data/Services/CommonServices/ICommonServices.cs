using FAMAndIMS.Data.Model.CommonModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Services.CommonServices
{
    public interface ICommonServices
    {
        Task<SpResponseMessage> UpdateStatus(int Id, string TableFlag, bool IsActive);

    }
}
