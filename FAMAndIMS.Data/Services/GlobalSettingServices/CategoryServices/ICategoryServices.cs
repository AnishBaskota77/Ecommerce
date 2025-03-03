using FAMAndIMS.Data.Model.CommonModel;
using FAMAndIMS.Data.Model.GlobalSettingModel.CategoryModel;
using FAMAndIMS.Data.Model.Paging;

namespace FAMAndIMS.Data.Services.GlobalSettingServices.CategoryServices
{
    public interface ICategoryServices
    {
        Task<PagedResponse<CategoryListModel>> GetCategoryList(CategoryDto categoryDto);
        Task<SpResponseMessage> SaveCategory(CategoryVm categoryVm);
        Task<CategoryVm> GetCategoryById(int Id);
        Task<SpResponseMessage> DeleteCategory(int id);
    }
}
