using FAMAndIMS.Data.Model.CommonModel;
using FAMAndIMS.Data.Model.GlobalSettingModel.UnitModel;
using FAMAndIMS.Data.Model.Paging;

namespace FAMAndIMS.Data.Services.GlobalSettingServices.UnitServices
{
    public interface IUnitServices
    {
        Task<PagedResponse<UnitListModel>> GetUnitList(UnitDto unitDto);
        Task<SpResponseMessage> SaveUnit(UnitVM unitVM);
        Task<UnitVM> GetUnitById(int Id);
        Task<SpResponseMessage> DeleteUnit(int id);
    }
}
