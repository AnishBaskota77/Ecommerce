using FAMAndIMS.Data.Model.CommonModel;
using FAMAndIMS.Data.Model.GlobalSettingModel.SubCategoryModel;
using FAMAndIMS.Data.Model.Paging;

namespace FAMAndIMS.Data.Services.GlobalSettingServices.SubCategoryServices
{
    public interface ISubCategoryServices
    {
        Task<PagedResponse<SubCategoryListModel>> GetSubCategoryList(SubCategoryDto subCategoryDto);
        Task<SpResponseMessage> SaveSubCategory(SubCategoryVm subCategoryVm);
        Task<SubCategoryVm> GetSubCategoryById(int Id);
        Task<SpResponseMessage> DeleteSubCategory(int id);
    }
}
