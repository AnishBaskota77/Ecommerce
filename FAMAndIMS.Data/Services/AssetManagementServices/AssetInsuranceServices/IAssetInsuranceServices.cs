using FAMAndIMS.Data.Model.AssetManagementModel.AssetInsuranceModel;
using FAMAndIMS.Data.Model.CommonModel;
using FAMAndIMS.Data.Model.Paging;

namespace FAMAndIMS.Data.Services.AssetManagementServices.AssetInsuranceServices
{
    public interface IAssetInsuranceServices
    {
        Task<PagedResponse<AssetInsuranceListModel>> GetAssetInsuranceList(AssetInsuranceDto assetInsuranceDto);
        Task<PagedResponse<ExportAssetInsuranceList>> ExportAssetInsuranceList(AssetInsuranceDto assetInsuranceDto);
        Task<SpResponseMessage> SaveAssetInsurance(AssetInsuranceVM assetInsuranceVM);
        Task<AssetInsuranceVM> GetAssetInsuranceById(int Id);
        Task<SpResponseMessage> DeleteAssetInsurance(int id);
        Task<SpResponseMessage> BulkExcelUpload(List<AssetInsuranceBulkVM> assetInsuranceBulkVM);
    }
}
