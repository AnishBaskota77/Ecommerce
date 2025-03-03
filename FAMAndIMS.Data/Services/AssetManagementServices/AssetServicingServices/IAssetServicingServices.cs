using FAMAndIMS.Data.Model.AssetManagementModel.AssetServicingModel;
using FAMAndIMS.Data.Model.CommonModel;
using FAMAndIMS.Data.Model.Paging;

namespace FAMAndIMS.Data.Services.AssetManagementServices.AssetServicingServices
{
    public interface IAssetServicingServices 
    {
        Task<PagedResponse<AssetServicingListModel>> GetAssetServicingList(AssetServicingDto assetServicingDto);
        Task<SpResponseMessage> SaveAssetServicing(AssetServicingVM assetServicingVM);
        Task<AssetServicingVM> GetAssetServicingById(int Id);
        Task<SpResponseMessage> DeleteAssetServicing(int id);
    }
}
