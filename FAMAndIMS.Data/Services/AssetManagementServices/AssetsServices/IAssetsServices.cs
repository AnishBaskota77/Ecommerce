using FAMAndIMS.Data.Model.AssetManagementModel.AssetsModel;
using FAMAndIMS.Data.Model.AssetManagementModel.AssetsModel.GeneratedDepreciationModel;
using FAMAndIMS.Data.Model.CommonModel;
using FAMAndIMS.Data.Model.Paging;

namespace FAMAndIMS.Data.Services.AssetManagementServices.AssetsServices
{
    public interface IAssetsServices
    {
        Task<PagedResponse<AssetsListModel>> GetAssetsList(AssetsDto assetsDto);
        Task<PagedResponse<ExportAssetsList>> ExportAssetsList(AssetsDto assetsDto);
        Task<SpResponseMessage> SaveAssets(AssetsVM assetsVM);
        Task<AssetsVM> GetAssetsById(int Id);
        Task<SpResponseMessage> DeleteAssets(int id);
        Task<PagedResponse<GeneratedDepreciationList>> GetDepreciationGeneratedList(GeneratedDepreciationDto generatedDepreciationDto);
        Task<PagedResponse<GeneratedDepreciationList>> ExportGenerateDepreciationList(GeneratedDepreciationDto generatedDepreciationDto);
    }
}
