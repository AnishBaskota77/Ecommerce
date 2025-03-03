using FAMAndIMS.Data.Model.AssetManagementModel.AssetAllocationModel;
using FAMAndIMS.Data.Model.CommonModel;
using FAMAndIMS.Data.Model.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMAndIMS.Data.Services.AssetManagementServices.AssetAllocationServices
{
    public interface IAssetAllocationServices
    {
        Task<PagedResponse<AssetAllocationListModel>> GetAssetAllocationList(AssetAllocationDto assetAllocationDto);
        Task<PagedResponse<ExportAssetAllocationList>> ExportAssetAllocationList(AssetAllocationDto assetAllocationDto);
        Task<SpResponseMessage> SaveAssetAllocation(AssetAllocationVM assetAllocationVM);
        Task<AssetAllocationVM> GetAssetAllocationById(int Id);
        Task<SpResponseMessage> DeleteAssetAllocation(int id);
        Task<SpResponseMessage> BulkExcelUpload(List<AssetAllocationBulkVM> assetAllocationBulkVM);
    }
}
