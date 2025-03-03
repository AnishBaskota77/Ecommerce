using FAMAndIMS.Data.Model.AssetManagementModel.AssetMaintenanceModel;
using FAMAndIMS.Data.Model.CommonModel;
using FAMAndIMS.Data.Model.Paging;

namespace FAMAndIMS.Data.Services.AssetManagementServices.AssetMaintenanceServices
{
    public interface IAssetMaintenanceServices 
    {
        Task<PagedResponse<AssetMaintenanceListModel>> GetAssetMaintenanceList(AssetMaintenanceDto assetMaintenanceDto);
        Task<PagedResponse<ExportAssetMaintenanceList>> ExportAssetMaintenanceList(AssetMaintenanceDto assetMaintenanceDto);
        Task<SpResponseMessage> SaveAssetMaintenance(AssetMaintenanceVM assetMaintenanceVM);
        Task<AssetMaintenanceVM> GetAssetMaintenanceById(int Id);
        Task<SpResponseMessage> DeleteAssetMaintenance(int id);
        Task<SpResponseMessage> BulkExcelUpload(List<AssetMaintenanceBulkVM> assetMaintenanceBulkVM);
        Task<string>GetAssetCode(int assetId);
    }
}
