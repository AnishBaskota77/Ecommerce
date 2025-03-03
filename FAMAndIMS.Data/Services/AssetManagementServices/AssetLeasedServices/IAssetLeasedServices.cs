using FAMAndIMS.Data.Model.AssetManagementModel.AssetLeasedModel;
using FAMAndIMS.Data.Model.CommonModel;
using FAMAndIMS.Data.Model.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Services.AssetManagementServices.AssetLeasedServices
{
    public interface IAssetLeasedServices
    {
        Task<PagedResponse<AssetLeasedListModel>> GetAssetLeasedList(AssetLeasedDto assetLeasedDto);
        Task<SpResponseMessage> SaveAssetLeased(AssetLeasedVM assetLeasedVM);
        Task<AssetLeasedVM> GetAssetLeasedById(int Id);
        Task<SpResponseMessage> DeleteAssetLeased(int id);
    }
}
