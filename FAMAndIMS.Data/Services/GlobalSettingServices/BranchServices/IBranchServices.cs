using FAMAndIMS.Data.Model.CommonModel;
using FAMAndIMS.Data.Model.GlobalSettingModel.BranchModel;
using FAMAndIMS.Data.Model.Paging;

namespace FAMAndIMS.Data.Services.GlobalSettingServices.BranchServices
{
    public interface IBranchServices
    {
        Task<PagedResponse<BranchListModel>> GetBranchList(BranchDto branchDto);
        Task<SpResponseMessage> SaveBranch(BranchVM branchVM);
        Task<BranchVM> GetBranchById(int Id);
        Task<SpResponseMessage> DeleteBranch(int id);
    }
}
