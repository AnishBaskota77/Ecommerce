using FAMAndIMS.Data.Model.CommonModel;
using FAMAndIMS.Data.Model.GlobalSettingModel.DepreciationModel;
using FAMAndIMS.Data.Model.Paging;

namespace FAMAndIMS.Data.Services.GlobalSettingServices.DepreciationServices
{
    public interface IDepreciationTypeServices
    {
        Task<PagedResponse<DepreciationTypeListModel>> GetDepreciationTypeList(DepreciationTypeDto depreciationTypeDto);
        Task<SpResponseMessage> SaveDepreciationType(DepreciationTypeVM depreciationTypeVM);
        Task<DepreciationTypeVM> GetDepreciationTypeById(int Id);
        Task<SpResponseMessage> DeleteDepreciationType(int id);
    }
}
